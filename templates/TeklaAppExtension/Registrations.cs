using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Dialog;
using TeklaAppExtension.ViewModels;
using Wpf.Ui;

namespace TeklaAppExtension;

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
    /// - View-related services (MainWindow and ViewModels)
    /// - UI services like the ContentDialogService
    ///
    /// When extending the application with new services:
    /// 1. Create your service/view model class
    /// 2. Add registration here using appropriate lifetime (AddSingleton, AddTransient, etc.)
    /// 3. For services with interfaces, register the interface and implementation
    /// </remarks>
    public static IServiceCollection RegisterAppServices(this IServiceCollection services, TeklaAppExtensionArgs args)
    {        // Register the command line arguments as a singleton for access throughout the app
        services.AddSingleton(args);
     
        // Register the main window as a singleton (one instance for the application)
        services.AddSingleton<MainWindow>();
        
        // Register view models as singletons
        services.AddSingleton<HomeViewModel>();  // View model for the home screen
        services.AddSingleton<AboutViewModel>();  // View model for the about screen
        
        // Register the Tekla service implementation with its interface
        services.AddSingleton<ITeklaService, TeklaService>();

        return services;
    }
}