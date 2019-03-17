using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace ServerGUI
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
