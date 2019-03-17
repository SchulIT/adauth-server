/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ServerGUI"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using ServerGUI.Services.Client;
using ServerGUI.Services.Dialogs;
using ServerGUI.Services.License;
using ServerGUI.Services.Service;
using ServerGUI.Services.Settings;

namespace ServerGUI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<IMessenger, Messenger>();
            SimpleIoc.Default.Register<ISettingsReader, FileSettingsReader>();
            SimpleIoc.Default.Register<ISettingsWriter, FileSettingsWriter>();
            SimpleIoc.Default.Register<IDialogHelper, DialogHelper>();
            SimpleIoc.Default.Register<IServiceHelper, ServiceHelper>();
            SimpleIoc.Default.Register<IClient, Client>();
            SimpleIoc.Default.Register<ILicenseLoader, FileLicenseLoader>();

            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<StatusViewModel>();
            SimpleIoc.Default.Register<ClientViewModel>();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get { return SimpleIoc.Default.GetInstance<MainViewModel>(); }
        }

        public SettingsViewModel Settings
        {
            get { return SimpleIoc.Default.GetInstance<SettingsViewModel>(); }
        }

        public StatusViewModel Status
        {
            get { return SimpleIoc.Default.GetInstance<StatusViewModel>(); }
        }

        public ClientViewModel Client
        {
            get { return SimpleIoc.Default.GetInstance<ClientViewModel>(); }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}