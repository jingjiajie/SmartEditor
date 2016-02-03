using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace SmartEditor
{

    public class Painter
    {
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public Painter()
        {

        }

        public void Paint(int Line, ref RichTextBox RTB)
        {
            this.Paint(Line, Line,ref RTB);
        }
        public void Paint(int StartLine, int FinalLine, ref RichTextBox RTB)
        {
            if(StartLine < 0 || FinalLine < 0)
            {
                Console.WriteLine("错误：着色起始或终止行号 < 0");
                return;
            }
            int OriSelectionStart = RTB.SelectionStart; //记录光标位置
            SendMessage(RTB.Handle, 0x0B, 0, 0); //第一轮加工开始，禁止组件重画，避免闪烁问题
            RTB.SelectAll();
            RTB.SelectionFont = Defines.TextBoxFont;
            RTB.DeselectAll();
            RTB.SelectionStart = OriSelectionStart;
            SendMessage(RTB.Handle, 0x0B, 1, 0); //第一轮加工结束，允许组件重画
            RTB.Refresh(); //组件刷新
            SendMessage(RTB.Handle, 0x0B, 0, 0); //第二轮加工开始，禁止组件重画，避免着色闪烁问题

            for (int i = StartLine; i <= FinalLine; i++) this.PaintLine(i, ref RTB); //对每行着色
            RTB.SelectionLength = 0;
            RTB.SelectionStart = OriSelectionStart; //光标归位
            RTB.SelectionColor = Color.Black;
            SendMessage(RTB.Handle, 0x0B, 1, 0); //第二轮加工结束，允许组件重画
            RTB.Refresh();
        }

        private void PaintLine(int LineNum, ref RichTextBox RTB) //考虑程序效率，改变SelectionStart以及SelectionColor等属性不会恢复，请自行恢复。
        {
            string LineStr;
            try { LineStr = RTB.Lines[LineNum]; } //处理只有0行，lines数组长度小于0的的特殊情况
            catch (Exception ex) { RTB.SelectionColor = Color.Empty; return; }
            int StartIndex = RTB.GetFirstCharIndexFromLine(LineNum); //本行开头的CharIndex
            int Strpos = LineStr.Length - 1; //LineStr中的pos，从该行最后一个字符开始
            string t = null;

            while (true)
            {
                t = null;
                if (Strpos < 0) break;


                while (Strpos >= 0
                    && LineStr[Strpos] != ' '
                    && LineStr[Strpos] != '\t'
                    && LineStr[Strpos] != ','
                    && LineStr[Strpos] != '.'
                    && LineStr[Strpos] != ';'
                    && LineStr[Strpos] != '('
                    && LineStr[Strpos] != ')'
                    && LineStr[Strpos] != '='
                    && LineStr[Strpos] != '+'
                    && LineStr[Strpos] != '-'
                    && LineStr[Strpos] != '*'
                    //  && LineStr[Strpos] != '/'
                    && LineStr[Strpos] != '>'
                    && LineStr[Strpos] != '<'
                    && LineStr[Strpos] != '!'
                    && LineStr[Strpos] != '|'
                    && LineStr[Strpos] != '&'
                    && LineStr[Strpos] != '['
                    && LineStr[Strpos] != ']'
                    && LineStr[Strpos] != '{'
                    && LineStr[Strpos] != '}'
                    )
                {

                    if (LineStr[Strpos] == '/') //处理注释和除号
                    {
                        if (Strpos >= 0 && LineStr[Strpos - 1] == '/')
                        {
                            RTB.Select(StartIndex + Strpos - 1, LineStr.Length - Strpos + 1);
                            RTB.SelectionColor = Color.Green;
                            Strpos--;
                            t = null;
                            break;
                        }
                        else { break; }
                    }

                    if (LineStr[Strpos] == '\"') //需要单独处理字符串，因为可能包含空格，注释符等特殊符号
                    {
                        t += LineStr[Strpos];
                        if (--Strpos < 0) break; //处理第一个字符为"的特殊情况
                        int Strpos1 = Strpos;
                        while (Strpos1 >= 0 && LineStr[Strpos1] != '\"')
                        {
                            Strpos1--;
                        }
                        if (Strpos1 >= 0) //是字符串的情况
                        {
                            while (LineStr[Strpos] != '\"')
                            {
                                t += LineStr[Strpos];
                                Strpos--;
                            }
                        }
                        else { continue; } //此种情况不是字符串
                    }

                    t += LineStr[Strpos];
                    Strpos--;
                }

                if (t == null)
                {
                    Strpos--;
                    continue;
                }
                char[] arr = t.ToCharArray();
                Array.Reverse(arr);
                t = new string(arr);

                if (Defines.KeyWords.Contains(t))
                {
                    RTB.Select(StartIndex + Strpos + 1, t.Length);
                    RTB.SelectionColor = Color.Blue;
                }
                else if (t.Length > 1 && t.StartsWith("\"") && t.EndsWith("\""))
                {
                    RTB.Select(StartIndex + Strpos + 1, t.Length);
                    RTB.SelectionColor = Color.Green;
                }
                else
                {
                    RTB.Select(StartIndex + Strpos + 1, t.Length);
                    RTB.SelectionColor = Color.Black;
                }

                Strpos--;
            }

        } //End of void PaintLine()

    } // End of class Painter
} //End of namespace SmartEditor
