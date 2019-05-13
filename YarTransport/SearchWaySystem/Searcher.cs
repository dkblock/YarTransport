using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TransportLibrary;

namespace SearchWaySystem
{
    public class Searcher
    {
        private AllRoutes _allRoutes;
        private AllStations _allStations;
        private RouteMatrix _routeMatrix;
        private HtmlWorker _htmlWorker;
        private List<string> _stations;

        public Searcher(AllRoutes allRoutes, AllStations allStations, RouteMatrix routeMatrix)
        {
            _allRoutes = allRoutes;
            _allStations = allStations;
            _routeMatrix = routeMatrix;
            _htmlWorker = new HtmlWorker();
        }

        public Searcher()
        {            
            _htmlWorker = new HtmlWorker();

            var formatter = new BinaryFormatter();

            using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"data\allroutes.dat", FileMode.Open, FileAccess.Read))
            {
                _allRoutes = (AllRoutes)formatter.Deserialize(fs);
            }

            using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"data\allstations.dat", FileMode.Open, FileAccess.Read))
            {
                _allStations = (AllStations)formatter.Deserialize(fs);

                _stations = new List<string>();

                for (int i = 0; i < _allStations.Count; i++)
                    _stations.Add(_allStations.GetStation(i).StationName);

                _stations.Sort();
            }

            using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"data\routematrix.dat", FileMode.Open, FileAccess.Read))
            {
                _routeMatrix = (RouteMatrix)formatter.Deserialize(fs);
            }
        }

        public List<string> GetStations()
        {
            return _stations;
        }

        public List<RouteInfo> GetRoutes(string pointOfDeparture, string pointOfDestination, bool isBusChecked, bool isTrolleyChecked, bool isTramChecked, bool isMiniBusChecked)
        {
            var indexOfDeparture = _allStations[pointOfDeparture];
            var indexOfDestination = _allStations[pointOfDestination];
            var routesForSearch = _routeMatrix.GetRoutes(indexOfDeparture, indexOfDestination);

            if (routesForSearch.Count == 0)
                return null;

            routesForSearch = RemoveNonSelectedTransport(routesForSearch, isBusChecked, isTrolleyChecked, isTramChecked, isMiniBusChecked);
            var routesInfo = new List<RouteInfo>();

            for (int i = 0; i < routesForSearch.Count; i++)
            {
                var route = routesForSearch[i];
                var directRoute = _allRoutes[route].DirectRoute;
                bool searchOnDirectRoute = GetRouteDirection(pointOfDeparture, pointOfDestination, directRoute);

                var nodesForSearch = _htmlWorker.GetNodesForSearch(route, searchOnDirectRoute);
                var lastRoutesCount = routesInfo.Count;

                foreach (var node in nodesForSearch)
                {
                    var schedule = _htmlWorker.GetSchedule(node);
                    var transportModel = _htmlWorker.GetTransportModel();
                    var indexOfDepartureStation = (from t in schedule where t.StationName == pointOfDeparture select t).FirstOrDefault();

                    if (indexOfDepartureStation != null)
                        routesInfo.Add(new RouteInfo(route.ToString(), transportModel, schedule, pointOfDeparture));
                }

                if (routesInfo.Count == lastRoutesCount && route.TransportType != Transport.Minibus)
                {
                    var arrivalTime = _htmlWorker.GetArrivalTime(_allRoutes, route, searchOnDirectRoute, pointOfDeparture);

                    if (arrivalTime != null)
                        routesInfo.Add(new RouteInfo(route.ToString(), arrivalTime, pointOfDeparture));
                }
            }

            routesInfo = (from t in routesInfo orderby t.ArrivalTime, t.RouteType select t).ToList();

            return routesInfo;
        }

        private List<Route> RemoveNonSelectedTransport(List<Route> routesForSearch, bool isBusChecked, bool isTrolleyChecked, bool isTramChecked, bool isMiniBusChecked)
        {
            var item = new List<Route>();
            item.AddRange(routesForSearch);

            if (!isBusChecked)
                item.RemoveAll(x => x.TransportType == Transport.Bus);

            if (!isTrolleyChecked)
                item.RemoveAll(x => x.TransportType == Transport.Trolley);

            if (!isTramChecked)
                item.RemoveAll(x => x.TransportType == Transport.Tram);

            if(!isMiniBusChecked)
                item.RemoveAll(x => x.TransportType == Transport.Minibus);

            return item;
        }

        private bool GetRouteDirection(string pointOfDeparture, string pointOfDestination, List<string> directRoute)
        {
            if (directRoute.Contains(pointOfDeparture) && directRoute.Contains(pointOfDestination)
                && directRoute.IndexOf(pointOfDestination) > directRoute.IndexOf(pointOfDeparture))
                return true;
            else
                return false;
        }   
    }
}
