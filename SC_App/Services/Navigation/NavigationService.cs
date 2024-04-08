using MvvmHelpers;
using System;

namespace SC_App.Services.Navigation
{
    public class NavigationService : BaseViewModel, INavigationService
    {
        private BaseViewModel _currentView;
        private Func<Type, BaseViewModel> _viewModelFactory;

        public NavigationService(Func<Type, BaseViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public BaseViewModel CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public void NavigateTo<TBaseViewModel>() where TBaseViewModel : BaseViewModel
        {
            BaseViewModel viewModel = _viewModelFactory.Invoke(typeof(TBaseViewModel));
            CurrentView = viewModel;
        }
    }
}
