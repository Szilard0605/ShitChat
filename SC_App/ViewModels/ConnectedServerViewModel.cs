using CommunityToolkit.Mvvm.ComponentModel;
using SC_App.Helpers;
using SC_App.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SC_App.ViewModels
{
    public partial class ConnectedServerViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _serverName;

        [ObservableProperty]
        private string _serverIp;

        [ObservableProperty]
        private ObservableCollection<Server> _servers;

        [ObservableProperty]
        private ObservableCollection<User> _users;

        [ObservableProperty]
        private ObservableCollection<Room> _rooms;

        public ConnectedServerViewModel()
        {
            ServerName = "test";
            Servers = ServerDto.Servers;

            ServerIp = Servers[0].IpAddress;

            Users = Servers[0].Users;

            Rooms = Servers[0].Rooms;
        }
    }
}
