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
using static StringUtils;
using static SyntaxUtils;
using static ScriptRuntime.Core.Lexer;
using ScriptRuntime.Core;
using ScriptRuntime.Runtime;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks.Dataflow;
using System.Collections;
using ScriptRuntime.Utils;

namespace ScriptRuntime.Core
{
    //V1.0.0版本重构，先解析AST再丢去解释器执行，支持更多语法
    public static class Parser
    {
        //关键字集合
        public static HashSet<string> KeyWords = new HashSet<string>() { "if", "for", "try", "catch", "while", "function" };
        public static Dictionary<string, int> OperatorPower = new Dictionary<string, int>()
        {
            // 一元运算符
            {"++", 16},    // 后置递增
            {"--", 16},    // 后置递减
            {"!", 15},     // 逻辑非
            {"~", 15},     // 按位取反
            
            // 乘除/模
            {"*", 14},
            {"/", 14},
            {"%", 14},     // 取模
            
            // 加减
            {"+", 13},
            {"-", 13},
            
            // 位移
            {"<<", 12},    // 左移
            {">>", 12},    // 右移
            
            // 关系运算符
            {"<", 11},
            {"<=", 11},
            {">", 11},
            {">=", 11},
            {"is", 11},    // 类型检查
            {"as", 11},    // 安全类型转换
            
            // 相等性
            {"==", 10},
            {"!=", 10},
            
            // 按位与
            {"&", 9},
            
            // 按位异或
            {"^", 8},
            
            // 按位或
            {"|", 7},
            
            // 逻辑与
            {"&&", 6},
            
            // 逻辑或
            {"||", 5},
            
            // 空值合并
            {"??", 4},
            
            // 三元条件
            {"?:", 3},
            
            // 赋值和复合赋值
            {"=", 2},
            {"+=", 2},
            {"-=", 2},
            {"*=", 2},
            {"/=", 2},
            {"%=", 2},
            {"&=", 2},
            {"|=", 2},
            {"^=", 2},
            {"<<=", 2},
            {">>=", 2},
        };

        public static List<string> UnaryOperator = new List<string>() { "++", "+", "-", "--", "!" };
        public static List<List<string>> PowerToOperators = new List<List<string>>()
        {
            {UnaryOperator },
            { new List<string> {"*", "/", "%"}}, // 乘除模
            { new List<string> {"+", "-"}},      // 加减
            { new List<string> {"<<", ">>"}},    // 位移
            { new List<string> {"<", "<=", ">", ">="}}, // 关系运算符
            { new List<string> {"==", "!="}},    // 相等性
            {  new List<string> {"&"}},           // 按位与
            {  new List<string> {"^"}},           // 按位异或
            {  new List<string> {"|"}},           // 按位或
            { new List<string> {"&&"}},          // 逻辑与
            {  new List<string> {"||"}},          // 逻辑或
            { new List<string> {"??"}},          // 空值合并
            { new List<string> {"=", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "^=", "<<=", ">>="}} // 赋值类
        };

        static List<Token> ASTParseStream = new List<Token>();
        static int _pos;

        static Token PollToken() //取出
        {
            if (_pos < ASTParseStream.Count)
            {
                return ASTParseStream[_pos++];
            }
            return new Token(";", TokenType.EOF);
        }

        static Token PeekToken() //查看但是不取出
        {
            if (_pos < ASTParseStream.Count)
            {
                return ASTParseStream[_pos];
            }
            return new Token(";", TokenType.EOF);
        }

        static ASTNode ProcessObjectDefination(Token part)
        {
            var sp = FunctionManager.SplitArgSyntax(part.raw);
            var retn = new ASTNode(ASTNode.ASTNodeType.Object, string.Empty);
            foreach (var item in sp)
            {
                var tokens = SplitTokens(item);
                var name = tokens[0];
                if (!char.IsLetter(name.raw[0])) 
                {
                    throw new SyntaxException("对象成员名称类型错误：" + name.raw, ClipTokenString(0, tokens.Count, tokens));
                }
                if (tokens[1].raw != ":")
                {
                    throw new SyntaxException("对象定义语法错误", ClipTokenString(0,tokens.Count,tokens));
                }
                var value = BuildASTByTokens(tokens.Skip(2).ToList());
                if(value.Childrens.Count != 1)
                {
                    throw new SyntaxException("值多解析歧义", ClipTokenString(0, tokens.Count, tokens));
                }
                ASTNode objNode = new ASTNode(ASTNode.ASTNodeType.KeyValuePair, string.Empty);
                objNode.Childrens.Add(new ASTNode(ASTNode.ASTNodeType.Identifier, name.raw));
                objNode.Childrens.Add(value.Childrens[0]);
                retn.Childrens.Add(objNode);
            }
            return retn;
        }
        
