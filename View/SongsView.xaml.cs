using System.Windows.Controls;

namespace AudioFil
{
    /// <summary>
    /// Logika interakcji dla klasy SongsView.xaml
    /// </summary>
    public partial class SongsView : UserControl
    {
        public SongsView()
        {
            InitializeComponent();

            InitPlayer();
        }

        private void InitPlayer()
        {
            PlayerView player = new PlayerView();
            player.DataContext = DataContext;

            GridPlayer.Children.Add(player);
        }
    }
}
