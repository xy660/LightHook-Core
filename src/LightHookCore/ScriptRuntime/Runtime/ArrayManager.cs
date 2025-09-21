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
using ValueType = ScriptRuntime.Core.ValueType; 

namespace ScriptRuntime.Runtime
{
    public class ArrayManager
    {
        public static Dictionary<string, ScriptFunction> ArrayFunctions = new Dictionary<string, ScriptFunction>()
        {
            {"add",new ScriptFunction("add",
                    new List<ValueType>(){ValueType.ANY},
                    ValueType.ARRAY,ArrayAdd) },
            {"length",new ScriptFunction("length",
                    new List<ValueType>(),
                    ValueType.NUM,ArrayLen) },
            {"insert",new ScriptFunction("insert",
                    new List<ValueType>(){ ValueType.NUM,ValueType.ANY},
                    ValueType.ARRAY,ArrayInsert) },
            {"remove",new ScriptFunction("remove",
                    new List<ValueType>(){ ValueType.NUM},
                    ValueType.ARRAY,ArrayRemove) },
            {"sort",new ScriptFunction("sort",
                    new List<ValueType>(),
                    ValueType.ARRAY,ArraySort) },
        };
        public static VariableValue ArraySort(List<VariableValue> args, VariableValue thisValue)
        {
            var list = (List<VariableValue>)thisValue.Value;
            list.Sort();
            return thisValue;
        }
        public static VariableValue ArrayLen(List<VariableValue> args, VariableValue thisValue) => new(ValueType.NUM, (double)((List<VariableValue>)thisValue.Value).Count);
        public static VariableValue ArrayAdd(List<VariableValue> args, VariableValue thisValue)
        {
            ((List<VariableValue>)thisValue.Value).Add(args[0]);
            return thisValue;
        }
        public static VariableValue ArrayInsert(List<VariableValue> args, VariableValue thisValue)
        {
            if ((int)(double)args[0].Value >= ((List<VariableValue>)thisValue.Value).Count ||
                (int)(double)args[0].Value < 0)
            {
                throw new ScriptException($"数组越界 len={((List<VariableValue>)thisValue.Value).Count} index={(int)(double)args[0].Value}");
            }
            ((List<VariableValue>)thisValue.Value).Insert((int)(double)args[0].Value, args[1]);
            return thisValue;
        }
        public static VariableValue ArrayRemove(List<VariableValue> args, VariableValue thisValue)
        {
            if ((int)(double)args[0].Value >= ((List<VariableValue>)thisValue.Value).Count ||
                (int)(double)args[0].Value < 0)
            {
                throw new ScriptException($"数组越界 len={((List<VariableValue>)thisValue.Value).Count} index={(int)(double)args[0].Value}");
            }
            ((List<VariableValue>)thisValue.Value).RemoveAt((int)(double)args[0].Value);
            return thisValue;
        }
    }
}