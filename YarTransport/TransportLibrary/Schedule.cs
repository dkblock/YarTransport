using System.Collections.Generic;

namespace TransportLibrary
{
    public class Schedule
    {
        public List<string> StationsList { get; private set; }
        public List<string> TimeList { get; private set; }

        public Schedule(List<string> stationsList, List<string> timeList)
        {
            StationsList = stationsList;
            TimeList = timeList;
        }
    }
}
