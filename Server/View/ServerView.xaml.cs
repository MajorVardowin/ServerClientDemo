using Server.ViewModel;

namespace Server.View
{
    /// <summary>
    /// Interaktionslogik für ServerView.xaml
    /// </summary>
    public partial class ServerView
    {
        public ServerView()
        {
            DataContext = new ServerViewModel();
            InitializeComponent();
        }
    }
}
