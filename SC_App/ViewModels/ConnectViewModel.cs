using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SC_App.Services;
using SC_App.Services.Navigation;
using SC_App.Services.ServerArchServices;
using SC_App.Utils;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SC_App.ViewModels
{
    public partial class ConnectViewModel : ViewModelBase
    {
        #region Services

        [ObservableProperty] private INavigationService _navigation;
        [ObservableProperty] private static IServerService _serverService;

        #endregion

        public ConnectViewModel(
            INavigationService navigation,
            IServerService serverService)
        {
            _navigation = navigation;
            _serverService = serverService;

            Initialize();
        }

        private void Initialize()
        {
            HostIpAddress = "192.168.1.68"; // dont hack me ples
            ConnectPort = 8080;
            HostPort = 8080;
            MaxClients = 2;
       
            NetworkingService.Client.SetIntroduceClientHandler(OnIntroduceClient);
            NetworkingService.Client.SetChatroomMessageHandler(OnIncomingChatroomMessage);
            NetworkingService.Client.SetConnectionAcceptedHandler(OnConnectionAccepted);
            NetworkingService.Server.SetClientConnectHandler(OnClientConnect);

        }
        public static void OnIntroduceClient(int clientId, string name)
        {
            Debug.WriteLine($"[Introduce]: {name}[{clientId}]");
            if (clientId != 0)
            {
                _serverService.AddUser(0, clientId, name);
            }
        }

        public static void OnIncomingChatroomMessage(int clientId, string message, int roomId)
        {
            Debug.WriteLine($"[Chatroom msg]{clientId} sent \"{message}\" to Room {roomId}");
            _serverService.Servers[0].Rooms[0].Messages.Add(message);
        }

        public static void OnConnectionAccepted(int id)
        {
            Debug.WriteLine($"Connection accepted, ID assigned: {id}");
        }

        public static void OnClientConnect(string name, int id)
        {
            Debug.WriteLine($"{name}[{id}] connected");
            _serverService.AddUser(0, id, name);
        }


        #region Host Properties

        [ObservableProperty] private string _hostIpAddress;
        [ObservableProperty] private int _hostPort;
        [ObservableProperty] private string _hostStatusText;
        [ObservableProperty] private bool _isHostStatusTextVisible;
        [ObservableProperty] private int _maxClients;

        #endregion

        #region Host Methods
        private bool AreHostFieldsValid()
        {
            if (string.IsNullOrWhiteSpace(HostIpAddress) || string.IsNullOrWhiteSpace(HostPort.ToString()) || string.IsNullOrWhiteSpace(MaxClients.ToString()))
            {
                HostStatusText = Constants.StatusMessages.BLANK_FIELD;
                IsHostStatusTextVisible = true;
                return false;
            }
            else if (MaxClients <= 0)
            {
                HostStatusText = Constants.StatusMessages.Host.MAX_CLIENTS_LOW;
                IsHostStatusTextVisible = true;
                return false;
            }
            else if (!Regex.IsMatch(HostIpAddress, Constants.IPV4_REGEX))
            {
                HostStatusText = Constants.StatusMessages.INVALID_ADDRESS_FORMAT;
                IsHostStatusTextVisible = true;
                return false;
            }
            else if (ServerService.Servers.Count > 0)
            {
                for (int i = 0; i < ServerService.Servers.Count; i++)
                {
                    if (ServerService.Servers[i].IpAddress == HostIpAddress && ServerService.Servers[i].Port == HostPort)
                    {
                        HostStatusText = Constants.StatusMessages.Host.DUPLICATE_SERVER;
                        IsHostStatusTextVisible = true;
                        return false;
                    }
                }
            }
            IsHostStatusTextVisible = false;
            return true;
        }

        #endregion


        #region Host Commands

        [RelayCommand]
        private void StartServer()
        {
            if (AreHostFieldsValid())
            {
                NetworkingService.Server.IsStarted = NetworkingService.Server.StartServer(HostIpAddress, HostPort, MaxClients);
                if (NetworkingService.Server.IsStarted)
                {
                    Navigation.NavigateTo<ConnectedServerViewModel>(INavigationService.NavDirection.Server);
                }
                else
                {
                    Debug.WriteLine("Failed to start server");
                    HostStatusText = Constants.StatusMessages.Host.UNEXPECTED_ERROR;
                    IsHostStatusTextVisible = true;
                    return;
                }

                // Instantly connect to created server as host
                string hostName = "hostname";
                Connect(HostIpAddress, HostPort, hostName);
            }

        }

        [RelayCommand]
        private void ShutdownServer()
        {
            if (NetworkingService.Server.IsStarted)
            {
                NetworkingService.Server.ShutdownServer();
                NetworkingService.Server.IsStarted = false;
            }
        }

        #endregion


        #region Connect Properties

        [ObservableProperty] private string _connectIpAddress;
        [ObservableProperty] private int _connectPort;
        [ObservableProperty] private string _connectStatusText;
        [ObservableProperty] private bool _isConnectStatusTextVisible;
        [ObservableProperty] private string _nickname;

        #endregion


        #region Connect Methods
        private bool AreConnectFieldsValid()
        {
            if (string.IsNullOrWhiteSpace(ConnectIpAddress) || string.IsNullOrWhiteSpace(ConnectPort.ToString()) || string.IsNullOrWhiteSpace(Nickname))
            {
                ConnectStatusText = Constants.StatusMessages.BLANK_FIELD;
                IsConnectStatusTextVisible = true;
                return false;
            }
            else if (Nickname.Length > Constants.MAX_NICKNAME_CHARS)
            {
                ConnectStatusText = Constants.StatusMessages.Connect.LONG_NICKNAME;
                IsConnectStatusTextVisible = true;
                return false;
            }
            else if (!Regex.IsMatch(ConnectIpAddress, Constants.IPV4_REGEX))
            {
                ConnectStatusText = Constants.StatusMessages.INVALID_ADDRESS_FORMAT;
                IsConnectStatusTextVisible = true;
                return false;
            }
            else if (ServerService.Servers.Count > 0)
            {
                for (int i = 0; i < ServerService.Servers.Count; i++)
                {
                    if (ServerService.Servers[i].IpAddress == ConnectIpAddress && ServerService.Servers[i].Port == ConnectPort)
                    {
                        ConnectStatusText = Constants.StatusMessages.Connect.DUPLICATE_SERVER;
                        IsConnectStatusTextVisible = true;
                        return false;
                    }
                }
            }
            IsConnectStatusTextVisible = false;
            return true;
        }

        private void Connect(string ipAddress, int port, string nickname)
        {
            int serverIndex = ServerService.GetServersCount();

            NetworkingService.Client.IsConnected = NetworkingService.Client.ConnectToServer(ipAddress, port, nickname);
            if (NetworkingService.Client.IsConnected)
            {
                ServerService.AddServer(serverIndex, "NewServer", ipAddress, port);
                ServerService.AddRoom(serverIndex, 0, "Default Room");
                Navigation.NavigateTo<ConnectedServerViewModel>(INavigationService.NavDirection.Server);
            }
            else
            {
                Debug.WriteLine("Couldnt connect to server.");
            }
        }

        #endregion



        #region Connect Commands

        [RelayCommand]
        private void ConnectToServer()
        {
            if (AreConnectFieldsValid())        
            {
                Connect(ConnectIpAddress, ConnectPort, Nickname);
            }
        }

        [RelayCommand]
        private void DisconnectFromServer()
        {
            if (NetworkingService.Client.IsConnected)
            {
                NetworkingService.Client.DisconnectFromServer();
                NetworkingService.Client.IsConnected = false;
            }
        }

        #endregion
    }
}
