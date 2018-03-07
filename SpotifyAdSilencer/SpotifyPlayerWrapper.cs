using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Get windows of Spotify Player Window
        /// </summary>
        /// <returns></returns>
        public Dictionary<IntPtr, string> GetSpotifyPlayerWindowByClassName()
        {
            Dictionary<IntPtr, string> handles = new Dictionary<IntPtr, string>();
            foreach (KeyValuePair<IntPtr, string> window in GetOpenWindowsByClassName())
            {
                IntPtr handle = window.Key;
                string title = window.Value;
                StringBuilder windowName = null;
                if (window.Value.Contains("SpotifyMain"))
                {
                    //Get a handle for the Calculator Application main window
                    int hwnd = FindWindow("SpotifyMainWindow", null);//null, "SpotifyMainWindow");
                    if (hwnd == 0)
                    {
                        s_Log.Info("Not found!");
                    }
                    else
                    {
                        int length = GetWindowTextLength(handle);
                        windowName = new StringBuilder(length);
                        GetWindowText(handle, windowName, length + 1);
                        s_Log.Info("Class Name: " + title);
                        handles.Add(handle, windowName.ToString());
                    }
                    s_Log.Info("handle name: " + handle + " | class name: " + title + " | window name: " + windowName);
                }
            }
            return handles;
        }
        
        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        private static IDictionary<IntPtr, string> GetOpenWindowsByClassName()
        {
            IntPtr shellWindow = GetShellWindow();
            Dictionary<IntPtr, string> windows = new Dictionary<IntPtr, string>();

            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                if (hWnd == shellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                StringBuilder className = new StringBuilder();
                className.Append("SpotifyMainWindow");
                GetClassName(hWnd, className, className.Length + 1);
                windows[hWnd] = className.ToString();
                return true;

            }, 0);

            return windows;
        }
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