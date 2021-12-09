using System;

namespace MemoryManagement
{
    public class Module
    {
        public string ModuleName  ;
        public IntPtr BaseAddress ;
        public uint   Size        ;

        public Module(string moduleName, IntPtr baseAddress, uint size)
        {
            ModuleName  = moduleName  ;
            BaseAddress = baseAddress ;
            Size        = size        ;
        }
    }
}
