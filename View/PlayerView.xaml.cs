using System.Windows;
using System.Windows.Controls;

namespace AudioFil
{
    /// <summary>
    /// Logika interakcji dla klasy RadioViewModel.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        private Window parent;

        public PlayerView()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MaximizedBtn_Click(object sender, RoutedEventArgs e)
        {
            parent = Window.GetWindow(this);

            if (parent.WindowState == WindowState.Normal)
            {
                parent.WindowState = WindowState.Maximized;
            }
            else
            {
                parent.WindowState = WindowState.Normal;
            }
        }

        private void MinimizedBtn_Click(object sender, RoutedEventArgs e)
        {
            parent = Window.GetWindow(this);

            parent.WindowState = WindowState.Minimized;
        }
    }
}
