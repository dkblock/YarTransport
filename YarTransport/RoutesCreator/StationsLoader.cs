using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text;

namespace RoutesCreator
{
    public class StationsLoader
    {
        public List<string> StationsList { get; private set; }
        public int CountOfStationsOnDirectRoute { get; private set; }

        public void Load(string url, int transportType)
        {
            StationsList = new List<string>();

            var Webget = new HtmlWeb()
            {
                OverrideEncoding = Encoding.Default
            };

            var doc = Webget.Load(url);
            var stations = doc.DocumentNode.SelectNodes("//div[@class='stations_list']//a");

            foreach (var station in stations)
                StationsList.Add(station.InnerText);

            var table = doc.DocumentNode.SelectNodes("//table//tbody//tr//td");

            if (transportType != 4)
                CountOfStationsOnDirectRoute = int.Parse(table[1].InnerText);
            else
                CountOfStationsOnDirectRoute = int.Parse(table[10].InnerText);
        }
    }
}
