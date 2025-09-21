/*
 * Copyright 2025 xy660
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using ScriptRuntime.FFI;
using ScriptRuntime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ScriptRuntime.Runtime.FunctionManager;
using ValueType = ScriptRuntime.Core.ValueType;
using ScriptRuntime.Utils;

namespace ScriptRuntime.Runtime
{
    public static class SystemFunctions
    {

        public static void InitSystemFunction()
        {
            // 控制台输入输出
            FunctionTable.Add("println", new ScriptFunction("println",
                new List<ValueType>() { ValueType.ANY }, ValueType.NULL, PrintLine));
            FunctionTable.Add("print", new ScriptFunction("print",
                new List<ValueType>() { ValueType.ANY }, ValueType.NULL, Print));
            FunctionTable.Add("readln", new ScriptFunction("readln",
                new List<ValueType>() { }, ValueType.STRING, ReadLine));
            FunctionTable.Add("clear", new ScriptFunction("clear",
                new List<ValueType>() { }, ValueType.NULL, Clear));
            FunctionTable.Add("throw", new ScriptFunction("throw",
                new List<ValueType>() { ValueType.STRING}, ValueType.NULL, ThrowError));

            //休眠
            FunctionTable.Add("sleep", new ScriptFunction("sleep", new List<ValueType>() { ValueType.NUM }, ValueType.NULL, Sleep));
            //退出程序
            FunctionTable.Add("exit", new ScriptFunction("exit", new List<ValueType>() { }, ValueType.NULL, Exit));
            //range函数
            FunctionTable.Add("range", new ScriptFunction("range", new List<ValueType>() { ValueType.NUM, ValueType.NUM, ValueType.NUM }, ValueType.ARRAY, Range));
            //时间相关
            FunctionTable.Add("unixTime", new ScriptFunction("unixTime", new List<ValueType>() { }, ValueType.NUM, UnixTime));
            FunctionTable.Add("unixTimeLocal", new ScriptFunction("unixTimeLocal", new List<ValueType>() { }, ValueType.NUM, UnixTimeLocal));
            // 数学函数
            FunctionTable.Add("abs", new ScriptFunction("abs",
                new List<ValueType>() { ValueType.NUM }, ValueType.NUM, Abs));
            FunctionTable.Add("sqrt", new ScriptFunction("sqrt",
                new List<ValueType>() { ValueType.NUM }, ValueType.NUM, Sqrt));
            FunctionTable.Add("sin", new ScriptFunction("sin",
                new List<ValueType>() { ValueType.NUM }, ValueType.NUM, Sin));
            FunctionTable.Add("cos", new ScriptFunction("cos",
                new List<ValueType>() { ValueType.NUM }, ValueType.NUM, Cos));
            FunctionTable.Add("tan", new ScriptFunction("tan",
                new List<ValueType>() { ValueType.NUM }, ValueType.NUM, Tan));
            FunctionTable.Add("pow", new ScriptFunction("pow",
                new List<ValueType>() { ValueType.NUM, ValueType.NUM }, ValueType.NUM, Pow));
            FunctionTable.Add("log", new ScriptFunction("log",
                new List<ValueType>() { ValueType.NUM, ValueType.NUM }, ValueType.NUM, Log));
            FunctionTable.Add("ceil", new ScriptFunction("ceil",
                new List<ValueType>() { ValueType.NUM }, ValueType.NUM, Ceil));
            FunctionTable.Add("floor", new ScriptFunction("floor",
                new List<ValueType>() { ValueType.NUM }, ValueType.NUM, Floor));
            FunctionTable.Add("round", new ScriptFunction("round",
                new List<ValueType>() { ValueType.NUM }, ValueType.NUM, Round));

            // 随机数
            FunctionTable.Add("random", new ScriptFunction("random",
                new List<ValueType>() { ValueType.NUM, ValueType.NUM }, ValueType.NUM, Random));

            // 类型转换
            FunctionTable.Add("num", new ScriptFunction("num",
                new List<ValueType>() { ValueType.STRING }, ValueType.NUM, ToNum));
            FunctionTable.Add("str", new ScriptFunction("str",
                new List<ValueType>() { ValueType.NUM }, ValueType.STRING, ToStr));

            
            //动态执行
            FunctionTable.Add("eval", new ScriptFunction("eval",
                new List<ValueType>() { ValueType.STRING }, ValueType.NUM, Eval));

            //文件相关函数
            FunctionTable.Add("listFile", new ScriptFunction("listFile",
                new List<ValueType>() { ValueType.STRING }, ValueType.ARRAY, ListFiles));
            FunctionTable.Add("listDir", new ScriptFunction("listDir",
                new List<ValueType>() { ValueType.STRING }, ValueType.ARRAY, ListDirectory));
            FunctionTable.Add("readText", new ScriptFunction("readText",
                new List<ValueType>() { ValueType.STRING }, ValueType.STRING, ReadAsText));

            FunctionTable.Add("writeText", new ScriptFunction("writeText",
                new List<ValueType>() { ValueType.STRING, ValueType.STRING },
                ValueType.BOOL,
                WriteText));

            FunctionTable.Add("ffiload", new ScriptFunction("ffiload",
                new List<ValueType>() { ValueType.STRING, ValueType.STRING, ValueType.STRING, ValueType.STRING },
                ValueType.FUNCTION,
                FFILoadFunction));
            //FunctionTable.Add("objectToPtr", new ScriptFunction("objectToPtr",
            //    new List<ValueType>() { ValueType.OBJECT, ValueType.STRING},
            //    ValueType.PTR,
            //    FFIManager.FFIObjectToPtr));
            //FunctionTable.Add("ptrToObject", new ScriptFunction("ptrToObject",
            //    new List<ValueType>() { ValueType.PTR, ValueType.STRING },
            //    ValueType.OBJECT,
            //    FFIManager.FFIPtrToObject));
            FunctionTable.Add("freePtr", new ScriptFunction("freePtr",
                new List<ValueType>() { ValueType.PTR},
                ValueType.NULL,
                FreePtr));
            FunctionTable.Add("createCallback", new ScriptFunction("nativeCallback",
                new List<ValueType>() { ValueType.OBJECT },
                ValueType.PTR,
                RegisterCallback));
            FunctionTable.Add("is64bit", new ScriptFunction("is64bit",
                new List<ValueType>(),
                ValueType.BOOL,
                Is64Bit));
            FunctionTable.Add("OSName", new ScriptFunction("OSName",
                new List<ValueType>(),
                ValueType.STRING,
                GetOSName));
            FunctionTable.Add("runtimeVersion", new ScriptFunction("runtimeVersion",
                new List<ValueType>(),
                ValueType.STRING,
                GetRuntimeVersion));
            FunctionTable.Add("loadFunc", new ScriptFunction("loadFunc",
                new List<ValueType>() { ValueType.PTR, ValueType.STRING, ValueType.STRING },
                ValueType.FUNCTION,
                FFIFunctionFromPtr));
            FunctionTable.Add("ptr", new ScriptFunction("ptr",
                new List<ValueType>() { ValueType.STRING },
                ValueType.PTR,
                CreatePTR));
        }

        public static VariableValue Eval(List<VariableValue> args, VariableValue thisValue)
        {
            //创建新的变量环境，eval不允许访问其他地方的变量
            var ast = Parser.BuildASTByTokens(Lexer.SplitTokens(args[0].Value.ToString())).Childrens[0];
            var result = Interpreter.ExecuteAST(ast, new Dictionary<string, VariableValue>());
            return result;
        }

        #region Math Functions
        public static VariableValue Abs(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Abs((double)args[0].Value));
        public static VariableValue Sqrt(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Sqrt((double)args[0].Value));
        public static VariableValue Sin(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Sin((double)args[0].Value));
        public static VariableValue Cos(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Cos((double)args[0].Value));
        public static VariableValue Tan(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Tan((double)args[0].Value));
        public static VariableValue Pow(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Pow((double)args[0].Value, (double)args[1].Value));
        public static VariableValue Log(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Log((double)args[0].Value, (double)args[1].Value));
        public static VariableValue Ceil(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Ceiling((double)args[0].Value));
        public static VariableValue Floor(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Floor((double)args[0].Value));
        public static VariableValue Round(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, Math.Round((double)args[0].Value));
        public static VariableValue Random(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, (double)new Random().Next((int)(double)args[0].Value, (int)(double)args[1].Value));
        #endregion

        #region Type Conversion
        public static VariableValue ToNum(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, double.Parse((string)args[0].Value));
        public static VariableValue ToStr(List<VariableValue> args, VariableValue thisValue) => new(ValueType.STRING, args[0].Value.ToString());
        #endregion

        #region Array Operations
        
        #endregion
        public static VariableValue Print(List<VariableValue> args, VariableValue thisValue)
        {
            if (args[0].VarType == ValueType.ARRAY)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                List<VariableValue> list = args[0].Value as List<VariableValue>;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].VarType == ValueType.STRING)
                    {
                        sb.Append($"\"{list[i].Value.ToString()}\"");
                    }
                    else
                    {
                        sb.Append(list[i].Value.ToString());
                    }

                    if (i != list.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append("]");
                Console.Write(sb.ToString());
            }
            else
            {
                Console.Write(args[0].Value.ToString());
            }
            return EmptyVariable;
        }
        public static VariableValue PrintLine(List<VariableValue> args, VariableValue thisValue)
        {
            Print(args,thisValue);
            Console.WriteLine();
            return EmptyVariable;
        }
        public static VariableValue Clear(List<VariableValue> args, VariableValue thisValue)
        {
            Console.Clear();
            return EmptyVariable;
        }
        public static VariableValue ThrowError(List<VariableValue> args, VariableValue thisValue)
        {
            throw new ScriptException((string)args[0].Value);
        }
        public static VariableValue ReadLine(List<VariableValue> args, VariableValue thisValue)
        {
            var ret = Console.ReadLine();
            return new VariableValue(ValueType.STRING, ret);
        }
        public static VariableValue Sleep(List<VariableValue> args, VariableValue thisValue)
        {
            Thread.Sleep((int)(double)args[0].Value);
            return EmptyVariable;
        }
        public static VariableValue Exit(List<VariableValue> args, VariableValue thisValue)
        {
            Environment.Exit(args.Count);
            return EmptyVariable;
        }
        public static VariableValue Range(List<VariableValue> args, VariableValue thisValue)
        {
            List<VariableValue> list = new List<VariableValue>();
            int start = (int)(double)args[0].Value;
            int end = (int)(double)args[1].Value;
            int step = (int)(double)args[2].Value;
            if (step == 0)
            {
                throw new ScriptException("step为零");
            }
            if (end < start ^ step < 0)
            {
                throw new ScriptException("无效的step");
            }
            if (start <= end)
            {
                for (int i = start; i < end; i += step)
                {
                    list.Add(new VariableValue(ValueType.NUM, (double)i));
                }
            }
            else
            {
                for (int i = start; i > end; i += step)
                {
                    list.Add(new VariableValue(ValueType.NUM, (double)i));
                }
            }
            return new VariableValue(ValueType.ARRAY, list);
        }
        public static VariableValue UnixTime(List<VariableValue> args, VariableValue thisValue)
        {
            var unixTime = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return new VariableValue(ValueType.NUM, unixTime);
        }
        public static VariableValue UnixTimeLocal(List<VariableValue> args, VariableValue thisValue)
        {
            var unixTime = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return new VariableValue(ValueType.NUM, unixTime);
        }

        #region 文件操作类
        public static VariableValue ListFiles(List<VariableValue> args, VariableValue thisValue)
        {
            string fname = args[0].Value.ToString();
            if (!Directory.Exists(fname))
            {
                throw new ScriptException("路径不存在");
            }
            try
            {
                var fs = Directory.GetFiles(args[0].Value.ToString());
                List<VariableValue> ret = new List<VariableValue>();
                foreach (var f in fs)
                {
                    ret.Add(new VariableValue(ValueType.STRING, f));
                }
                return new VariableValue(ValueType.ARRAY, ret);
            }
            catch (Exception ex)
            {
                throw new ScriptException(ex.Message); //转换系统错误为脚本错误
            }
        }
        public static VariableValue ListDirectory(List<VariableValue> args, VariableValue thisValue)
        {
            string fname = args[0].Value.ToString();
            if (!Directory.Exists(fname))
            {
                throw new ScriptException("路径不存在");
            }
            try
            {
                var fs = Directory.GetDirectories(args[0].Value.ToString());
                List<VariableValue> ret = new List<VariableValue>();
                foreach (var f in fs)
                {
                    ret.Add(new VariableValue(ValueType.STRING, f));
                }
                return new VariableValue(ValueType.ARRAY, ret);
            }
            catch (Exception ex)
            {
                throw new ScriptException(ex.Message); //转换系统错误为脚本错误
            }
        }
        public static VariableValue ReadAsText(List<VariableValue> args, VariableValue thisValue)
        {
            string fname = args[0].Value.ToString();
            if (!File.Exists(fname))
            {
                throw new ScriptException("路径不存在");
            }
            try
            {
                return new VariableValue(ValueType.STRING, File.ReadAllText(fname));
            }
            catch (Exception ex)
            {
                throw new ScriptException(ex.Message); //转换系统错误为脚本错误
            }
        }
        public static VariableValue WriteText(List<VariableValue> args, VariableValue thisValue)
        {
            string fname = args[0].Value.ToString();
            try
            {
                File.WriteAllText(fname, args[1].Value.ToString());
                return new VariableValue(ValueType.BOOL, true);
            }
            catch (Exception e)
            {
                throw new ScriptException(e.Message);
            }
        }
        #endregion

        #region 字符串操作类

        #endregion

        #region FFI相关
        //ffiload(libName,funcName,argDefine,retDefine)
        public static VariableValue Is64Bit(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.BOOL, Environment.Is64BitProcess);
        }
        public static VariableValue GetOSName(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.STRING, Environment.OSVersion.VersionString);
        }
        public static VariableValue GetRuntimeVersion(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.STRING, Interpreter.VersionString);
        }


        public static VariableValue FFILoadFunction(List<VariableValue> args, VariableValue thisValue)
        {
            string libName = (string)args[0].Value;
            string funcName = (string)args[1].Value;
            string argDef = (string)args[2].Value;
            string retDef = (string)args[3].Value;
            nint libHandler = NativeLibrary.Load(libName);
            nint funcPointer = 0;
            if (!NativeLibrary.TryGetExport(libHandler, funcName, out funcPointer))
            {
                throw new ScriptException($"加载外部库函数失败 {retDef} [{libName}]::{funcName}({argDef})");
            }
            var func = new ScriptFunction(funcName, FFIManager.NativeTypeMapper[retDef], argDef, retDef, funcPointer);
            FunctionTable.TryAdd(funcName, func);
            return new VariableValue(ValueType.FUNCTION,func);
        }
        public static VariableValue FFIFunctionFromPtr(List<VariableValue> args, VariableValue thisValue)
        {
            nint funcPointer = (nint)args[0].Value;
            string argDef = (string)args[1].Value;
            string retDef = (string)args[2].Value;
            string name = "funcPointer" + funcPointer.ToString();
            var func = new ScriptFunction(name, FFIManager.NativeTypeMapper[retDef], argDef, retDef, funcPointer);
            FunctionTable.TryAdd(name, func);
            return new VariableValue(ValueType.FUNCTION, func);
        }

        static unsafe VariableValue FreePtr(List<VariableValue> args, VariableValue thisValue)
        {
            if (CallbackManager.Ptr2Callback.ContainsKey((nint)args[0].Value))
            {
                CallbackManager.UnregisterCallback((nint)args[0].Value);
            }
            else
            {
                Marshal.FreeHGlobal((nint)args[0].Value);
            }
            return FunctionManager.EmptyVariable;

        }

        //返回函数指针
        static unsafe VariableValue RegisterCallback(List<VariableValue> args, VariableValue thisValue)
        {
            var objContainer = (Dictionary<string, VariableValue>)args[0].Value;

            if(!objContainer.TryGetValue("func",out var targetFunction))
            {
                throw new ScriptException("参数对象不包含 func");
            }
            string abi = string.Empty;
            if(!objContainer.TryGetValue("abi",out var tmp_abi))
            {
                abi = string.Empty;
            }
            else
            {
                abi = (string)tmp_abi.Value;
            }

            if(!objContainer.TryGetValue("argTypes",out var argTypes))
            {
                throw new ScriptException("参数对象不包含 argTypes");
            }

            if (!objContainer.TryGetValue("retType", out var retTypes))
            {
                throw new ScriptException("参数对象不包含 retType");
            }

            var info = CallbackManager.RegisterCallback((ScriptFunction)targetFunction.Value, abi,
                (string)argTypes.Value, (string)retTypes.Value);
            return new VariableValue(ValueType.PTR, info.addr);
        }

        public static VariableValue CreatePTR(List<VariableValue> args, VariableValue thisValue)
        {
            string input = (string)args[0].Value;
            //判断是不是16进制
            if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return new VariableValue(ValueType.PTR, (nint)Convert.ToInt64(input.Substring(2), 16));
            }
            else
            {
                return new VariableValue(ValueType.PTR, (nint)Convert.ToInt64(input, 10));
            }
        }

        #endregion
    }

}