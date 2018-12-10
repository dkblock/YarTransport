using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchWaySystem
{
    public class RouteInfo
    {
        public string RouteType { get; private set; }
        public string TransportModel { get; private set; }
        public List<ScheduleNode> Schedule { get; private set; } 
        public RouteTime ArrivalTime { get; private set; }

        public RouteInfo(string routeType, string transportModel, List<ScheduleNode> schedule, string pointOfDeparture)
        {
            RouteType = routeType;
            Schedule = schedule;
            TransportModel = transportModel;
            ArrivalTime = GetArrivalTime(pointOfDeparture);
        }

        private RouteTime GetArrivalTime(string pointOfDeparture)
        {
            return (from t in Schedule where t.StationName == pointOfDeparture select t.Time).FirstOrDefault();
        }

        public override string ToString()
        {
            return $"{ArrivalTime.ToString()}  {RouteType}";
        }
    }

    public class ScheduleNode
    {
        public string StationName { get; private set; }
        public RouteTime Time { get; private set; }

        public ScheduleNode(string stationName, string time)
        {
            StationName = stationName;
            Time = new RouteTime(time);
        }

        public override string ToString()
        {
            return $"{Time.ToString()}  {StationName}";
        }
    }

    public class RouteTime : IComparable
    {
        public int Hour { get; private set; }
        public int Minute { get; private set; }

        public RouteTime(string time)
        {
            var timeContent = time.Split(':');

            Hour = int.Parse(timeContent[0]);
            Minute = int.Parse(timeContent[1]);
        }

        public int CompareTo(object obj)
        {
            var t = (RouteTime)obj;

            if (Hour == t.Hour)
            {
                if (Minute > t.Minute)
                    return 1;
                else
                {
                    if (Minute < t.Minute)
                        return -1;
                    else
                        return 0;
                }
            }
            else
            {
                if (Hour > t.Hour)
                    return 1;
                else
                    return -1;
            }
        }

        public override string ToString()
        {
            var hourToString = Hour.ToString();
            var minuteToString = Minute.ToString();

            if (Hour < 10)
                hourToString = hourToString.Insert(0, "0");

            if (Minute < 10)
                minuteToString = minuteToString.Insert(0, "0");

            return $"{hourToString}:{minuteToString}";
        }
    }
}
