using CommunityToolkit.Mvvm.ComponentModel;
using SC_App.Models;
using System.Collections.ObjectModel;

namespace SC_App.ViewModels
{
    public partial class ConnectedServerViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _serverName;

        [ObservableProperty]
        private string _serverIp;

        [ObservableProperty]
        private ObservableCollection<User> _users;

        [ObservableProperty]
        private ObservableCollection<Room> _rooms;

        public ConnectedServerViewModel()
        {
            ServerName = "test";
            ServerIp = "127.0.0.1";

            Users = new ObservableCollection<User>
            {
                new User{ Name = "Asd"},
                new User{ Name = "Asd1"},
                new User{ Name = "Asd2"},
                new User{ Name = "Asd3"},
            };

            Rooms = new ObservableCollection<Room>
            {
                new Room{ Name = "1Asd"},
                new Room{ Name = "2Asd"},
                new Room{ Name = "3Asd"},
                new Room{ Name = "4Asd"},
                new Room{ Name = "5Asd"},
            };
        }

        private bool IsConnected()
        {
            return false;
        }
    }
}
