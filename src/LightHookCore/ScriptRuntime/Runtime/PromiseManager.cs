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
using System.Text;
using System.Threading.Tasks;

namespace ScriptRuntime.Runtime
{
    public class PromiseManager
    {
        public static Dictionary<string, ScriptFunction> PromiseFunctions = new Dictionary<string, ScriptFunction>()
        {
            {"getResult",new ScriptFunction("getResult",
                    new List<Core.ValueType>(),
                    Core.ValueType.ANY,GetResult) },
            {"isRunning",new ScriptFunction("isRunning",
                    new List<Core.ValueType>(),
                    Core.ValueType.BOOL,IsCompleted) },
            {"wait",new ScriptFunction("wait",
                    new List<Core.ValueType>(),
                    Core.ValueType.NULL,WaitTask) },
            {"waitTimeout",new ScriptFunction("waitTimeout",
                    new List<Core.ValueType>(){Core.ValueType.NUM},
                    Core.ValueType.BOOL,WaitTimeout) },
        };
        //等待，等待成功就返回true，超时返回false
        public static VariableValue WaitTimeout(List<VariableValue> args, VariableValue thisValue)
        {
            var task = (TaskContext)thisValue.Value;
            try
            {
                var res = task.thread.Join((int)(double)args[0].Value);
                return new VariableValue(Core.ValueType.BOOL, res);
            }
            catch
            {
                throw new ScriptException("无法等待任务");
            }
            
        }
        //无限等待任务，返回值为结果
        public static VariableValue WaitTask(List<VariableValue> args, VariableValue thisValue)
        {
            var task = (TaskContext)thisValue.Value;
            try
            {
                task.thread.Join();
                return task.Result;
            }
            catch
            {
                throw new ScriptException("无法等待任务");
            }
        }
        public static VariableValue IsCompleted(List<VariableValue> args, VariableValue thisValue)
        {
            var task = (TaskContext)thisValue.Value;
            return new VariableValue(Core.ValueType.BOOL, task.IsRunning);
        }
        public static VariableValue GetResult(List<VariableValue> args, VariableValue thisValue)
        {
            var task = (TaskContext)thisValue.Value;
            return task.Result;
        }
    }
}
