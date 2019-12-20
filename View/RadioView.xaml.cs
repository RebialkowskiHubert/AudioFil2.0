using System.Windows.Controls;

namespace AudioFil
{
    /// <summary>
    /// Logika interakcji dla klasy RadioView.xaml
    /// </summary>
    public partial class RadioView : UserControl
    {
        public RadioView()
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
