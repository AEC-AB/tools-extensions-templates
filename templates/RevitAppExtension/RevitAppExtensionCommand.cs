using RevitAppFramework.Extensions;
using RevitAppFramework.Helpers;

namespace RevitAppExtension;

/// <summary>
/// Main entry point for the RevitAppExtension. This class implements IRevitExtension interface
/// which is required for all Revit extensions that integrate with the Assistant platform.
/// </summary>
/// <remarks>
/// This is the primary starting point of the application. When the extension is launched from
/// Assistant, this class's Run method is called and provided with the Revit context, user arguments,
/// and a cancellation token for handling long-running operations.
/// </remarks>
public class RevitAppExtensionCommand : IRevitExtension<RevitAppExtensionArgs>
{
    /// <summary>
    /// Main entry point method for the Revit extension.
    /// </summary>
    /// <param name="context">Provides access to Revit's API through UIApplication and other properties.</param>
    /// <param name="args">User-provided arguments defined in RevitAppExtensionArgs class.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IExtensionResult indicating success or failure with a message.</returns>
    /// <remarks>
    /// This method:
    /// 1. Checks if a Revit document is open
    /// 2. Creates a dependency injection service provider
    /// 3. Registers application services using the RegisterAppServices extension method
    /// 4. Shows the main application window
    /// 5. Returns a success result
    /// </remarks>
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

        // Return a result with the message
        return Result.Text.Succeeded("Application was started");
    }
}