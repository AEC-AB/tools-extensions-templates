using RevitAppFramework.Resources;
using System.Windows;
using Wpf.Ui.Controls;

namespace RevitAppExtension;

/// <summary>
/// Interaction logic for MainWindow.xaml - the primary window for the Revit extension application.
/// </summary>
/// <remarks>
/// This class provides the main application window using WPF-UI's FluentWindow to create
/// a modern UI experience. It utilizes the NavigationView control for navigation between
/// different application views and sets up the required services for UI components.
/// 
/// The window implements:
/// 1. Navigation between different views (Home, About, etc.)
/// 2. Content dialog service for displaying modal dialogs
/// 3. Snackbar service for toast notifications
/// </remarks>
public partial class MainWindow : FluentWindow
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider for dependency injection and view resolution</param>
    /// <param name="contentDialogService">Service for displaying modal dialogs</param>
    /// <param name="snackbarService">Service for displaying toast notifications</param>
    /// <remarks>
    /// This constructor performs several important setup actions:
    /// 1. Loads view bindings from resources
    /// 2. Initializes the window components
    /// 3. Configures the navigation view with the service provider
    /// 4. Sets up the content dialog host and snackbar presenter
    /// 5. Attaches a Loaded event handler to perform initial navigation
    /// </remarks>
    public MainWindow(IServiceProvider serviceProvider, IContentDialogService contentDialogService, ISnackbarService snackbarService)
    {
        // Load view bindings resource dictionary
        var viewBindings = new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/{GetType().Assembly.GetName().Name};component/Resources/ViewBindings.xaml")
        };
        GlobalResourceManager.ResourceDictionary.MergedDictionaries.Add(viewBindings);

        InitializeComponent();
        DataContext = this;
        NavView.SetServiceProvider(serviceProvider);
        Loaded += MainWindow_Loaded;
        contentDialogService.SetDialogHost(this.RootContentDialogPresenter);
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
    }

    /// <summary>
    /// Event handler that's called when the window has loaded.
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="e">The event arguments</param>
    /// <remarks>
    /// This method handles the initial navigation setup when the window first loads:
    /// 1. Removes itself from the Loaded event to prevent multiple executions
    /// 2. Attempts to navigate to the first available navigation item
    /// 3. Gracefully handles any navigation failures
    /// </remarks>
    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Remove the event handler to ensure it only runs once
        this.Loaded -= MainWindow_Loaded;

        try
        {
            NavView.AttachGlobalResourcesToNavigationView();

            // Find the first navigation item with a valid target page and navigate to it
            if (NavView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(x => x.TargetPageType is not null) is { } navViewItem)
                NavView.Navigate(navViewItem.TargetPageType!);
        }
        catch (Exception)
        {
            // Navigation failures are ignored to prevent the application from crashing
            // In a production app, you might want to log this exception or handle it differently
        }
    }
}
