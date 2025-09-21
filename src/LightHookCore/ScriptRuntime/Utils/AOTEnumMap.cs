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
using System.Text;
using System.Threading.Tasks;
using ScriptRuntime.Core;
using ScriptRuntime.Runtime;
using static ScriptRuntime.Core.ASTNode;
using ValueType = ScriptRuntime.Core.ValueType;


namespace ScriptRuntime.Utils
{
    internal class AOTEnumMap
    {
        public static Dictionary<ValueType, string> ValueTypeString = new Dictionary<ValueType, string>()
{
    {ValueType.NULL, "NULL"},
    {ValueType.ANY, "ANY"},
    {ValueType.NUM, "NUM"},
    {ValueType.STRING, "STRING"},
    {ValueType.BOOL, "BOOL"},
    {ValueType.ARRAY, "ARRAY"},
    {ValueType.OBJECT, "OBJECT"},
    {ValueType.FUNCTION, "FUNCTION"},
    {ValueType.PTR, "PTR"},
};
        public static Dictionary<ASTNodeType, string> ASTNodeTypeString = new Dictionary<ASTNodeType, string>()
{
    {ASTNodeType.EOF, "EOF"},
    {ASTNodeType.VariableDefination, "VariableDefination"},
    {ASTNodeType.Assignment, "Assignment"},
    {ASTNodeType.CallFunction, "CallFunction"},
    {ASTNodeType.FunctionDefinition, "FunctionDefinition"},
    {ASTNodeType.IfStatement, "IfStatement"},
    {ASTNodeType.WhileStatement, "WhileStatement"},
    {ASTNodeType.ForEachStatement, "ForEachStatement"},
    {ASTNodeType.ReturnStatement, "ReturnStatement"},
    {ASTNodeType.ContinueStatement, "ContinueStatement"},
    {ASTNodeType.BreakStatement, "BreakStatement"},
    {ASTNodeType.BinaryOperation, "BinaryOperation"},
    {ASTNodeType.UnaryOperation, "UnaryOperation"},
    {ASTNodeType.LeftUnaryOperation, "LeftUnaryOperation"},
    {ASTNodeType.MemberAccess,"MemberAccess" },
    {ASTNodeType.ArrayLabel, "ArrayLabel"},
    {ASTNodeType.TryCatchStatement, "TryCatchStatement"},
    {ASTNodeType.BlockCode, "BlockCode"},
    {ASTNodeType.Array, "Array"},
    {ASTNodeType.Identifier, "Identifier"},
    {ASTNodeType.Number, "Number"},
    {ASTNodeType.Boolean, "Boolean"},
    {ASTNodeType.StringValue, "StringValue"},
    {ASTNodeType.Object, "Object"},
    {ASTNodeType.KeyValuePair, "KeyValuePair"},
    {ASTNodeType.AsyncStatement,"AsyncStatement" },
    {ASTNodeType.LockStatement,"LockStatement" },
};
        public static Dictionary<FunctionType, string> FunctionEnumString = new Dictionary<FunctionType, string>()
        {
            {FunctionType.Native ,"Native"},
            {FunctionType.Local,"Local"},
            {FunctionType.System,"System" },
        };
    }

}
