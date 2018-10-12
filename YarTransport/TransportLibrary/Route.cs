using System;

namespace TransportLibrary
{
    [Serializable]
    public class Route
    {
        public string RouteNumber { get; private set; }
        public Transport TransportType { get; private set; }

        public Route(string routeNumber, Transport transport)
        {
            RouteNumber = routeNumber;
            TransportType = transport;
        }

        public Route()
        {
            RouteNumber = "";
        }
    }

    public enum Transport
    {
        Bus,
        Trolley,
        Tram,
        Minibus
    }
}
