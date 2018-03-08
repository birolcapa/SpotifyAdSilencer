using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static Dictionary<IntPtr, MicrosoftSpyValues> GetSpotifyPlayerWindowByProcessName()
        {
            Dictionary<IntPtr, MicrosoftSpyValues> handles = new Dictionary<IntPtr, MicrosoftSpyValues>();
            foreach (KeyValuePair<IntPtr, MicrosoftSpyValues> window in GetOpenedWindows())
            {
                IntPtr handle = window.Key;
                MicrosoftSpyValues microsoftSpyValues = window.Value;
                
                if (microsoftSpyValues.ProcessName.Contains("Spotify.exe"))
                {
                    handles.Add(handle, microsoftSpyValues);
                    s_Log.Debug("handle name: " + handle +
                                " | class name: " + microsoftSpyValues.ClassName +
                                " | window name: " + microsoftSpyValues.WindowName +
                                " | process name: " + microsoftSpyValues.ProcessName);
                }
            }
            return handles;
        }

        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        private static IDictionary<IntPtr, MicrosoftSpyValues> GetOpenedWindows()
        {
            IntPtr shellWindow = GetShellWindow();
            Dictionary<IntPtr, MicrosoftSpyValues> windows = new Dictionary<IntPtr, MicrosoftSpyValues>();

            EnumWindows(delegate (IntPtr hWnd, int lParam)
                        {
                            if (hWnd == shellWindow)
                                return true;
                            if (!IsWindowVisible(hWnd))
                                return true;

                            int length = GetWindowTextLength(hWnd);
                            if (length == 0)
                                return true;

                            StringBuilder windowsName = new StringBuilder(length);
                            GetWindowText(hWnd, windowsName, length + 1);

                            windows[hWnd] = new MicrosoftSpyValues();
                            windows[hWnd].WindowName = windowsName.ToString();

                            string className = GetWindowClass(hWnd);
                            windows[hWnd].ClassName = className;

                            string processName = GetProcessPath(hWnd);
                            windows[hWnd].ProcessName = processName;

                            return true;

                        }, 0);

            return windows;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/38233596/how-do-i-get-the-length-of-a-window-class-name-so-i-know-how-large-of-a-buffer-t
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        private static string GetWindowClass(IntPtr hWnd)
        {
            const int size = 256;
            StringBuilder buffer = new StringBuilder(size + 1);

            if (GetClassName(hWnd, buffer, size) == 0)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            return buffer.ToString();
        }

        /// <summary>
        /// Taken from: https://social.msdn.microsoft.com/Forums/windowsdesktop/en-US/3d93c3fb-9882-4924-9b19-06c3de11a63b/get-a-executable-path-from-a-window-handle?forum=windowssecurity
        ///  Retrieves the Path of a running process. 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns>The path of process</returns>
        private static string GetProcessPath(IntPtr hwnd)
        {
            try
            {
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                Process proc = Process.GetProcessById((int)pid); //Gets the process by ID. 
                return proc.MainModule.FileName;    //Returns the path. 
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Retrieves the identifier of the thread that created the specified window and, optionally, the identifier of the process that created the window. 
        /// http://msdn.microsoft.com/en-us/library/ms633522(VS.85).aspx
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

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
    }
}