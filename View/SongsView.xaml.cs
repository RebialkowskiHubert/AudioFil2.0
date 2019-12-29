using System.Windows.Controls;

namespace AudioFil
{
    /// <summary>
    /// Logika interakcji dla klasy SongsView.xaml
    /// </summary>
    public partial class SongsView : UserControl
    {
        private SongsViewModel viewModel;

        public SongsView()
        {
            InitializeComponent();
            InitPlayer();

            viewModel = (SongsViewModel)DataContext;
        }

        private void InitPlayer()
        {
            PlayerView player = new PlayerView();
            player.DataContext = DataContext;

            GridPlayer.Children.Add(player);
        }

        private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            viewModel.StartDrag();
        }

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            viewModel.StopDrag();
        }

        private void Slider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            viewModel.ValueChange();
        }
    }
}
