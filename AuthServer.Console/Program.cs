using AuthServer.Core.Provider;
using AuthServer.Core.Handler;
using AuthServer.Core.Network;
using AuthServer.Core.Performance;
using AuthServer.Core.Security;
using AuthServer.Core.Server;
using AuthServer.Core.Settings;
using Autofac;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using AuthServer.Core.Protocol;

namespace AuthServer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            var settings = JsonSettings.LoadSettings();

            builder.Register(c => settings).As<ISettings>();
            builder.RegisterType<LdapProvider>().As<IUserProvider>();
            builder.RegisterType<JsonProtocol>().As<IProtocol>();
            builder.RegisterType<EventCountersPerformanceMonitor>().As<IPerformanceMonitor>();
            builder.RegisterType<FileCertificateProvider>().As<ICertificateProvider>();
            builder.RegisterType<NetworkServer>().As<IServer>();
            builder.RegisterType<TlsTransport>().As<INetworkTransport>();

            builder.RegisterAssemblyTypes(typeof(IRequestHandler).Assembly)
                .Where(x => x.IsAssignableTo<IRequestHandler>())
                .AsImplementedInterfaces();
            builder.RegisterType<ProtocolHandler>().As<IProtocolHandler>().SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            builder.RegisterType<NLogLoggerFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

            var container = builder.Build();
            var server = container.Resolve<IServer>();

            server.Start();
            while (true) { }
        }
    }
}
