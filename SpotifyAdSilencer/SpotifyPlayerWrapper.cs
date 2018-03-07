using System;
using System.Runtime.InteropServices;
using System.Text;
using log4net;

namespace SpotifyAdSilencer
{
    /// <summary>
    /// Spotify player wrapper
    /// </summary>
    public class SpotifyPlayerWrapper
    {
        /// <summary>
        /// Logger for Wrapper
        /// </summary>
        private static readonly ILog s_Log = LogManager.GetLogger(typeof(SpotifyPlayerWrapper));
        
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("user32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.DLL")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.DLL")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.DLL")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.DLL")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.DLL")]
        private static extern int FindWindow(String lpClassName, String lpWindowName);

    }
}