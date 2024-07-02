using Avalonia.Controls;
using Avalonia.Input;

namespace SC_App.Views
{
    public partial class ServerView : UserControl
    {
        public ServerView()
        {
            InitializeComponent();

            RoomListBox.AddHandler(KeyDownEvent, OnPreviewKeyDown, Avalonia.Interactivity.RoutingStrategies.Tunnel);
        }

        void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
