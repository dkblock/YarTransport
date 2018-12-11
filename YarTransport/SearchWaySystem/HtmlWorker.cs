using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TransportLibrary;

namespace SearchWaySystem
{
    public class HtmlWorker
    {
        private HtmlWeb _webget;
        private string _transportModel;

        public HtmlWorker()
        {
            _webget = new HtmlWeb()
            {
                OverrideEncoding = Encoding.Default
            };
        }

        public List<HtmlNode> GetNodesForSearch(Route route, bool searchOnDirectRoute)
        {
            var url = $"http://www.ot76.ru/mob/getroutestr.php?vt={GetTransportType(route)}&nmar={route.RouteNumber}";
            var doc = _webget.Load(url);
            var tableNodes = doc.DocumentNode.SelectNodes("//tr//td");
            var nodesForSearch = new List<HtmlNode>();

            if (tableNodes != null)
            {
                var item = (from t in tableNodes where t.InnerText.Contains("Прямое направление") select t).FirstOrDefault();
                var directRouteTableStart = tableNodes.IndexOf(item);

                item = (from t in tableNodes where t.InnerText.Contains("Обратное направление") select t).FirstOrDefault();
                var reverseRouteTableStart = tableNodes.IndexOf(item);

                if (searchOnDirectRoute)
                {
                    if (reverseRouteTableStart != -1)
                        nodesForSearch = (from t in tableNodes where t.InnerText.Contains("Расписание")
                                          && tableNodes.IndexOf(t) < reverseRouteTableStart select t).ToList();
                    else
                        nodesForSearch = (from t in tableNodes where t.InnerText.Contains("Расписание") select t).ToList();
                }
                else
                {
                    if (reverseRouteTableStart != -1)
                        nodesForSearch = (from t in tableNodes where t.InnerText.Contains("Расписание")
                                          && tableNodes.IndexOf(t) > reverseRouteTableStart select t).ToList();
                }
            }

            return nodesForSearch;
        }

        public List<ScheduleNode> GetSchedule(HtmlNode node)
        {
            var page = node.FirstChild.Attributes["href"].Value;
            var url = $"http://www.ot76.ru/mob/{page}/";
            var doc = _webget.Load(url);

            var schedule = new List<ScheduleNode>();
            var scheduleItems = doc.DocumentNode.SelectNodes("//tr//td");

            _transportModel = scheduleItems[1].InnerText;

            for (int i = 3; i < scheduleItems.Count; i += 2)
                schedule.Add(new ScheduleNode(scheduleItems[i].InnerText, scheduleItems[i + 1].InnerText));

            return schedule;
        }

        public RouteTime GetArrivalTime(AllRoutes allRoutes, Route route, bool searchOnDirectRoute, string stationOfDeparture)
        {
            var url = $"http://yartr.ru/config.php?vt={GetTransportType(route)}&nmar={route.RouteNumber}";
            var doc = _webget.Load(url);
            var tableNodes = doc.DocumentNode.SelectNodes("//a").ToList();
            var reverseRouteTableStart = allRoutes[route].DirectRoute.Count;

            tableNodes.RemoveAll(x => WebUtility.HtmlDecode(x.InnerText) == "назад");

            if (tableNodes.Count > 0)
            {
                if (searchOnDirectRoute)
                {
                    tableNodes.RemoveRange(reverseRouteTableStart, allRoutes[route].ReverseRoute.Count);
                    return ParseArrivalTime(route, stationOfDeparture, out url, out doc, ref tableNodes);
                }
                else
                {
                    tableNodes.RemoveRange(0, allRoutes[route].DirectRoute.Count);
                    return ParseArrivalTime(route, stationOfDeparture, out url, out doc, ref tableNodes);
                }
            }
            else
                return null;
        }

        private RouteTime ParseArrivalTime(Route route, string stationOfDeparture, out string url, out HtmlDocument doc, ref List<HtmlNode> tableNodes)
        {
            var node = (from t in tableNodes where WebUtility.HtmlDecode(t.InnerText).Contains(stationOfDeparture) select t).First();
            var page = node.Attributes["href"].Value.Replace("amp;", "");

            url = $"http://yartr.ru/{page}";
            doc = _webget.Load(url);
            tableNodes = doc.DocumentNode.SelectNodes("//body").ToList();

            var tableStrings = ReplaceTrashSymbols(WebUtility.HtmlDecode(tableNodes[0].InnerText), stationOfDeparture).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var routeToFind = GetStringToFind(route);

            for (int i = 0; i < tableNodes.Count; i++)
            {
                var item = $"{tableStrings[i]} {tableStrings[i + 1]}";

                if (item == routeToFind && tableStrings[i + 2] != "рейсов")
                    return new RouteTime(tableStrings[i + 2].Replace('.', ':'));
            }

            return null;
        }

        private string GetStringToFind(Route route)
        {
            switch(route.TransportType)
            {
                case Transport.Bus:
                    var routeNumber = route.ToString().Replace("Автобус № ", "");
                    return $"Ав {routeNumber}:";
                case Transport.Trolley:
                    return $"Тб {route.RouteNumber}:";
                case Transport.Tram:
                    return $"Тб {route.RouteNumber}:";
                default:
                    return "";
            }
        }

        private string ReplaceTrashSymbols(string text, string stationOfDeparture)
        {
            text = text.Replace("назад", "");
            text = text.Replace("Время прохождения", "");
            text = text.Replace($"Отправление от {stationOfDeparture}", "");
            text = text.Replace("Табло", "");

            return text;
        }

        public string GetTransportModel()
        {
            return _transportModel;
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
