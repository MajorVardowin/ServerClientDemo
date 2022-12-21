using System.Windows;
using Client.ViewModel;

namespace ServerClientDemo.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var client = new ClientViewModel();
            client.WriteToServer();
        }
    }
}
