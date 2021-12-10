using System;
using System.Runtime.InteropServices;

namespace MemoryManagement
{
    public static class User32
    {
        public const int WS_EX_TRANSPARENT = (0x000020);   
        public const int WS_EX_LAYERED     = (0x080000);
        public const int GWL_EXSTYLE       = (-0000020);

        public const int VK_LCONTROL = (0xA2);
        public const int VK_RBUTTON  = (0x02);
        public const int VK_LBUTTON  = (0x01);
        public const int VK_DELETE   = (0x2E);
        public const int VK_LSHIFT   = (0xA0);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left   ;
            public int top    ;
            public int right  ;
            public int bottom ;
        }

        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(int vKeys);
        public static bool IsKeyDown(int vKey) => (GetAsyncKeyState(vKey) & 0x8000) != 0;

        [DllImport("User32.dll")]
        public static extern int GetWindowLong(UIntPtr hWnd, int nIndex);

        [DllImport("User32.dll")]
        public static extern int SetWindowLong(UIntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll")] [return : MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(UIntPtr hWnd, out RECT lpRect);
    }
}
