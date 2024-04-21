using System;
using System.Collections.ObjectModel;

namespace SC_App.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<User> Users { get; set; } = new();
        public ObservableCollection<string> Messages { get; set; } = new();

    }
}
