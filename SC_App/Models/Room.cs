using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace SC_App.Models
{
    public partial class Room : ObservableObject
    {
        public Guid Id { get; set; }

        [ObservableProperty] private string _name;
        [ObservableProperty] private bool _isSelected;
        [ObservableProperty] private ObservableCollection<Message> _messages;
        [ObservableProperty] private ObservableCollection<User> _users;

    }
}
