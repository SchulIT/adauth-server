using ServerCore.Auth;
using ServerCore.Handler;
using ServerCore.Log;
using ServerCore.Network;
using ServerCore.Performance;
using ServerCore.Security;
using ServerCore.Server;
using ServerCore.Settings;

namespace ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var settings = JsonSettings.LoadSettings();
            var handler = new JsonRequestHandler(new ActiveDirectoryAuthenticationService(settings), settings);
            
            INetworkTransport transport = new UnencryptedTransport();

            if(settings.Tls != null && settings.Tls.IsEnabled)
            {
                var certificateProvider = new FileCertificateProvider(settings);
                transport = new TlsTransport(certificateProvider, logger);
            }

            var server = new NetworkServer(handler, settings, transport, new ConsolePerformanceMonitor(), logger);
            server.Start();
            while (true) { }
        }
    }
}
