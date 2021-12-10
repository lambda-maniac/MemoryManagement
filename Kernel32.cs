using System;
using System.Runtime.InteropServices;

namespace MemoryManagement
{
    public static class Kernel32
    {
        public const int PROCESS_VM_OPERATION = (0x0008);
        public const int PROCESS_VM_READ      = (0x0010);
        public const int PROCESS_VM_WRITE     = (0x0020);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("Kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);

        [DllImport("Kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytetsWritten);

    }
}
