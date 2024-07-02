using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using SC_App.ViewModels;

namespace SC_App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var minimizeButton = this.FindControl<Button>("MinimizeButton");
        var maximizeButton = this.FindControl<Button>("MaximizeButton");
        var closeButton = this.FindControl<Button>("CloseButton");
        var titleBar = this.FindControl<Border>("TitleBar");

        minimizeButton!.Click += MinimizeButton_Click!;
        maximizeButton!.Click += MaximizeButton_Click!;
        closeButton!.Click += CloseButton_Click!;
        titleBar!.PointerPressed += TitleBar_PointerPressed;
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        this.BeginMoveDrag(e);
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }
    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
            TitleBar.Margin = new Thickness(0, 0, 0, 0);
        }
        else
        {
            this.WindowState = WindowState.Maximized;
            TitleBar.Margin = new Thickness(10, 10, 10, 0);
        }
    }
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
