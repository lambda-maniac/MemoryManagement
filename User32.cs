using System;
using System.Runtime.InteropServices;

namespace MemoryManagement
{
    public static class User32
    {
        public const int WS_EX_TRANSPARENT = (0x000020);   
        public const int WS_EX_LAYERED     = (0x080000);
        public const int GWL_EXSTYLE       = (-0000020);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left   ;
            public int top    ;
            public int right  ;
            public int bottom ;
        }

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")] [return : MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
    }
}
