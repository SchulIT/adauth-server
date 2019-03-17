using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ServerGUI.Services.Client;
using ServerGUI.Services.Dialogs;
using System;
using System.Security.Cryptography.X509Certificates;

namespace ServerGUI.ViewModel
{
    public class ClientViewModel : ViewModelBase
    {
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { Set(() => IsBusy, ref isBusy, value); }
        }

        private string hostname = "127.0.0.1";

        public string Hostname
        {
            get { return hostname; }
            set
            {
                Set(() => Hostname, ref hostname, value);

                RequestCommand?.RaiseCanExecuteChanged();
            }
        }

        private int port = 55117;

        public int Port
        {
            get { return port; }
            set
            {
                Set(() => Port, ref port, value);

                RequestCommand?.RaiseCanExecuteChanged();
            }
        }

        private bool useTls;

        public bool UseTls
        {
            get { return useTls; }
            set { Set(() => UseTls, ref useTls, value); }
        }

        private string request;

        public string Request
        {
            get { return request; }
            set
            {
                Set(() => Request, ref request, value);

                RequestCommand?.RaiseCanExecuteChanged();
            }
        }

        private string response;

        public string Response
        {
            get { return response; }
            set { Set(() => Response, ref response, value); }
        }

        private X509Certificate2 certificate;

        public X509Certificate2 Certificate
        {
            get { return certificate; }
            set { Set(() => Certificate, ref certificate, value); }
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
                var response = await client.RequestAsync(Hostname, Port, UseTls, Request);

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
