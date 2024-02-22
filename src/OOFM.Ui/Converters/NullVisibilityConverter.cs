using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace OOFM.Ui.Converters;
internal class NullVisibilityConverter : MarkupExtension, IValueConverter
{
    private static NullVisibilityConverter? _instance;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return (_instance ??= new NullVisibilityConverter());
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is not null ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}