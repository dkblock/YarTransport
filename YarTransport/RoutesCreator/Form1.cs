using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using TransportLibrary;

namespace RoutesCreator
{
    public partial class Form1 : Form
    {
        StationsLoader _stationsLoader;
        AllRoutes _allRoutes;
        AllStations _allStations;
        RouteMatrix _routeMatrix;

        public Form1()
        {
            InitializeComponent();

            var formatter = new BinaryFormatter();
            _allRoutes = new AllRoutes();
            _allStations = new AllStations();
            _routeMatrix = new RouteMatrix();
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            _stationsLoader = new StationsLoader();

            for (int i = 1; i <= 4; i++)
            {
                var transportType = i;
                var url = $"http://www.yargortrans.ru/list.php?vt={transportType}";
                var _transportType = SwitchTransportType(i);

                var Webget = new HtmlWeb()
                {
                    OverrideEncoding = Encoding.Default
                };

                var doc = Webget.Load(url);
                var routes = doc.DocumentNode.SelectNodes("//li[@class='route_list_item']//a");

                for (int j = 0; j < routes.Count; j++)
                {
                    string[] items = routes[j].InnerText.Split('.');
                    var routeNumber = items[0];
                    var routeNumberEn = ReplaceLetter(routeNumber);

                    url = $"http://www.yargortrans.ru/config.php?vt={transportType}&nmar={routeNumberEn}";
                    _stationsLoader.Load(url,transportType);

                    var directRoute = new List<string>();
                    var reverseRoute = new List<string>();
                    var lengthOfDirectRoute = _stationsLoader.CountOfStationsOnDirectRoute;

                    for (int k = 0; k < lengthOfDirectRoute; k++)
                    {
                        directRoute.Add(_stationsLoader.StationsList[k]);
                        _allStations.AddStation(new Station(_stationsLoader.StationsList[k]));
                    }

                    for (int k = lengthOfDirectRoute; k < _stationsLoader.StationsList.Count; k++)
                    {
                        reverseRoute.Add(_stationsLoader.StationsList[k]);
                        _allStations.AddStation(new Station(_stationsLoader.StationsList[k]));
                    }

                    var route = new Route(routeNumberEn, _transportType);
                    var routeContent = new RouteContent(directRoute, reverseRoute);

                    _allRoutes.AddRoute(route, routeContent);
                    _routeMatrix.AddRoute(route, directRoute, reverseRoute, _allStations);
                }
            }                            

            var formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("allroutes.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, _allRoutes);
            }

            using (FileStream fs = new FileStream("allstations.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, _allStations);
            }

            using (FileStream fs = new FileStream("routematrix.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, _routeMatrix);
            }

            label1.Visible = true;
        }

        private static Transport SwitchTransportType(int i)
        {
            Transport _transportType;

            switch (i)
            {
                case 1:
                    _transportType = Transport.Bus;
                    break;
                case 2:
                    _transportType = Transport.Trolley;
                    break;
                case 3:
                    _transportType = Transport.Tram;
                    break;
                case 4:
                    _transportType = Transport.Minibus;
                    break;
                default:
                    _transportType = Transport.Bus;
                    break;
            }

            return _transportType;
        }

        private string ReplaceLetter(string routeNumber)
        {
            if (routeNumber.Contains("а"))
                return routeNumber.Replace('а', 'a');

            if (routeNumber.Contains("б"))
                return routeNumber.Replace('б', 'b');

            if (routeNumber.Contains("с"))
                return routeNumber.Replace('с', 'c');

            if (routeNumber.Contains("д"))
                return routeNumber.Replace('д', 'd');

            if (routeNumber.Contains("г"))
                return routeNumber.Replace('г', 'g');

            if (routeNumber.Contains("к"))
                return routeNumber.Replace('к', 'k');

            if (routeNumber.Contains("м"))
                return routeNumber.Replace('м', 'm');

            if (routeNumber.Contains("п"))
                return routeNumber.Replace('п', 'p');

            if (routeNumber.Contains("т"))
                return routeNumber.Replace('т', 't');

            return routeNumber;
        }
    }
}
