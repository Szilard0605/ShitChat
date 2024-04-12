using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentIcons.Common;
using SC_App.Services.Navigation;

namespace SC_App.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private INavigationService _navigation;

    [ObservableProperty]
    private bool _isPaneOpen;

    [ObservableProperty]
    private Symbol _paneSymbol;

    public MainViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        Navigation.NavigateTo<ServerViewModel>(INavigationService.NavDirection.Main);

        PaneSymbol = Symbol.ArrowRight;
    }

    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = ! IsPaneOpen;

        if (IsPaneOpen)
        {
            PaneSymbol = Symbol.ArrowLeft;
        }
        else
        {
            PaneSymbol = Symbol.ArrowRight;
        }
    }

    [RelayCommand]
    private void NavigateToServer()
    {
        Navigation.NavigateTo<ServerViewModel>(INavigationService.NavDirection.Main);
    }
    [RelayCommand]
    private void NavigateToConnect()
    {
        Navigation.NavigateTo<ConnectViewModel>(INavigationService.NavDirection.Main);
    }
    [RelayCommand]
    private void NavigateToSettings()
    {
        Navigation.NavigateTo<SettingsViewModel>(INavigationService.NavDirection.Main);
    }
}
