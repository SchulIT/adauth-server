using GalaSoft.MvvmLight;
using ServerGUI.Services.License;

namespace ServerGUI.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private string licenses;

        public string Licenses
        {
            get { return licenses; }
            set { Set(() => Licenses, ref licenses, value); }
        }

        #region Services

        private readonly ILicenseLoader licenseLoader;

        #endregion

        public MainViewModel(ILicenseLoader licenseLoader)
        {
            this.licenseLoader = licenseLoader;
        }

        public async void LoadLicenses()
        {
            Licenses = await licenseLoader.LoadLicenseAsync();
        }
    }
}