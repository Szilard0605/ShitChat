using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace SC_App.Models
{
    public partial class Server : ObservableObject
    {
        [ObservableProperty] public string _name;
        public Guid Id { get; set; }
        [ObservableProperty] public string _ipAddress;
        [ObservableProperty] public int _port;
        [ObservableProperty] public ObservableCollection<Room> _rooms;
        [ObservableProperty] public ObservableCollection<User> _users;

    }
}
