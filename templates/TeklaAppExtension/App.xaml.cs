using System.Windows;
using System.Windows.Interop;
using TeklaAppExtension.Framework.Helpers;
using TeklaAppExtension;

namespace TeklaAppExtension;

public partial class App : Application
{
    public App()
    {
        var args = new TeklaAppExtensionArgs
        {
            InitialComment = "Hello from the design project!",
        };

        var provider = ServiceFactory.Create(services =>
        {
            services.RegisterAppServices(args);
        });

        var hostWindow = new Window();
        var mainWindowHandle = new WindowInteropHelper(hostWindow).Handle;
        WindowHandler.ShowWindow<MainWindow>(provider, mainWindowHandle);
    }
}