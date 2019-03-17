using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ServerGUI.Services.Client
{
    public class Client : IClient
    {
        public async Task<ClientResponse> RequestAsync(string hostname, int port, bool useTls, string request)
        {
            using (var client = new TcpClient())
            {
                client.Connect(hostname, port);
                Stream stream = client.GetStream();
                X509Certificate2 certificate = null;

                if (useTls)
                {
                    var sslStream = new SslStream(client.GetStream(), true, OnRemoteCertificateValidation);
                    await sslStream.AuthenticateAsClientAsync(hostname, null, System.Security.Authentication.SslProtocols.Tls12, false);

                    certificate = new X509Certificate2(sslStream.RemoteCertificate);

                    stream = sslStream;
                }

                var response = "";

                var writer = new StreamWriter(stream);
                var reader = new StreamReader(stream);

                await writer.WriteAsync(request).ConfigureAwait(false);
                await writer.FlushAsync().ConfigureAwait(false);
                response = await reader.ReadToEndAsync().ConfigureAwait(false);

                return new ClientResponse(response, certificate);
            }
        }

        private bool OnRemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
