using MvvmHelpers;
using SC_App.ViewModels;
using System;
using System.Collections;

namespace SC_App.Services.Navigation
{
    public interface INavigationService
    {
        ViewModelBase CurrentMainView { get; }
        ViewModelBase CurrentServerView { get; }
        void NavigateTo<T>(NavDirection navProp) where T : ViewModelBase;
        enum NavDirection
        {
            Main,
            Server
        };

    }
}
