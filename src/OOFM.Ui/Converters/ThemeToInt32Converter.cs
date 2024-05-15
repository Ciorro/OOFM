using OOFM.Core.Settings;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace OOFM.Ui.Converters;

class ThemeToInt32Converter : MarkupExtension, IValueConverter
{
    private static ThemeToInt32Converter? _instance;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return (_instance ??= new ThemeToInt32Converter());
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Theme)
        {
            return (int)value;
        }

        throw new ArgumentException("The value is not a theme.");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int)
        {
            return (Theme)value;
        }

        throw new ArgumentException("The value is not an int.");
    }
}
