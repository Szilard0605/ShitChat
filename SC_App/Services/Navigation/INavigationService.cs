using MvvmHelpers;

namespace SC_App.Services.Navigation
{
    public interface INavigationService
    {
        BaseViewModel CurrentView { get; }
        void NavigateTo<T>() where T : BaseViewModel;
    }
}
