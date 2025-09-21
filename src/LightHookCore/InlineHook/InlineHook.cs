namespace LibHook
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class InlineHook
    {
        public abstract void Install(nint targetFunction, nint hookFunction);
        public abstract void Suspend();
        public abstract void Resume();
        public abstract void Uninstall();
        public abstract nint GetProcAddress(Delegate methodDelegate);
        public abstract nint GetProcAddress(string libraryName, string functionName);

        public static InlineHook Create()
        {
            return Environment.Is64BitProcess
                ? new InlineHook64()
                : new InlineHook32();
        }

        public static InlineHook CreateAndInstall(nint targetFunction, nint hookFunction)
        {
            var hookInstance = Create();
            hookInstance.Install(targetFunction, hookFunction);
            return hookInstance;
        }
    }
}