using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowTracker.Model
{
    internal class NativeMethods
    {
        internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr state);

        [DllImport("user32.dll")]
        internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr state);

        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr state);

        [DllImport("user32")]
        internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);
    }
}
