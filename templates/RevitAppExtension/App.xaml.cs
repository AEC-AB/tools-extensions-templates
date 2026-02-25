using RevitAppFramework.Helpers;
using System.Windows;
using System.Windows.Interop;
using RevitAppFramework.Design;

namespace RevitAppExtension;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        var args = new RevitAppExtensionArgs
        {
            InitialComment = "Hello from the design project!",
        };

        var provider = DesignServiceFactory.Create(services =>
        {
            services.RegisterAppServices(args, useDesignQueryHandlers: true);
        });

        var hostWindow = new Window();

        var mainWindowHandle = new WindowInteropHelper(hostWindow).Handle;

        WindowHandler.ShowWindow<MainWindow>(provider, mainWindowHandle);
    }
}
