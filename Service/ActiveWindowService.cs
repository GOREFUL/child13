using System.Runtime.InteropServices;
using System.Text;

namespace child13.Service
{
    public static class ActiveWindowService
    {
        public static string _GetActiveWindow()
        {
            string name = CheckActiveWindow();
            return name;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_STYLE = -16;
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public static void MakeInvisible()
        {
            IntPtr handle = GetForegroundWindow();
            int currentStyle = GetWindowLong(handle, GWL_STYLE);
            SetWindowLong(handle, GWL_STYLE, currentStyle & ~WS_VISIBLE);
            SetWindowLong(handle, GWL_STYLE, currentStyle | WS_EX_TOOLWINDOW);
        }

        public static void MakeVisible()
        {
            IntPtr handle = GetForegroundWindow();
            int currentStyle = GetWindowLong(handle, GWL_STYLE);
            SetWindowLong(handle, GWL_STYLE, currentStyle | WS_VISIBLE);
            SetWindowLong(handle, GWL_STYLE, currentStyle & ~WS_EX_TOOLWINDOW);
        }

        private static string CheckActiveWindow()
        {
            IntPtr handle = GetForegroundWindow();
            int length = GetWindowTextLength(handle);
            StringBuilder windowTitle = new StringBuilder(length + 1);
            GetWindowText(handle, windowTitle, windowTitle.Capacity);
            string programName = windowTitle.ToString();
            return programName;
        }
    }
}
