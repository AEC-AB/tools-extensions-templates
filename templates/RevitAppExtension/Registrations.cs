using Microsoft.Extensions.DependencyInjection;
using RevitAppExtension.ViewModels;
using RevitAppFramework.Extensions;
using System.Reflection;

namespace RevitAppExtension;

public static class Registrations
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services, RevitAppExtensionArgs args, bool useDesignQueryHandlers = false)
    {
        services.AddSingleton(args);
        services.AddCqrs(typeof(Registrations).Assembly, useDesignQueryHandlers);
        services.AddSingleton<MainWindow>();
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<AboutViewModel>();
        services.AddSingleton<IContentDialogService, ContentDialogService>();

        return services;
    }
}