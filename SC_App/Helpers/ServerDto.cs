using SC_App.Models;
using System.Collections.ObjectModel;

namespace SC_App.Helpers
{
    public class ServerDto
    {
        public static ObservableCollection<Server> Servers { get; set; } = new();
        public ServerDto()
        {

        }
    }
}
