using MvvmHelpers;
using MvvmHelpers.Commands;
using SC_App.Services;
using System.Windows.Input;

namespace SC_App.ViewModels
{
    public class StartupViewModel : BaseViewModel
    {
        private string _ipAddress;
        private string _port;
        private string _statusText;
        private bool _isStatusTextVisible;
        private int _maxClients;

        public string IpAddress
        {
            get => _ipAddress;
            set => SetProperty(ref _ipAddress, value);
        }
        public string Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }
        public bool IsStatusTextVisible
        {
            get => _isStatusTextVisible;
            set => SetProperty(ref _isStatusTextVisible, value);
        }
        public StartupViewModel()
        {
            StatusText = "random";
            _maxClients = 2;

            StartServerCommand = new Command(StartServer);
            ShutdownServerCommand = new Command(ShutdownServer);
            JoinHostCommand = new Command(JoinHost);
        }

        public ICommand StartServerCommand { get; private set; }
        public ICommand ShutdownServerCommand { get; private set; }
        public ICommand JoinHostCommand { get; private set; }

        private void StartServer()
        {
            if (AreFieldsValid())
            {
                NetworkingService.StartServer(IpAddress, int.Parse(Port), _maxClients);
            } 
        }

        private void ShutdownServer()
        {
            NetworkingService.ShutdownServer();
        }
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
