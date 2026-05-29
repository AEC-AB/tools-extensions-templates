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
    public static IServiceCollection RegisterAppServices(this IServiceCollection services, TeklaAppExtensionArgs args)
    {
        services.AddSingleton(args);

        services.AddSingleton<MainWindow>();

        services.AddSingleton<HomeViewModel>();
        services.AddSingleton<AboutViewModel>();
        services.AddSingleton<ITeklaService, TeklaService>();

        return services;
    }
}