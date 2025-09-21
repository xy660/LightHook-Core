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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ScriptRuntime.Core;
using ScriptRuntime.Runtime;
using ScriptRuntime.Utils;
using ValueType = ScriptRuntime.Core.ValueType;

namespace ScriptRuntime.FFI
{
    public class FFIManager
    {
        public static Dictionary<string, ValueType> NativeTypeMapper = new Dictionary<string, ValueType>()
        {
            {"string",ValueType.STRING},
            {"ptr",ValueType.PTR},
            {"int",ValueType.NUM},
            {"long",ValueType.NUM},
            {"uint",ValueType.NUM},
            {"ulong",ValueType.NUM},
            {"short",ValueType.NUM},
            {"ushort",ValueType.NUM},
            {"byte",ValueType.NUM},
            {"float",ValueType.NUM},
            {"double",ValueType.NUM},
            {"bool",ValueType.BOOL},
            {"void",ValueType.NULL},
            {"data",ValueType.ARRAY},
        };
        public static Dictionary<string, int> NativeTypeSize = new Dictionary<string, int>()
        {
            {"string",-1},
            {"ptr",nint.Size},
            {"int",sizeof(int)},
            {"long",sizeof(long)},
            {"uint",sizeof(uint)},
            {"ulong",sizeof(ulong)},
            {"short",sizeof(short)},
            {"ushort",sizeof(ushort)},
            {"byte",sizeof(byte)},
            {"float",sizeof(float)},
            {"double",sizeof(double)},
            {"bool",sizeof(bool)},
            {"void",-1},
            {"data",-1},
        };
        public static Dictionary<ValueType, string[]> NativeTypeReverseMapper = new Dictionary<ValueType, string[]>()
        {
            { ValueType.STRING, new[] { "string" } },
            { ValueType.NUM, new[] { "int", "long", "short", "byte" ,"float", "ptr","ushort","ulong","uint"} },
            { ValueType.ARRAY, new[] { "data" } },
            {ValueType.BOOL, new[] { "bool" } },
        };
        public static ValueTuple<List<ValueType>, string[]> NativeDefToScriptDef(string nativeDef)
        {
            var ret = new List<ValueType>();
            var sp = nativeDef.Split(",", StringSplitOptions.RemoveEmptyEntries);
            foreach (var type in sp)
            {
                ret.Add(NativeTypeMapper[type]);
            }
            return (ret, sp);
        }
        public static VariableValue CallNativeFunction(ScriptFunction function, List<VariableValue> args)
        {
            if (function.FuncType != FunctionType.Native)
            {
                throw new ScriptException("不允许通过FFI调用非原生函数");
            }

            List<nint> nativeArgs = new List<nint>();
            List<nint> freeList = new List<nint>(); //需要释放的指针，防止内存泄漏
            try
            {
                for (int i = 0; i < args.Count; i++)
                {
                    string nativeType = function.NativeFunctionArgdefines[i];
                    if (args[i].VarType == ValueType.NUM)
                    {
                        if (nativeType == "float" || nativeType == "double") //浮点数
                        {
                            long tmp = BitConverter.DoubleToInt64Bits((double)args[i].Value);
                            nativeArgs.Add((nint)tmp);
                        }
                        else //整数
                        {
                            long tmp = (long)(double)args[i].Value;
                            nativeArgs.Add((nint)tmp);
                        }
                    }
                    else if (args[i].VarType == ValueType.BOOL)
                    {
                        nativeArgs.Add((bool)args[i].Value ? 1 : 0);
                    }
                    else if (args[i].VarType == ValueType.STRING)
                    {
                        nint pstr = Marshal.StringToHGlobalUni((string)args[i].Value);
                        nativeArgs.Add(pstr);
                        freeList.Add(pstr);
                    }
                    else if (args[i].VarType == ValueType.PTR)
                    {
                        var ptr = (nint)args[i].Value;
                        nativeArgs.Add(ptr);
                    }
                    else if (args[i].VarType == ValueType.ARRAY)
                    {
                        var list = (List<VariableValue>)args[i].Value;
                        //throw new ScriptException("暂不支持");
                        nint buf = Marshal.AllocHGlobal(list.Count);
                        freeList.Add(buf);
                        nativeArgs.Add(buf);
                        unsafe
                        {
                            byte* ptr = (byte*)buf;
                            for (int j = 0; j < list.Count; j++)
                            {
                                if (list[j].VarType != ValueType.NUM)
                                {
                                    throw new ScriptException("FFI传递data类型时集合中不全是数字");
                                }
                                ptr[j] = (byte)(double)list[j].Value;
                            }
                        }
                    }
                }
                string retTypeNative = function.NativeReturnType;
                if (retTypeNative != "void")
                {
                    nint ret = InvokeHelper.CallPtr(function.NativePointer, nativeArgs.ToArray(), false);
                    foreach (var f in freeList.ToArray())
                    {
                        Marshal.FreeHGlobal(f); //释放申请的内存
                        freeList.Remove(f);
                    }

                    if (retTypeNative == "string")
                    {
                        string s = Marshal.PtrToStringAuto(ret);
                        if (s is null) s = "null_string";
                        return new VariableValue(ValueType.STRING, s);
                    }
                    else if (retTypeNative == "bool")
                    {
                        return new VariableValue(ValueType.BOOL, ret != 0);
                    }
                    else if (retTypeNative == "float" || retTypeNative == "double")
                    {
                        return new VariableValue(ValueType.NUM, BitConverter.Int64BitsToDouble(ret));
                    }
                    else if(retTypeNative == "ptr")
                    {
                        //直接指针拷贝数据
                        return new VariableValue(ValueType.PTR, ret);
                    }
                    else //剩下就是整数了
                    {
                        return new VariableValue(ValueType.NUM, (double)ret);
                    }
                }
                else //无返回值函数
                {
                    InvokeHelper.CallPtr(function.NativePointer, nativeArgs.ToArray(), true);
                    foreach (var f in freeList.ToArray())
                    {
                        Marshal.FreeHGlobal(f); //释放申请的内存
                        freeList.Remove(f);
                    }
                    return new VariableValue(ValueType.NULL, null);
                }

            }
            finally
            {
                foreach (var f in freeList.ToArray())
                {
                    Marshal.FreeHGlobal(f); //释放申请的内存
                    freeList.Remove(f);
                }
            }
        }
        public static unsafe VariableValue FFIObjectToPtr(VariableValue obj,string def)
        {
            Dictionary<string, VariableValue> objContainer = (Dictionary<string, VariableValue>)obj.Value;
            string NativeTypesDef = def;

            // 计算总内存大小（含对齐）
            int totalSize = 0;
            var typeList = NativeTypesDef.Split(',');
            foreach (var type in typeList)
            {
                var sp = type.Split(':');
                var name = sp[0];
                var nativeType = sp[1];
                var value = objContainer[name];

                var (baseType, isArray, arrayLength) = ParseNativeType(nativeType);
                int elemSize = FFIManager.NativeTypeSize[baseType];

                if (isArray)
                {
                    if (value.VarType != ValueType.ARRAY)
                        throw new ScriptException($"预期数组类型，实际得到 {AOTEnumMap.ValueTypeString[value.VarType]}");

                    var list = (List<VariableValue>)value.Value;
                    if (arrayLength > 0 && list.Count != arrayLength)
                        throw new ScriptException($"数组长度不匹配，预期 {arrayLength}，实际 {list.Count}");

                    totalSize = GetNextStructPosition(totalSize, elemSize * list.Count);
                }
                else
                {
                    totalSize = GetNextStructPosition(totalSize, elemSize);
                }
            }

            // 申请非托管内存
            byte* memoryPtr = (byte*)Marshal.AllocHGlobal(totalSize);
            if (memoryPtr == (byte*)0)
            {
                throw new ScriptException("内存不足");
            }
            int currentOffset = 0;

            try
            {
                foreach (var type in typeList)
                {
                    var sp = type.Split(':');
                    var name = sp[0];
                    var nativeType = sp[1];
                    var value = objContainer[name];

                    var (baseType, isArray, arrayLength) = ParseNativeType(nativeType);
                    int elemSize = FFIManager.NativeTypeSize[baseType];

                    if (isArray)
                    {
                        // 数组处理
                        var list = (List<VariableValue>)value.Value;
                        int count = arrayLength > 0 ? arrayLength : list.Count;

                        currentOffset = GetNextStructPosition(currentOffset, elemSize);
                        byte* pDest = memoryPtr + currentOffset;

                        for (int i = 0; i < count; i++)
                        {
                            if (i >= list.Count)
                                throw new ScriptException($"数组索引超出范围: {i}");

                            if (list[i].VarType != ValueType.NUM && list[i].VarType != ValueType.BOOL)
                                throw new ScriptException($"数组元素必须全部是NUM或BOOL类型");

                            byte* elemPtr = pDest + i * elemSize;
                            WriteValueToMemory(elemPtr, baseType, list[i]);
                        }
                        currentOffset += elemSize * count;
                    }
                    else
                    {
                        // 非数组处理
                        currentOffset = GetNextStructPosition(currentOffset, elemSize);
                        byte* pDest = memoryPtr + currentOffset;

                        if (value.VarType == ValueType.NUM || value.VarType == ValueType.PTR || value.VarType == ValueType.BOOL)
                        {
                            WriteValueToMemory(pDest, baseType, value);
                        }
                        else
                        {
                            throw new ScriptException($"不支持 {AOTEnumMap.ValueTypeString[value.VarType]} 进行结构体转换");
                        }
                        currentOffset += elemSize;
                    }
                }
                return new VariableValue(ValueType.PTR, (nint)memoryPtr);
            }
            catch
            {
                Marshal.FreeHGlobal((nint)memoryPtr);
                throw;
            }
        }

