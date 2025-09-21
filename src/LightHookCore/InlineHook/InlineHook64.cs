namespace LibHook
{
    using System;
    using System.Runtime.InteropServices;

    public partial class InlineHook64 : InlineHook
    {
        private int originalMemoryProtection;
        private IntPtr targetFunctionPtr;
        private IntPtr hookFunctionPtr;
        private byte[] originalFunctionBytes;
        private byte[] hookJumpBytes;

        private abstract partial class NativeMethods
        {
            public const int MEMORY_PROTECT_READWRITE_EXECUTE = 64;
            public static readonly IntPtr INVALID_PTR = IntPtr.Zero;

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr GetModuleHandle(string moduleName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr LoadLibrary(string libraryPath);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetProcAddress(IntPtr moduleHandle, string procedureName);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool VirtualProtect(IntPtr address, int size, int newProtection, out int oldProtection);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool FreeLibrary([In] IntPtr libraryHandle);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool WriteProcessMemory(IntPtr processHandle, IntPtr baseAddress, byte[] buffer, uint size, IntPtr bytesWritten);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetCurrentProcess();
        }
    }

    public partial class InlineHook64 : InlineHook
    {
        public override void Install(IntPtr originalFunction, IntPtr hookFunction)
        {
            if (originalFunction == NativeMethods.INVALID_PTR || hookFunction == NativeMethods.INVALID_PTR)
                throw new Exception("Invalid function pointer");

            if (!SetMemoryProtection(originalFunction, NativeMethods.MEMORY_PROTECT_READWRITE_EXECUTE))
                throw new Exception("Failed to set memory protection");

            this.targetFunctionPtr = originalFunction;
            this.hookFunctionPtr = hookFunction;
            this.originalFunctionBytes = ExtractFunctionHeader(originalFunction);
            this.hookJumpBytes = CreateJumpInstruction(hookFunction);

            if (!WriteMemory(this.hookJumpBytes, originalFunction, 12))
                throw new Exception("Failed to write jump instruction");
        }

        public override void Suspend()
        {
            if (this.targetFunctionPtr == NativeMethods.INVALID_PTR)
                throw new Exception("Hook not installed");
            WriteMemory(this.originalFunctionBytes, targetFunctionPtr, 12);
        }

        public override void Resume()
        {
            if (this.targetFunctionPtr == NativeMethods.INVALID_PTR)
                throw new Exception("Hook not installed");
            WriteMemory(this.hookJumpBytes, targetFunctionPtr, 12);
        }

        public override void Uninstall()
        {
            if (this.targetFunctionPtr == NativeMethods.INVALID_PTR)
                throw new Exception("Hook not installed");

            if (!WriteMemory(this.originalFunctionBytes, targetFunctionPtr, 12))
                throw new Exception("Failed to restore original code");

            if (!SetMemoryProtection(targetFunctionPtr, originalMemoryProtection))
                throw new Exception("Failed to restore memory protection");

            this.originalMemoryProtection = 0;
            this.originalFunctionBytes = null;
            this.hookJumpBytes = null;
            this.targetFunctionPtr = NativeMethods.INVALID_PTR;
            this.hookFunctionPtr = NativeMethods.INVALID_PTR;
        }
    }

    public partial class InlineHook64 : InlineHook
    {
        private byte[] ExtractFunctionHeader(IntPtr functionPtr)
        {
            byte[] headerBytes = new byte[12];
            Marshal.Copy(functionPtr, headerBytes, 0, 12);
            return headerBytes;
        }

        private byte[] CreateJumpInstruction(IntPtr jumpTarget)
        {
            byte[] addressBytes = ConvertIntPtrToBytes((long)jumpTarget);
            byte[] jumpInstruction = CombineByteArrays(new byte[] { 0x48, 0xB8 }, addressBytes);
            return CombineByteArrays(jumpInstruction, new byte[] { 0xFF, 0xE0 });
        }

        private byte[] ConvertIntPtrToBytes(long value)
        {
            byte[] buffer = new byte[8];
            IntPtr tempPtr = Marshal.AllocHGlobal(8);
            Marshal.WriteInt64(tempPtr, value);
            Marshal.Copy(tempPtr, buffer, 0, 8);
            Marshal.FreeHGlobal(tempPtr);
            return buffer;
        }

        private byte[] CombineByteArrays(byte[] first, byte[] second)
        {
            byte[] combined = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, combined, 0, first.Length);
            Buffer.BlockCopy(second, 0, combined, first.Length, second.Length);
            return combined;
        }

        private bool WriteMemory(byte[] data, IntPtr address, uint size)
        {
            IntPtr processHandle = NativeMethods.GetCurrentProcess();
            return NativeMethods.WriteProcessMemory(processHandle, address, data, size, NativeMethods.INVALID_PTR);
        }
    }

    public partial class InlineHook64 : InlineHook
    {
        public override IntPtr GetProcAddress(Delegate methodDelegate)
        {
            return Marshal.GetFunctionPointerForDelegate(methodDelegate);
        }

        public override IntPtr GetProcAddress(string libraryName, string functionName)
        {
            IntPtr moduleHandle;
            if ((moduleHandle = NativeMethods.GetModuleHandle(libraryName)) == NativeMethods.INVALID_PTR)
                moduleHandle = NativeMethods.LoadLibrary(libraryName);
            return NativeMethods.GetProcAddress(moduleHandle, functionName);
        }
    }

    public partial class InlineHook64 : InlineHook
    {
        private bool SetMemoryProtection(IntPtr address, int protectionFlags)
        {
            return NativeMethods.VirtualProtect(address, 12, protectionFlags, out this.originalMemoryProtection);
        }
    }
}