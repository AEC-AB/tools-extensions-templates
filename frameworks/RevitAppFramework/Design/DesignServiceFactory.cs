namespace RevitAppFramework.Design;

public static class DesignServiceFactory
{
    public static global::System.IServiceProvider Create(global::System.Action<global::Microsoft.Extensions.DependencyInjection.IServiceCollection> register)
    {
        var eventExecutor = new DesignExternalEventExecutor();
        // Register services
        var services = new global::Microsoft.Extensions.DependencyInjection.ServiceCollection();
        global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<IExternalEventExecutor>(services, eventExecutor);

        global::RevitAppFramework.Helpers.ServiceFactory.AddFrameworkServices(services);

        register(services);

        var provider = global::Microsoft.Extensions.DependencyInjection.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
        eventExecutor.SetServiceProvider(provider);

        return provider;
    }
}
