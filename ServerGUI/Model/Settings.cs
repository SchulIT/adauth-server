using GalaSoft.MvvmLight;
using ServerCore.Settings;

namespace ServerGUI.Model
{
    public class Settings : ObservableObject, ISettings, IServerSettings, ITlsSettings, ILdapSettings
    {
        /*
         * General
         */
        private string uniqueIdAttributeName;

        /*
         * LDAP
         */
        private string dc;
        private string domain;
        private string username;
        private string password;

        /*
         * TLS
         */
        private bool isEnabled;
        private string certificate;
        private string psk;

        /*
         * Server
         */
        private int port = 55117;
        private bool ipv6;

        public IServerSettings Server => this;

        public ITlsSettings Tls => this;

        public ILdapSettings Ldap => this;

        public string DC
        {
            get { return dc; }
            set { Set(() => DC, ref dc, value); }
        }

        public string Domain
        {
            get { return domain; }
            set { Set(() => Domain, ref domain, value); }
        }

        public string Username
        {
            get { return username; }
            set { Set(() => Username, ref username, value); }
        }

        public string Password
        {
            get { return password; }
            set { Set(() => Password, ref password, value); }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { Set(() => IsEnabled, ref isEnabled, value); }
        }

        public string Certificate
        {
            get { return certificate; }
            set { Set(() => Certificate, ref certificate, value); }
        }

        public string PreSharedKey
        {
            get { return psk; }
            set { Set(() => PreSharedKey, ref psk, value); }
        }

        public int Port
        {
            get { return port; }
            set { Set(() => Port, ref port, value); }
        }

        public bool IPv6
        {
            get { return ipv6; }
            set { Set(() => IPv6, ref ipv6, value); }
        }

        public string UniqueIdAttributeName
        {
            get { return uniqueIdAttributeName; }
            set { Set(() => UniqueIdAttributeName, ref uniqueIdAttributeName, value); }
        }
    }
}
