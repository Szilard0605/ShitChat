using CommunityToolkit.Mvvm.ComponentModel;
using SC_App.Services.Navigation;

namespace SC_App.ViewModels
{
    public partial class ServerViewModel : ViewModelBase
    {
        [ObservableProperty] private INavigationService _navigation;

        public ServerViewModel(INavigationService navigation)
        {
            _navigation = navigation;

            Navigation.NavigateTo<NotConnectedServerViewModel>(INavigationService.NavDirection.Server);
        }
    }
}
