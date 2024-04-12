using System.Collections.ObjectModel;

namespace SC_App.Models
{
    public class Server
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<User> Users { get; set; }

        public Server()
        {
            Users = new();
            Rooms = new();
        }
    }
}
