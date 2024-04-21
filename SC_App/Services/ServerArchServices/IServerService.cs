using SC_App.Models;
using System.Collections.ObjectModel;

namespace SC_App.Services.ServerArchServices
{
    public interface IServerService
    {
        ObservableCollection<Server> Servers { get; set; }
        Server CurrentServer { get; set; }
        void AddRoom(int serverIndex, int id, string name);
        void AddServer(int id, string name, string ipAddress, int port);
        void AddUser(int serverIndex, int id, string name);
        void RemoveRoom(int serverIndex, Room room);
        void RemoveServer(Server server);
        void RemoveUser(int serverIndex, User user);
        void SelectServer(Server server);
        void SelectServer(int serverIndex);
        int GetServersCount();
    }
}
