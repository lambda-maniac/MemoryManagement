using System;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace MemoryManagement
{
    public class MemoryManager
    {
        public Process process;
        public IntPtr  processHandle;

        public MemoryManager(string processName)
        {
            process       = Process.GetProcessesByName(processName).FirstOrDefault();
            processHandle = Kernel32.OpenProcess(Kernel32.PROCESS_VM_READ | Kernel32.PROCESS_VM_WRITE | Kernel32.PROCESS_VM_OPERATION, false, process.Id);
        }

        public IntPtr ReadIntPtr(IntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return (IntPtr) BitConverter.ToInt32(buffer, 0);
        }

        public IntPtr ReadIntPtr(UIntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return (IntPtr) BitConverter.ToInt32(buffer, 0);
        }

        public UIntPtr ReadUIntPtr(IntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return (UIntPtr) BitConverter.ToUInt32(buffer, 0);
        }

        public UIntPtr ReadUIntPtr(UIntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return (UIntPtr) BitConverter.ToUInt32(buffer, 0);
        }

        public int ReadInt(IntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return BitConverter.ToInt32(buffer, 0);
        }

        public int ReadInt(UIntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return BitConverter.ToInt32(buffer, 0);
        }

        public uint ReadUInt(IntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public uint ReadUInt(UIntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public bool WriteInt(IntPtr lpBaseAddress, int value)
        {
            return Kernel32.WriteProcessMemory(processHandle, lpBaseAddress, BitConverter.GetBytes(value), 4, out _);
        }

        public bool WriteUInt(IntPtr lpBaseAddress, uint value)
        {
            return Kernel32.WriteProcessMemory(processHandle, lpBaseAddress, BitConverter.GetBytes(value), 4, out _);
        }

        public bool WriteInt(UIntPtr lpBaseAddress, int value)
        {
            return Kernel32.WriteProcessMemory(processHandle, lpBaseAddress, BitConverter.GetBytes(value), 4, out _);
        }
        
        public bool WriteUInt(UIntPtr lpBaseAddress, uint value)
        {
            return Kernel32.WriteProcessMemory(processHandle, lpBaseAddress, BitConverter.GetBytes(value), 4, out _);
        }

        public float ReadFloat(IntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return BitConverter.ToSingle(buffer, 0);
        }

        public float ReadFloat(UIntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return BitConverter.ToSingle(buffer, 0);
        }

        public bool WriteFloat(IntPtr lpBaseAddress, float value)
        {
            return Kernel32.WriteProcessMemory(processHandle, lpBaseAddress, BitConverter.GetBytes(value), 4, out _);
        }

        public bool WriteFloat(UIntPtr lpBaseAddress, float value)
        {
            return Kernel32.WriteProcessMemory(processHandle, lpBaseAddress, BitConverter.GetBytes(value), 4, out _);
        }

        public Module GetModuleByName(string moduleName)
        {
            IntPtr[] modulePointers = new IntPtr[0] ;
            int      bytesNeeded    = 0             ;

            if (!Native.EnumProcessModulesEx(process.Handle, modulePointers, 0, out bytesNeeded, Native.LIST_MODULES_ALL))
                return null;

            int numberOfModules = bytesNeeded / IntPtr.Size;
                modulePointers  = new IntPtr[numberOfModules];

            if (Native.EnumProcessModulesEx(process.Handle, modulePointers, bytesNeeded, out bytesNeeded, Native.LIST_MODULES_ALL))
            {
                for (int index = 0; index < numberOfModules; ++index)
                {
                    StringBuilder moduleFilePath = new StringBuilder(1024);
                    Native.GetModuleFileNameEx(process.Handle, modulePointers[index], moduleFilePath, (uint)moduleFilePath.Capacity);;

                    if (moduleName == System.IO.Path.GetFileName(moduleFilePath.ToString()))
                    {
                        Native.MODULE_INFO module = new Native.MODULE_INFO();
                        Native.GetModuleInformation(process.Handle, modulePointers[index], out module, (uint)(IntPtr.Size * (modulePointers.Length)));

                        return new Module(moduleName, module.lpBaseOfDll, module.SizeOfImage);
                    }
                }
            }

            return null;
        }
    }
}
