using ModernWpf;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;
using static OOFM.Ui.Native.NativeData;
using static OOFM.Ui.Native.NativeMethods;

namespace OOFM.Ui.Windows;

public partial class FluentWindow : Window
{
    public FluentWindow()
    {
        InitializeComponent();

        nint hWnd = new WindowInteropHelper(this).EnsureHandle();

        SetWindowTheme(hWnd);
        EnableMica(hWnd);
        ExtendGlassFrame();

        ThemeManager.Current.ActualApplicationThemeChanged += (_, _) =>
        {
            SetWindowTheme(hWnd);
        };
    }

    private void EnableMica(nint hWnd)
    {
        uint micaValue = 2;

        DwmSetWindowAttribute(
            hWnd: hWnd,
            dwAttribute: DWMWA_SYSTEM_BACKDROP_TYPE,
            pvAttribute: ref micaValue,
            cbAttribute: Marshal.SizeOf<uint>()
        );
    }

    private void SetWindowTheme(nint hWnd, ApplicationTheme? theme = null)
    {
        uint themeValue = (uint)(theme ?? ThemeManager.Current.ActualApplicationTheme);
        uint captionColor = 0xFFFFFFFE;

        DwmSetWindowAttribute(
            hWnd: hWnd,
            dwAttribute: DWMWA_USE_IMMERSIVE_DARK_MODE,
            pvAttribute: ref themeValue,
            cbAttribute: Marshal.SizeOf<uint>()
        );

        DwmSetWindowAttribute(
            hWnd: hWnd,
            dwAttribute: DWMWA_CAPTION_COLOR,
            pvAttribute: ref captionColor,
            cbAttribute: Marshal.SizeOf<uint>()
        );
    }

    private void ExtendGlassFrame()
    {
        WindowChrome.SetWindowChrome(this, new WindowChrome
        {
            GlassFrameThickness = new Thickness(-1),
            NonClientFrameEdges = NonClientFrameEdges.Bottom
        });

        SizeChanged += (_, __) =>
        {
            Container.Margin = new Thickness(WindowState == WindowState.Maximized ? 8 : 0);
        };
    }
}
