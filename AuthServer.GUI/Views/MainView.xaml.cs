using ModernWpf.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace AuthServer.GUI.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            navigationView.SelectedItem = navigationView.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as NavigationViewItem;
            if(selectedItem == null)
            {
                return;
            }

            var targetPage = selectedItem.Tag switch
            {
                "status" => typeof(StatusPage),
                "client" => typeof(ClientPage),
                "settings" => typeof(SettingsPage),
                "about" => typeof(AboutPage),
                _ => null
            };

            if(targetPage != null)
            {
                try
                {
                    frame.Navigate(targetPage);
                } catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
