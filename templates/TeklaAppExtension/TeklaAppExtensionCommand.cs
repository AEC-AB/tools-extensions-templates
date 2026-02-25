using System.Diagnostics;
using TeklaAppExtension.Framework.Helpers;

namespace TeklaAppExtension;

/// <summary>
/// Main entry point class for the Tekla application extension.
/// Implements the ITeklaExtension interface to provide integration with the Tekla Structures environment.
/// </summary>
public class TeklaAppExtensionCommand : ITeklaExtension<TeklaAppExtensionArgs>
{
    /// <summary>
    /// Executes the extension with the provided context and arguments.
    /// This is the main entry point that creates and displays the application window.
    /// </summary>
    /// <param name="context">The extension context providing access to Tekla environment.</param>
    /// <param name="args">Custom arguments for the extension, passed from the calling environment.</param>
    /// <param name="cancellationToken">Token for monitoring cancellation requests.</param>
    /// <returns>An IExtensionResult indicating the success or failure of the operation.</returns>
    public IExtensionResult Run(ITeklaExtensionContext context, TeklaAppExtensionArgs args, CancellationToken cancellationToken)
    {
        // Create a service provider with all required services registered
        var provider = ServiceFactory.Create(services =>
        {
            services.RegisterAppServices(args);
        });
        
        // Get the handle of the current process main window (Tekla Structures)
        var handle = Process.GetCurrentProcess().MainWindowHandle;
        
        // Show the application main window, passing the service provider and window handle
        WindowHandler.ShowWindow<MainWindow>(provider, handle);

        // Return a result indicating success with a message
        return Result.Text.Succeeded("App started");
    }
}