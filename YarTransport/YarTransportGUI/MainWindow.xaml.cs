using System.Collections.Generic;
using System.Linq;
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

        public MainWindow()
        {
            InitializeComponent();

            StationsLoader.Load();
            _stations = StationsLoader.StationsList;

            Init(TB_PointOfDeparture, Popup_StationsOfDeparture, LB_StationsOfDeparture);
            Init(TB_PointOfDestination, Popup_StationsOfDestination, LB_StationsOfDestination);
        }

        private void Init(TextBox textBox, Popup popup, ListBox listBox)
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

        private void Btn_Where7_Click(object sender, RoutedEventArgs e)
        {
            Locator transportLocator = new Locator();
            TB_Locator.Clear();
            TB_Locator.Text = transportLocator.Locate(8);
        }
    }
}
