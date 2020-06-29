using AuthServer.GUI.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace AuthServer.GUI.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var locator = App.Current.Resources["ViewModelLocator"] as ViewModelLocator;
            var statusViewModel = locator.Status;

            statusViewModel.CheckCommand.Execute(null);
        }

        private void OnRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
