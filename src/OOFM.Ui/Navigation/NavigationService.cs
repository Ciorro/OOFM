namespace OOFM.Ui.Navigation;
internal class NavigationService : INavigationService, IDisposable
{
    public event Action<INavigationPage>? Navigated;

    private List<INavigationPage> _pages;
    private int _currentPageIndex = -1;

    public NavigationService()
    {
        _pages = new List<INavigationPage>();
    }

    public INavigationPage? CurrentPage
    {
        get => _pages.ElementAtOrDefault(_currentPageIndex);
    }

    public void Navigate(INavigationPage page)
    {
        if (_currentPageIndex != _pages.Count - 1)
        {
            foreach (var toDispose in _pages.Skip(_currentPageIndex + 1))
            {
                (toDispose as IDisposable)?.Dispose();
            }

            int diff = _pages.Count - _currentPageIndex - 1;
            _pages.RemoveRange(_currentPageIndex + 1, diff);
            _currentPageIndex = _pages.Count - 1;
        }

        if (!_pages.Contains(page))
        {
            page.OnInitialized();
        }
        _pages.Add(page);

        Next();
    }

    public void Back()
    {
        if (_currentPageIndex < 1)
            return;

        CurrentPage?.OnPaused();
        _currentPageIndex--;
        CurrentPage?.OnResumed();

        Navigated?.Invoke(CurrentPage!);
    }

    public void Next()
    {
        if (_currentPageIndex >= _pages.Count - 1)
            return;

        CurrentPage?.OnPaused();
        _currentPageIndex++;
        CurrentPage?.OnResumed();

        Navigated?.Invoke(CurrentPage!);
    }

    public void Dispose()
    {
        _pages.Clear();
    }
}
