using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
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

            if (tableNodes != null)
            {
                var item = (from t in tableNodes where t.InnerText.Contains("Обратное направление") select t).FirstOrDefault();
                var reverseRouteTableStart = tableNodes.IndexOf(item);

                if (searchOnDirectRoute)
                    return (from t in tableNodes
                            where t.InnerText.Contains("Расписание") && tableNodes.IndexOf(t) < reverseRouteTableStart select t).ToList();
                else
                    return (from t in tableNodes where t.InnerText.Contains("Расписание") && tableNodes.IndexOf(t) > reverseRouteTableStart select t).ToList();
            }
            else
                return new List<HtmlNode>();
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
