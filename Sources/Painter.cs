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
using SmartEditor.Lexers;

namespace SmartEditor
{

    public class Painter
    {
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        Lexer lexer;
        MainForm mainform;
        public Painter(string LanguageName,MainForm mainform)
        {
            this.mainform = mainform;
            switch (LanguageName)
            {
                case "CPP": { this.lexer = new CPPLexer();break; }
                case "CSharp": { this.lexer = new CSharpLexer(); break; }
                default: break;
            }
        }
       
        public void Repaint(RichTextBox RTB)
        {
            this.Paint(0, RTB.Lines.Length - 1, RTB);
        }
        public void Paint(int Line, RichTextBox RTB)
        {
            this.Paint(Line, Line,RTB);
        }
        public void Paint(int StartLine, int FinalLine, RichTextBox RTB)
        {
            if(StartLine < 0 || FinalLine < 0)
            {
                Console.WriteLine("错误：着色起始或终止行号 < 0");
                return;
            }
            int OriSelectionStart = RTB.SelectionStart; //记录光标位置
            RTB.SelectionChanged -= new System.EventHandler(mainform.richTextBox1_SelectionChanged);//注销选区改变事件，以免处理过程中多次触发、
            RTB.TextChanged -= new System.EventHandler(mainform.richTextBox1_TextChanged); //注销文字改变事件
            SendMessage(RTB.Handle, 0x0B, 0, 0); //第一轮加工开始，禁止组件重画，避免闪烁问题
            RTB.SelectAll();
            RTB.SelectionFont = Defines.TextBoxFont;
            RTB.DeselectAll();
            RTB.SelectionStart = OriSelectionStart;
            SendMessage(RTB.Handle, 0x0B, 1, 0); //第一轮加工结束，允许组件重画
            RTB.Refresh(); //组件刷新

            SendMessage(RTB.Handle, 0x0B, 0, 0); //第二轮加工开始，禁止组件重画，避免着色闪烁问题
            if (RTB.Lines.Length > 0 )
            {
                for (int i = StartLine; i <= FinalLine; i++) this.PaintLine(i, ref RTB); //对每行着色
            }
                RTB.SelectionLength = 0;
            RTB.SelectionStart = OriSelectionStart; //光标归位
            RTB.SelectionColor = Color.Black;
            SendMessage(RTB.Handle, 0x0B, 1, 0); //第二轮加工结束，允许组件重画
            RTB.TextChanged += new System.EventHandler(mainform.richTextBox1_TextChanged); //恢复事件
            RTB.SelectionChanged += new System.EventHandler(mainform.richTextBox1_SelectionChanged); //恢复事件
            RTB.Refresh();
        }

        /*
            着色程序
            考虑程序效率，改变SelectionStart以及SelectionColor等属性不会恢复，请自行恢复。
            当富文本框有0个字符时不能处理，会引发错误。
        */
        private void PaintLine(int LineNum, ref RichTextBox RTB)
        {
            string LineStr;
            LineStr = RTB.Lines[LineNum];
            int StartIndex = RTB.GetFirstCharIndexFromLine(LineNum); //本行开头的CharIndex
            int StrPos = 0; //LineStr中的pos，从该行首个字符开始
            string t = null;

            while (true)
            {
                t = lexer.Next(ref StrPos,LineStr);
                if (t == null) break;
               // Console.WriteLine(t);
                if (lexer.IsKeyWord(t))
                {
                    //Console.WriteLine("SelectionStart:" + (StartIndex + StrPos - t.Length) + " Length:" + t.Length);
                    RTB.Select(StartIndex + StrPos - t.Length, t.Length);
                    RTB.SelectionColor = Color.Blue;
                }
                else if (lexer.IsString(t))
                {
                    RTB.Select(StartIndex + StrPos - t.Length, t.Length);
                    RTB.SelectionColor = Color.Brown;
                }else if(lexer.IsComment(t))
                {
                    RTB.Select(StartIndex + StrPos - t.Length, t.Length);
                    RTB.SelectionColor = Color.Green;
                }
                else
                {
                    RTB.Select(StartIndex + StrPos - t.Length, t.Length);
                    RTB.SelectionColor = Color.Black;
                }

            }

        } //End of void PaintLine()

    } // End of class Painter
} //End of namespace SmartEditor
