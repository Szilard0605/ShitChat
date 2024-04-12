using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SC_App.Services.Navigation;

namespace SC_App.ViewModels
{
    public partial class NotConnectedServerViewModel : ViewModelBase
    {
        [ObservableProperty]
        private INavigationService _navigation;
        public NotConnectedServerViewModel(INavigationService navigation)
        {
            _navigation = navigation;
        }

        [RelayCommand]
        private void ConnectToServer()
        {
            Navigation.NavigateTo<ConnectedServerViewModel>(INavigationService.NavDirection.Server);
        }

    }
}
