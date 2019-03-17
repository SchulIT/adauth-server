using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ServerCore.Settings;
using ServerGUI.Model;
using ServerGUI.Services.Dialogs;
using ServerGUI.Services.Settings;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ServerGUI.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private bool isBusy = false;

        public bool IsBusy
        {
            get { return isBusy; }
            set { Set(() => IsBusy, ref isBusy, value); }
        }

        private readonly Settings settings = new Settings();

        public Settings Settings
        {
            get { return settings; }
        }

        private X509Certificate2 certificate;

        public X509Certificate2 Certificate
        {
            get { return certificate; }
            set { Set(() => Certificate, ref certificate, value); }
        }
        
        public RelayCommand SaveCommand { get; }
        public RelayCommand ResetCommand { get; }

        public RelayCommand TestCommand { get; }

        public RelayCommand BrowseCommand { get; }

        private ISettingsWriter settingsWriter;
        private ISettingsReader settingsReader;
        private IDialogHelper dialogHelper;

        public SettingsViewModel(ISettingsReader settingsReader, ISettingsWriter settingsWriter, IDialogHelper dialogHelper)
        {
            this.settingsReader = settingsReader;
            this.settingsWriter = settingsWriter;
            this.dialogHelper = dialogHelper;

            SaveCommand = new RelayCommand(Save, CanSave);
            ResetCommand = new RelayCommand(Reset);

            TestCommand = new RelayCommand(TestCertificate);
            BrowseCommand = new RelayCommand(BrowseCertificate);
        }

        private void TestCertificate()
        {
            try
            {
                LoadCertificate(Settings);
            }
            catch(Exception e)
            {
                dialogHelper.ShowException(e);
            }
        }

        private void BrowseCertificate()
        {
            var filename = dialogHelper.BrowseFile();

            if(filename != null)
            {
                Settings.Certificate = filename;
            }
        }

        private async void Reset()
        {
            await LoadSettingsAsync();
        }

        private async void Save()
        {
            IsBusy = true;

            try
            {
                await settingsWriter.WriteAsync(Settings);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanSave() => settings != null;

        private void LoadCertificate(ISettings settings)
        {
            if (!string.IsNullOrEmpty(settings.Tls.Certificate) && !string.IsNullOrEmpty(settings.Tls.PreSharedKey) && File.Exists(settings.Tls.Certificate))
            {
                try
                {
                    Certificate = new X509Certificate2(settings.Tls.Certificate, settings.Tls.PreSharedKey);
                }
                catch (Exception e)
                {
                    dialogHelper.ShowException(e);
                }
            }
            else
            {
                Certificate = null;
            }
        }

        public async Task LoadSettingsAsync()
        {
            IsBusy = true;

            try
            {
                var settings = await settingsReader.ReadAsync();
                PopulateSettings(settings);
                LoadCertificate(settings);
            }
            catch (FileNotFoundException)
            {
                PopulateSettings(new Settings());
            }
            catch(Exception e)
            {
                dialogHelper.ShowException(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void PopulateSettings(ISettings storedSettings)
        {
            if(storedSettings == null)
            {
                return;
            }

            Settings.UniqueIdAttributeName = storedSettings.UniqueIdAttributeName;

            Settings.DC = storedSettings.Ldap.DC;
            Settings.Domain = storedSettings.Ldap.Domain;
            Settings.Password = storedSettings.Ldap.Password;
            Settings.Username = storedSettings.Ldap.Username;
            
            Settings.IPv6 = storedSettings.Server.IPv6;
            Settings.Port = storedSettings.Server.Port;
            
            Settings.Certificate = storedSettings.Tls.Certificate;
            Settings.IsEnabled = storedSettings.Tls.IsEnabled;
            Settings.PreSharedKey = storedSettings.Tls.PreSharedKey;
        }
    }
}
