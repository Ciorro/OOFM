using FluentIcons.Common;
using System.Windows;
using System.Windows.Controls;

namespace OOFM.Ui.Controls;

public class NavigationButton : RadioButton
{
    static NavigationButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(NavigationButton),
            new FrameworkPropertyMetadata(typeof(NavigationButton))
        );
    }

    public Symbol Icon
    {
        get { return (Symbol)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        "Icon",
        typeof(Symbol),
        typeof(NavigationButton)
    );
}
