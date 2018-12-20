using System.Windows;

namespace YarTransportGUI
{
    /// <summary>
    /// Логика взаимодействия для SaveWindow.xaml
    /// </summary>
    public partial class ExceptionWindow : Window
    {
        public ExceptionWindow(string message)
        {
            InitializeComponent();

            TB_Exception.Text = message;
        }
    }
}
