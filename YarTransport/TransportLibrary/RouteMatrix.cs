using System;
using System.Collections.Generic;

namespace TransportLibrary
{
    [Serializable]
    public class RouteMatrix
    {
        public List<List<List<Route>>> RouteList { get; private set; }

        public RouteMatrix()
        {
            RouteList = new List<List<List<Route>>>();
        }

        public void AddRoute(Route route, List<string> stationsOnDirectRoute, List<string> stationsOnReverseRoute, AllStations allStations)
        {
            AddNewStations(allStations);
            FillMatrix(route, stationsOnDirectRoute, allStations);
            FillMatrix(route, stationsOnReverseRoute, allStations);
        }

        private void AddNewStations(AllStations allStations)
        {
            if (RouteList.Count < allStations.Count)
            {
                var needToAdd = allStations.Count - RouteList.Count;

                for (int i = 0; i < needToAdd; i++)
                {
                    RouteList.Add(new List<List<Route>>());
                }

                for (int i = 0; i < RouteList.Count; i++)
                {
                    needToAdd = allStations.Count - RouteList[i].Count;

                    for (int j = 0; j < needToAdd; j++)
                    {
                        RouteList[i].Add(new List<Route>());
                    }
                }
            }
        }

        private void FillMatrix(Route route, List<string> stationsOnCurrentDirection, AllStations allStations)
        {
            for(int i=0; i<stationsOnCurrentDirection.Count;i++)
            {
                for(int j=i+1; j<stationsOnCurrentDirection.Count;j++)
                {
                    var stationOfDeparture = allStations[stationsOnCurrentDirection[i]];
                    var stationOfDestination = allStations[stationsOnCurrentDirection[j]];

                    RouteList[stationOfDeparture][stationOfDestination].Add(route);
                }
            }
        }
    }
}
