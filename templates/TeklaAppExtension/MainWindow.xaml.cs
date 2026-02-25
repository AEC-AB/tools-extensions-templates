using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeklaAppExtension.Framework.Resources;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace TeklaAppExtension
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
}
