using AuthServer.Core.Settings;
using AuthServer.GUI.Settings;
using AuthServer.GUI.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AuthServer.GUI.ViewModels
{
    public class SettingsViewModel : ObservableRecipient
    {
        private bool isBusy = false;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private readonly MemorySettings settings = new MemorySettings();

        public MemorySettings Settings
        {
            get { return settings; }
        }

        private X509Certificate2 certificate;

        public X509Certificate2 Certificate
        {
            get { return certificate; }
            set { SetProperty(ref certificate, value); }
        }
        
        public RelayCommand SaveCommand { get; }
        public RelayCommand ResetCommand { get; }

        public RelayCommand TestCommand { get; }

        public RelayCommand BrowseCommand { get; }

        private readonly ISettingsWriter settingsWriter;
        private readonly ISettingsReader settingsReader;
        private readonly IDialogHelper dialogHelper;

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
                (Settings.Tls as MemorySettings.MemoryTlsSettings).Certificate = filename;
                OnPropertyChanged();
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
                PopulateSettings(new MemorySettings());
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

            var ldapSettings = (Settings.Ldap as MemorySettings.MemoryLdapSettings);

            ldapSettings.Server = storedSettings.Ldap.Server;
            ldapSettings.Port = storedSettings.Ldap.Port;
            ldapSettings.UseSSL = storedSettings.Ldap.UseSSL;
            ldapSettings.UseTLS = storedSettings.Ldap.UseTLS;
            ldapSettings.DomainFQDN = storedSettings.Ldap.DomainFQDN;
            ldapSettings.DomainNetBIOS = storedSettings.Ldap.DomainNetBIOS;
            ldapSettings.Password = storedSettings.Ldap.Password;
            ldapSettings.Username = storedSettings.Ldap.Username;
            ldapSettings.CertificateThumbprint = storedSettings.Ldap.CertificateThumbprint;

            var serverSettings = (Settings.Server as MemorySettings.MemoryServerSettings);
            serverSettings.IPv6 = storedSettings.Server.IPv6;
            serverSettings.Port = storedSettings.Server.Port;

            var tlsSettings = (Settings.Tls as MemorySettings.MemoryTlsSettings);
            tlsSettings.Certificate = storedSettings.Tls.Certificate;
            tlsSettings.PreSharedKey = storedSettings.Tls.PreSharedKey;

            OnPropertyChanged();
        }

        public class MemorySettings : ISettings
        {
            public IServerSettings Server { get; } = new MemoryServerSettings();

            public ITlsSettings Tls { get; } = new MemoryTlsSettings();

            public ILdapSettings Ldap { get; } = new MemoryLdapSettings();

            public string UniqueIdAttributeName { get; set; }

            public class MemoryServerSettings : IServerSettings
            {
                public int Port { get; set; } = 55117;

                public bool IPv6 { get; set; } = false;
            }

            public class MemoryTlsSettings : ITlsSettings
            {
                public string Certificate { get; set; }

                public string PreSharedKey { get; set; }
            }

            public class MemoryLdapSettings : ILdapSettings
            {
                public string Server { get; set; }

                public int Port { get; set; } = 389;

                public bool UseSSL { get; set; } = false;

                public bool UseTLS { get; set; } = true;

                public string DomainFQDN { get; set; }

                public string DomainNetBIOS { get; set; }

                public string Username { get; set; }

                public string Password { get; set; }

                public string CertificateThumbprint { get; set; }

                public UsernameProperty UsernameProperty { get; set; } = UsernameProperty.sAMAccountName;

                public string[] AllowedUpnSuffixes { get; set; } = Array.Empty<string>();
            }
        }
    }
}
