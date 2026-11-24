using System.Windows;
using System.Windows.Input;

namespace AirlineClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
