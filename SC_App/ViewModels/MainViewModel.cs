using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentIcons.Avalonia;
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
        Navigation.NavigateTo<HomeViewModel>();

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
    private void NavigateToHome()
    {
        Navigation.NavigateTo<HomeViewModel>();
    }
    [RelayCommand]
    private void NavigateToConnect()
    {
        Navigation.NavigateTo<ConnectViewModel>();
    }
    [RelayCommand]
    private void NavigateToSettings()
    {
        Navigation.NavigateTo<SettingsViewModel>();
    }
}
