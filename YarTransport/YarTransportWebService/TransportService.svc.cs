using SearchWaySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TransportLibrary;

namespace YarTransportWebService
{
    public class TransportService : ITransportService
    {
        private Searcher _searcher;
        private List<string> _stations;

        public TransportService()
        {
            _searcher = InitSearcher();
        }

        public List<RouteInfo> GetRoutes(string pointOfDeparture, string pointOfDestination, bool isBusChecked, bool isTrolleyChecked, bool isTramChecked, bool isMiniBusChecked)
        {
            return _searcher.GetRoutes(pointOfDeparture, pointOfDestination, isBusChecked, isTrolleyChecked, isTramChecked, isMiniBusChecked);
        }

        public List<string> GetStations()
        {
            return _stations;
        }

        private Searcher InitSearcher()
        {
            AllRoutes allRoutes;
            AllStations allStations;
            RouteMatrix routeMatrix;
            var formatter = new BinaryFormatter();

            using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"Data\allroutes.dat", FileMode.Open, FileAccess.Read))
            {
                allRoutes = (AllRoutes)formatter.Deserialize(fs);
            }

            using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"Data\allstations.dat", FileMode.Open, FileAccess.Read))
            {
                allStations = (AllStations)formatter.Deserialize(fs);

                _stations = new List<string>();

                for (int i = 0; i < allStations.Count; i++)
                    _stations.Add(allStations.GetStation(i).StationName);

                _stations.Sort();
            }

            using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"Data\routematrix.dat", FileMode.Open, FileAccess.Read))
            {
                routeMatrix = (RouteMatrix)formatter.Deserialize(fs);
            }

            return new Searcher(allRoutes, allStations, routeMatrix);
        }
    }
}
