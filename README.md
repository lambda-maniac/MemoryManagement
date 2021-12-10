# Memory Management C# Library
This is a simples C# library to facilitate the manipulatation of a process memory (Reading/Writting), built over the Windows Api, using things like the `User32.dll` and `Kernel32.dll`.

### How to use:
- Declare a `using MemoryManagement;` (After referencing this project to your own) on top of your main file.
- Instantiate the `MemoryManager` class with the name of the process you want to "Debug", Ex: 
```cs
MemoryManager mm = new MemoryManager(PROCESS_NAME);
```
- Extract the modules you want, using the `GetModuleByName` method, Ex:
```cs
Module module = mm.GetModuleByName(MODULE_NAME);
```
Or, if the only thing you want is the address of the module, you can just ask for it's `BaseAddress`, Ex:
```cs
UIntPtr module_address = mm.GetModuleByName(MODULE_NAME).BaseAddress;
```
- After setting up all you need, you can read five types of data from the process: `int`, `uint`, `IntPtr`, `UIntPtr` and`float`, by simply calling 'ReadType' from the MemoryManager. Ex:
```cs
int int_val = mm.ReadInt(ADDRESS);
uint uint_val = mm.ReadUInt(ADDRESS);
IntPtr intPtr_val = mm.ReadIntPtr(ADDRESS);
UIntPtr uIntPtr_val = mm.ReadUIntPtr(ADDRESS);
float float_val = mm.ReadFloat(ADDRESS);
```
- The same method naming convention is valid for writing, with the exception that now you only have access to two data types (`int` and `float`), and you pass a second argument with the value to be written. Ex:
```cs
mm.WriteInt(ADDRESS, int_val);
mm.WriteFloat(ADDRESS, float_val);
```

### That is all, thanks for reading!
