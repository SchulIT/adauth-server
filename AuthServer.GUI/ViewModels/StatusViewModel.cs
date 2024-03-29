﻿using AuthServer.GUI.Service;
using AuthServer.GUI.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AuthServer.GUI.ViewModels
{
    public class StatusViewModel : ObservableRecipient
    {
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        
        private bool isServiceInstalled;

        public bool IsServiceInstalled
        {
            get { return isServiceInstalled; }
            set
            {
                SetProperty(ref isServiceInstalled, value);

                StartServiceCommand?.NotifyCanExecuteChanged();
                StopServiceCommand?.NotifyCanExecuteChanged();
            }
        }

        private bool isServiceRunning;

        public bool IsServiceRunning
        {
            get { return isServiceRunning; }
            set
            {
                SetProperty(ref isServiceRunning, value);

                StartServiceCommand?.NotifyCanExecuteChanged();
                StopServiceCommand?.NotifyCanExecuteChanged();
            }
        }

        private bool isServerRunning;

        public bool IsServerRunning
        {
            get { return isServerRunning; }
            set
            {
                SetProperty(ref isServerRunning, value);    

                StartServiceCommand?.NotifyCanExecuteChanged();
                StopServiceCommand?.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand CheckCommand { get; }

        public RelayCommand StartServiceCommand { get; }
        public RelayCommand StopServiceCommand { get; }

        private IDialogHelper dialogHelper;
        private IServiceHelper serviceHelper;
        private IDispatcher dispatcher;
        private SettingsViewModel settingsViewModel;

        public StatusViewModel(SettingsViewModel settingsViewModel, IServiceHelper serviceHelper, IDialogHelper dialogHelper, IDispatcher dispatcher)
        {
            this.settingsViewModel = settingsViewModel;
            this.serviceHelper = serviceHelper;
            this.dialogHelper = dialogHelper;
            this.dispatcher = dispatcher;

            CheckCommand = new RelayCommand(async () => await CheckAsync());

            StartServiceCommand = new RelayCommand(Start, CanStart);
            StopServiceCommand = new RelayCommand(Stop, CanStop);
        }

        private async void Start()
        {
            IsBusy = true;

            try
            {
                await serviceHelper.StartServiceAsync();
                await Task.Delay(3000);
                await CheckAsync();
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

        private bool CanStart() => IsServiceRunning == false && IsServiceInstalled == true;

        private async void Stop()
        {
            IsBusy = true;

            try
            {
                await serviceHelper.StopServiceAsync();
                await Task.Delay(3000);
                await CheckAsync();
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

        private bool CanStop() => IsServiceRunning == true;

        public async Task CheckAsync()
        {
            IsBusy = true;

            try
            {
                IsServiceInstalled = await serviceHelper.IsServiceInstalledAsync();
                IsServiceRunning = await serviceHelper.IsServiceRunningAsync();

                await Task.Run(() =>
                {
                    var isServerRunning = false;

                    using (var client = new TcpClient())
                    {
                        var port = settingsViewModel.Settings.Server.Port;

                        if(port <= 0)
                        {
                            port = 55117;
                        }

                        var ipAddress = settingsViewModel.Settings.Server.IPv6 ? "::1" : "127.0.0.1";

                        try
                        { 
                            var result = client.BeginConnect(ipAddress, port, null, null);
                            isServerRunning = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                            client.EndConnect(result);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                    }

                    dispatcher.RunOnUI(() =>
                    {
                        IsServerRunning = isServerRunning;
                    });
                });
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
