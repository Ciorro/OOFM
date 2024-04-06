namespace OOFM.Ui.Navigation.Attributes;
[AttributeUsage(AttributeTargets.Class)]
sealed class PageKeyAttribute : Attribute
{
    public string PageKey { get; }

    public PageKeyAttribute(string key)
    {
        PageKey = key;
    }
}
