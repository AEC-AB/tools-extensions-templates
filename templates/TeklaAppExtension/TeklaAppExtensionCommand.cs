using System.Diagnostics;
using TeklaAppExtension.Framework.Helpers;

namespace TeklaAppExtension;

public class TeklaAppExtensionCommand : ITeklaExtension<TeklaAppExtensionArgs>
{
    public IExtensionResult Run(ITeklaExtensionContext context, TeklaAppExtensionArgs args, CancellationToken cancellationToken)
    {
        var provider = ServiceFactory.Create(services =>
        {
            services.RegisterAppServices(args);
        });

        var handle = Process.GetCurrentProcess().MainWindowHandle;

        WindowHandler.ShowWindow<MainWindow>(provider, handle);
        return Result.Text.Succeeded("App started");
    }
}