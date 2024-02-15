using System.Runtime.InteropServices;

namespace OOFM.Ui.Native;

internal static class NativeData
{
    public const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
    public const int DWMWA_CAPTION_COLOR = 35;
    public const int DWMWA_SYSTEM_BACKDROP_TYPE = 38;
    public const int WCA_ACCENT_POLICY = 19;
    public const int ACCENT_DISABLED = 0;
    public const int ACCENT_ENABLE_GRADIENT = 1;
    public const int ACCENT_ENABLE_TRANSPARENTGRADIENT = 2;
    public const int ACCENT_ENABLE_BLURBEHIND = 3;
    public const int ACCENT_ENABLE_ACRYLICBLURBEHIND = 4;
    public const int ACCENT_INVALID_STATE = 5;

    [StructLayout(LayoutKind.Sequential)]
    public struct WINCOMPATTRDATA
    {
        public int nAttribute;
        public nint pData;
        public int cbSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ACCENTPOLICY
    {
        public int nAccentState;
        public int nFlags;
        public int nColor;
        public int nAnimationId;
    }
}
