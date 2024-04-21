using CommunityToolkit.Mvvm.ComponentModel;

namespace SC_App.ViewModels
{
    public partial class NotConnectedServerViewModel : ViewModelBase
    {
        [ObservableProperty] private ConnectViewModel _connectViewModel;
        public NotConnectedServerViewModel(ConnectViewModel connectViewModel)
        {
            ConnectViewModel = connectViewModel;
        }

    }
}
