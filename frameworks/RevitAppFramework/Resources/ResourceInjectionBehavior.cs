namespace RevitAppFramework.Resources;

public static class ResourceInjectionBehavior
{
    public static readonly global::System.Windows.DependencyProperty InjectResourcesProperty =
        global::System.Windows.DependencyProperty.RegisterAttached(
            "InjectResources",
            typeof(bool),
            typeof(global::RevitAppFramework.Resources.ResourceInjectionBehavior),
            new global::System.Windows.PropertyMetadata(false, OnInjectResourcesChanged));

    public static bool GetInjectResources(global::System.Windows.DependencyObject obj)
    {
        return (bool)obj.GetValue(InjectResourcesProperty);
    }

    public static void SetInjectResources(global::System.Windows.DependencyObject obj, bool value)
    {
        obj.SetValue(InjectResourcesProperty, value);
    }

    private static void OnInjectResourcesChanged(global::System.Windows.DependencyObject d, global::System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (d is global::System.Windows.FrameworkElement element && (bool)e.NewValue)
        {
            element.Resources.MergedDictionaries.Add(global::RevitAppFramework.Resources.GlobalResourceManager.ResourceDictionary);
            element.SetResourceReference(global::System.Windows.Controls.Control.ForegroundProperty, "TextFillColorPrimaryBrush");
        }
    }
}
