using CommunityToolkit.Mvvm.ComponentModel;
using SC_App.Models;
using System.Collections.ObjectModel;

namespace SC_App.DTOs
{
    public partial class ServerDTO : ObservableObject
    {
        [ObservableProperty] private Server _selectedServer;
        [ObservableProperty] private Room _selectedRoom;
        [ObservableProperty] private ObservableCollection<Server> _servers;
    }
}
