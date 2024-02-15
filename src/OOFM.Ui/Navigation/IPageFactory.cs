namespace OOFM.Ui.Navigation;
internal interface IPageFactory
{
    INavigationPage CreatePage(string pageKey);
}
