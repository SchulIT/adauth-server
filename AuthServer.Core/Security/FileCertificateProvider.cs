using AuthServer.Core.Settings;
using System.Security.Cryptography.X509Certificates;

namespace AuthServer.Core.Security
{
    /// <summary>
    /// Reads a TLS certificate from the filesystem
    /// </summary>
    public class FileCertificateProvider : ICertificateProvider
    {
        private X509Certificate2 certificate;

        private ISettings settings;

        public FileCertificateProvider(ISettings settings)
        {
            this.settings = settings;
        }

        public X509Certificate2 GetCertificate()
        {
            if(certificate != null)
            {
                return certificate;
            }

            return certificate = new X509Certificate2(settings.Tls.Certificate, settings.Tls.PreSharedKey);
        }
    }
}
