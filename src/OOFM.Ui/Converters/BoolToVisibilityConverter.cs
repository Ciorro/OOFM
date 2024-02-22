using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace OOFM.Ui.Converters;
internal class BoolToVisibilityConverter : MarkupExtension, IValueConverter
{
    private static BoolToVisibilityConverter? _instance;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return (_instance ??= new BoolToVisibilityConverter());
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value as bool? == true)
            return Visibility.Visible;
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as Visibility?) == Visibility.Visible;
    }
}
