using RevitAppFramework.Extensions;
using RevitAppFramework.Helpers;

namespace RevitAppExtension;

public class RevitAppExtensionCommand : IRevitExtension<RevitAppExtensionArgs>
{
    public IExtensionResult Run(IRevitExtensionContext context, RevitAppExtensionArgs args, CancellationToken cancellationToken)
    {
        var document = context.UIApplication.ActiveUIDocument?.Document;

        if (document is null)
            return Result.Text.Failed("Revit has no active model open");

        var provider = ServiceFactory.Create(context.UIApplication, services =>
        {
            services.RegisterAppServices(args);
        });

        WindowHandler.ShowWindow<MainWindow>(provider, context.UIApplication.MainWindowHandle);
        return Result.Text.Succeeded("Application was started");
    }
}