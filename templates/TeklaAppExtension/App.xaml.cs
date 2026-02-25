using System.Windows;
using System.Windows.Interop;
using TeklaAppExtension.Framework.Helpers;
using TeklaAppExtension;

namespace TeklaAppExtension;

/// <summary>
/// Interaction logic for App.xaml
/// The main application class that initializes the application environment
/// and creates the main window when launched in design mode.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes a new instance of the App class.
    /// Sets up the application environment, creates the dependency injection container,
    /// and launches the main window.
    /// </summary>
    public App()
    {
        // Create application arguments with an initial default comment
        // This is used for design-time testing only
        var args = new TeklaAppExtensionArgs
        {
            InitialComment = "Hello from the design project!",
        };

        // Create a service provider with all required services registered
        var provider = ServiceFactory.Create(services =>
        {
            services.RegisterAppServices(args);
        });

        // Create a host window to get a window handle
        var hostWindow = new Window();

        // Get the window handle for interop operations
        var mainWindowHandle = new WindowInteropHelper(hostWindow).Handle;

        // Show the main application window using the service provider and window handle
        WindowHandler.ShowWindow<MainWindow>(provider, mainWindowHandle);
    }
}