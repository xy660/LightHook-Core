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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ValueType = ScriptRuntime.Core.ValueType;

namespace ScriptRuntime.Runtime
{
    public class StringManager
    {
        public static Dictionary<string, ScriptFunction> StringFunctions = new Dictionary<string, ScriptFunction>()
        {
            
            {"toArray",new ScriptFunction("toArray",
                    new List<ValueType>(),
                    ValueType.ARRAY,StrToArray) },
            {"length",new ScriptFunction("length",
                    new List<ValueType>(),
                    ValueType.NUM,StrLength) },
            {"split",new ScriptFunction("split",
                    new List<ValueType>(){ValueType.STRING},
                    ValueType.ARRAY,StringSplit) },
            {"subString",new ScriptFunction("subString",
                    new List<ValueType>(){ValueType.NUM,ValueType.NUM},
                    ValueType.STRING,SubString) },
            {"contains",new ScriptFunction("contains",
                    new List<ValueType>(){ValueType.STRING},
                    ValueType.ARRAY,StringContains) },
            {"toUniPtr",new ScriptFunction("toUniPtr",
                    new List<ValueType>(),
                    ValueType.ARRAY,StringToPtrW) },
            {"toAnsiPtr",new ScriptFunction("toAnsiPtr",
                    new List<ValueType>(),
                    ValueType.ARRAY,StringToPtrA) },
        };
        public static VariableValue StrLength(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.NUM, (double)((string)thisValue.Value).Length);
        }
        public static VariableValue StrToArray(List<VariableValue> args, VariableValue thisValue)
        {
            List<VariableValue> retn = new List<VariableValue>();
            foreach (var f in (string)thisValue.Value)
            {
                retn.Add(new VariableValue(ValueType.STRING, f.ToString()));
            }
            return new VariableValue(ValueType.ARRAY, retn);
        }
        public static VariableValue StringSplit(List<VariableValue> args, VariableValue thisValue)
        {
            List<VariableValue> retn = new List<VariableValue>();
            foreach (var f in thisValue.Value.ToString().Split(new string[] { args[0].Value.ToString() }, StringSplitOptions.RemoveEmptyEntries))
            {
                retn.Add(new VariableValue(ValueType.STRING, f.ToString()));
            }
            return new VariableValue(ValueType.ARRAY, retn);
        }
        public static VariableValue SubString(List<VariableValue> args, VariableValue thisValue)
        {
            string s = thisValue.Value.ToString().Substring((int)(double)args[0].Value, (int)(double)args[1].Value);
            return new VariableValue(ValueType.STRING, s);
        }
        public static VariableValue StringContains(List<VariableValue> args, VariableValue thisValue)
        {
            return new VariableValue(ValueType.BOOL, thisValue.Value.ToString().Contains(args[0].Value.ToString()));
        }

        static VariableValue StringToPtrW(List<VariableValue> args, VariableValue thisValue)
        {
            nint ptr = Marshal.StringToHGlobalUni(thisValue.Value.ToString());
            return new VariableValue(ValueType.PTR, ptr);
        }
        static VariableValue StringToPtrA(List<VariableValue> args, VariableValue thisValue)
        {
            nint ptr = Marshal.StringToHGlobalAnsi(thisValue.Value.ToString());
            return new VariableValue(ValueType.PTR, ptr);
        }
        
    }
}
