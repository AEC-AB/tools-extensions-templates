using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeklaAppExtension.Framework.Helpers;

public static class ServiceFactory
{
    public static global::System.IServiceProvider Create(global::System.Action<global::Microsoft.Extensions.DependencyInjection.IServiceCollection> register)
    {
        

        // Register services
        var services = new global::Microsoft.Extensions.DependencyInjection.ServiceCollection();
        
        services.AddFrameworkServices();

        register(services);

        var provider = global::Microsoft.Extensions.DependencyInjection.ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);

        return provider;
    }

    public static global::Microsoft.Extensions.DependencyInjection.IServiceCollection AddFrameworkServices(this global::Microsoft.Extensions.DependencyInjection.IServiceCollection services)
    {
        global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::Wpf.Ui.ISnackbarService, global::Wpf.Ui.SnackbarService>(services);
        global::Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<global::Wpf.Ui.IContentDialogService, global::Wpf.Ui.ContentDialogService>(services);
        return services;
    }
}

