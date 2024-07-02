using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace SC_App.Models
{
    public partial class Message : ObservableObject
    {
        public Guid Id { get; set; }
        [ObservableProperty] public string _content;
        [ObservableProperty] public string _sender;
        [ObservableProperty] public DateTime _sentTime;
    }
}
