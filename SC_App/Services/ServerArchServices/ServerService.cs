using CommunityToolkit.Mvvm.ComponentModel;
using SC_App.Models;
using SC_App.ViewModels;
using System.Collections.ObjectModel;

namespace SC_App.Services.ServerArchServices
{
    public partial class ServerService : ViewModelBase, IServerService
    {
        [ObservableProperty] private ObservableCollection<Server> _servers = new();
        [ObservableProperty] private Server _currentServer = new();
        public void AddRoom(
            int serverIndex,
            int id,
            string name)
        {
            Servers[serverIndex].Rooms.Add(new Room
            {
                Id = id,
                Name = name,
                Messages = new(),
                Users = new(),
            });
        }

        public void AddServer(
            int id,
            string name,
            string ipAddress,
            int port)
        {
            Servers.Add(new Server
            {
                Id = id,
                Name = name,
                IpAddress = ipAddress,
                Port = port,
                Rooms = new(),
                Users = new()
            });
        }

        public void AddUser(
            int serverIndex,
            int id,
            string name)
        {
            Servers[serverIndex].Users.Add(new User
            {
                Id= id,
                Name = name,
            });
        }

        public void RemoveRoom(
            int serverIndex,
            Room room)
        {
            Servers[serverIndex].Rooms.Remove(room);
        }

        public void RemoveServer(Server server)
        {
            Servers.Remove(server);
        }

        public void RemoveUser(
            int serverIndex,
            User user)
        {
            Servers[serverIndex].Users.Remove(user);
        }

        public void SelectServer(int serverIndex)
        {
            CurrentServer = Servers[serverIndex];
        }
        public void SelectServer(Server server)
        {
            CurrentServer = server;
        }
        public int GetServersCount()
        {
            return Servers.Count;
        }
    }
}
