using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace OOFM.Ui.Converters;

internal class CollectionToVisibilityConverter : MarkupExtension, IValueConverter
{
    private static CollectionToVisibilityConverter? _instance;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _instance ??= new CollectionToVisibilityConverter();
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ICollection collection)
        {
            return collection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
