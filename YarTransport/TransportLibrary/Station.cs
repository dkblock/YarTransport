using System;

namespace TransportLibrary
{
    [Serializable]
    public class Station
    {
        public string StationName { get; private set; }

        public Station(string stationName)
        {
            StationName = stationName;
        }
    }
}
