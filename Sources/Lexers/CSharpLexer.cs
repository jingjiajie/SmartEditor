using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEditor.Lexers
{
    class CSharpLexer:Lexer
    {
        private Dictionary<string,int> KeyWords; //所有关键字
        private Dictionary<string,int> Operators; //所有运算符
        StringBuilder t = new StringBuilder(10);
        public CSharpLexer()
        {
            this.KeyWords = new Dictionary<string,int>() { { "abstract", 1 }, { "as", 1 }, { "base", 1 }, { "bool", 1 }, { "break", 1 }, { "byte", 1 }, { "case", 1 }, { "catch", 1 }, { "char", 1 }, { "checked", 1 }, { "class", 1 }, { "const", 1 }, { "continue", 1 }, { "decimal", 1 }, { "default", 1 }, { "delegate", 1 }, { "do", 1 }, { "double", 1 }, { "else", 1 }, { "enum", 1 }, { "ecent", 1 }, { "explicit", 1 }, { "extern", 1 }, { "false", 1 }, { "finally", 1 }, { "fixed", 1 }, { "float", 1 }, { "for", 1 }, { "foreach", 1 }, { "get", 1 }, { "goto", 1 }, { "if", 1 }, { "implicit", 1 }, { "in", 1 }, { "int", 1 }, { "interface", 1 }, { "internal", 1 }, { "is", 1 }, { "lock", 1 }, { "long", 1 }, { "namespace", 1 }, { "new", 1 }, { "null", 1 }, { "object", 1 }, { "out", 1 }, { "override", 1 }, { "partial", 1 }, { "private", 1 }, { "protected", 1 }, { "public", 1 }, { "readonly", 1 }, { "ref", 1 }, { "return", 1 }, { "sbyte", 1 }, { "sealed", 1 }, { "set", 1 }, { "short", 1 }, { "sizeof", 1 }, { "stackalloc", 1 }, { "static", 1 }, { "string", 1 }, { "struct", 1 }, { "switch", 1 }, { "this", 1 }, { "throw", 1 }, { "true", 1 }, { "try", 1 }, { "typeof", 1 }, { "uint", 1 }, { "ulong", 1 }, { "unchecked", 1 }, { "unsafe", 1 }, { "ushort", 1 }, { "using", 1 }, { "value", 1 }, { "virtual", 1 }, { "volatile", 1 }, { "void", 1 }, { "where", 1 }, { "while", 1 }, { "yield", 1 } };
            this.Operators = new Dictionary<string,int>() { { "[", 1 }, { "]", 1 }, { "(", 1 }, { ")", 1 }, { "++", 1 }, { "--", 1 }, { "~", 1 }, { "!", 1 }, { "+", 1 }, { "-", 1 }, { "&", 1 }, { "*", 1 }, { "/", 1 }, { "%", 1 }, { ">>", 1 }, { "<<", 1 }, { ">", 1 }, { "<", 1 }, { ">=", 1 }, { "<=", 1 }, { "==", 1 }, { "!=", 1 }, { "|", 1 }, { "^", 1 }, { "&&", 1 }, { "||", 1 }, { "?", 1 }, { ":", 1 }, { "=", 1 }, { "+=", 1 }, { "-=", 1 }, { "*=", 1 }, { "/=", 1 }, { "%=", 1 }, { "|=", 1 }, { "&=", 1 }, { "^=", 1 }, { "<<=", 1 }, { ">>=", 1 }, { ",", 1 }, { ".", 1 } };
        }
        public override bool IsKeyWord(string str)
        {
            if (this.KeyWords.ContainsKey(str))
                return true;
            else
                return false;
        }

        public override bool IsOperator(string str)
        {
            if (this.Operators.ContainsKey(str))
                return true;
            else
                return false;
        }

        public override bool IsString(string str)
        {
            if (str.Length > 1 && str.StartsWith("\"") && str.EndsWith("\"")) return true;
            else return false;
        }

        public override bool IsComment(string str)
        {
            if (str.StartsWith("//")) return true;
            else return false;
        }

        public override string Next(ref int StartPos, string LineStr) //提取某行的下一个单词或操作符，提取后StartPos滑动到单词或操作符的下一个位置
        {
            this.t.Length = 0;
            int FinalPos = LineStr.Length - 1;
            if (StartPos > FinalPos) return null;

            while (true)
            {
                while (StartPos <= FinalPos
                    && LineStr[StartPos] != ' '
                    && LineStr[StartPos] != '\t'
                    )
                {
                    if (t.Length==0 && LineStr[StartPos] == '\"') //需要单独处理字符串，因为可能包含空格，注释符等特殊符号
                    {
                        t.Append(LineStr[StartPos]);
                        if (++StartPos > FinalPos) break; //处理最后一个字符为"的特殊情况
                        int StrPos1 = StartPos;
                        while (StrPos1 <= FinalPos && ((LineStr[StrPos1] == '\"') ? (LineStr[StrPos1 - 1] == '\\' ? true : false) : true))
                        {
                            StrPos1++;
                        }
                        if (StrPos1 <= FinalPos) //是字符串的情况
                        {
                            for (; StartPos <= StrPos1; StartPos++)
                            {
                                t.Append(LineStr[StartPos]);
                            }
                            break;
                        }
                        else { continue; } //此种情况不是字符串
                    }

                    if (t.Length==0 && StartPos < FinalPos && LineStr[StartPos] == '/' && LineStr[StartPos + 1] == '/')
                    {
                        while (StartPos <= FinalPos)
                        {
                            t.Append(LineStr[StartPos]);
                            StartPos++;
                        }
                        break;
                    }

                    if (t.Length==0 && this.IsOperator(LineStr[StartPos].ToString()))
                    {
                        t.Append(LineStr[StartPos]);
                        while (true)
                        {
                            if (StartPos < FinalPos) t.Append(LineStr[++StartPos]);
                            else { StartPos++; return t.ToString(); }
                            if (this.IsOperator(t.ToString())) continue;
                            else {  return t.Remove(t.Length - 1, 1).ToString(); }
                        }
                    }

                    if (this.IsOperator(LineStr[StartPos].ToString()) || LineStr[StartPos] == '\"')
                    {
                        break;
                    }

                    t.Append(LineStr[StartPos]);
                    StartPos++;
                }
                if (t.Length != 0) //如果取到词了就返回。如果什么都没有取到（第一个字符为运算符）就再往后取词
                {
                    break;
                }
                else if (StartPos <= FinalPos)
                {
                    StartPos++;
                    continue;
                }
                else if (StartPos > FinalPos)
                {
                    break;
                }
            }

            if (t.Length != 0) return t.ToString();
            else return null;
        }
    }
}
