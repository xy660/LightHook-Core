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
using ScriptRuntime.Core;
using ScriptRuntime.FFI;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ValueType = ScriptRuntime.Core.ValueType;

namespace ScriptRuntime.Runtime
{
    public class PtrManager
    {
        public static Dictionary<string, ScriptFunction> PtrFunctions = new Dictionary<string, ScriptFunction>()
        {
            {"asUniString",new ScriptFunction("asUniString",
                    new List<ValueType>(),
                    ValueType.STRING,PtrToStringW) },
            {"asAnsiString",new ScriptFunction("asAnsiString",
                    new List<ValueType>(),
                    ValueType.STRING,PtrToStringW) },
            {"asObject",new ScriptFunction("asObject",
                    new List<ValueType>(){ValueType.STRING},
                    ValueType.STRING,PtrToObject) },
            {"move",new ScriptFunction("move",
                    new List<ValueType>(){ValueType.NUM},
                    ValueType.PTR,PtrMove) },
            {"copy",new ScriptFunction("copy",
                    new List<ValueType>(),
                    ValueType.PTR,PtrCopy) },
            {"readPointer",new ScriptFunction("readPointer",
                    new List<ValueType>(),
                    ValueType.PTR,ReadPointer) },
            {"readULong",new ScriptFunction("readULong",
                    new List<ValueType>(),
                    ValueType.NUM,ReadULong) },
            {"readLong",new ScriptFunction("readLong",
                    new List<ValueType>(),
                    ValueType.NUM,ReadLong) },
            {"readUInt",new ScriptFunction("readUInt",
                    new List<ValueType>(),
                    ValueType.NUM,ReadUInt) },
            {"readInt",new ScriptFunction("readInt",
                    new List<ValueType>(),
                    ValueType.NUM,ReadInt) },
            {"readUShort",new ScriptFunction("readUShort",
                    new List<ValueType>(),
                    ValueType.NUM,ReadUShort) },
            {"readShort",new ScriptFunction("readShort",
                    new List<ValueType>(),
                    ValueType.NUM,ReadShort) },
            {"readByte",new ScriptFunction("readByte",
                    new List<ValueType>(),
                    ValueType.NUM,ReadByte) },
            {"putPointer",new ScriptFunction("putPointer",
                    new List<ValueType>(){ValueType.PTR},
                    ValueType.PTR,WritePointer) },
            {"putULong",new ScriptFunction("putULong",
                    new List<ValueType>(){ValueType.NUM},
                    ValueType.PTR,WriteULong) },
            {"putLong",new ScriptFunction("putLong",
                new List<ValueType>(){ValueType.NUM},
                ValueType.PTR,WriteLong) },
            {"putUInt",new ScriptFunction("putUInt",
                new List<ValueType>(){ValueType.NUM},
                ValueType.PTR,WriteUInt) },
            {"putInt",new ScriptFunction("putInt",
                new List<ValueType>(){ValueType.NUM},
                ValueType.PTR,WriteInt) },
            {"putUShort",new ScriptFunction("putUShort",
                new List<ValueType>(){ValueType.NUM},
                ValueType.PTR,WriteUShort) },
            {"putShort",new ScriptFunction("putShort",
                new List<ValueType>(){ValueType.NUM},
                ValueType.PTR,WriteShort) },
            {"putByte",new ScriptFunction("putByte",
                new List<ValueType>(){ValueType.NUM},
                ValueType.PTR,WriteByte) },
        };
        static unsafe VariableValue WritePointer(List<VariableValue> args, VariableValue thisValue)
        {
            var p = (nint*)(nint)(thisValue.Value);
            *p = (nint)(args[0].Value);
            return thisValue;
        }
        static unsafe VariableValue WriteULong(List<VariableValue> args, VariableValue thisValue)
        {
            var p = (ulong*)(nint)(thisValue.Value);
            *p = (ulong)(double)(args[0].Value);
            return thisValue;
        }
        static unsafe VariableValue WriteLong(List<VariableValue> args, VariableValue thisValue)
        {
            var p = (long*)(nint)(thisValue.Value);
            *p = (long)(double)(args[0].Value);
            return thisValue;
        }

        static unsafe VariableValue WriteUInt(List<VariableValue> args, VariableValue thisValue)
        {
            var p = (uint*)(nint)(thisValue.Value);
            *p = (uint)(double)(args[0].Value);
            return thisValue;
        }

        static unsafe VariableValue WriteInt(List<VariableValue> args, VariableValue thisValue)
        {
            var p = (int*)(nint)(thisValue.Value);
            *p = (int)(double)(args[0].Value);
            return thisValue;
        }

        static unsafe VariableValue WriteUShort(List<VariableValue> args, VariableValue thisValue)
        {
            var p = (ushort*)(nint)(thisValue.Value);
            *p = (ushort)(double)(args[0].Value);
            return thisValue;
        }

        static unsafe VariableValue WriteShort(List<VariableValue> args, VariableValue thisValue)
        {
            var p = (short*)(nint)(thisValue.Value);
            *p = (short)(double)(args[0].Value);
            return thisValue;
        }

        static unsafe VariableValue WriteByte(List<VariableValue> args, VariableValue thisValue)
        {
            var p = (byte*)(nint)(thisValue.Value);
            *p = (byte)(double)(thisValue.Value);
            return thisValue;
        }
        static unsafe VariableValue ReadPointer(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.PTR, (nint)(*(ulong*)(nint)thisValue.Value));
        }
        static unsafe VariableValue ReadULong(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.NUM, (double)(*(ulong*)(nint)thisValue.Value));
        }
        static unsafe VariableValue ReadLong(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.NUM, (double)(*(long*)(nint)thisValue.Value));
        }
        static unsafe VariableValue ReadInt(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.NUM, (double)(*(int*)(nint)thisValue.Value));
        }
        static unsafe VariableValue ReadUInt(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.NUM, (double)(*(uint*)(nint)thisValue.Value));
        }
        static unsafe VariableValue ReadUShort(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.NUM, (double)(*(ushort*)(nint)thisValue.Value));
        }
        static unsafe VariableValue ReadShort(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.NUM, (double)(*(short*)(nint)thisValue.Value));
        }
        static unsafe VariableValue ReadByte(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.NUM, (double)(*(byte*)(nint)thisValue.Value));
        }

        static unsafe VariableValue PtrCopy(List<VariableValue> args, VariableValue thisValue)
        {
            nint dest = 0;
            var p = &dest;
            *p = (nint)thisValue.Value;
            return new VariableValue(ValueType.PTR, dest);
        }
        static VariableValue PtrMove(List<VariableValue> args, VariableValue thisValue)
        {
            thisValue.Value = (nint)thisValue.Value + (int)(double)args[0].Value;
            return thisValue;
        }
        static VariableValue PtrToObject(List<VariableValue> args, VariableValue thisValue)
        {
            var objContainer = FFIManager.PtrToObject((nint)thisValue.Value, (string)args[0].Value);
            return new VariableValue(ValueType.OBJECT, objContainer);
        }
        static VariableValue PtrToStringW(List<VariableValue> args, VariableValue thisValue)
        {
            var str = Marshal.PtrToStringUni((nint)thisValue.Value);
            return new VariableValue(ValueType.STRING, str is null ? "null" : str);
        }
        static VariableValue PtrToStringA(List<VariableValue> args, VariableValue thisValue)
        {
            var str = Marshal.PtrToStringAnsi((nint)thisValue.Value);
            return new VariableValue(ValueType.STRING, str is null ? "null" : str);
        }

    }
}
