using LibHook;
using ScriptRuntime;
using ScriptRuntime.Core;
using ScriptRuntime.FFI;
using ScriptRuntime.Runtime;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using ValueType = ScriptRuntime.Core.ValueType;

namespace LightHookCore
{
    public partial class Entry
    {
        class ReadInfo
        {
            public ManualResetEvent wait = new ManualResetEvent(false);
            public string result = string.Empty;
        }
        public static VariableValue RemotePrintln(List<VariableValue> args, VariableValue thisValue)
        {
            RemotePrint((string)args[0].Value);
            return FunctionManager.EmptyVariable;
        }
        static Queue<ReadInfo> ReadQueue = new Queue<ReadInfo>();
        public static VariableValue RemoteReadln(List<VariableValue> args, VariableValue thisValue)
        {
            ReadInfo info = new ReadInfo();
            ReadQueue.Enqueue(info);
            connection.Send(new byte[1], 3);
            info.wait.WaitOne();
            return new VariableValue(ValueType.STRING, info.result);
        }

        static Dictionary<string, InlineHook> hooks = new Dictionary<string, InlineHook>();
        public static VariableValue GetProcAddress(List<VariableValue> args, VariableValue thisValue)
        {
            try
            {
                nint pf = NativeLibrary.GetExport(NativeLibrary.Load((string)args[0].Value), (string)args[1].Value);
                return new VariableValue(ValueType.PTR, pf);
            }
            catch (Exception ex)
            {
                throw new ScriptException(ex.Message);
            }
        }

        public static VariableValue Hook(List<VariableValue> args, VariableValue thisValue)
        {
            try
            {
                nint source = (nint)args[0].Value; //原函数
                nint dest = (nint)args[1].Value; //HookProc函数
                string name = (string)args[2].Value; //名称
                var hook = InlineHook.CreateInstance();
                hook.Install(source, dest);
                hooks.Add(name, hook);

                return FunctionManager.EmptyVariable;
            }
            catch (Exception ex)
            {
                throw new ScriptException(ex.Message);
            }
        }
        static object StorageLock = new object();
        static Dictionary<string, VariableValue> GlobalStorge = new Dictionary<string, VariableValue>();
        public static VariableValue StorageGet(List<VariableValue> args, VariableValue thisValue)
        {
            lock (StorageLock)
            {
                if (!GlobalStorge.ContainsKey((string)args[0].Value))
                {
                    throw new ScriptException("全局储存不存在此Key " + args[0].Value);
                }
                return GlobalStorge[((string)args[0].Value)];
            }
        }
        public static VariableValue StorageSet(List<VariableValue> args, VariableValue thisValue)
        {
            lock (StorageLock)
            {
                if (GlobalStorge.ContainsKey((string)args[0].Value))
                {
                    GlobalStorge[(string)args[0].Value] = args[1];
                }
                else
                {
                    GlobalStorge.Add((string)args[0].Value, args[1]);
                }
                return FunctionManager.EmptyVariable;
            }
        }

        public static VariableValue Suspend(List<VariableValue> args, VariableValue thisValue)
        {
            if (!hooks.ContainsKey((string)args[0].Value))
            {
                throw new ScriptException("错误，不存在" + (string)args[0].Value);
            }
            hooks[(string)args[0].Value].Suspend();
            return FunctionManager.EmptyVariable;
        }
        public static VariableValue Resume(List<VariableValue> args, VariableValue thisValue)
        {
            if (!hooks.ContainsKey((string)args[0].Value))
            {
                throw new ScriptException("错误，不存在" + (string)args[0].Value);
            }
            hooks[(string)args[0].Value].Resume();
            return FunctionManager.EmptyVariable;
        }
        public static VariableValue Unhook(List<VariableValue> args, VariableValue thisValue)
        {
            var name = (string)args[0].Value; //名称
            if (!hooks.ContainsKey(name))
            {
                throw new ScriptException("错误，不存在" + name);
            }
            hooks[name].Uninstall();
            hooks.Remove(name);
            return FunctionManager.EmptyVariable;
        }

        //请求Server注入到新进程  参数：(pid)
        public static VariableValue Inject(List<VariableValue> args, VariableValue thisValue)
        {
            int pid = (int)(double)args[0].Value;
            connection.Send(BitConverter.GetBytes(pid), 4);
            return FunctionManager.EmptyVariable;
        }
    }
}
