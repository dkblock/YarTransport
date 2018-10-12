using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TransportLibrary;
using System.IO;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Runtime.Serialization.Formatters.Binary;

namespace GMap
{
    public partial class Form1 : Form
    {
        private StationsLoader _stationsLoader;
        private AllStations _allStations;
        private RouteMatrix _routeMatrix;
        private List<string> _namesOfStationsOnCurrentRoute;
        private List<string> _currentDirectRoute;
        private List<string> _currentReverseRoute;
        private int _lengthOfDirectRoute;
        private Route _currentRoute;
        private int _currentStationOnCurrentRoute;
        private Metrika _metrika;
        private GMapOverlay _markersOverlay;

        public Form1()
        {
            InitializeComponent();
            _allStations = new AllStations();
            _routeMatrix = new RouteMatrix();
            _namesOfStationsOnCurrentRoute = new List<string>();
            _currentDirectRoute = new List<string>();
            _currentReverseRoute = new List<string>();
            _currentStationOnCurrentRoute = 0;
            _metrika = new Metrika();
            _markersOverlay = new GMapOverlay(gMapControl1, "marker");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gMapControl1.Bearing = 0;

            gMapControl1.CanDragMap = true;

            gMapControl1.DragButton = MouseButtons.Left;

            gMapControl1.GrayScaleMode = true;

            gMapControl1.MarkersEnabled = true;

            gMapControl1.MaxZoom = 18;

            gMapControl1.MinZoom = 2;

            gMapControl1.MouseWheelZoomType =
                GMap.NET.MouseWheelZoomType.MousePositionAndCenter;

            gMapControl1.NegativeMode = false;

            gMapControl1.PolygonsEnabled = true;

            gMapControl1.RoutesEnabled = true;

            gMapControl1.ShowTileGridLines = false;

            gMapControl1.Zoom = 16;

            gMapControl1.Dock = DockStyle.Fill;

            gMapControl1.MapProvider =
                GMapProviders.OpenStreetMap;            // вместо гугл мапс можно заюзать любую, почекай если хош, там их много

            GMap.NET.GMaps.Instance.Mode =
                GMap.NET.AccessMode.ServerOnly;

            GMapProvider.WebProxy =
                System.Net.WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials =
                System.Net.CredentialCache.DefaultCredentials;

            gMapControl1.Position = new GMap.NET.PointLatLng(57.687079, 39.781152);
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            var lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

            if (_currentStationOnCurrentRoute < _namesOfStationsOnCurrentRoute.Count)
            {
                var station = new Station(_namesOfStationsOnCurrentRoute[_currentStationOnCurrentRoute], new List<Location>() { new Location(lat, lng) });
                _allStations.AddStation(station);

                AddMarker(lat, lng, station);
            }

            AddStationToRoute();
        }


        private void gMapControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
                AddStationToRoute();
        }

        private void AddStationToRoute()
        {
            if (_currentStationOnCurrentRoute < _namesOfStationsOnCurrentRoute.Count)
            {
                if (_currentStationOnCurrentRoute < _lengthOfDirectRoute)
                    _currentDirectRoute.Add(_namesOfStationsOnCurrentRoute[_currentStationOnCurrentRoute]);
                else
                    _currentReverseRoute.Add(_namesOfStationsOnCurrentRoute[_currentStationOnCurrentRoute]);

                _currentStationOnCurrentRoute++;

                if (_currentStationOnCurrentRoute < _namesOfStationsOnCurrentRoute.Count)
                    richTextBox1.AppendText($"{_namesOfStationsOnCurrentRoute[_currentStationOnCurrentRoute]}\n");
                else
                    richTextBox1.AppendText("Заканчивай!\n");
            }
        }

        private void AddMarker(double lat, double lng, Station station)
        {
            var marker = new GMapMarkerGoogleGreen(new NET.PointLatLng(lat, lng));
            marker.ToolTip = new GMapRoundedToolTip(marker);
            marker.ToolTipText = station.StationName;
            _markersOverlay.Markers.Add(marker);
            gMapControl1.Overlays.Add(_markersOverlay);
        }

        private void Btn_StartRoute_Click(object sender, EventArgs e)
        {
            _stationsLoader = new StationsLoader();
            var transportType = comboBox_TransportType.SelectedIndex + 1;
            var routeNumber = textBox_RouteNumber.Text;
            var url = $"http://www.yargortrans.ru/config.php?vt={transportType}&nmar={routeNumber}";

            var formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("allstations.dat", FileMode.OpenOrCreate))
            {
                _allStations = (AllStations)formatter.Deserialize(fs);
            }

            using (FileStream fs = new FileStream("routematrix.dat", FileMode.OpenOrCreate))
            {
                _routeMatrix = (RouteMatrix)formatter.Deserialize(fs);
            }

            SetMarkers();
            _stationsLoader.Load(url);
            _namesOfStationsOnCurrentRoute = _stationsLoader.StationsList;
            _lengthOfDirectRoute = _stationsLoader.CountOfStationsOnDirectRoute;

            richTextBox1.AppendText($"{_namesOfStationsOnCurrentRoute[_currentStationOnCurrentRoute]}\n");
        }

        private void SetMarkers()
        {
            _markersOverlay.Markers.Clear();

            for (int i = 0; i < _allStations.Count; i++)
            {
                var station = _allStations.GetStation(i);

                foreach (var location in station.StationLocations)
                    AddMarker(location.Latitude, location.Longitude, station);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void Btn_EndRoute_Click(object sender, EventArgs e)
        {
            Transport transportType;

            switch (comboBox_TransportType.SelectedItem)
            {
                case "Автобус":
                    transportType = Transport.Bus;
                    break;
                case "Троллейбус":
                    transportType = Transport.Trolley;
                    break;
                case "Трамвай":
                    transportType = Transport.Tram;
                    break;
                case "Маршрутка":
                    transportType = Transport.Minibus;
                    break;
                default:
                    transportType = Transport.Bus;
                    break;
            }

            _currentRoute = new Route(textBox_RouteNumber.Text, transportType);
            _routeMatrix.AddRoute(_currentRoute, _currentDirectRoute, _currentReverseRoute, _allStations);

            var formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("allstations.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, _allStations);
            }

            using (FileStream fs = new FileStream("routematrix.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, _routeMatrix);
            }

            _currentStationOnCurrentRoute = 0;
            _namesOfStationsOnCurrentRoute.Clear();
            _currentDirectRoute.Clear();
            _currentReverseRoute.Clear();
            richTextBox1.Clear();
            textBox_RouteNumber.Clear();
        }
    }
}