        static ASTNode ProcessPrimaryExpression()
        {
            var val = PollToken();
            if (val.tokenType == TokenType.Part) //处理括号内内容，拆出来重新递归解析
            {
                return BuildASTByTokens(SplitTokens(val.raw)).Childrens[0];
            }
            else if (val.tokenType == TokenType.IndexLabel) // 数组字面量
            {
                var arr = FunctionManager.SplitArgSyntax(val.raw);
                ASTNode ret = new ASTNode(ASTNode.ASTNodeType.Array, string.Empty);
                foreach (var element in arr)
                {
                    ret.Childrens.Add(BuildASTByTokens(SplitTokens(element)).Childrens[0]);
                }
                return ret;
            }
            else if(val.tokenType == TokenType.CodeBlock) //对象字面量
            {
                return ProcessObjectDefination(val);
            }
            else if (val.tokenType != TokenType.Idfefinder)
            {
                throw new SyntaxException("无效的解析值类型", val.raw);
            }

            if (char.IsDigit(val.raw[0]))
            {
                return new ASTNode(ASTNode.ASTNodeType.Number, val.raw);
            }
            else if (val.raw[0] == '"')
            {
                return new ASTNode(ASTNode.ASTNodeType.StringValue, ProcessEscapeToChar(val.raw));
            }
            else if (val.raw == "true" || val.raw == "false")
            {
                return new ASTNode(ASTNode.ASTNodeType.Boolean, val.raw);
            }
            else if (val.raw == "break")
            {
                return new ASTNode(ASTNode.ASTNodeType.BreakStatement, string.Empty);
            }
            else if (val.raw == "continue")
            {
                return new ASTNode(ASTNode.ASTNodeType.ContinueStatement, string.Empty);
            }
            else if (val.raw == "return")
            {
                var retnAST = new ASTNode(ASTNode.ASTNodeType.ReturnStatement, string.Empty);
                retnAST.Childrens.Add(ProcessOperation(PowerToOperators.Count - 1));
                return retnAST;
            }
            else if (val.raw == "var")
            {
                var varAST = new ASTNode(ASTNode.ASTNodeType.VariableDefination, PeekToken().raw);
                return varAST;
            }
            else if (val.raw == "function")
            {
                var argsToken = PollToken();
                if (argsToken.tokenType != TokenType.Part)
                {
                    throw new SyntaxException("函数定义语法不正确", ClipTokenString(_pos - 1, 2, ASTParseStream));
                }
                var args = argsToken.raw.Replace(" ", "").Split(",", StringSplitOptions.RemoveEmptyEntries);
                var block = PollToken();
                var blockAST = BuildASTByTokens(SplitTokens(block.raw));
                var funcAST = new ASTNode(ASTNode.ASTNodeType.FunctionDefinition, string.Empty);
                foreach (var arg in args)
                {
                    funcAST.Childrens.Add(new ASTNode(ASTNode.ASTNodeType.Identifier, arg));
                }
                funcAST.Childrens.Add(blockAST);
                return funcAST;
            }
            else if(val.raw == "async") //创建异步任务
            {
                var func = ProcessLogicStatement();
                var ast = new ASTNode(ASTNode.ASTNodeType.AsyncStatement, string.Empty);
                ast.Childrens.Add(func);
                return ast;
            }
            else if(val.raw == "lock") //同步块锁代码
            {
                var lockObj = PollToken();
                var code = PollToken();
                
                if(lockObj.tokenType != TokenType.Part || code.tokenType != TokenType.CodeBlock)
                {
                    throw new SyntaxException("lock语句语法不正确",ClipTokenString(_pos - 3,ASTParseStream.Count,ASTParseStream));
                }
                var lockIdf = BuildASTByTokens(SplitTokens(lockObj.raw)).Childrens[0];
                if(lockIdf.NodeType != ASTNode.ASTNodeType.Identifier)
                {
                    throw new SyntaxException("lock语句锁对象表达式不正确", ClipTokenString(_pos - 3, ASTParseStream.Count, ASTParseStream));
                }
                var ast = BuildASTByTokens(SplitTokens(code.raw));
                var ret = new ASTNode(ASTNode.ASTNodeType.LockStatement, string.Empty);
                ret.Childrens.Add(lockIdf);
                ret.Childrens.Add(ast);
                return ret;
            }
            else
            {
                return new ASTNode(ASTNode.ASTNodeType.Identifier, val.raw);
            }
        }

