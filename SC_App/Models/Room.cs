using Avalonia.Media;
using System.Collections.ObjectModel;

namespace SC_App.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<User> Users { get; set; }

        public Room()
        {
            Users = new();
        }
    }
}
