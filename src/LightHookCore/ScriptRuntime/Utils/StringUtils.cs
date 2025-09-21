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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRuntime.Core;
using ScriptRuntime.Runtime;
using ScriptRuntime.Utils;

static class StringUtils
{
    
    public static Dictionary<char, char> bracket = new Dictionary<char, char>()
    {
        {'(',')'},
        {'[',']'},
        {'{','}'}
    };
    public static Dictionary<char, TokenType> bracketTokenType = new Dictionary<char, TokenType>()
    {
        {'(',TokenType.Part},
        {'[',TokenType.IndexLabel},
        {'{',TokenType.CodeBlock}
    };
    public static string FindNextToken(string s, int index) //单行语句搜寻
    {
        Stack<char> bracketStack = new Stack<char>(); //括号平衡栈
        bool inString = false;
        for (int i = index; i < s.Length; i++)
        {
            if (s[i] == '"')
            {
                inString = !inString;
            }
            else if (bracket.ContainsKey(s[i]) && !inString)
            {
                bracketStack.Push(s[i]);
            }
            else if (bracket.ContainsValue(s[i]) && !inString)
            {
                if (bracket[bracketStack.Pop()] != s[i])
                {
                    throw new SyntaxException("解析语法错误：括号错误",s);
                }
            }
            if (bracketStack.Count == 0 && s[i] == ';' && !inString) //寻找到分号表示token末尾
            {
                return s.Substring(index, i - index + 1);
            }
        }
        return string.Empty;
    }
    public static string FindNextWord(string s, int index) //单个单词搜寻
    {
        Stack<char> bracketStack = new Stack<char>(); //括号平衡栈
        for (int i = index; i < s.Length; i++)
        {
            if (bracket.ContainsKey(s[i]))
            {
                bracketStack.Push(s[i]);
            }
            else if (bracket.ContainsValue(s[i]))
            {
                if (bracket[bracketStack.Pop()] != s[i])
                {
                    throw new SyntaxException("解析语法错误：括号错误",s);
                }
            }
            if (bracketStack.Count == 0 && !char.IsLetterOrDigit(s[i])) //寻找到分号表示token末尾
            {
                return s.Substring(index, i - index + 1);
            }
        }
        return string.Empty;
    }
    public static ValueTuple<string, int> FindNextCharacter(string s, int index, string target)
    {
        for (int i = index; i <= s.Length - target.Length; i++)
        {
            if (s.Substring(i, target.Length) == target)
            {
                return (s.Substring(index, i - index), i);
            }
        }
        return (string.Empty, -1);
    }
    public static string NextToken(string s, ref int index) 
    {
        for(int i = index; i < s.Length; i++)
        {
            if(!char.IsLetterOrDigit(s[i]))
            {
                var ret = s.Substring(index, i - index);
                index = i;
                return ret;
            }
        }
        return string.Empty;
    }
    public static string GenerateStackTrace(Stack<ScriptFunction> stack)
    {
        var result = new StringBuilder();
        foreach (var func in stack)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < func.FunctionArgumentTypes.Count; i++)
            {
                sb.Append($"[{AOTEnumMap.ValueTypeString[func.FunctionArgumentTypes[i]]}]");
                sb.Append(func.FuncType == FunctionType.Local ? func.FunctionArgumentNames[i] : $"arg{i + 1}");
                if (i != func.FunctionArgumentTypes.Count - 1)
                {
                    sb.Append(',');
                }
            }

            result.AppendLine($"at {func.Name}{sb.ToString()})");
        }
        return result.ToString();
    }
    public static string ClearMultiSpace(string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        // 去除开头和结尾的空格
        s = s.Trim();

        StringBuilder sb = new StringBuilder();
        bool inString = false;
        bool hasSpace = false;

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            // 检查是否进入或退出字符串
            if (c == '"' && (i == 0 || s[i - 1] != '\\'))
            {
                inString = !inString;
            }

            if (inString)
            {
                // 字符串内内容原样保留
                sb.Append(c);
            }
            else
            {
                if (c == ' ')
                {
                    // 非字符串内的空格，检查是否已有空格
                    if (!hasSpace)
                    {
                        sb.Append(c);
                        hasSpace = true;
                    }
                }
                else
                {
                    hasSpace = false;
                    sb.Append(c);
                }
            }
        }

        return sb.ToString();
    }
}

