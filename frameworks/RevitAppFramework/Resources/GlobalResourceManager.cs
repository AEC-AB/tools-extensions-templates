namespace RevitAppFramework.Resources;

public static class GlobalResourceManager
{
    private static readonly global::System.Collections.Concurrent.ConcurrentDictionary<string, global::System.Windows.ResourceDictionary> _resources = [];
    private const string ControlsSourceKey = "RevitAppFramework.Resources.ControlsSource";
    private const string ThemeSourceKeyPrefix = "RevitAppFramework.Resources.ThemeSource.";
    private static readonly object _sourceCacheLock = new();

    public static global::System.Windows.ResourceDictionary ResourceDictionary
    {
        get
        {
            var threadName = global::System.Threading.Thread.CurrentThread.Name;

            if (_resources.TryGetValue(threadName, out var resourceDictionary))
                return resourceDictionary;

            // Create resource dictionary and add converters programmatically
            resourceDictionary = new global::System.Windows.ResourceDictionary
            {
                // Add converters directly
                { "BoolToVisibilityConverter", new global::RevitAppFramework.Converters.BoolToVisibilityConverter() },
                { "InvertedBoolToVisibilityConverter", new global::RevitAppFramework.Converters.InvertedBoolToVisibilityConverter() },
                { "InvertedBoolToBoolConverter", new global::RevitAppFramework.Converters.InvertedBoolToBoolConverter() }
            };

            // Add theme resources
            var appTheme = GetApplicationTheme();
            AddThemeDictionary(resourceDictionary, appTheme);
            AddControlsDictionary(resourceDictionary);

            _resources[threadName] = resourceDictionary;

            return resourceDictionary;
        }
    }

    private static void AddThemeDictionary(global::System.Windows.ResourceDictionary target, global::Wpf.Ui.Appearance.ApplicationTheme appTheme)
    {
        var key = ThemeSourceKeyPrefix + appTheme;

        lock (_sourceCacheLock)
        {
            if (global::System.AppDomain.CurrentDomain.GetData(key) is global::System.Uri cachedThemeSource)
            {
                target.MergedDictionaries.Add(new global::System.Windows.ResourceDictionary
                {
                    Source = cachedThemeSource
                });
                return;
            }

            var themes = new global::Wpf.Ui.Markup.ThemesDictionary { Theme = appTheme };
            target.MergedDictionaries.Add(themes);

            if (themes.Source is global::System.Uri themeSource)
                global::System.AppDomain.CurrentDomain.SetData(key, themeSource);
        }
    }

    private static void AddControlsDictionary(global::System.Windows.ResourceDictionary target)
    {
        lock (_sourceCacheLock)
        {
            if (global::System.AppDomain.CurrentDomain.GetData(ControlsSourceKey) is global::System.Uri cachedControlsSource)
            {
                target.MergedDictionaries.Add(new global::System.Windows.ResourceDictionary
                {
                    Source = cachedControlsSource
                });
                return;
            }

            var controls = new global::Wpf.Ui.Markup.ControlsDictionary();
            target.MergedDictionaries.Add(controls);

            if (controls.Source is global::System.Uri controlsSource)
                global::System.AppDomain.CurrentDomain.SetData(ControlsSourceKey, controlsSource);
        }
    }

    private static global::Wpf.Ui.Appearance.ApplicationTheme GetApplicationTheme()
    {
        global::Wpf.Ui.Appearance.SystemThemeManager.UpdateSystemThemeCache();
        var systemTheme = global::Wpf.Ui.Appearance.SystemThemeManager.GetCachedSystemTheme();
        global::Wpf.Ui.Appearance.ApplicationTheme themeToSet = global::Wpf.Ui.Appearance.ApplicationTheme.Light;

        if (systemTheme is
            global::Wpf.Ui.Appearance.SystemTheme.Dark or
            global::Wpf.Ui.Appearance.SystemTheme.CapturedMotion or
            global::Wpf.Ui.Appearance.SystemTheme.Glow)
        {
            themeToSet = global::Wpf.Ui.Appearance.ApplicationTheme.Dark;
        }
        else if (systemTheme is
            global::Wpf.Ui.Appearance.SystemTheme.HC1 or
            global::Wpf.Ui.Appearance.SystemTheme.HC2 or
            global::Wpf.Ui.Appearance.SystemTheme.HCBlack or
            global::Wpf.Ui.Appearance.SystemTheme.HCWhite)
        {
            themeToSet = global::Wpf.Ui.Appearance.ApplicationTheme.HighContrast;
        }
        return themeToSet;
    }

    public static void AttachGlobalResourcesToNavigationView(this global::Wpf.Ui.Controls.NavigationView NavView)
    {
        // Define the event handlers separately so we can unsubscribe
        global::System.Windows.RoutedEventHandler? navViewLoadedHandler = null;
        global::System.Windows.RoutedEventHandler? contentPresenterLoadedHandler = null;

        navViewLoadedHandler = (s, e) =>
        {
            // Unsubscribe from the NavView.Loaded event
            NavView.Loaded -= navViewLoadedHandler;

            // Find the PART_NavigationViewContentPresenter in the visual tree
            if (NavView.Template.FindName("PART_NavigationViewContentPresenter", NavView) is global::System.Windows.Controls.Frame contentPresenter)
            {
                contentPresenterLoadedHandler = (sender, args) =>
                {
                    // Unsubscribe from the contentPresenter.Loaded event
                    contentPresenter.Loaded -= contentPresenterLoadedHandler;

                    // Get the first child of the Frame in the visual tree
                    if (global::System.Windows.Media.VisualTreeHelper.GetChild(contentPresenter, 0) is global::System.Windows.FrameworkElement child)
                    {
                        child.Resources.MergedDictionaries.Add(ResourceDictionary);
                    }
                };

                // Subscribe to the Frame's Loaded event
                contentPresenter.Loaded += contentPresenterLoadedHandler;
            }
        };

        // Subscribe to the NavigationView's Loaded event
        NavView.Loaded += navViewLoadedHandler;
    }

}
