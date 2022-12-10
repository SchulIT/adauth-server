using AuthServer.Client;
using AuthServer.GUI.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Security.Cryptography.X509Certificates;

namespace AuthServer.GUI.ViewModels
{
    public class ClientViewModel : ObservableRecipient
    {
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private string hostname = "127.0.0.1";

        public string Hostname
        {
            get { return hostname; }
            set
            {
                SetProperty(ref hostname, value);

                RequestCommand?.NotifyCanExecuteChanged();
            }
        }

        private int port = 55117;

        public int Port
        {
            get { return port; }
            set
            {
                SetProperty(ref port, value);

                RequestCommand?.NotifyCanExecuteChanged();
            }
        }

        private string request;

        public string Request
        {
            get { return request; }
            set
            {
                SetProperty<string>(ref request, value);

                RequestCommand?.NotifyCanExecuteChanged();
            }
        }

        private string response;

        public string Response
        {
            get { return response; }
            set { SetProperty(ref response, value); }
        }

        private X509Certificate2 certificate;

        public X509Certificate2 Certificate
        {
            get { return certificate; }
            set { SetProperty(ref certificate, value); }
        }

        public RelayCommand RequestCommand { get; }

        private IClient client;
        private IDialogHelper dialogHelper;

        public ClientViewModel(IClient client, IDialogHelper dialogHelper)
        {
            this.client = client;
            this.dialogHelper = dialogHelper;

            RequestCommand = new RelayCommand(DoRequest, CanRequest);
        }

        private async void DoRequest()
        {
            IsBusy = true;

            try
            {
                var response = await client.RequestAsync(Hostname, Port, Request);

                Response = response.Response;
                Certificate = response.Certificate;
            }
            catch (Exception e)
            {
                dialogHelper.ShowException(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanRequest() => !string.IsNullOrEmpty(Hostname) && Port > 0 && !string.IsNullOrEmpty(Request);
    }
}
