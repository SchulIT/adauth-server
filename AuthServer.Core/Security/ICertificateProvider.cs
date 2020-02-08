using System.Security.Cryptography.X509Certificates;

namespace AuthServer.Core.Security
{
    /// <summary>
    /// Interface for providing TLS certificates
    /// </summary>
    public interface ICertificateProvider
    {
        X509Certificate2 GetCertificate();
    }
}
