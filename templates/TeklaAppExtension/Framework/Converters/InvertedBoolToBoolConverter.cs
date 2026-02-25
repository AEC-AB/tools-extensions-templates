namespace TeklaAppExtension.Framework.Converters;
public class InvertedBoolToBoolConverter : global::System.Windows.Data.IValueConverter
{
    public object Convert(object value, global::System.Type targetType, object parameter, global::System.Globalization.CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }

        return true;
    }

    public object ConvertBack(object value, global::System.Type targetType, object parameter, global::System.Globalization.CultureInfo culture)
    {
        throw new global::System.NotImplementedException();
    }
}
