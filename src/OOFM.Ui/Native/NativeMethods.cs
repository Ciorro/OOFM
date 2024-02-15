using System.Runtime.InteropServices;
using static OOFM.Ui.Native.NativeData;

namespace OOFM.Ui.Native;

internal static class NativeMethods
{
    //User32
    [DllImport("user32.dll")]
    public static extern int GetWindowLong(nint hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern int SetWindowCompositionAttribute(nint hWnd, ref WINCOMPATTRDATA pAttrData);


    // Dwmapi
    [DllImport("dwmapi.dll")]
    public static extern int DwmSetWindowAttribute(nint hWnd, int dwAttribute, ref uint pvAttribute, int cbAttribute);
}