        //成员访问
        static ASTNode ProcessMemberAccess()
        {
            var left = ProcessPrimaryExpression();

            while (true)
            {
                if (PeekToken().tokenType == TokenType.Operator && PeekToken().raw == ".")
                {
                    PollToken(); // 消耗点操作符
                    var right = ProcessPrimaryExpression();
                    var ast = new ASTNode(ASTNode.ASTNodeType.MemberAccess, string.Empty);
                    ast.Childrens.Add(left);
                    ast.Childrens.Add(right);
                    left = ast;
                }
                else if (PeekToken().tokenType == TokenType.Part)
                {
                    var funcAST = new ASTNode(ASTNode.ASTNodeType.CallFunction, string.Empty);
                    funcAST.Childrens.Add(left);
                    var args = FunctionManager.SplitArgSyntax(PollToken().raw);
                    foreach (var arg in args)
                    {
                        var argAST = BuildASTByTokens(SplitTokens(arg));
                        if (argAST.Childrens.Count != 1)
                        {
                            throw new SyntaxException("函数调用参数内出现多重解析的表达式", GetASTString(argAST));
                        }
                        funcAST.Childrens.Add(argAST.Childrens[0]);
                    }
                    left = funcAST;
                }
                else if (PeekToken().tokenType == TokenType.IndexLabel)
                {
                    var indexAST = new ASTNode(ASTNode.ASTNodeType.ArrayLabel, string.Empty);
                    indexAST.Childrens.Add(left);
                    var indexTokens = SplitTokens(PollToken().raw);
                    var indexExpr = BuildASTByTokens(indexTokens);
                    if (indexExpr.Childrens.Count != 1)
                    {
                        throw new SyntaxException("数组下标表达式内出现多重解析", GetASTString(indexExpr));
                    }
                    indexAST.Childrens.Add(indexExpr.Childrens[0]);
                    left = indexAST;
                }
                else
                {
                    break;
                }
            }

            return left;
        }

        //数学和逻辑操作符处理，按照顶上依次下降
        static ASTNode ProcessOperation(int power)
        {
            if (power == 0)
            {
                if (PeekToken().tokenType == TokenType.Operator && UnaryOperator.Contains(PeekToken().raw))
                {
                    var op = PollToken();
                    ASTNode operand = ProcessOperation(0);
                    var unaryNode = new ASTNode(ASTNode.ASTNodeType.UnaryOperation, op.raw);
                    unaryNode.Childrens.Add(operand);
                    return unaryNode;
                }
                else
                {
                    var expr = ProcessMemberAccess();

                    if (PeekToken().tokenType == TokenType.Operator &&
                        (PeekToken().raw == "++" || PeekToken().raw == "--"))
                    {
                        var op = PollToken();
                        var ret = new ASTNode(ASTNode.ASTNodeType.LeftUnaryOperation, op.raw);
                        ret.Childrens.Add(expr);
                        return ret;
                    }

                    return expr;
                }
            }

            ASTNode left = ProcessOperation(power - 1);
            while (PeekToken().tokenType != TokenType.EOF && PowerToOperators[power].Contains(PeekToken().raw))
            {
                var op = PollToken();
                ASTNode right = ProcessOperation(power - 1);
                var ret = power == PowerToOperators.Count - 1 ?
                    new ASTNode(ASTNode.ASTNodeType.Assignment, op.raw) :
                    new ASTNode(ASTNode.ASTNodeType.BinaryOperation, op.raw);
                ret.Childrens.Add(left);
                ret.Childrens.Add(right);
                left = ret;
            }

            return left;
        }

