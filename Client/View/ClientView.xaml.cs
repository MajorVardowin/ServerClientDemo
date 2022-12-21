using Client.ViewModel;

namespace Client.View
{
    /// <summary>
    /// Interaktionslogik für ClientView.xaml
    /// </summary>
    public partial class ClientView
    {
        public ClientView()
        {
            InitializeComponent();
            DataContext = new ClientViewModel();
        }
    }
}
