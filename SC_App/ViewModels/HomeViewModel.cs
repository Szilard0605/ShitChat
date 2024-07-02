using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SC_App.DTOs;
using SC_App.Models;
using SC_App.Services.Navigation;
using System.Collections.ObjectModel;

namespace SC_App.ViewModels
{
    public partial class HomeViewModel : ViewModelBase
    {
        [ObservableProperty] private ServerDTO _serverDTO;
        [ObservableProperty] private INavigationService _navService;

        [ObservableProperty] private bool _isConnectionTabVisible = false;
        public HomeViewModel(ServerDTO serverDTO, INavigationService navService)
        {
            _serverDTO = serverDTO;
            _navService = navService;

            ServerDTO.Servers = new ObservableCollection<Server>
            {
                new Server {Name = "TestServer"},
                new Server {Name = "TestServerTestServerTestServer"},
                new Server {Name = "TestServerTestServerTestServer"},
                new Server {Name = "TestServerTestServerTestServer"},
                new Server {Name = "TestServerTestServerTestServer"},
                new Server {Name = "TestServerTestServerTestServer"},
                new Server {Name = "TestServerTestServerTestServer"}
            };
        }

        [RelayCommand]
        private void ChangeConnectionTabStatus()
        {
            IsConnectionTabVisible = !IsConnectionTabVisible;
        }

        [RelayCommand]
        private void NavigateToServerView(Server server)
        {
            ServerDTO.SelectedServer = server;
            NavService.NavigateTo<ServerViewModel>();
        }

    }
}
