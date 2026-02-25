namespace RevitAppFramework.Helpers;

public static class ServiceFactory
{
    public static global::System.IServiceProvider Create(global::Autodesk.Revit.UI.UIApplication uiApplication, global::System.Action<global::Microsoft.Extensions.DependencyInjection.IServiceCollection> register)
    {
        var eventHandler = new global::RevitAppFramework.AppExternalEventHandler();
        var externalEvent = global::Autodesk.Revit.UI.ExternalEvent.Create(eventHandler);
        var eventExecutor = new global::RevitAppFramework.ExternalEventExecutor(eventHandler, externalEvent);

        // Register services
        var services = new global::Microsoft.Extensions.DependencyInjection.ServiceCollection();
        global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped(services, _ => new global::RevitAppFramework.RevitContext(uiApplication));
        global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::RevitAppFramework.IExternalEventExecutor>(services, eventExecutor);

        services.AddFrameworkServices();

        register(services);

        var provider = global::Microsoft.Extensions.DependencyInjection.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
        eventExecutor.SetServiceProvider(provider);
        eventHandler.SetServiceProvider(provider);

        return provider;
    }

    public static global::Microsoft.Extensions.DependencyInjection.IServiceCollection AddFrameworkServices(this global::Microsoft.Extensions.DependencyInjection.IServiceCollection services)
    {
        global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::Wpf.Ui.ISnackbarService, global::Wpf.Ui.SnackbarService>(services);
        global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient<global::RevitAppFramework.Mvvm.ViewModelBaseDeps>(services);

        return services;
    }
}
