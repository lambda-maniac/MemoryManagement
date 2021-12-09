using System;
using System.Text;
using System.Runtime.InteropServices;

namespace MemoryManagement
{
    public static class Native
    {
        public const int LIST_MODULES_DEFAULT = 0x0  ;
        public const int LIST_MODULES_32_BIT  = 0x01 ;
        public const int LIST_MODULES_64_BIT  = 0x02 ;
        public const int LIST_MODULES_ALL     = 0x03 ;

        [StructLayout(LayoutKind.Sequential)] 
        public struct MODULE_INFO
        {
            public IntPtr lpBaseOfDll ;
            public uint   SizeOfImage ;
            public IntPtr EntryPoint  ;
        }

        [DllImport("psapi.dll")]
        public static extern bool EnumProcessModulesEx(IntPtr hProcess, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)][In][Out] IntPtr[] lphModule, int cb, [MarshalAs(UnmanagedType.U4)] out int lpcbNeeded, uint dwFilterFlag);

        [DllImport("psapi.dll")]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] uint nSize);

        [DllImport("psapi.dll")]
        public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out MODULE_INFO lpmodinfo, uint cb);
    }
}
