using System;

namespace MemoryManagement
{
    public class Module
    {
        public string  ModuleName  ;
        public UIntPtr BaseAddress ;
        public uint    Size        ;

        public Module(string moduleName, UIntPtr baseAddress, uint size)
        {
            ModuleName  = moduleName  ;
            BaseAddress = baseAddress ;
            Size        = size        ;
        }
    }
}
