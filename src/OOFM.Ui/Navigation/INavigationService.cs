namespace OOFM.Ui.Navigation;
internal interface INavigationService
{
    event Action<INavigationPage> Navigated;
    INavigationPage? CurrentPage { get; }

    void Navigate(INavigationPage page);
    void Back();
    void Next();
}
