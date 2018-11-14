using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TransportLibrary;

namespace SearchWaySystem
{
    public class Searcher
    {
        private AllRoutes _allRoutes;
        private AllStations _allStations;
        private RouteMatrix _routeMatrix;
        private HtmlWeb _webget;

        public Searcher()
        {
            var formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("allroutes.dat", FileMode.OpenOrCreate))
            {
                _allRoutes = (AllRoutes)formatter.Deserialize(fs);
            }

            using (FileStream fs = new FileStream("allstations.dat", FileMode.OpenOrCreate))
            {
                _allStations = (AllStations)formatter.Deserialize(fs);
            }

            using (FileStream fs = new FileStream("routematrix.dat", FileMode.OpenOrCreate))
            {
                _routeMatrix = (RouteMatrix)formatter.Deserialize(fs);
            }

            _webget = new HtmlWeb()
            {
                OverrideEncoding = Encoding.Default
            };
        }

        public List<RouteInfo> GetRoutes(string pointOfDeparture, string pointOfDestination)
        {
            var indexOfDeparture = _allStations[pointOfDeparture];
            var indexOfDestination = _allStations[pointOfDestination];
            var routesForSearch = _routeMatrix.GetRoutes(indexOfDeparture, indexOfDestination);

            var routesInfo = new List<RouteInfo>();

            for (int i = 0; i < routesForSearch.Count; i++)
            {
                var route = routesForSearch[i];
                var directRoute = _allRoutes[route].DirectRoute;
                bool searchOnDirectRoute = GetRouteDirection(pointOfDeparture, pointOfDestination, directRoute);

                var nodesForSearch = GetNodesForSearch(route, searchOnDirectRoute);

                foreach (var node in nodesForSearch)
                    routesInfo.Add(LocateTransport(route, node));
            }

            return routesInfo;
        }

        private RouteInfo LocateTransport(Route route, HtmlNode node)
        {
            var page = node.FirstChild.Attributes["href"].Value;
            var url = $"http://www.ot76.ru/mob/{page}/";
            var doc = _webget.Load(url);

            var schedule = new List<ScheduleNode>();
            var scheduleItems = doc.DocumentNode.SelectNodes("//tr//td");

            for (int j = 3; j < scheduleItems.Count; j += 2)
            {
                schedule.Add(new ScheduleNode(scheduleItems[j].InnerText, scheduleItems[j + 1].InnerText));
            }

            var transportModel = scheduleItems[1].InnerText;

            return new RouteInfo(route.ToString(), transportModel, schedule);
        }

        private List<HtmlNode> GetNodesForSearch(Route route, bool searchOnDirectRoute)
        {
            var url = $"http://www.ot76.ru/mob/getroutestr.php?vt={GetTransportType(route)}&nmar={route.RouteNumber}";
            var doc = _webget.Load(url);
            var tableNodes = doc.DocumentNode.SelectNodes("//tr//td");

            if (tableNodes!=null)
            {
                var item = (from t in tableNodes where t.InnerText.Contains("Обратное направление") select t).FirstOrDefault();
                var reverseRouteTableStart = tableNodes.IndexOf(item);

                List<HtmlNode> nodesForSearch;

                if (searchOnDirectRoute)
                    nodesForSearch = (from t in tableNodes
                                      where t.InnerText.Contains("Расписание") &&
                 tableNodes.IndexOf(t) < reverseRouteTableStart
                                      select t).ToList();
                else
                    nodesForSearch = (from t in tableNodes
                                      where t.InnerText.Contains("Расписание") &&
                 tableNodes.IndexOf(t) > reverseRouteTableStart
                                      select t).ToList();

                return nodesForSearch;
            }
            else
                return new List<HtmlNode>();
        }

        private bool GetRouteDirection(string pointOfDeparture, string pointOfDestination, List<string> directRoute)
        {
            if (directRoute.Contains(pointOfDeparture) && directRoute.Contains(pointOfDestination)
                && directRoute.IndexOf(pointOfDestination) > directRoute.IndexOf(pointOfDeparture))
                return true;
            else
                return false;
        }

        private int GetTransportType(Route route)
        {
            switch (route.TransportType)
            {
                case Transport.Bus:
                    return 1;
                case Transport.Trolley:
                    return 2;
                case Transport.Tram:
                    return 3;
                case Transport.Minibus:
                    return 4;
                default:
                    return 0;
            }
        }
    }
}
