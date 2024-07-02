using Microsoft.Extensions.DependencyInjection;
using SC_App.DTOs;
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
            collection.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));

            collection.AddSingleton<ServerDTO>();

            collection.AddSingleton<MainViewModel>();
            collection.AddSingleton<HomeViewModel>();
            collection.AddSingleton<ServerViewModel>();
        }
    }
}
