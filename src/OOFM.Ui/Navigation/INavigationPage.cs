namespace OOFM.Ui.Navigation;

internal interface INavigationPage
{
    void OnInitialized() { }
    void OnResumed() { }
    void OnPaused() { }
}
