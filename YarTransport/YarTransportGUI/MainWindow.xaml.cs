using SearchWaySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using YarTransportGUI.TransportServiceReference;

namespace YarTransportGUI
{
    public partial class MainWindow : Window
    {
        private List<string> _stations;
        private TransportServiceClient _client;
        private List<RouteInfo> _routes;
        private FavoriteRoutes _favoriteRoutes;

        private bool _isBusChecked;
        private bool _isTrolleyChecked;
        private bool _isTramChecked;
        private bool _isMiniBusChecked;

        public MainWindow()
        {
            InitializeComponent();

            _client = new TransportServiceClient();
            _stations = _client.GetStations().ToList();
            InitFavoriteRoutes();
            InitPopups(TB_PointOfDeparture, Popup_StationsOfDeparture, LB_StationsOfDeparture);
            InitPopups(TB_PointOfDestination, Popup_StationsOfDestination, LB_StationsOfDestination);
        }     

        private void InitFavoriteRoutes()
        {
            var formatter = new BinaryFormatter();

            using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"data\favoriteroutes.dat", FileMode.OpenOrCreate))
            {
                _favoriteRoutes = (FavoriteRoutes)formatter.Deserialize(fs);
            }

            foreach (var favorite in _favoriteRoutes.FavoriteRoutesList)
                LB_Favorite.Items.Add($"{favorite.RouteName}");

            if (_favoriteRoutes.FavoriteRoutesList.Count == 0)
                LB_Favorite.Visibility = Visibility.Collapsed;
        }

        private void InitPopups(TextBox textBox, Popup popup, ListBox listBox)
        {
            textBox.PreviewKeyUp += (sender, e) =>
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

        private void Btn_Search_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LB_Routes.Items.Clear();

            var stationOfDeparture = TB_PointOfDeparture.Text;
            var stationOfDestination = TB_PointOfDestination.Text;

            if (IsConnectedToInternet())
            {
                if (_stations.Contains(stationOfDeparture) && _stations.Contains(stationOfDestination))
                {
                    _isBusChecked = CB_Bus.IsChecked ?? false;
                    _isTrolleyChecked = CB_Trolley.IsChecked ?? false;
                    _isTramChecked = CB_Tram.IsChecked ?? false;
                    _isMiniBusChecked = CB_MiniBus.IsChecked ?? false;

                    _routes = _client.GetRoutes(stationOfDeparture, stationOfDestination, _isBusChecked, _isTrolleyChecked, _isTramChecked, _isMiniBusChecked).ToList();

                    if (_routes != null)
                        DisplayRoutes(_routes);
                    else
                        CallExceptionWindow("Не существует транспорта, следующего по заданному маршруту!");
                }
                else
                    CallExceptionWindow("Не существует остановки (остановок) с таким названием. Проверьте параметры поиска!");
            }
            else
                CallExceptionWindow("Проверьте подключение к сети Интернет!");
        }

        private void DisplayRoutes(List<RouteInfo> routes)
        {
            LB_Routes.Items.Clear();

            if (routes.Count > 0)
            {
                foreach (var route in routes)
                    LB_Routes.Items.Add($"{route.ToString()}");
            }
            else
            {
                if (!_isBusChecked && !_isMiniBusChecked && !_isTramChecked && !_isTrolleyChecked)
                    CallExceptionWindow("Не выбран ни один вид транспорта для поиска!");
                else
                    CallExceptionWindow("В данный момент на линии нет транспорта, следующего по заданному маршруту!");
            }
        }

        private void LB_Routes_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var route = _routes[LB_Routes.SelectedIndex];

            Grid_MainWindow.Visibility = Visibility.Collapsed;
            Grid_RouteInfo.Visibility = Visibility.Visible;

            TB_RouteInfo.Clear();
            TB_RouteInfo.AppendText(route.GetRouteInfo());
        }

        private void Btn_AddFavorite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var sw = new SaveWindow();
            sw.Owner = this;

            if (sw.ShowDialog() == true)
                sw.Show();

            if (sw.RouteName != "" && sw.RouteName!= null)
            {
                var routeName = sw.RouteName;
                var item = (from f in _favoriteRoutes.FavoriteRoutesList where f.RouteName == routeName select f).FirstOrDefault();

                if (item == null)
                {
                    _favoriteRoutes.Add(routeName, TB_PointOfDeparture.Text, TB_PointOfDestination.Text);
                    LB_Favorite.Items.Add($"{routeName}");
                    LB_Favorite.Visibility = Visibility.Visible;

                    SerializeFavoriteRoutes();
                }
                else
                    CallExceptionWindow("Маршрут с таким названием уже существует!");
            }
        }

        private void Btn_RemoveFavorite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LB_Favorite.SelectedIndex != -1)
            {
                var route = _favoriteRoutes.FavoriteRoutesList[LB_Favorite.SelectedIndex];
                _favoriteRoutes.Remove(route.RouteName);
                LB_Favorite.Items.Remove(LB_Favorite.Items[LB_Favorite.SelectedIndex]);
                SerializeFavoriteRoutes();

                if (LB_Favorite.Items.Count == 0)
                    LB_Favorite.Visibility = Visibility.Collapsed;
            }
        }

        private void SerializeFavoriteRoutes()
        {
            using (var fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"data\favoriteroutes.dat", FileMode.OpenOrCreate))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, _favoriteRoutes);
            }
        }

        private void LB_Favorite_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TB_PointOfDeparture.Text = _favoriteRoutes.FavoriteRoutesList[LB_Favorite.SelectedIndex].PointOfDeparture;
            TB_PointOfDestination.Text = _favoriteRoutes.FavoriteRoutesList[LB_Favorite.SelectedIndex].PointOfDestination;
        }

        private void Btn_Clear_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TB_PointOfDeparture.Clear();
            TB_PointOfDestination.Clear();
            LB_Routes.Items.Clear();
        }

        private void Btn_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid_RouteInfo.Visibility = Visibility.Collapsed;
            Grid_MainWindow.Visibility = Visibility.Visible;
        }

        private void CallExceptionWindow(string message)
        {
            var ew = new ExceptionWindow(message);
            ew.Owner = this;

            if (ew.ShowDialog() == true)
                ew.Show();
        }

        private bool IsConnectedToInternet()
        {
            IPStatus status = IPStatus.Unknown;

            try
            {
                status = new Ping().Send("ot76.ru").Status;
            }
            catch { }

            if (status == IPStatus.Success)
                return true;
            else
                return false; 
    
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _client.Close();
        }
    }
}
