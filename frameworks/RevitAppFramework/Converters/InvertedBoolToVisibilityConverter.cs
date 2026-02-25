namespace RevitAppFramework.Converters;
public class InvertedBoolToVisibilityConverter : global::System.Windows.Data.IValueConverter
{
    public object Convert(object value, global::System.Type targetType, object parameter, global::System.Globalization.CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? global::System.Windows.Visibility.Collapsed : global::System.Windows.Visibility.Visible;
        }

        return global::System.Windows.Visibility.Visible;
    }

    public object ConvertBack(object value, global::System.Type targetType, object parameter, global::System.Globalization.CultureInfo culture)
    {
        throw new global::System.NotImplementedException();
    }
}
