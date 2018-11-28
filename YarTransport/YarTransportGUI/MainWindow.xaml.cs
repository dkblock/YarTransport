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
        private List<RouteInfo> _routes;

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
                var isBusChecked = CB_Bus.IsChecked ?? false;
                var isTrolleyChecked = CB_Trolley.IsChecked ?? false;
                var isTramChecked = CB_Tram.IsChecked ?? false;

                _routes = _searcher.GetRoutes(stationOfDeparture, stationOfDestination, isBusChecked, isTrolleyChecked, isTramChecked);
                DisplayRoutes(_routes);
            }
        }

        private void DisplayRoutes(List<RouteInfo> routes)
        {
            LB_Routes.Items.Clear();

            foreach (var route in routes)
                LB_Routes.Items.Add($"{route.RouteType}\t\t\t{route.ArrivalTime}");
        }

        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            Grid_RouteInfo.Visibility = Visibility.Collapsed;
            Grid_MainWindow.Visibility = Visibility.Visible;
        }

        private void LB_Routes_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var route = _routes[LB_Routes.SelectedIndex];

            Grid_MainWindow.Visibility = Visibility.Collapsed;
            Grid_RouteInfo.Visibility = Visibility.Visible;

            TB_RouteInfo.Clear();
            TB_RouteInfo.AppendText($"{route.RouteType}\n");
            TB_RouteInfo.AppendText($"{route.TransportModel}\n\n");

            foreach (var node in route.Schedule)
                TB_RouteInfo.AppendText($"{node.ToString()}\n");
        }
    }
}
