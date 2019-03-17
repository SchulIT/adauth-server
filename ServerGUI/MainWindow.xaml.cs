using ServerGUI.ViewModel;
using System.Windows;

namespace ServerGUI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var locator = App.Current.Resources["Locator"] as ViewModelLocator;

            locator.Main.LoadLicenses();

            await locator.Status.CheckAsync();
            await locator.Settings.LoadSettingsAsync();
        }
    }
}