        //逻辑控制语句
        static ASTNode ProcessLogicStatement()
        {
            if (KeyWords.Contains(PeekToken().raw))
            {
                var key = PollToken();
                if (key.raw == "if")
                {
                    var ifSyntax = PollToken();
                    if (ifSyntax.tokenType != TokenType.Part)
                    {
                        throw new SyntaxException("if语句后条件表达式错误", ifSyntax.raw);
                    }
                    ASTNode astIfStat = new ASTNode(ASTNode.ASTNodeType.IfStatement, string.Empty);
                    var synAST = BuildASTByTokens(SplitTokens(ifSyntax.raw));
                    if (synAST.Childrens.Count != 1)
                    {
                        throw new SyntaxException("if条件语句歧义", GetASTString(synAST));
                    }
                    astIfStat.Childrens.Add(synAST.Childrens[0]);
                    if (PeekToken().tokenType != TokenType.CodeBlock)
                    {
                        var syn = new ASTNode(ASTNode.ASTNodeType.BlockCode, string.Empty);
                        syn.Childrens.Add(ProcessOperation(PowerToOperators.Count - 1));
                        astIfStat.Childrens.Add(syn);
                    }
                    else
                    {
                        var eqPart = PollToken();
                        astIfStat.Childrens.Add(BuildASTByTokens(SplitTokens(eqPart.raw)));
                    }

                    if (PeekToken().raw == "else")
                    {
                        PollToken();
                        if (PeekToken().tokenType != TokenType.CodeBlock)
                        {
                            if (PeekToken().raw == "if")
                            {
                                var ast = ProcessLogicStatement();
                                astIfStat.Childrens.Add(ast);
                            }
                            else
                            {
                                var syn = new ASTNode(ASTNode.ASTNodeType.BlockCode, string.Empty);
                                syn.Childrens.Add(ProcessOperation(PowerToOperators.Count - 1));
                                astIfStat.Childrens.Add(syn);
                            }
                        }
                        else
                        {
                            var nePart = PollToken();
                            astIfStat.Childrens.Add(BuildASTByTokens(SplitTokens(nePart.raw)));
                        }
                    }
                    return astIfStat;
                }
                else if (key.raw == "while")
                {
                    var whileSyntax = PollToken();
                    if (whileSyntax.tokenType != TokenType.Part)
                    {
                        throw new SyntaxException("while语句后条件表达式错误", whileSyntax.raw);
                    }
                    ASTNode astWhileStat = new ASTNode(ASTNode.ASTNodeType.WhileStatement, string.Empty);
                    var synAST = BuildASTByTokens(SplitTokens(whileSyntax.raw));
                    if (synAST.Childrens.Count != 1)
                    {
                        throw new SyntaxException("while条件语句歧义", GetASTString(synAST));
                    }
                    astWhileStat.Childrens.Add(synAST.Childrens[0]);
                    if (PeekToken().tokenType != TokenType.CodeBlock)
                    {
                        var syn = new ASTNode(ASTNode.ASTNodeType.BlockCode, string.Empty);
                        syn.Childrens.Add(ProcessOperation(PowerToOperators.Count - 1));
                        astWhileStat.Childrens.Add(syn);
                    }
                    else
                    {
                        var whilePart = PollToken();
                        astWhileStat.Childrens.Add(BuildASTByTokens(SplitTokens(whilePart.raw)));
                    }
                    return astWhileStat;
                }
                else if (key.raw == "for")
                {
                    var forEachSyntax = PollToken();
                    var synSplit = SplitTokens(forEachSyntax.raw);
                    var forEachVariable = synSplit[0];
                    if (synSplit[1].raw != ":")
                    {
                        throw new Exception("for语句语法错误");
                    }
                    synSplit.RemoveAt(0);
                    synSplit.RemoveAt(0);
                    var forSyntax = BuildASTByTokens(synSplit);
                    if (forSyntax.Childrens.Count != 1)
                    {
                        throw new SyntaxException("for表达式语句内存在多重解析", GetASTString(forSyntax));
                    }
                    forSyntax = forSyntax.Childrens[0];
                    var forAST = new ASTNode(ASTNode.ASTNodeType.ForEachStatement, string.Empty);
                    var forVarAST = new ASTNode(ASTNode.ASTNodeType.Identifier, forEachVariable.raw);
                    forAST.Childrens.Add(forVarAST);
                    forAST.Childrens.Add(forSyntax);
                    if (PeekToken().tokenType != TokenType.CodeBlock)
                    {
                        var syn = new ASTNode(ASTNode.ASTNodeType.BlockCode, string.Empty);
                        syn.Childrens.Add(ProcessOperation(PowerToOperators.Count - 1));
                        forAST.Childrens.Add(syn);
                    }
                    else
                    {
                        var forEachPart = PollToken();
                        forAST.Childrens.Add(BuildASTByTokens(SplitTokens(forEachPart.raw)));
                    }
                    return forAST;
                }
                else if (key.raw == "try")
                {
                    ASTNode astTryStat = new ASTNode(ASTNode.ASTNodeType.TryCatchStatement, string.Empty);

                    if (PeekToken().tokenType != TokenType.CodeBlock)
                    {
                        var syn = new ASTNode(ASTNode.ASTNodeType.BlockCode, string.Empty);
                        syn.Childrens.Add(ProcessOperation(PowerToOperators.Count - 1));
                        astTryStat.Childrens.Add(syn);
                    }
                    else
                    {
                        var eqPart = PollToken();
                        astTryStat.Childrens.Add(BuildASTByTokens(SplitTokens(eqPart.raw)));
                    }

                    if (PeekToken().raw == "catch")
                    {
                        PollToken();
                        var exceptionVariable = new ASTNode(ASTNode.ASTNodeType.Identifier, PollToken().raw);
                        astTryStat.Childrens.Add(exceptionVariable);
                        if (PeekToken().tokenType != TokenType.CodeBlock)
                        {
                            var syn = new ASTNode(ASTNode.ASTNodeType.BlockCode, string.Empty);
                            syn.Childrens.Add(ProcessOperation(PowerToOperators.Count - 1));
                            astTryStat.Childrens.Add(syn);
                        }
                        else
                        {
                            var nePart = PollToken();
                            astTryStat.Childrens.Add(BuildASTByTokens(SplitTokens(nePart.raw)));
                        }
                    }
                    else
                    {
                        throw new SyntaxException("try语句后找不到catch语句", ClipTokenString(_pos - 2, 3, ASTParseStream));
                    }
                    return astTryStat;
                }
                else if (key.raw == "function")
                {
                    if (PeekToken().tokenType == TokenType.Part) //没名字的就是复制性方法
                    {
                        _pos--;
                        return ProcessPrimaryExpression();
                    }
                    var funcName = PollToken().raw;
                    var argsToken = PollToken();
                    if (argsToken.tokenType != TokenType.Part)
                    {
                        throw new SyntaxException("函数定义语法不正确", ClipTokenString(_pos - 1, 2, ASTParseStream));
                    }
                    var args = argsToken.raw.Replace(" ", "").Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var block = PollToken();
                    var blockAST = BuildASTByTokens(SplitTokens(block.raw));
                    var funcAST = new ASTNode(ASTNode.ASTNodeType.FunctionDefinition, funcName);
                    foreach (var arg in args)
                    {
                        funcAST.Childrens.Add(new ASTNode(ASTNode.ASTNodeType.Identifier, arg));
                    }
                    funcAST.Childrens.Add(blockAST);
                    return funcAST;
                }
            }

            return ProcessOperation(PowerToOperators.Count - 1);
        }

