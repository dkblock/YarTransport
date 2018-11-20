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

        public Searcher()
        {
            var formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("allroutes.dat", FileMode.OpenOrCreate))
            {
                _allRoutes = (AllRoutes)formatter.Deserialize(fs);
            }

            using (FileStream fs = new FileStream("allstations.dat", FileMode.OpenOrCreate))
            {
                _allStations = (AllStations)formatter.Deserialize(fs);
            }

            using (FileStream fs = new FileStream("routematrix.dat", FileMode.OpenOrCreate))
            {
                _routeMatrix = (RouteMatrix)formatter.Deserialize(fs);
            }

            _htmlWorker = new HtmlWorker();
        }

        public List<RouteInfo> GetRoutes(string pointOfDeparture, string pointOfDestination)
        {
            var indexOfDeparture = _allStations[pointOfDeparture];
            var indexOfDestination = _allStations[pointOfDestination];
            var routesForSearch = _routeMatrix.GetRoutes(indexOfDeparture, indexOfDestination);
            var routesInfo = new List<RouteInfo>();

            for (int i = 0; i < routesForSearch.Count; i++)
            {
                var route = routesForSearch[i];
                var directRoute = _allRoutes[route].DirectRoute;
                bool searchOnDirectRoute = GetRouteDirection(pointOfDeparture, pointOfDestination, directRoute);

                var nodesForSearch = _htmlWorker.GetNodesForSearch(route, searchOnDirectRoute);

                foreach (var node in nodesForSearch)
                {
                    var schedule = _htmlWorker.GetSchedule(node);
                    var transportModel = _htmlWorker.GetTransportModel();
                    var indexOfDepartureStation = (from t in schedule where t.StationName == pointOfDeparture select t).FirstOrDefault();

                    if(indexOfDepartureStation!=null)
                        routesInfo.Add(new RouteInfo(route.ToString(), transportModel, schedule, pointOfDeparture));
                }
            }

            routesInfo = (from t in routesInfo orderby t.ArrivalTime, t.RouteType select t).ToList();

            return routesInfo;
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
