using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEditor.Lexers
{
    class CPPLexer:Lexer
    {
        private Dictionary<string, int> KeyWords; //所有关键字
        private Dictionary<string, int> Operators; //所有运算符

        public CPPLexer()
        {
            this.KeyWords=new Dictionary<string, int>() { { "asm", 1 }, { "do", 1 }, { "if", 1 }, { "return", 1 }, { "typedef", 1 }, { "auto", 1 }, { "double", 1 }, { "inline", 1 }, { "short", 1 }, { "typeid", 1 }, { "bool", 1 }, { "dynamic_cast", 1 }, { "int", 1 }, { "signed", 1 }, { "typename", 1 }, { "break", 1 }, { "else", 1 }, { "long", 1 }, { "sizeof", 1 }, { "union", 1 }, { "case", 1 }, { "enum", 1 }, { "mutable", 1 }, { "static", 1 }, { "unsigned", 1 }, { "catch", 1 }, { "explicit", 1 }, { "namespace", 1 }, { "static_cast", 1 }, { "using", 1 }, { "char", 1 }, { "export", 1 }, { "new", 1 }, { "struct", 1 }, { "virtual", 1 }, { "class", 1 }, { "extern", 1 }, { "operator", 1 }, { "switch", 1 }, { "void", 1 }, { "const", 1 }, { "false", 1 }, { "private", 1 }, { "template", 1 }, { "volatile", 1 }, { "const_cast", 1 }, { "float", 1 }, { "protected", 1 }, { "this", 1 }, { "wchar_t", 1 }, { "continue", 1 }, { "for", 1 }, { "public", 1 }, { "throw", 1 }, { "while", 1 }, { "default", 1 }, { "friend", 1 }, { "register", 1 }, { "true", 1 }, { "delete", 1 }, { "goto", 1 }, { "reinterpret_cast", 1 }, { "try", 1 } };
            this.Operators = new Dictionary<string, int>() { { "::", 1 }, { "->", 1 }, { "[", 1 }, { "]", 1 }, { "(", 1 }, { ")", 1 }, { "++", 1 }, { "--", 1 }, { "~", 1 }, { "!", 1 }, { "+", 1 }, { "-", 1 }, { "&", 1 }, { "*", 1 }, { "/", 1 }, { "%", 1 }, { ">>", 1 }, { "<<", 1 }, { ">", 1 }, { "<", 1 }, { ">=", 1 }, { "<=", 1 }, { "==", 1 }, { "!=", 1 }, { "|", 1 }, { "^", 1 }, { "&&", 1 }, { "||", 1 }, { "?", 1 }, { ":", 1 }, { "=", 1 }, { "+=", 1 }, { "-=", 1 }, { "*=", 1 }, { "/=", 1 }, { "%=", 1 }, { "|=", 1 }, { "&=", 1 }, { "^=", 1 }, { "<<=", 1 }, { ">>=", 1 }, { ",", 1 }, { ".", 1 } };
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
            string t = null;
            int FinalPos = LineStr.Length - 1;
            if (StartPos > FinalPos) return null;

            while (true)
            {
                while (StartPos <= FinalPos
                    && LineStr[StartPos] != ' '
                    && LineStr[StartPos] != '\t'
                    )
                {
                    if (t==null && LineStr[StartPos] == '\"') //需要单独处理字符串，因为可能包含空格，注释符等特殊符号
                    {
                        t += LineStr[StartPos];
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
                                t += LineStr[StartPos];
                            }
                            break;
                        }
                        else { continue; } //此种情况不是字符串
                    }

                    if (t == null && StartPos < FinalPos && LineStr[StartPos] == '/' && LineStr[StartPos + 1] == '/')
                    {
                        while (StartPos <= FinalPos)
                        {
                            t += LineStr[StartPos];
                            StartPos++;
                        }
                        break;
                    }

                    if (t == null && this.IsOperator(LineStr[StartPos].ToString()))
                    {
                        t += LineStr[StartPos];
                        string t1 = LineStr[StartPos].ToString();
                        if (StartPos < FinalPos) t1 += LineStr[StartPos + 1];
                        else { StartPos++; break; }
                        if (this.IsOperator(t1)) { t = t1; StartPos += 2; break; }
                        else { StartPos++; break; }
                    }

                    if (this.IsOperator(LineStr[StartPos].ToString()) || LineStr[StartPos]=='\"') //此种情况t应该是单词
                    {
                        break;
                    }

                    t += LineStr[StartPos];
                    StartPos++;
                }
                if (t != null) //如果取到词了就返回。如果什么都没有取到（第一个字符为运算符）就再往后取词
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

            return t;
        }
    }
}
