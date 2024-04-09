using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SC_App.Services;

namespace SC_App.ViewModels
{
    public partial class ConnectViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _ipAddress;

        [ObservableProperty]
        private string _port;

        [ObservableProperty]
        private string _statusText;

        [ObservableProperty]
        private bool _isStatusTextVisible;

        private int _maxClients;

        public ConnectViewModel()
        {
            StatusText = "random";
            _maxClients = 2;
        }

        [RelayCommand]
        private void StartServer()
        {
            if (AreFieldsValid())
            {
                NetworkingService.StartServer(IpAddress, int.Parse(Port), _maxClients);
            } 
        }

        [RelayCommand]
        private void ShutdownServer()
        {
            NetworkingService.ShutdownServer();
        }

        [RelayCommand]
        private void JoinHost()
        {
            if (AreFieldsValid())        
            {
                //joinhost
            }
        }

        private bool AreFieldsValid()
        {
            if (string.IsNullOrEmpty(IpAddress) || string.IsNullOrEmpty(Port))
            {
                IsStatusTextVisible = true;
                StatusText = "Fields cannot be blank";
                return false;
            }
            else
            {
                IsStatusTextVisible = false;
                return true;
            }
        }


    }
}
