using AuthServer.Client;
using AuthServer.GUI.Service;
using AuthServer.GUI.Settings;
using AuthServer.GUI.UI;
using Autofac;
using CommunityToolkit.Mvvm.Messaging;

namespace AuthServer.GUI.ViewModels
{
    public class ViewModelLocator
    {
        private static IContainer container;

        static ViewModelLocator()
        {
            RegisterServices();
        }

        public static void RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FileSettingsReader>().As<ISettingsReader>().SingleInstance();
            builder.RegisterType<FileSettingsWriter>().As<ISettingsWriter>().SingleInstance();
            builder.RegisterType<DialogHelper>().As<IDialogHelper>().SingleInstance();
            builder.RegisterType<ServiceHelper>().As<IServiceHelper>().SingleInstance();
            builder.RegisterType<Client.Client>().As<IClient>().SingleInstance();

            builder.RegisterType<UIDispatcher>().As<IDispatcher>().SingleInstance();

            builder.RegisterType<SettingsViewModel>().AsSelf().SingleInstance().OnActivated(async vm => await vm.Instance.LoadSettingsAsync());
            builder.RegisterType<StatusViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<ClientViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<AboutViewModel>().AsSelf().SingleInstance();


            container = builder.Build();
        }

        public SettingsViewModel Settings { get { return container.Resolve<SettingsViewModel>(); } }

        public ClientViewModel Client { get { return container.Resolve<ClientViewModel>(); } }

        public StatusViewModel Status { get { return container.Resolve<StatusViewModel>(); } }

        public AboutViewModel About { get { return container.Resolve<AboutViewModel>(); } }

    }
}
 