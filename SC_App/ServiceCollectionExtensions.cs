using Microsoft.Extensions.DependencyInjection;
using MvvmHelpers;
using SC_App.Services.Navigation;
using SC_App.ViewModels;
using System;

namespace SC_App
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<INavigationService, NavigationService>();
            collection.AddSingleton<Func<Type, BaseViewModel>>(serviceProvider => viewModelType => (BaseViewModel)serviceProvider.GetRequiredService(viewModelType));

            collection.AddSingleton<MainViewModel>();
            collection.AddSingleton<StartupViewModel>();
            collection.AddSingleton<HomeViewModel>();

        }
    }
}
