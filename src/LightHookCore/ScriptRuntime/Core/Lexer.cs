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
using ScriptRuntime.Core;
using ScriptRuntime.Runtime;

namespace ScriptRuntime.Core
{
    public enum TokenType
    {
        Part,   //括号内表达式
        CodeBlock,   //代码块或者数组常量 {xxx;xxx;}
        Operator,     // +, -, *, /
        Idfefinder,   // a, b,while...
        IndexLabel,    //数组下标[...]
        EOF,           // 输入结束
        Processed    //解析完成的
    }
    public static class Lexer
    {

        public static readonly Dictionary<string, char> EscapeToChar = new Dictionary<string, char>
        {
            ["\\\\"] = '\\',  // 反斜杠
            ["\\\'"] = '\'',  // 单引号
            ["\\\""] = '\"',  // 双引号
            ["\\0"] = '\0',   // 空字符
            ["\\a"] = '\a',   // 警报
            ["\\b"] = '\b',   // 退格
            ["\\f"] = '\f',   // 换页
            ["\\n"] = '\n',   // 换行
            ["\\r"] = '\r',   // 回车
            ["\\t"] = '\t',   // 水平制表
            ["\\v"] = '\v',   // 垂直制表
        };
        public static readonly Dictionary<char, string> CharToEscape = new Dictionary<char, string>
        {
            ['\\'] = "\\\\",  // 反斜杠
            ['\''] = "\\\'",  // 单引号
            ['\"'] = "\\\"",  // 双引号
            ['\0'] = "\\0",   // 空字符
            ['\a'] = "\\a",   // 警报
            ['\b'] = "\\b",   // 退格
            ['\f'] = "\\f",   // 换页
            ['\n'] = "\\n",   // 换行
            ['\r'] = "\\r",   // 回车
            ['\t'] = "\\t",   // 水平制表
            ['\v'] = "\\v"    // 垂直制表
        };

