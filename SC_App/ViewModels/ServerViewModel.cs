using CommunityToolkit.Mvvm.ComponentModel;
using SC_App.DTOs;
using SC_App.Models;
using System;
using System.Collections.ObjectModel;

namespace SC_App.ViewModels
{
    public partial class ServerViewModel : ViewModelBase
    {
        [ObservableProperty] private ServerDTO _serverDTO;
        [ObservableProperty] private Room _selectedRoom;

        public ServerViewModel(ServerDTO serverDTO)
        {
            _serverDTO = serverDTO;

            ServerDTO.Servers[0].Rooms = new ObservableCollection<Room>
            {
                new Room { Name = "Room 1", Messages = new ObservableCollection<Message>{ new Message { Content = "buzi", Sender="David", SentTime = DateTime.Now} } },
                new Room { Name = "Room 2" },
                new Room { Name = "Room 3" }
            };

            ServerDTO.Servers[1].Rooms = new ObservableCollection<Room>
            {
                new Room { Name = "Room 1", Messages = new ObservableCollection<Message>{ new Message { Content = "buziiiiiiii", Sender="David", SentTime = DateTime.Now} } },
                new Room { Name = "Room 2" },
                new Room { Name = "Room 3" }
            };
        }

        partial void OnSelectedRoomChanged(Room value)
        {
            if (ServerDTO.SelectedServer != null)
            {
                foreach (var room in ServerDTO.SelectedServer.Rooms)
                {
                    room.IsSelected = room == value;
                }
            }
        }
    }
}