        public static unsafe VariableValue FFIPtrToObject(List<VariableValue> args, VariableValue thisValue)
        {
            var result = PtrToObject((nint)args[0].Value, (string)args[1].Value);
            return new VariableValue(ValueType.OBJECT, result);
        }

        public static unsafe Dictionary<string, VariableValue> PtrToObject(nint ptr, string nativeTypesDef)
        {
            if (ptr == IntPtr.Zero)
                throw new ScriptException("无效的空指针");

            byte* memoryPtr = (byte*)ptr;
            var result = new Dictionary<string, VariableValue>();
            int currentOffset = 0;

            try
            {
                foreach (var type in nativeTypesDef.Split(','))
                {
                    var sp = type.Split(':');
                    if (sp.Length != 2)
                        throw new ScriptException($"无效的类型定义: {type}");

                    var name = sp[0];
                    var nativeType = sp[1];

                    var (baseType, isArray, arrayLength) = ParseNativeType(nativeType);
                    int elemSize = FFIManager.NativeTypeSize[baseType];
                    int totalSize = isArray ? elemSize * arrayLength : elemSize;

                    // 对齐
                    currentOffset = GetNextStructPosition(currentOffset, elemSize);
                    byte* pSrc = memoryPtr + currentOffset;

                    VariableValue value;
                    if (isArray)
                    {
                        var list = new List<VariableValue>();
                        for (int i = 0; i < arrayLength; i++)
                        {
                            byte* elemPtr = pSrc + i * elemSize;
                            list.Add(ReadValueFromMemory(elemPtr, baseType));
                        }
                        value = new VariableValue(ValueType.ARRAY, list);
                    }
                    else
                    {
                        value = ReadValueFromMemory(pSrc, baseType);
                    }

                    result.Add(name, value);
                    currentOffset += totalSize;
                }

                return result;
            }
            catch (AccessViolationException ex)
            {
                throw new ScriptException($"内存访问冲突: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new ScriptException($"指针转换失败: {ex.Message}");
            }
        }

        // 辅助方法
        private static (string baseType, bool isArray, int arrayLength) ParseNativeType(string nativeType)
        {
            if (nativeType.EndsWith("]"))
            {
                var parts = nativeType.Split('[', ']');
                if (parts.Length != 3 || !int.TryParse(parts[1], out int length))
                    throw new ScriptException($"无效的数组类型定义: {nativeType}");

                return (parts[0], true, length);
            }
            return (nativeType, false, 0);
        }

        public static VariableValue Native2ScriptVariable(nint ret, string retTypeNative)
        {

            if (retTypeNative == "string")
            {
                string s = Marshal.PtrToStringAuto(ret);
                if (s is null) s = "null_string";
                return new VariableValue(ValueType.STRING, s);
            }
            else if (retTypeNative == "bool")
            {
                return new VariableValue(ValueType.BOOL, ret != 0);
            }
            else if (retTypeNative == "float" || retTypeNative == "double")
            {
                return new VariableValue(ValueType.NUM, BitConverter.Int64BitsToDouble(ret));
            }
            else if (retTypeNative == "ptr")
            {
                //直接指针拷贝数据
                return new VariableValue(ValueType.PTR, ret);
            }
            else //剩下就是整数了
            {
                return new VariableValue(ValueType.NUM, (double)ret);
            }
        }

        public static unsafe void WriteValueToMemory(void* ptr, string type, VariableValue value)
        {
            switch (type)
            {
                case "byte":
                    *(byte*)ptr = value.VarType == ValueType.BOOL ?
                        ((bool)value.Value ? (byte)1 : (byte)0) :
                        (byte)(double)value.Value;
                    break;
                case "short":
                    *(short*)ptr = (short)(double)value.Value;
                    break;
                case "ushort":
                    *(ushort*)ptr = (ushort)(double)value.Value;
                    break;
                case "int":
                    *(int*)ptr = (int)(double)value.Value;
                    break;
                case "uint":
                    *(uint*)ptr = (uint)(double)value.Value;
                    break;
                case "long":
                    *(long*)ptr = (long)(double)value.Value;
                    break;
                case "ulong":
                    *(ulong*)ptr = (ulong)(double)value.Value;
                    break;
                case "float":
                    *(float*)ptr = (float)(double)value.Value;
                    break;
                case "double":
                    *(double*)ptr = (double)value.Value;
                    break;
                case "ptr":
                    *(nint*)ptr = (nint)value.Value;
                    break;
                case "bool":
                    *(byte*)ptr = (bool)value.Value ? (byte)1 : (byte)0;
                    break;
                default:
                    throw new ScriptException($"不支持的类型: {type}");
            }
        }

        private static unsafe VariableValue ReadValueFromMemory(byte* ptr, string type)
        {
            switch (type)
            {
                case "byte":
                    return new VariableValue(ValueType.NUM, (double)(*(byte*)ptr));
                case "short":
                    return new VariableValue(ValueType.NUM, (double)(*(short*)ptr));
                case "ushort":
                    return new VariableValue(ValueType.NUM, (double)(*(ushort*)ptr));
                case "int":
                    return new VariableValue(ValueType.NUM, (double)(*(int*)ptr));
                case "uint":
                    return new VariableValue(ValueType.NUM, (double)(*(uint*)ptr));
                case "long":
                    return new VariableValue(ValueType.NUM, (double)(*(long*)ptr));
                case "ulong":
                    return new VariableValue(ValueType.NUM, (double)(*(ulong*)ptr));
                case "float":
                    return new VariableValue(ValueType.NUM, (double)(*(float*)ptr));
                case "double":
                    return new VariableValue(ValueType.NUM, *(double*)ptr);
                case "ptr":
                    return new VariableValue(ValueType.PTR, *(nint*)ptr);
                case "bool":
                    return new VariableValue(ValueType.BOOL, *(byte*)ptr != 0);
                default:
                    throw new ScriptException($"不支持的类型: {type}");
            }
        }

        static int GetNextStructPosition(int current, int size)
        {
            return (current + size - 1) / size * size;
        }

    }
}
