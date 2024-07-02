using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SC_App.DTOs;
using SC_App.Services.Navigation;
using System.Collections.ObjectModel;

namespace SC_App.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty] private ServerDTO _serverDTO;
        [ObservableProperty] private ObservableCollection<NavItem> _navList;
        [ObservableProperty] private INavigationService _navService;

        public MainViewModel(INavigationService navService, ServerDTO serverDTO)
        {
            _navService = navService;
            _serverDTO = serverDTO;

            NavService.NavigateTo<HomeViewModel>();

            NavList = new ObservableCollection<NavItem>
            {
                new NavItem{ Title = "Home", Icon = MaterialIconKind.Home },
                new NavItem{ Title = "Settings", Icon = MaterialIconKind.Settings },
            };
        }

        [RelayCommand]
        private void NavigateToHomeView()
        {
            ServerDTO.SelectedServer = null;
            NavService.NavigateTo<HomeViewModel>();
        }
    }

    public class NavItem
    {
        public string Title { get; set; }
        public MaterialIconKind Icon { get; set; }
    }
}
