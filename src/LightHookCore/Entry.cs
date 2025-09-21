using LibHook;
using ScriptRuntime;
using ScriptRuntime.Core;
using ScriptRuntime.FFI;
using ScriptRuntime.Runtime;
using System.Diagnostics;
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
        [DllImport("user32.dll",CharSet = CharSet.Unicode)]
        public static extern int MessageBoxW(int hwnd,string msg,string title,int param);

        public const int Port = 19924;

        public static EncSocket connection = new EncSocket();

        public static nint SelfHandle;

        public static ulong LastestId = 1;
        public static VariableValue RunScript(string code,string name)
        {
            //将当前主线程注册到线程上下文
            TaskContext.ThreadContext.TryAdd(TaskContext.GetCurrentThreadId(), new TaskContext());
            //清空栈回溯（如果有，避免残留上一个报错的信息）
            TaskContext.ThreadContext[TaskContext.GetCurrentThreadId()].StackTrace.Clear();
            /*
            foreach(var key in FunctionManager.FunctionTable.Keys)
            {
                if (FunctionManager.FunctionTable[key].FuncType == FunctionType.Local || FunctionManager.FunctionTable[key].FuncType == FunctionType.Native)
                {
                    FunctionManager.FunctionTable.Remove(key); //清理掉原本的函数
                }
            }
            */

            var codeLines = Lexer.CleanCode(code);
            var ast = Parser.BuildASTByTokens(Lexer.SplitTokens(codeLines));
            var mainFunc = FunctionManager.RegisterFunction(name, new List<string>(), ast);
            return FunctionManager.CallFunction(name, new List<VariableValue>());
        }
        static void RecvFromServer()
        {
            try
            {
                connection.Send(BitConverter.GetBytes(Process.GetCurrentProcess().Id), 0); //向IPC服务器发送自己pid
                RemotePrint($"Process [{Process.GetCurrentProcess().Id}]{Process.GetCurrentProcess().ProcessName} Attached");
                while (true)
                {
                    byte flag = 0;
                    var data = connection.Recv(ref flag);
                    if (flag == 1) //RunScript
                    {
                        string code = Encoding.Unicode.GetString(data);
                        new Thread(() =>
                        {
                            try
                            {
                                RunScript(code, "main" + (++LastestId));
                            }
                            catch (Exception ex)
                            {
                                RemotePrint("Exception thrown:\r\nMessage=>" + ex.Message);
                                var stack = TaskContext.ThreadContext[TaskContext.GetCurrentThreadId()].StackTrace;
                                RemotePrint("Stack=>\r\n" + StringUtils.GenerateStackTrace(stack));
                            }
                            RemotePrint("Script End");
                            
                        }).Start();
                        connection.Send(new byte[] { 1 }, 1); //响应完成
                    }
                    else if(flag == 3) //Input
                    {
                        var info = ReadQueue.Dequeue();
                        info.result = Encoding.Unicode.GetString(data);
                        info.wait.Set();
                    }
                }
            }
            catch
            {

            }
        }
        public unsafe static void InitEnvironment()
        {
            SystemFunctions.InitSystemFunction();
            //加载自身Hook框架的函数
            FunctionManager.FunctionTable.Add("hook", new ScriptFunction("hook",
                new List<ValueType>() { ValueType.PTR, ValueType.PTR, ValueType.STRING }, ValueType.NULL, Hook));
            FunctionManager.FunctionTable.Add("unhook", new ScriptFunction("unhook",
                new List<ValueType>() { ValueType.STRING }, ValueType.NULL, Unhook));
            FunctionManager.FunctionTable.Add("suspend", new ScriptFunction("suspend",
                new List<ValueType>() { ValueType.STRING }, ValueType.NULL, Suspend));
            FunctionManager.FunctionTable.Add("resume", new ScriptFunction("resume",
                new List<ValueType>() { ValueType.STRING }, ValueType.NULL, Resume));
            FunctionManager.FunctionTable.Add("getProcAddress", new ScriptFunction("getProcAddress",
                new List<ValueType>() { ValueType.STRING,ValueType.STRING }, ValueType.PTR, GetProcAddress));
            FunctionManager.FunctionTable.Add("storageGet", new ScriptFunction("storageGet",
                new List<ValueType>() { ValueType.STRING}, ValueType.ANY, StorageGet));
            FunctionManager.FunctionTable.Add("storageSet", new ScriptFunction("storageSet",
                new List<ValueType>() { ValueType.STRING, ValueType.ANY }, ValueType.NULL, StorageSet));
            FunctionManager.FunctionTable.Add("inject", new ScriptFunction("inject",
                new List<ValueType>() { ValueType.NUM }, ValueType.NULL, Inject));

            //覆盖脚本引擎内置的println
            FunctionManager.FunctionTable["println"] = new ScriptFunction("println",
                new List<ValueType>() { ValueType.STRING }, ValueType.NULL, RemotePrintln);
            FunctionManager.FunctionTable["readln"] = new ScriptFunction("readln",
                new List<ValueType>() , ValueType.STRING, RemoteReadln);

            Task.Run(() =>
            {
                while (true) //循环尝试连接
                {
                    try
                    {
                        //MessageBoxW(0, "begin con", "hook", 0);
                        connection = new EncSocket();
                        connection.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port));
                        RecvFromServer();
                    }
                    catch(Exception ex)
                    {
                        MessageBoxW(0, ex.ToString(), "hook", 0);
                        Thread.Sleep(1000);
                    }
                }
            });
            
        }
        public static void RemotePrint(string s)
        {
            connection.Send(Encoding.Unicode.GetBytes(s), 2);
        }
        

        [UnmanagedCallersOnly(EntryPoint = "DllMain")]
        public static bool DllMain(IntPtr hModule, uint ul_reason_for_call, IntPtr lpReserved)
        {
            if(ul_reason_for_call == 1)
            {
                SelfHandle = hModule;
                InitEnvironment();
            }
            return true;
        }
    }
}