        //返回值定义：(截取的token,下一个token起始index)
        public static ValueTuple<string, int> SearchNextToken(string syn, int index)
        {
            Stack<char> bracketStack = new Stack<char>();
            bool inString = false;
            StringBuilder sb = new StringBuilder();
            for (int i = index; i < syn.Length; i++)
            {
                if (syn[i] == '\\')
                {
                    i++;
                    continue;
                }
                if (syn[i] == '"')
                {
                    inString = !inString;
                }
                else if (bracket.ContainsKey(syn[i]) && !inString)
                {

                    return (syn.Substring(index, i - index), i); //遇到括号必须独立一个token，所以直接返回

                }
                else if (bracket.ContainsValue(syn[i]) && !inString)
                {
                    if (bracket[bracketStack.Pop()] != syn[i])
                    {
                        throw new SyntaxException("解析语法错误：括号错误",syn);
                    }
                    if (bracketStack.Count == 0)
                    {
                        return (syn.Substring(index, i - index + 1), i + 1); //包括最后一个括号
                    }
                }
                else if (!inString && bracketStack.Count == 0 && (syn[i] == ' ' || syn[i] == ';' || SymbolMap.Contains(syn[i].ToString())))
                {
                    return (syn.Substring(index, i - index), (syn[i] == ' ') ? i + 1 : i); //如果是空格不包含当前空格 
                }
                sb.Append(syn[i]);
            }
            return (syn.Substring(index), syn.Length);
        }
        //返回搜索到的Token和下一个起始位置，这个方法仅能用于寻找变量和函数，类似xxx或xxx(...)
        private static ValueTuple<Token, int> FindNextToken(string syn, int index)
        {
            int jmp = index;
            bool inString = false;
            char cur = syn[jmp];
            while (cur == ' ' || cur == '\n' || cur == '\t') //跳过无关符号
            {
                jmp++;
                if (jmp >= syn.Length)
                {
                    return (new Token(";", TokenType.EOF), jmp + 1);
                }
                cur = syn[jmp];
            }
            //由第一个字符类型判断Token类型
            if (bracket.ContainsKey(cur)) //括号部分
            {
                int next = BracketScan(syn, jmp) + 1;
                return (new Token(syn.Substring(jmp + 1, next - jmp - 2), bracketTokenType[cur]), next);
            }
            else if (char.IsLetter(cur)) //常量/变量
            {
                (string res, int next) = SearchNextToken(syn, jmp);
                return (new Token(res, TokenType.Idfefinder), next);
            }
            else if (char.IsDigit(cur)) //数学常量
            {
                int tmp = jmp;
                while (tmp < syn.Length && (char.IsDigit(syn[tmp]) || syn[tmp] == '.'))
                {
                    tmp++;
                }
                return (new Token(syn.Substring(jmp, tmp - jmp), TokenType.Idfefinder), tmp);
            }
            else if (SymbolMap.Contains(cur.ToString())) //运算符
            {
                int tmp = jmp;
                while (tmp < syn.Length && SymbolMap.Contains(syn[tmp].ToString()))
                {
                    tmp++;
                }
                return (new Token(syn.Substring(jmp, tmp - jmp), TokenType.Operator), tmp);
            }
            else if (cur == '"')
            {
                int tmp = jmp + 1;
                while (tmp < syn.Length && syn[tmp] != '"')
                {
                    if (syn[tmp] == '\\')
                    {
                        tmp++; //跳过转义
                    }
                    tmp++;
                }
                return (new Token(syn.Substring(jmp, tmp - jmp + 1), TokenType.Idfefinder), tmp + 1);
            }
            else if (cur == ';')
            {
                return (new Token(";", TokenType.EOF), jmp + 1);
            }
            else if (cur == ':') //for循环的运算符
            {
                return (new Token(":", TokenType.EOF), jmp + 1);
            }
            else if (cur == '{')
            {
                var end = BracketScan(syn, jmp);
                return (new Token(syn.Substring(jmp + 1, end - jmp - 1), TokenType.CodeBlock), end + 1);
            }
            else
            {
                throw new SyntaxException("未知解析Token错误", syn);
            }
        }
        public static List<Token> SplitTokens(string syntax,uint line = 0)
        {
            syntax = syntax.Replace("\n", string.Empty).Replace("\r", string.Empty);
            TokenType tokenType = TokenType.Part;
            List<Token> tokens = new List<Token>();
            int index = 0;
            while (tokenType != TokenType.EOF && index < syntax.Length)
            {
                (var token, index) = FindNextToken(syntax, index);
                token.line = line;
                tokens.Add(token);
            }
            return tokens;
        }
        //传入带引号的未处理字符串，返回不带引号已经处理的字符串
        public static string ProcessEscapeToChar(string str)
        {
            var rmk = str.Substring(1, str.Length - 2).ToCharArray();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < rmk.Length; i++)
            {
                if (i < rmk.Length - 1)
                {
                    var s2 = $"{rmk[i]}{rmk[i + 1]}";
                    if (EscapeToChar.ContainsKey(s2))
                    {
                        sb.Append(EscapeToChar[s2]);
                        i++;
                    }
                    else
                    {
                        sb.Append(rmk[i]);
                    }
                }
                else
                {
                    sb.Append(rmk[i]);
                }
            }
            return sb.ToString();
        }
        //传入不带引号的字符串，返回带引号和转义的字符串
        public static string ProcessCharToEscape(string str)
        {
            var rmk = str;
            foreach (var esc in CharToEscape)
            {
                rmk = rmk.Replace(esc.Key.ToString(), esc.Value);
            }
            return $"\"{rmk}\"";
        }

        //返回括号的下一个平衡点，例如start(()())end或者start()end()
        public static int BracketScan(string syn, int start)
        {
            Stack<char> bracketStack = new Stack<char>();
            for (int i = start; i < syn.Length; i++)
            {
                if (bracket.ContainsKey(syn[i]))
                {
                    bracketStack.Push(syn[i]);
                }
                else if (bracket.ContainsValue(syn[i]))
                {
                    if (bracket[bracketStack.Pop()] != syn[i])
                    {
                        throw new SyntaxException("括号错误",syn);
                    }
                    if (bracketStack.Count == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        //过滤换行注释缩进
        public static string CleanCode(string code)
        {
            var rt = code.Replace("\r", string.Empty).Replace("\t", string.Empty);
            var lines = new StringBuilder();
            foreach (var line in rt.Split('\n'))
            {
                bool inString = false;
                int end = line.Length;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '\\')
                    {
                        i++;
                        continue;
                    }
                    if (line[i] == '"')
                    {
                        inString = !inString;
                    }
                    if (!inString && line[i] == '/' && line[i + 1] == '/')
                    {
                        end = i;
                    }
                }
                lines.Append(line.Substring(0, end));
                lines.Append(" ");
            }
            return lines.ToString();
        }
    }

    public class Token
    {
        public string raw;
        public ASTNode? processedValue;
        public TokenType tokenType;
        public uint line;
        public Token(string raw, TokenType tokenType)
        {
            this.raw = raw;
            this.tokenType = tokenType;
            processedValue = null;
        }
        public Token(ASTNode processedValue)
        {
            this.processedValue = processedValue;
            this.tokenType = TokenType.Processed;
            raw = string.Empty;
        }
    }
}