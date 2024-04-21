using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SC_App.Services;
using SC_App.Services.ServerArchServices;

namespace SC_App.ViewModels
{
    public partial class ConnectedServerViewModel : ViewModelBase
    {
        [ObservableProperty] private IServerService _serverService;
        public ConnectedServerViewModel(IServerService serverService)
        {
            _serverService = serverService;
        }

        [ObservableProperty] private string _message;

        [RelayCommand]
        private void SendMessage(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(Message))
            {
                NetworkingService.Client.SendChatroomMessage(Message, 0);
                //ServerService.SelectedServer.Rooms[0].Messages.Add(Message);
                Message = string.Empty;
            }
        }
    }
}
