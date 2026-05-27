using RevitAppFramework.Resources;
using System.Windows;
using Wpf.Ui.Controls;

namespace RevitAppExtension;

public partial class MainWindow : FluentWindow
{
    public MainWindow(IServiceProvider serviceProvider, IContentDialogService contentDialogService, ISnackbarService snackbarService)
    {
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

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        this.Loaded -= MainWindow_Loaded;

        try
        {
            NavView.AttachGlobalResourcesToNavigationView();

            if (NavView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault(x => x.TargetPageType is not null) is { } navViewItem)
                NavView.Navigate(navViewItem.TargetPageType!);
        }
        catch (Exception)
        {
        }
    }
}
