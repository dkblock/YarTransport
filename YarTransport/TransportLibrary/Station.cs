using System;
using System.Collections.Generic;

namespace TransportLibrary
{
    [Serializable]
    public class Station
    {
        public string StationName { get; private set; }
        public List<Location> StationLocations { get; private set; }

        public Station()
        {
            StationName = string.Empty;
            StationLocations = new List<Location>();
        }

        public Station(string stationName, List<Location> stationLocations)
        {
            StationName = stationName;
            StationLocations = stationLocations;
        }

        public void AddLocation(Location location)
        {
            StationLocations.Add(location);
        }
    }
}
