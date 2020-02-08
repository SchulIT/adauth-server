using System.Security.Cryptography.X509Certificates;

namespace AuthServer.Client
{
    public class ClientResponse
    {
        private readonly string response;
        private readonly X509Certificate2 certificate;

        public string Response { get { return response; } }
        public X509Certificate2 Certificate { get { return certificate; } }

        public ClientResponse(string response, X509Certificate2 certificate = null)
        {
            this.response = response;
            this.certificate = certificate;
        }
    }
}
