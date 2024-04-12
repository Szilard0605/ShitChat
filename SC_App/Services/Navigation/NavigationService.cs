using CommunityToolkit.Mvvm.ComponentModel;
using SC_App.ViewModels;
using System;
using System.Collections;

namespace SC_App.Services.Navigation
{
    public partial class NavigationService : ViewModelBase, INavigationService
    {
        [ObservableProperty]
        private ViewModelBase _currentMainView;

        [ObservableProperty]
        private ViewModelBase _currentServerView;

        private Func<Type, ViewModelBase> _viewModelFactory;

        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModelBase>(INavigationService.NavDirection navProp) where TViewModelBase : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelFactory.Invoke(typeof(TViewModelBase));
            switch (navProp)
            {
                case INavigationService.NavDirection.Main:
                    CurrentMainView = viewModel;
                    break;
                case INavigationService.NavDirection.Server:
                    CurrentServerView = viewModel;
                    break;
                default:
                    break;
            }
        }
    }
}
