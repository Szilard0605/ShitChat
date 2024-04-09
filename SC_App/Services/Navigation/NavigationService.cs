using MvvmHelpers;
using SC_App.ViewModels;
using System;

namespace SC_App.Services.Navigation
{
    public class NavigationService : ViewModelBase, INavigationService
    {
        private ViewModelBase _currentView;
        private Func<Type, ViewModelBase> _viewModelFactory;

        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public void NavigateTo<TViewModelBase>() where TViewModelBase : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelFactory.Invoke(typeof(TViewModelBase));
            CurrentView = viewModel;
        }
    }
}
