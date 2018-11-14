using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using TransportLibrary;

namespace RoutesCreator
{
    public partial class Form1 : Form
    {
        StationsLoader _stationsLoader;
        AllRoutes _allRoutes;

        public Form1()
        {
            InitializeComponent();

            var formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("allroutes.dat", FileMode.OpenOrCreate))
            {
                _allRoutes = (AllRoutes)formatter.Deserialize(fs);
            }
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            _stationsLoader = new StationsLoader();
            var transportType = comboBox_TransportType.SelectedIndex + 1;
            var routeNumber = textBox_RouteNumber.Text;
            var url = $"http://www.yargortrans.ru/config.php?vt={transportType}&nmar={routeNumber}";

            _stationsLoader.Load(url);
            var lengthOfDirectRoute = _stationsLoader.CountOfStationsOnDirectRoute;

            var directRoute = new List<string>();
            var reverseRoute = new List<string>();

            for (int i = 0; i < lengthOfDirectRoute; i++)
                directRoute.Add(_stationsLoader.StationsList[i]);

            for (int i = lengthOfDirectRoute; i < _stationsLoader.StationsList.Count; i++)
                reverseRoute.Add(_stationsLoader.StationsList[i]);

            Transport _transportType;

            switch (comboBox_TransportType.SelectedItem)
            {
                case "Автобус":
                    _transportType = Transport.Bus;
                    break;
                case "Троллейбус":
                    _transportType = Transport.Trolley;
                    break;
                case "Трамвай":
                    _transportType = Transport.Tram;
                    break;
                case "Маршрутка":
                    _transportType = Transport.Minibus;
                    break;
                default:
                    _transportType = Transport.Bus;
                    break;
            }

            var route = new Route(textBox_RouteNumber.Text, _transportType);
            var routeContent = new RouteContent(directRoute, reverseRoute);

            _allRoutes.AddRoute(route, routeContent);

            var formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("allroutes.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, _allRoutes);
            }
        }
    }
}
