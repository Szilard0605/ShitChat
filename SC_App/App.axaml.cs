using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using SC_App.Services;
using SC_App.ViewModels;
using SC_App.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SC_App;

public partial class App : Application
{
    private DispatcherTimer _timer = new();
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        AttachTimer();
    }

    private void AttachTimer()
    {
        _timer.Interval = TimeSpan.FromMilliseconds(1000);
        _timer.Tick += async (sender, e) => await CheckForUpdates();
        _timer.Start();
    }

    private async Task CheckForUpdates()
    {
        // Host section
        if (NetworkingService.IsServerStarted)
        {
            NetworkingService.UpdateServer();
            Debug.WriteLine("Server running");
        }
        else
        {
            Debug.WriteLine("Server not running");
        }

        //Connect section
        if (NetworkingService.IsClientConnected)
        {
            NetworkingService.ClientUpdate();
            Debug.WriteLine("Connected");
        }
        else
        {
            Debug.WriteLine("Not Connected");
        }

    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        //Register Services
        var collection = new ServiceCollection();
        collection.AddCommonServices();

        var services = collection.BuildServiceProvider();
        var vm = services.GetRequiredService<MainViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
