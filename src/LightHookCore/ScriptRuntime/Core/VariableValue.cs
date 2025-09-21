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
using ScriptRuntime.Runtime;

namespace ScriptRuntime.Core
{
    //此类用于表示脚本中的变量
    public class VariableValue : IComparable<VariableValue>
    {
        public VariableValue Parent; //父对象
        public bool ReadOnly = false;
        public ValueType VarType;
        //public object Value;
        private object _value;
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!ReadOnly)
                {
                    _value = value;
                }
            }
        }
        public VariableValue(ValueType type, object value)
        {
            VarType = type;
            _value = value;
            Parent = FunctionManager.EmptyVariable;
        }
        //取大小用于排序，数字直接是面值，字符串是长度，布尔是0或1
        public int GetSize()
        {
            var curSzie = 0;
            switch (this.VarType)
            {
                case ValueType.NUM: curSzie = (int)(double)Value; break;
                case ValueType.STRING: curSzie = ((string)Value).Length; break;
                case ValueType.PTR: curSzie = (int)(nint)Value; break;
                case ValueType.BOOL: curSzie = ((bool)Value) ? 1 : 0; break;
                default:
                    curSzie = -1;
                    break;
            }
            return curSzie;
        }
        public int CompareTo(VariableValue other)
        {
            return GetSize() - other.GetSize();
        }
    }

    public enum ValueType
    {
        NULL,
        CONTEXT,
        ANY,
        NUM,
        STRING,
        BOOL,
        ARRAY,
        OBJECT,
        FUNCTION,
        PTR,
        PROMISE,
    }
}