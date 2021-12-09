using System;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MemoryManagement
{
    public class MemoryManager
    {
        public Process process;
        public IntPtr  processHandle;

        public MemoryManager(string processName)
        {
            process       = Process.GetProcessesByName(processName).FirstOrDefault();
            processHandle = Kernel32.OpenProcess(Kernel32.PROCESS_VM_READ | Kernel32.PROCESS_VM_WRITE, false, process.Id);
        }

        public IntPtr ReadIntPtr(IntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return (IntPtr)BitConverter.ToInt32(buffer, 0);
        }

        public int ReadInt(IntPtr lpBaseAddress)
        {
            byte[] buffer = new byte[4];

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, 4, out _);

            return BitConverter.ToInt32(buffer, 0);
        }

        // TODO: Implement reading/writing of other types
        /* Well, I tried...
        public Type Read<Type>(IntPtr lpBaseAddress) where Type : unmanaged
        {
            int size = Marshal.SizeOf<Type>();

            object buffer = default(Type);

            Kernel32.ReadProcessMemory(processHandle, lpBaseAddress, buffer, size, out int lpNumberOfBytesRead);

            return lpNumberOfBytesRead == size ? (Type) buffer : default;
        }
        */

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
