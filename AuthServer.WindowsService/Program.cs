using AuthServer.Core.Handler;
using AuthServer.Core.Network;
using AuthServer.Core.Performance;
using AuthServer.Core.Protocol;
using AuthServer.Core.Provider;
using AuthServer.Core.Security;
using AuthServer.Core.Server;
using AuthServer.Core.Settings;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthServer.WindowsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
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

                    builder.RegisterType<Worker>().AsSelf();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
