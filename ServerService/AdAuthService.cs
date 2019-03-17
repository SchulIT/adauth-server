using ServerCore.Auth;
using ServerCore.Handler;
using ServerCore.Log;
using ServerCore.Network;
using ServerCore.Performance;
using ServerCore.Security;
using ServerCore.Server;
using ServerCore.Settings;
using System.ServiceProcess;

namespace ServerService
{
    public partial class AdAuthServer : ServiceBase
    {
        private IServer server;

        public AdAuthServer()
        {
            InitializeComponent();

            var logger = new EventLogger();
            var settings = JsonSettings.LoadSettings();
            var handler = new JsonRequestHandler(new ActiveDirectoryAuthenticationService(settings), settings);
            INetworkTransport transport = new UnencryptedTransport();

            if (settings.Tls != null && settings.Tls.IsEnabled)
            {
                var certificateProvider = new FileCertificateProvider(settings);
                transport = new TlsTransport(certificateProvider, logger);
            }

            var performance = new WindowsPerformanceCounter(logger);

            server = new NetworkServer(handler, settings, transport, performance, logger);
            server.Start();
        }

        protected override void OnStart(string[] args)
        {
            server.Start();
        }

        protected override void OnStop()
        {
            server.Stop();
        }
    }
}
