using ModernWpf;
using System.Windows;
using System.Windows.Media;

namespace AuthServer.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ThemeManager.Current.AccentColor = (Color)ColorConverter.ConvertFromString("#0078D7");
        }
    }
}