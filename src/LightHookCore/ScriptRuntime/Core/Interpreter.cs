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
using ScriptRuntime.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScriptRuntime.Core
{
    public class Interpreter
    {

        public static string VersionString = "3.2.1";

        public static object GlobalLock = new object();
        public static bool VariableEquals(VariableValue left, VariableValue right)
        {
            // 类型不同立即返回false
            if (left.VarType != right.VarType)
                return false;

            // 根据类型执行严格比较
            switch (left.VarType)
            {
                case ValueType.NUM:
                    return (double)left.Value == (double)right.Value;

                case ValueType.BOOL:
                    return (bool)left.Value == (bool)right.Value;

                case ValueType.STRING:
                    return (string)left.Value == (string)right.Value;

                case ValueType.ARRAY:
                    return object.ReferenceEquals(left.Value, right.Value); // 数组比较引用
                default:
                    return object.ReferenceEquals(left.Value, right.Value);
            }
        }
        public static Dictionary<string, VariableValue> ConstValue = new Dictionary<string, VariableValue>()
        {
            {"nullptr",new VariableValue(ValueType.PTR,(nint)0){ReadOnly = true } },
            {"null",FunctionManager.EmptyVariable },
            {"PI",new VariableValue(ValueType.NUM,(double)3.14159265){ReadOnly = true } }
        };
        static VariableValue GetVariable(string name, Dictionary<string, VariableValue> localVariable)
        {
            if(ConstValue.ContainsKey(name))
            {
                return ConstValue[name];
            }
            else if (localVariable.ContainsKey(name))
            {
                return localVariable[name];
            }
            else if(FunctionManager.FunctionTable.ContainsKey(name))
            {
                return new VariableValue(ValueType.FUNCTION, FunctionManager.FunctionTable[name]);
            }
            else
            {
                throw new ScriptException("未找到变量：" + name);
            }
        }
        static void SetVariable(string name,VariableValue value, Dictionary<string, VariableValue> localVariable)
        {
            if (localVariable.ContainsKey(name))
            {
                localVariable[name] = value;
            }
            else
            {
                localVariable.Add(name, value);
            }
        }
        static VariableValue ExecCodeBlock(ASTNode root, Dictionary<string, VariableValue> localVariable, bool clearLocalVar = true)
        {
            var retn = FunctionManager.EmptyVariable;
            List<string> backup = new List<string>(localVariable.Keys);
            //开始执行
            try
            {
                foreach (var node in root.Childrens)
                {
                    ExecuteAST(node, localVariable);
                }
            }
            finally
            {
                //处理执行出来的变量
                if (clearLocalVar)
                {
                    foreach (var variable in localVariable.ToList()) //销毁局部变量
                    {
                        if (!backup.Contains(variable.Key))
                        {
                            localVariable.Remove(variable.Key);
                        }
                    }
                }
            }
            return FunctionManager.EmptyVariable;
        }
        static VariableValue ExecUnaryOperator(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            if(root.Raw == "!")
            {
                var val = ExecuteAST(root.Childrens[0], localVariable);
                if (val.VarType != ValueType.BOOL)
                    throw new ScriptBaseException("运算符!类型不匹配，实际类型：" + val.VarType);
                val.Value = !(bool)val.Value;
                return val;
            }
            else if (root.Raw == "+")
            {
                var val = ExecuteAST(root.Childrens[0], localVariable);
                if (val.VarType != ValueType.NUM) 
                    throw new ScriptBaseException("运算符+类型不匹配，实际类型：" + val.VarType);
                val.Value = +(double)val.Value;
                return val;
            }
            else if(root.Raw == "-")
            {
                var val = ExecuteAST(root.Childrens[0],localVariable);
                if (val.VarType != ValueType.NUM)
                    throw new ScriptBaseException("运算符+类型不匹配，实际类型：" + val.VarType);
                val.Value = -(double)val.Value;
                return val;
            }
            else if(root.Raw == "++")
            {
                VariableValue variable;
                if (root.Childrens[0].NodeType == ASTNode.ASTNodeType.ArrayLabel)
                {
                    variable = ExecArrayLabel(root.Childrens[0],localVariable);
                }
                else
                {
                    variable = GetVariable(root.Childrens[0].Raw, localVariable);   
                }
                if (variable.VarType != ValueType.NUM)
                    throw new ScriptBaseException("运算符++类型不匹配，实际类型：" + variable.VarType);
                variable.Value = (double)variable.Value + 1;
                return variable;
            }
            else if (root.Raw == "--")
            {
                VariableValue variable;
                if (root.Childrens[0].NodeType == ASTNode.ASTNodeType.ArrayLabel)
                {
                    variable = ExecArrayLabel(root.Childrens[0], localVariable);
                }
                else
                {
                    variable = GetVariable(root.Childrens[0].Raw, localVariable);
                }
                if (variable.VarType != ValueType.NUM)
                    throw new ScriptBaseException("运算符--类型不匹配，实际类型：" + variable.VarType);
                variable.Value = (double)variable.Value - 1;
                return variable;
            }
            else
            {
                throw new ScriptException("不支持的一元运算符");
            }
        }
        static VariableValue ExecVariableDefination(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            if(localVariable.ContainsKey(root.Raw))
            {
                throw new ScriptException("变量重复定义：" + root);
            }
            else
            {
                localVariable.Add(root.Raw, new VariableValue(ValueType.NULL,null));
            }
            return FunctionManager.EmptyVariable;
        }
       
        static void ExecReturnStatement(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            throw new ReturnException(ExecuteAST(root.Childrens[0], localVariable));
        }
        static VariableValue ExecAssignment(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            if (root.Raw == "=")
            {
                var target = ExecuteAST(root.Childrens[0],localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);
                target.Value = val.Value;
                target.VarType = val.VarType;
                return target;
            }
            else if (root.Raw == "+=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType == ValueType.NUM && val.VarType == ValueType.NUM)
                {
                    target.Value = (double)target.Value + (double)val.Value;
                }
                else
                {
                    target.Value = target.Value.ToString() + val.Value.ToString();
                    target.VarType = ValueType.STRING;
                }
                return target;
            }
            else if (root.Raw == "-=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType != ValueType.NUM || val.VarType != ValueType.NUM)
                {
                    throw new ScriptException("-=类型不匹配，实际类型：" + val.VarType);
                }
                target.Value = (double)target.Value - (double)val.Value;
                return target;
            }
            else if (root.Raw == "*=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType != ValueType.NUM || val.VarType != ValueType.NUM)
                {
                    throw new ScriptException("*=类型不匹配，实际类型：" + val.VarType);
                }
                target.Value = (double)target.Value * (double)val.Value;
                return target;
            }
            else if (root.Raw == "/=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType != ValueType.NUM || val.VarType != ValueType.NUM)
                {
                    throw new ScriptException("/=类型不匹配，实际类型：" + val.VarType);
                }
                target.Value = (double)target.Value / (double)val.Value;
                return target;
            }
            else if (root.Raw == "%=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType != ValueType.NUM || val.VarType != ValueType.NUM)
                {
                    throw new ScriptException("%=类型不匹配，实际类型：" + val.VarType);
                }
                target.Value = (double)target.Value % (double)val.Value;
                return target;
            }
            else if (root.Raw == "&=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType != ValueType.NUM || val.VarType != ValueType.NUM)
                {
                    throw new ScriptException("&=类型不匹配，实际类型：" + val.VarType);
                }
                target.Value = (int)(double)target.Value & (int)(double)val.Value;
                return target;
            }
            else if (root.Raw == "|=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType != ValueType.NUM || val.VarType != ValueType.NUM)
                {
                    throw new ScriptException("|=类型不匹配，实际类型：" + val.VarType);
                }
                target.Value = (int)(double)target.Value | (int)(double)val.Value;
                return target;
            }
            else if (root.Raw == "^=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType != ValueType.NUM || val.VarType != ValueType.NUM)
                {
                    throw new ScriptException("^=类型不匹配，实际类型：" + val.VarType);
                }
                target.Value = (int)(double)target.Value ^ (int)(double)val.Value;
                return target;
            }
            else if (root.Raw == "<<=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType != ValueType.NUM || val.VarType != ValueType.NUM)
                {
                    throw new ScriptException("<<=类型不匹配，实际类型：" + val.VarType);
                }
                target.Value = (int)(double)target.Value << (int)(double)val.Value;
                return target;
            }
            else if (root.Raw == ">>=")
            {
                var target = ExecuteAST(root.Childrens[0], localVariable);
                var val = ExecuteAST(root.Childrens[1], localVariable);

                if (target.VarType != ValueType.NUM || val.VarType != ValueType.NUM)
                {
                    throw new ScriptException(">>=类型不匹配，实际类型：" + val.VarType);
                }
                target.Value = (int)(double)target.Value >> (int)(double)val.Value;
                return target;
            }
            else
            {
                throw new ScriptException($"未知的赋值运算符: {root.Raw}");
            }
        }
        static VariableValue ExecIfStatement(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            var condResult = ExecuteAST(root.Childrens[0], localVariable);
            if (condResult.VarType != ValueType.BOOL)
                throw new ScriptException("if语句条件类型不为BOOL");
            if ((bool)condResult.Value)
            {
                ExecuteAST(root.Childrens[1], localVariable);
            }
            else
            {
                if(root.Childrens.Count > 2)
                {
                    ExecuteAST(root.Childrens[2], localVariable);
                }
            }
            
            return FunctionManager.EmptyVariable;
        }
        static VariableValue ExecWhileStatement(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            var condResult = ExecuteAST(root.Childrens[0], localVariable);
            while(condResult.VarType == ValueType.BOOL && (bool)condResult.Value)
            {
                try
                {
                    ExecuteAST(root.Childrens[1], localVariable);
                }
                catch(BreakException ex)
                {
                    break;
                }
                catch(ContuineException ex)
                {
                    continue;
                }
                condResult = ExecuteAST(root.Childrens[0], localVariable);
            }
            return FunctionManager.EmptyVariable;
        }
        static VariableValue ExecForEachStatement(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            var forEachVar = root.Childrens[0].Raw;
            var array = ExecuteAST(root.Childrens[1],localVariable);
            if (array.VarType != ValueType.ARRAY)
                throw new ScriptException("for-each循环表达式结果不为集合");
            localVariable.Add(forEachVar,FunctionManager.EmptyVariable);
            try
            {
                foreach (var child in (List<VariableValue>)array.Value)
                {
                    SetVariable(forEachVar, child, localVariable);
                    try
                    {
                        ExecuteAST(root.Childrens[2], localVariable);
                    }
                    catch (BreakException ex)
                    {
                        break;
                    }
                    catch (ContuineException ex)
                    {
                        continue;
                    }
                }
            }
            finally
            {
                localVariable.Remove(forEachVar);
            }          
            return FunctionManager.EmptyVariable;
        }
        static VariableValue ExecTryCatchStatement(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            //保存一下当前的栈大小，便于引发异常后恢复，删掉多出来的
            var currentStackDepth = TaskContext.ThreadContext[TaskContext.GetCurrentThreadId()].StackTrace.Count;
            try
            {
                ExecuteAST(root.Childrens[0],localVariable);
            }
            catch(ScriptException ex) 
            {
                //保存异常栈，然后删掉多余的，恢复到当前位置
                var stackTrace = StringUtils.GenerateStackTrace(TaskContext.ThreadContext[TaskContext.GetCurrentThreadId()].StackTrace);
                while (TaskContext.ThreadContext[TaskContext.GetCurrentThreadId()].StackTrace.Count > currentStackDepth)
                {
                    TaskContext.ThreadContext[TaskContext.GetCurrentThreadId()].StackTrace.Pop();
                }

                if (localVariable.ContainsKey(root.Childrens[1].Raw))
                {
                    throw new ScriptException($"变量{root.Childrens[1].Raw}已存在");
                }

                Dictionary<string, VariableValue> container = new Dictionary<string, VariableValue>()
                {
                    {"message",new VariableValue(ValueType.STRING,ex.Message)},
                    {"stackTrace",new VariableValue(ValueType.STRING,stackTrace)}
                };
                
                localVariable.Add(root.Childrens[1].Raw, new VariableValue(ValueType.OBJECT, container));
                try
                {
                    ExecuteAST(root.Childrens[2], localVariable);
                }
                finally
                {
                    localVariable.Remove(root.Childrens[1].Raw);
                }
            }
            return FunctionManager.EmptyVariable;
        }
        static VariableValue ExecFuncDefStatement(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            List<string> args = new List<string>();
            for(int i = 0;i < root.Childrens.Count - 1;i++)
            {
                args.Add(root.Childrens[i].Raw);
            }
            if (root.Raw != string.Empty)
            {
                return new VariableValue(ValueType.FUNCTION, FunctionManager.RegisterFunction(root.Raw, args, root.Childrens.Last()));
            }
            else
            {
                var argTypes = new List<ValueType>();
                for (int i = 0; i < args.Count; i++)
                {
                    argTypes.Add(ValueType.ANY);
                }
                return new VariableValue(ValueType.FUNCTION, new ScriptFunction(string.Empty, argTypes, args, ValueType.ANY, root.Childrens.Last()));
            }
        }
        static VariableValue ExecFunctionCall(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {   
            List<VariableValue> args = new List<VariableValue>();
            var targetFunctionIdentifier = root.Childrens[0];
            for (int i = 1; i < root.Childrens.Count; i++)
            {
                var ast = root.Childrens[i];
                args.Add(ExecuteAST(ast, localVariable));
            }
            ScriptFunction? targetFunction;
            VariableValue? thisValue = null;
            if (targetFunctionIdentifier.NodeType == ASTNode.ASTNodeType.Identifier
                && FunctionManager.FunctionTable.TryGetValue(targetFunctionIdentifier.Raw, out targetFunction))
            {
                //弃用这个，改成统一调用入口
                //return FunctionManager.CallFunction(targetFunctionIdentifier.Raw, args);
                thisValue = FunctionManager.EmptyVariable; //全局函数没有this
            }
            else 
            {
                var result = ExecuteAST(targetFunctionIdentifier, localVariable);
                if (result.VarType != ValueType.FUNCTION)
                {
                    throw new ScriptException("非函数类型进行函数调用");
                }
                targetFunction = (ScriptFunction)result.Value;
                thisValue = result.Parent;
            }
            try
            {
                var retn = targetFunction.Invoke(args, thisValue);
                return retn;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#else
                throw new ScriptException(ex.Message); //转换系统异常到脚本异常
#endif
            }
        }
        static VariableValue ExecLeftUnaryOperator(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            if (root.Raw == "++")
            {
                VariableValue variable;
                if (root.Childrens[0].NodeType == ASTNode.ASTNodeType.ArrayLabel)
                {
                    variable = ExecArrayLabel(root.Childrens[0],localVariable);
                }
                else 
                {
                    variable = GetVariable(root.Childrens[0].Raw, localVariable);
                }
                if (variable.VarType != ValueType.NUM)
                    throw new ScriptBaseException("运算符++类型不匹配，实际类型：" + variable.VarType);
                double ret = (double)variable.Value;
                variable.Value = ret + 1;
                return new VariableValue(ValueType.NUM, ret);
            }
            else if (root.Raw == "--")
            {
                VariableValue variable;
                if (root.Childrens[0].NodeType == ASTNode.ASTNodeType.ArrayLabel)
                {
                    variable = ExecArrayLabel(root.Childrens[0], localVariable);
                }
                else
                {
                    variable = GetVariable(root.Childrens[0].Raw, localVariable);
                }
                if (variable.VarType != ValueType.NUM)
                    throw new ScriptBaseException("运算符--类型不匹配，实际类型：" + variable.VarType);
                double ret = (double)variable.Value;
                variable.Value = ret - 1;
                return new VariableValue(ValueType.NUM,ret);
            }
            else
            {
                throw new ScriptException("不支持的一元运算符");
            }
        }
        static VariableValue ExecBinaryOperator(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            var left = ExecuteAST(root.Childrens[0], localVariable);
            var right = ExecuteAST(root.Childrens[1], localVariable);

            if (root.Raw == "+")
            {
                if (left.VarType == ValueType.NUM && right.VarType == ValueType.NUM)
                {
                    return new VariableValue(ValueType.NUM, (double)left.Value + (double)right.Value);
                }
                else
                {
                    return new VariableValue(ValueType.STRING, left.Value.ToString() + right.Value.ToString());
                }
            }
            else if (root.Raw == "-")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"-类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.NUM, (double)left.Value - (double)right.Value);
            }
            else if (root.Raw == "*")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"*类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.NUM, (double)left.Value * (double)right.Value);
            }
            else if (root.Raw == "/")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"/类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.NUM, (double)left.Value / (double)right.Value);
            }
            else if (root.Raw == "%")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"%类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.NUM, (double)left.Value % (double)right.Value);
            }
            else if (root.Raw == "|")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"|类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.NUM, (double)((long)(double)left.Value | (long)(double)right.Value));
            }
            else if (root.Raw == "^")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"^类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.NUM, (double)((long)(double)left.Value ^ (long)(double)right.Value));
            }
            else if (root.Raw == "&")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"&类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.NUM, (double)((long)(double)left.Value & (long)(double)right.Value));
            }
            else if (root.Raw == ">>")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($">>类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.NUM, (double)((int)(double)left.Value >> (int)(double)right.Value));
            }
            else if (root.Raw == "<<")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"<<类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.NUM, (double)((int)(double)left.Value << (int)(double)right.Value));
            }
            else if (root.Raw == "<")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"<类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.BOOL, (double)left.Value < (double)right.Value);
            }
            else if (root.Raw == ">")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($">类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.BOOL, (double)left.Value > (double)right.Value);
            }
            else if (root.Raw == ">=")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($">=类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.BOOL, (double)left.Value >= (double)right.Value);
            }
            else if (root.Raw == "<=")
            {
                if (left.VarType != ValueType.NUM || right.VarType != ValueType.NUM)
                    throw new ScriptException($"<=类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.BOOL, (double)left.Value <= (double)right.Value);
            }
            else if (root.Raw == "==")
            {
                return new VariableValue(ValueType.BOOL, VariableEquals(left, right));
            }
            else if (root.Raw == "!=")
            {
                return new VariableValue(ValueType.BOOL, !VariableEquals(left, right));
            }
            else if (root.Raw == "&&")
            {
                if (left.VarType != ValueType.BOOL || right.VarType != ValueType.BOOL)
                    throw new ScriptException($"&&类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.BOOL, (bool)left.Value && (bool)right.Value);
            }
            else if (root.Raw == "||")
            {
                if (left.VarType != ValueType.BOOL || right.VarType != ValueType.BOOL)
                    throw new ScriptException($"||类型不匹配，type(left)={left.VarType} type(right)={right.VarType}");
                return new VariableValue(ValueType.BOOL, (bool)left.Value || (bool)right.Value);
            }
            else
            {
                throw new ScriptException("不支持的二元运算符: " + root.Raw);
            }
        }
        static VariableValue ExecArray(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            var list = new List<VariableValue>();
            foreach(var child in root.Childrens)
            {
                list.Add(ExecuteAST(child,localVariable));
            }
            return new VariableValue(ValueType.ARRAY,list);
        }
        static VariableValue ExecArrayLabel(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            var left = ExecuteAST(root.Childrens[0], localVariable);
            //var left = GetVariable(leftResult.Value.ToString(),localVariable);
            if(left.VarType != ValueType.ARRAY && left.VarType != ValueType.OBJECT)
            {
                throw new ScriptException($"[]下标操作无法应用于{AOTEnumMap.ValueTypeString[left.VarType]}类型");
            }
            var index = ExecuteAST(root.Childrens[1],localVariable);
            if(left.VarType == ValueType.ARRAY)
            {
                if (index.VarType != ValueType.NUM)
                {
                    throw new ScriptException("无法使用非数字类型作为集合索引");
                }
                return ((List<VariableValue>)left.Value)[(int)(double)index.Value];
            }
            else
            {
                if(index.VarType != ValueType.STRING)
                {
                    throw new ScriptException("无法使用非数字类型作为对象索引");
                }
                return ((Dictionary<string, VariableValue>)left.Value)[(string)index.Value];
            } 
        }
        static VariableValue ExecObjectParse(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            Dictionary<string, VariableValue> ObjContainer = new Dictionary<string, VariableValue>();
            VariableValue retn = new VariableValue(ValueType.OBJECT, ObjContainer);
            foreach (var item in root.Childrens)
            {
                if(item.NodeType != ASTNode.ASTNodeType.KeyValuePair)
                {
                    throw new ScriptException("对象定义解析出错");
                }
                var key = item.Childrens[0].Raw;
                var value = ExecuteAST(item.Childrens[1],localVariable);
                value.Parent = retn;
                ObjContainer.Add(key, value);
            }
            return retn;
        }
        static VariableValue ExecMemberAccess(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            var parent = ExecuteAST(root.Childrens[0],localVariable);
            if (parent.VarType == ValueType.OBJECT)
            {
                Dictionary<string, VariableValue> objContainer = (Dictionary<string, VariableValue>)parent.Value;
                VariableValue? result;
                if (objContainer.TryGetValue(root.Childrens[1].Raw, out result))
                {
                    return result;
                }
                else if (ObjectManager.ObjectFunctions.ContainsKey(root.Childrens[1].Raw))
                {
                    var retn = new VariableValue(ValueType.FUNCTION, ObjectManager.ObjectFunctions[root.Childrens[1].Raw]);
                    retn.Parent = parent;
                    return retn;
                }
                throw new ScriptException($"不存在成员 {root.Childrens[1].Raw}");
            }
            else if(parent.VarType == ValueType.ARRAY)
            {
                if (ArrayManager.ArrayFunctions.ContainsKey(root.Childrens[1].Raw))
                {
                    var retn = new VariableValue(ValueType.FUNCTION, ArrayManager.ArrayFunctions[root.Childrens[1].Raw]);
                    retn.Parent = parent;
                    return retn;
                }
                throw new ScriptException($"不存在成员 {root.Childrens[1].Raw}");
            }
            else if(parent.VarType == ValueType.STRING)
            {
                if (StringManager.StringFunctions.ContainsKey(root.Childrens[1].Raw))
                {
                    var retn = new VariableValue(ValueType.FUNCTION, StringManager.StringFunctions[root.Childrens[1].Raw]);
                    retn.Parent = parent;
                    return retn;
                }
                throw new ScriptException($"不存在成员 {root.Childrens[1].Raw}");
            }
            else if(parent.VarType == ValueType.PTR)
            {
                if (PtrManager.PtrFunctions.ContainsKey(root.Childrens[1].Raw))
                {
                    var retn = new VariableValue(ValueType.FUNCTION, PtrManager.PtrFunctions[root.Childrens[1].Raw]);
                    retn.Parent = parent;
                    return retn;
                }
                throw new ScriptException($"不存在成员 {root.Childrens[1].Raw}");
            }
            else if(parent.VarType == ValueType.PROMISE)
            {
                if (PromiseManager.PromiseFunctions.ContainsKey(root.Childrens[1].Raw))
                {
                    var retn = new VariableValue(ValueType.FUNCTION, PromiseManager.PromiseFunctions[root.Childrens[1].Raw]);
                    retn.Parent = parent;
                    return retn;
                }
                throw new ScriptException($"不存在成员 {root.Childrens[1].Raw}");
            }
            else
            {
                throw new ScriptException($"{AOTEnumMap.ValueTypeString[parent.VarType]}类型无法进行成员访问");
            }
            
        }
        static VariableValue ExecAsyncStatement(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            using (var startEvent = new ManualResetEvent(false))
            {
                var context = new TaskContext();
                var t = new Thread(() =>
                {
                    startEvent.Set();
                    context.IsRunning = true;
                    TaskContext.ThreadContext.Add(TaskContext.GetCurrentThreadId(), context);
                    var result = ExecuteAST(root.Childrens[0], localVariable);
                    TaskContext.ThreadContext[TaskContext.GetCurrentThreadId()].Result = result;
                    context.IsRunning = false;
                    TaskContext.ThreadContext.Remove(TaskContext.GetCurrentThreadId()); //任务结束移除
                });
                t.Start();
                context.thread = t;
                if(!startEvent.WaitOne(1000)) //等待任务启动
                {
                    throw new ScriptException("错误，异步任务启动超时");
                }
                var promise = new VariableValue(ValueType.PROMISE, context);
                return promise;
            }
        }
        static VariableValue ExecLockStatement(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            var lockObj = ExecuteAST(root.Childrens[0],localVariable).Value;
            lock(lockObj)
            {
                return ExecuteAST(root.Childrens[1],localVariable);
            }
        }
        public static VariableValue ExecuteAST(ASTNode root, Dictionary<string, VariableValue> localVariable)
        {
            switch (root.NodeType)
            {
                case ASTNode.ASTNodeType.BlockCode: return ExecCodeBlock(root,localVariable);
                case ASTNode.ASTNodeType.UnaryOperation: return ExecUnaryOperator(root,localVariable);
                case ASTNode.ASTNodeType.LeftUnaryOperation: return ExecLeftUnaryOperator(root,localVariable);
                case ASTNode.ASTNodeType.BinaryOperation: return ExecBinaryOperator(root,localVariable);
                case ASTNode.ASTNodeType.ArrayLabel: return ExecArrayLabel(root,localVariable);
                case ASTNode.ASTNodeType.VariableDefination: return ExecVariableDefination(root,localVariable);
                case ASTNode.ASTNodeType.Assignment: return ExecAssignment(root,localVariable);
                case ASTNode.ASTNodeType.Number: return new VariableValue(ValueType.NUM, double.Parse(root.Raw));
                case ASTNode.ASTNodeType.StringValue:return new VariableValue(ValueType.STRING, root.Raw);
                case ASTNode.ASTNodeType.Array: return ExecArray(root,localVariable);
                case ASTNode.ASTNodeType.Boolean: return new VariableValue(ValueType.BOOL, bool.Parse(root.Raw));
                case ASTNode.ASTNodeType.BreakStatement: throw new BreakException();
                case ASTNode.ASTNodeType.ContinueStatement: throw new ContuineException();
                case ASTNode.ASTNodeType.ReturnStatement: ExecReturnStatement(root,localVariable);break;
                case ASTNode.ASTNodeType.Identifier: return GetVariable(root.Raw,localVariable);
                case ASTNode.ASTNodeType.IfStatement:return ExecIfStatement(root,localVariable);
                case ASTNode.ASTNodeType.WhileStatement: return ExecWhileStatement(root,localVariable);
                case ASTNode.ASTNodeType.ForEachStatement: return ExecForEachStatement(root,localVariable);
                case ASTNode.ASTNodeType.FunctionDefinition:return ExecFuncDefStatement(root,localVariable);
                case ASTNode.ASTNodeType.CallFunction:return ExecFunctionCall(root,localVariable);
                case ASTNode.ASTNodeType.TryCatchStatement:return ExecTryCatchStatement(root,localVariable);
                case ASTNode.ASTNodeType.Object: return ExecObjectParse(root,localVariable);
                case ASTNode.ASTNodeType.MemberAccess: return ExecMemberAccess(root,localVariable);
                case ASTNode.ASTNodeType.AsyncStatement:return ExecAsyncStatement(root,localVariable);
                case ASTNode.ASTNodeType.LockStatement:return ExecLockStatement(root,localVariable);
            }
            throw new SyntaxException("不支持的ASTNode类型", $"{AOTEnumMap.ASTNodeTypeString[root.NodeType]} {root.Raw}");
        }

        //执行代码块记得try一下break和contuine异常
        public static VariableValue ExecuteBlock(ASTNode root, Dictionary<string, VariableValue> localVariable, bool clearLocalVar = true)
        {
            return ExecCodeBlock(root,localVariable,clearLocalVar);
        }
    }
    public class TaskContext
    {
        public static ulong GetCurrentThreadId()
        {
            return NativeThread.GetCurrentNativeThreadId();
        }

        public static Dictionary<ulong, TaskContext> ThreadContext = new Dictionary<ulong, TaskContext>();

        public Stack<ScriptFunction> StackTrace = new Stack<ScriptFunction>();

        public VariableValue Result = FunctionManager.EmptyVariable;

        public Thread thread;

        public bool IsRunning = false;
        
    }
}
