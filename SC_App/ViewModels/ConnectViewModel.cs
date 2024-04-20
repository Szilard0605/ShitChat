using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SC_App.Helpers;
using SC_App.Models;
using SC_App.Services;
using SC_App.Services.Navigation;
using SC_App.Utils;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SC_App.ViewModels
{
    public partial class ConnectViewModel : ViewModelBase
    {
        [ObservableProperty]
        private INavigationService _navigation;
        public ConnectViewModel(INavigationService navigation)
        {
            _navigation = navigation;

            ConnectPort = 8080;
            HostPort = 8080;
            MaxClients = 2;
        }

        // Host section
        [ObservableProperty]
        private string _hostIpAddress;

        [ObservableProperty]
        private int _hostPort;

        [ObservableProperty]
        private string _hostStatusText;

        [ObservableProperty]
        private bool _isHostStatusTextVisible;

        [ObservableProperty]
        private int _maxClients;

        private bool AreHostFieldsValid()
        {
            if (string.IsNullOrWhiteSpace(HostIpAddress) || string.IsNullOrWhiteSpace(HostPort.ToString()) || string.IsNullOrWhiteSpace(MaxClients.ToString()))
            {
                HostStatusText = "Fields cannot be blank";
                IsHostStatusTextVisible = true;
                return false;
            }
            else if (MaxClients <= 0)
            {
                HostStatusText = "Max clients cannot be 0 or lower.";
                IsHostStatusTextVisible = true;
                return false;
            }
            else if (!Regex.IsMatch(HostIpAddress, Constants.IPV4_REGEX))
            {
                HostStatusText = "Ip address is not in a valid format";
                IsHostStatusTextVisible = true;
                return false;
            }
            IsHostStatusTextVisible = false;
            return true;
        }

        [RelayCommand]
        private void StartServer()
        {
            if (AreHostFieldsValid())
            {
                NetworkingService.IsServerStarted = NetworkingService.StartServer(HostIpAddress, HostPort, MaxClients);
                if (NetworkingService.IsServerStarted)
                {
                    Debug.WriteLine("Successfully started server");
                    NetworkingService.SetClientConnectHandler(NetworkingService.OnClientConnect);
                }
                else
                {
                    Debug.WriteLine("Failed to start server");
                }

                // Instantly connect to created server as host
                string _hostName = "host";
                NetworkingService.IsClientConnected = NetworkingService.ConnectToServer(HostIpAddress, HostPort, _hostName);
                if (NetworkingService.IsClientConnected)
                {
                    Debug.WriteLine("Connected to server successfully.");

                    NetworkingService.SetConnectionAcceptedHandler(NetworkingService.OnConnectionAccepted);
                }
                else
                {
                    Debug.WriteLine("Couldnt connect to server.");
                }

                //for testing if ui update works
                int id = 0;

                //Add server to data store
                ServerDto.Servers.Add(new Server
                {
                    Id = id,
                    IpAddress = HostIpAddress
                });

                //Add user to server
                ServerDto.Servers[id].Users.Add(new User
                {
                    Name = _hostName,
                    Id = id
                });

                //Add def room to server
                ServerDto.Servers[id].Rooms.Add(new Room
                {
                    Name = "Default Room",
                    Id = id,
                });

                //Add user to def room
                ServerDto.Servers[id].Rooms[id].Users.Add(ServerDto.Servers[id].Users[0]);

                Navigation.NavigateTo<ConnectedServerViewModel>(INavigationService.NavDirection.Server);
            }
        }

        [RelayCommand]
        private void ShutdownServer()
        {
            if (NetworkingService.IsServerStarted)
            {
                NetworkingService.ShutdownServer();
                NetworkingService.IsServerStarted = false;
            }
        }

        // Connect section
        [ObservableProperty]
        private string _connectIpAddress;

        [ObservableProperty]
        private int _connectPort;

        [ObservableProperty]
        private string _connectStatusText;

        [ObservableProperty]
        private bool _isConnectStatusTextVisible;

        [ObservableProperty]
        private string _nickname;

        private bool AreConnectFieldsValid()
        {
            if (string.IsNullOrWhiteSpace(ConnectIpAddress) || string.IsNullOrWhiteSpace(ConnectPort.ToString()) || string.IsNullOrWhiteSpace(Nickname))
            {
                ConnectStatusText = "Fields cannot be blank";
                IsConnectStatusTextVisible = true;
                return false;
            }
            else if (Nickname.Length > Constants.MAX_NICKNAME_CHARS)
            {
                ConnectStatusText = "Nickname cannot be longer than 7 characters";
                IsConnectStatusTextVisible = true;
                return false;
            }
            else if (!Regex.IsMatch(ConnectIpAddress, Constants.IPV4_REGEX))
            {
                ConnectStatusText = "Ip address is not in a valid format";
                IsConnectStatusTextVisible = true;
                return false;
            }
            IsConnectStatusTextVisible = false;
            return true;
        }

        [RelayCommand]
        private void ConnectToServer()
        {
            if (AreConnectFieldsValid())        
            {
                NetworkingService.IsClientConnected = NetworkingService.ConnectToServer(ConnectIpAddress, ConnectPort, Nickname);
                if (NetworkingService.IsClientConnected)
                {
                    Debug.WriteLine("Connected to server successfully.");

                    NetworkingService.SetConnectionAcceptedHandler(NetworkingService.OnConnectionAccepted);
                }
                else
                {
                    Debug.WriteLine("Couldnt connect to server.");
                }
            }
        }

        [RelayCommand]
        private void DisconnectFromServer()
        {
            if (NetworkingService.IsClientConnected)
            {
                NetworkingService.DisconnectFromServer();
                NetworkingService.IsClientConnected = false;
            }
        }
    }
}
