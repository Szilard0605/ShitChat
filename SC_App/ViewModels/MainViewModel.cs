using MvvmHelpers;
using SC_App.Services.Navigation;

namespace SC_App.ViewModels;

public class MainViewModel : BaseViewModel
{
    private INavigationService _navigation;
    public INavigationService Navigation
    { 
        get =>  _navigation; 
        set => SetProperty(ref _navigation, value);
    }

    public MainViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        Navigation.NavigateTo<StartupViewModel>();
    }
}
