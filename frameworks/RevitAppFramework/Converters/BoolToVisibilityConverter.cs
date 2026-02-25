namespace RevitAppFramework.Converters;
public class BoolToVisibilityConverter : global::System.Windows.Data.IValueConverter
{
    public object Convert(object value, global::System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is bool boolValue && boolValue)
        {
            return global::System.Windows.Visibility.Visible;
        }
        else
        {
            return global::System.Windows.Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, global::System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new global::System.NotImplementedException();
    }
}
