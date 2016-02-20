using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEditor.Lexers
{
    class SmartZLexer:Lexer
    {
        private Dictionary<string,int> KeyWords; //所有关键字
        private Dictionary<string,int> Operators; //所有运算符
        StringBuilder t = new StringBuilder(10);
        public SmartZLexer()
        {
            this.KeyWords = new Dictionary<string,int>() { { "显示",1},{ "抽象", 1 }, { "as", 1 }, { "基类", 1 }, { "布尔型", 1 }, { "跳出", 1 }, { "字节型", 1 }, { "情况", 1 }, { "捕获", 1 }, { "字符型", 1 }, { "checked", 1 }, { "类", 1 }, { "常量", 1 }, { "继续", 1 }, { "decimal", 1 }, { "默认", 1 }, { "委托", 1 }, { "判断循环首", 1 }, { "双精度小数型", 1 }, { "否则", 1 }, { "枚举", 1 }, { "ecent", 1 }, { "explicit", 1 }, { "extern", 1 }, { "假", 1 }, { "finally", 1 }, { "fixed", 1 }, { "小数型", 1 }, { "计次循环", 1 }, { "foreach", 1 }, { "get", 1 }, { "跳转到", 1 }, { "如果", 1 }, { "implicit", 1 }, { "in", 1 }, { "整数型", 1 }, { "接口", 1 }, { "内部", 1 }, { "is", 1 }, { "lock", 1 }, { "长整数型", 1 }, { "命名空间", 1 }, { "新建", 1 }, { "空", 1 }, { "object", 1 }, { "out", 1 }, { "重写", 1 }, { "partial", 1 }, { "私有", 1 }, { "保护", 1 }, { "公共", 1 }, { "只读", 1 }, { "引用", 1 }, { "返回", 1 }, { "sbyte", 1 }, { "sealed", 1 }, { "set", 1 }, { "短整数型", 1 }, { "大小", 1 }, { "stackalloc", 1 }, { "静态", 1 }, { "字符串", 1 }, { "结构体", 1 }, { "开关", 1 }, { "本类", 1 }, { "抛出", 1 }, { "真", 1 }, { "尝试", 1 }, { "typeof", 1 }, { "无符号整数型", 1 }, { "无符号长整型", 1 }, { "unchecked", 1 }, { "unsafe", 1 }, { "无符号短整型", 1 }, { "使用", 1 }, { "value", 1 }, { "虚函数", 1 }, { "虚类", 1 }, { "volatile", 1 }, { "空类型", 1 }, { "where", 1 }, { "判断循环", 1 }, { "yield", 1 } };
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
