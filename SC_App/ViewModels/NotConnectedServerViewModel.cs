using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SC_App.Services.Navigation;

namespace SC_App.ViewModels
{
    public partial class NotConnectedServerViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ConnectViewModel _connectViewModel;
        public NotConnectedServerViewModel(ConnectViewModel connectViewModel)
        {
            ConnectViewModel = connectViewModel;
        }

    }
}