        public static ASTNode BuildASTByTokens(List<Token> tokens)
        {
            var old_ASTStream = ASTParseStream;
            var old_pos = _pos;
            ASTParseStream = tokens;
            _pos = 0;

            var ret = new ASTNode(ASTNode.ASTNodeType.BlockCode, string.Empty);
            while (_pos < ASTParseStream.Count && PeekToken().tokenType != TokenType.EOF)
            {
                var ast = ProcessLogicStatement();
                if (PeekToken().tokenType == TokenType.EOF)
                {
                    PollToken();
                }
                else
                {
                    if (ast.NodeType != ASTNode.ASTNodeType.IfStatement &&
                        ast.NodeType != ASTNode.ASTNodeType.WhileStatement &&
                        ast.NodeType != ASTNode.ASTNodeType.ForEachStatement &&
                        ast.NodeType != ASTNode.ASTNodeType.TryCatchStatement &&
                        ast.NodeType != ASTNode.ASTNodeType.VariableDefination &&
                        ast.NodeType != ASTNode.ASTNodeType.FunctionDefinition)
                    {
                        throw new SyntaxException("错误，未在语句结尾找到 ;   ", ClipTokenString(_pos < 6 ? 0 : _pos - 5, _pos + 1, ASTParseStream));
                    }
                }
                ret.Childrens.Add(ast);
            }
            ASTParseStream = old_ASTStream;
            _pos = old_pos;
            return ret;
        }

        public static void PrintAST(ASTNode node, string indent = "", bool isLast = true)
        {
            Console.WriteLine(GetASTString(node));
        }
    }

    public class ASTNode
    {
        public enum ASTNodeType
        {
            EOF,
            VariableDefination,
            Assignment,
            CallFunction,
            FunctionDefinition,
            IfStatement,
            WhileStatement,
            ForEachStatement,
            ReturnStatement,
            ContinueStatement,
            BreakStatement,
            BinaryOperation,
            UnaryOperation,
            LeftUnaryOperation,
            MemberAccess,
            ArrayLabel,
            TryCatchStatement,
            BlockCode,
            Array,
            Identifier,
            Number,
            Boolean,
            StringValue,
            Object,
            KeyValuePair,
            AsyncStatement,
            LockStatement,
        }

        public ASTNodeType NodeType;
        public string Raw;
        public List<ASTNode> Childrens;
        public uint line;

        public ASTNode(ASTNodeType nodeType, string raw)
        {
            NodeType = nodeType;
            Raw = raw;
            Childrens = new List<ASTNode>();
        }
    }
}