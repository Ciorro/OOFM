namespace OOFM.Ui.Attributes;
[System.AttributeUsage(AttributeTargets.Class)]
sealed class PageKeyAttribute : Attribute
{
    public string PageKey { get; }

    public PageKeyAttribute(string key)
    {
        PageKey = key;
    }
}
