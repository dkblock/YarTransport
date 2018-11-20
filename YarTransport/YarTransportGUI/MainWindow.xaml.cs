using SearchWaySystem;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TransportLibrary;

namespace YarTransportGUI
{
    public partial class MainWindow : Window
    {
        private List<string> _stations;
        private Searcher _searcher;

        public MainWindow()
        {
            InitializeComponent();

            _searcher = new Searcher();
            InitStations();
            InitPopups(TB_PointOfDeparture, Popup_StationsOfDeparture, LB_StationsOfDeparture);
            InitPopups(TB_PointOfDestination, Popup_StationsOfDestination, LB_StationsOfDestination);
        }

        private void InitPopups(TextBox textBox, Popup popup, ListBox listBox)
        {
            textBox.TextChanged += (sender, e) =>
            {
                listBox.Items.Clear();
                var result = _stations.Where(x => x.ToLower().Contains(textBox.Text.ToLower())).ToArray();

                foreach (var item in result)
                    if (listBox.Items.Count < 10)
                        listBox.Items.Add(item);

                if (listBox.Items.Count > 0) popup.IsOpen = true;
                if (textBox.Text.Length == 0) popup.IsOpen = false;

            };

            textBox.LostFocus += (sender, e) =>
            {
                popup.IsOpen = false;
            };
        }

        private void InitStations()
        {
            var formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("allstations.dat", FileMode.OpenOrCreate))
            {
                var allStations = (AllStations)formatter.Deserialize(fs);
                _stations = new List<string>();

                for (int i = 0; i < allStations.Count; i++)
                    _stations.Add(allStations.GetStation(i).StationName);

            }
        }

        private void LB_StationsOfDeparture_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(LB_StationsOfDeparture, e.OriginalSource as DependencyObject) as ListBoxItem;
            TB_PointOfDeparture.Text = item.DataContext.ToString();
            Popup_StationsOfDeparture.IsOpen = false;
        }

        private void LB_StationsOfDestination_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(LB_StationsOfDestination, e.OriginalSource as DependencyObject) as ListBoxItem;
            TB_PointOfDestination.Text = item.DataContext.ToString();
            Popup_StationsOfDestination.IsOpen = false;
        }

        private void Btn_Search_Click(object sender, RoutedEventArgs e)
        {
            var stationOfDeparture = TB_PointOfDeparture.Text;
            var stationOfDestination = TB_PointOfDestination.Text;

            if (stationOfDeparture.Length > 0 && stationOfDestination.Length > 0)
            {
                var routes = _searcher.GetRoutes(stationOfDeparture, stationOfDestination);
                DisplayRoutes(routes);
            }
        }

        private void DisplayRoutes(List<RouteInfo> routes)
        {
            TB_Routes.Clear();
            TB_Routes.AppendText($"От: {TB_PointOfDeparture.Text}\n");
            TB_Routes.AppendText($"До: {TB_PointOfDestination.Text}\n\n");

            foreach (var route in routes)
                TB_Routes.AppendText($"{route.RouteType}\t \t \t \t \t {route.ArrivalTime}\n");
        }
    }
}
