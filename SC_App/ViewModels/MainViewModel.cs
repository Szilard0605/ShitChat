using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentIcons.Common;
using SC_App.Services.Navigation;
using SC_App.Services.ServerArchServices;

namespace SC_App.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    [ObservableProperty] private INavigationService _navigation;
    [ObservableProperty] private IServerService _serverService;

    public MainViewModel(
        INavigationService navigation,
        IServerService serverService)
    {
        _navigation = navigation;
        _serverService = serverService;

        Navigation.NavigateTo<ServerViewModel>(INavigationService.NavDirection.Main);
        PaneSymbol = Symbol.ArrowRight;
    }

    [ObservableProperty] private bool _isPaneOpen;
    [ObservableProperty] private Symbol _paneSymbol;
    [ObservableProperty] private int _selectedServerIndex;

    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;

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

    [RelayCommand]
    private void ChangeServer()
    {
        ServerService.SelectServer(SelectedServerIndex);
    }
}
