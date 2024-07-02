using SC_App.Models;
using SC_App.ViewModels;

namespace SC_App.Services.Navigation
{
    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }
        void NavigateTo<T>() where T : ViewModelBase;
    }
}
