using System.Collections.Generic;

namespace SearchWaySystem
{
    public class RouteInfo
    {
        public string RouteType { get; private set; }
        public string TransportModel { get; private set; }
        public List<ScheduleNode> Schedule { get; private set; } 

        public RouteInfo(string routeType, string transportModel, List<ScheduleNode> schedule)
        {
            RouteType = routeType;
            Schedule = schedule;
            TransportModel = transportModel;
        }
    }

    public class ScheduleNode
    {
        public string StationName { get; private set; }
        public string Time { get; private set; }

        public ScheduleNode(string stationName, string time)
        {
            StationName = stationName;
            Time = time;
        }

        public override string ToString()
        {
            return $"{StationName} {Time}";
        }
    }
}
