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

        public override string ToString()
        {
            return $"{GetTransportType()} № {ReplaceLetter(RouteNumber)}";
        }

        private string GetTransportType()
        {
            switch (TransportType)
            {
                case Transport.Bus:
                    return "Автобус";
                case Transport.Trolley:
                    return "Троллейбус";
                case Transport.Minibus:
                    return "Маршрутка";
                case Transport.Tram:
                    return "Трамвай";
                default:
                    return "";
            }
        }

        private string ReplaceLetter(string routeNumber)
        {
            if (routeNumber.Contains("a"))
                return routeNumber.Replace('a', 'а');

            if (routeNumber.Contains("b"))
                return routeNumber.Replace('b', 'б');

            if (routeNumber.Contains("c"))
                return routeNumber.Replace('c', 'с');

            if (routeNumber.Contains("d"))
                return routeNumber.Replace('d', 'д');

            if (routeNumber.Contains("g"))
                return routeNumber.Replace('g', 'г');

            if (routeNumber.Contains("k"))
                return routeNumber.Replace('k', 'к');

            if (routeNumber.Contains("m"))
                return routeNumber.Replace('m', 'м');

            if (routeNumber.Contains("p"))
                return routeNumber.Replace('p', 'п');

            if (routeNumber.Contains("t"))
                return routeNumber.Replace('t', 'т');

            return routeNumber;
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
