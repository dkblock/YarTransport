using System;

namespace TransportLibrary
{
    [Serializable]
    public class Location
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Location(double latitude, double longtitude)
        {
            Longitude = longtitude;
            Latitude = latitude;
        }

        public Location()
        {
            Longitude = 0;
            Latitude = 0;
        }
    }
}
