using System.Security.Cryptography.X509Certificates;

namespace ServerCore.Security
{
    /// <summary>
    /// Interface for providing TLS certificates
    /// </summary>
    public interface ICertificateProvider
    {
        X509Certificate2 GetCertificate();
    }
}
