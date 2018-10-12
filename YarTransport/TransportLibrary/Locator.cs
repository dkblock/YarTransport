using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text;

namespace TransportLibrary
{
    public class Locator
    {
        private List<Schedule> _scheduleList;

        public string Locate(int number)
        {
            _scheduleList = new List<Schedule>();

            string url = $"http://www.ot76.ru/mob/getroutestr.php?vt=2&nmar={number}";

            var Webget = new HtmlWeb()
            {
                OverrideEncoding = Encoding.Default
            };

            var doc = Webget.Load(url);
            var transportSchedule = doc.DocumentNode.SelectNodes("//tr//td//a");

            foreach (var schedule in transportSchedule)
            {
                var page = schedule.Attributes["href"].Value;
                url = $"http://www.ot76.ru/mob/{page}/";
                doc = Webget.Load(url);

                var stationsList = new List<string>();
                var timeList = new List<string>();
                var scheduleItems = doc.DocumentNode.SelectNodes("//tr//td");

                for (int i = 3; i < scheduleItems.Count; i++)
                {
                    if (i % 2 != 0)
                        stationsList.Add(scheduleItems[i].InnerText);
                    else
                        timeList.Add(scheduleItems[i].InnerText);
                }

                _scheduleList.Add(new Schedule(stationsList, timeList));
            }

            string answer = "";

            for (int i = 0; i < _scheduleList.Count; i++)
            {
                answer += $"<Транспорт №{i}> До остановки: {_scheduleList[i].StationsList[_scheduleList[i].StationsList.Count - 1]}\n";

                for (int j = 0; j < _scheduleList[i].StationsList.Count; j++)
                {
                    answer += $"{_scheduleList[i].StationsList[j]}  ";
                    answer += $"{_scheduleList[i].TimeList[j]}\n";
                }

                answer += "\n";
            }

            return answer;
        }
    }
}
