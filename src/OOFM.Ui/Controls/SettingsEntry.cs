using FluentIcons.Common;
using System.Windows;
using System.Windows.Controls;

namespace OOFM.Ui.Controls;

public class SettingsEntry : ContentControl
{
    static SettingsEntry()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(SettingsEntry), 
            new FrameworkPropertyMetadata(typeof(SettingsEntry))
        );
    }
    
    public Symbol Symbol
    {
        get => (Symbol)GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    public static readonly DependencyProperty SymbolProperty = 
        DependencyProperty.Register(nameof(Symbol), typeof(Symbol), typeof(SettingsEntry));


    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(nameof(Header), typeof(string), typeof(SettingsEntry));



    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(string), typeof(SettingsEntry));
}
