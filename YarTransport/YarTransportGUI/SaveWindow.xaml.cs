using System.Windows;

namespace YarTransportGUI
{
    /// <summary>
    /// Логика взаимодействия для SaveWindow.xaml
    /// </summary>
    public partial class SaveWindow : Window
    {
        public string RouteName { get; private set; }

        public SaveWindow()
        {
            InitializeComponent();
        }

        private void Btn_Back_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RouteName = "";
            Close();
        }

        private void Btn_Forward_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RouteName = TB_RouteName.Text;

            if (RouteName != "")
                Close();
        }
    }
}
