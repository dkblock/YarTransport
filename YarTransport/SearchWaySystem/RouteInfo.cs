﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SearchWaySystem
{ 
    [DataContract]
    public class RouteInfo
    {
        [DataMember]
        public string RouteType { get; private set; }

        [DataMember]
        public string TransportModel { get; private set; }

        [DataMember]
        public List<ScheduleNode> Schedule { get; private set; }

        [DataMember]
        public RouteTime ArrivalTime { get; private set; }

        public RouteInfo(string routeType, string transportModel, List<ScheduleNode> schedule, string pointOfDeparture)
        {
            RouteType = routeType;
            Schedule = schedule;
            TransportModel = transportModel;
            ArrivalTime = GetArrivalTime(pointOfDeparture);
        }

        public RouteInfo(string route, RouteTime arrivalTime, string pointOfDeparture)
        {
            RouteType = route;
            TransportModel = "Unknown";
            ArrivalTime = arrivalTime;
            Schedule = new List<ScheduleNode>() { new ScheduleNode(pointOfDeparture, arrivalTime) };
        }

        public string GetRouteInfo()
        {
            StringBuilder text = new StringBuilder();

            text.Append($"{RouteType}\n");

            if (TransportModel != "Unknown")
            {
                text.Append($"{TransportModel}\n\n");

                foreach (var node in Schedule)
                    text.Append($"{node.ToString()}\n");
            }
            else
            {
                text.Append($"\n{Schedule[0].ToString()}\n\n");
                text.Append($"Подробная информация о маршруте будет доступна по прибытии транспорта на конечную остановку");
            }

            return text.ToString();
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

    [DataContract]
    public class ScheduleNode
    {
        [DataMember]
        public string StationName { get; private set; }

        [DataMember]
        public RouteTime Time { get; private set; }

        public ScheduleNode(string stationName, string time)
        {
            StationName = stationName;
            Time = new RouteTime(time);
        }

        public ScheduleNode(string stationName, RouteTime time)
        {
            StationName = stationName;
            Time = time;
        }

        public override string ToString()
        {
            return $"{Time.ToString()}  {StationName}";
        }
    }

    [DataContract]
    public class RouteTime : IComparable
    {
        [DataMember]
        public int Hour { get; private set; }

        [DataMember]
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
