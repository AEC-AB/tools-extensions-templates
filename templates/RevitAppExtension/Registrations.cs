using Microsoft.Extensions.DependencyInjection;
using RevitAppExtension.ViewModels;
using RevitAppFramework.Extensions;
using System.Reflection;

namespace RevitAppExtension;

/// <summary>
/// Provides dependency injection registration for the application.
/// This static class contains extension methods for configuring the service collection
/// with all required services for the application.
/// </summary>
public static class Registrations
{
    /// <summary>
    /// Registers all application services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="args">The user arguments passed from the extension startup.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <remarks>
    /// This method registers:
    /// - User arguments as a singleton
    /// - CQRS handlers from the assembly using the AddCqrs extension method
    /// - View-related services (MainWindow and ViewModels)
    /// - UI services like the ContentDialogService
    ///
    /// When extending the application with new services:
    /// 1. Create your service/view model class
    /// 2. Add registration here using appropriate lifetime (AddSingleton, AddTransient, etc.)
    /// 3. For services with interfaces, register the interface and implementation
    /// </remarks>
    public static IServiceCollection RegisterAppServices(this IServiceCollection services, RevitAppExtensionArgs args, bool useDesignQueryHandlers = false)
    {
        // Register the application services
        services.AddSingleton(args);
        services.AddCqrs(typeof(Registrations).Assembly, useDesignQueryHandlers);
        services.AddSingleton<MainWindow>();
        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<AboutViewModel>();
        services.AddSingleton<IContentDialogService, ContentDialogService>();

        return services;
    }
}