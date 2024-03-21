using System.Windows.Controls;
namespace OOFM.Ui.Views.Pages;

public partial class PlayerPageView : UserControl
{
    const int BreakpointWidth = 768;

    public PlayerPageView()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
    {
        if (ActualWidth < BreakpointWidth)
        {
            SimilarChannels.Visibility = System.Windows.Visibility.Collapsed;
        }
        else
        {
            SimilarChannels.Visibility = System.Windows.Visibility.Visible;
        }

        SimilarChannels.Width = ActualWidth * 0.4;
    }
}
