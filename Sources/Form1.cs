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

using HWND = System.IntPtr;

namespace SmartEditor
{
    
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private Stack<string> KeyWords = new Stack<string>();
     //   private Thread TPaint;
        private int Lines = 1;
        private Font TextBoxFont = null;
        public MainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.richTextBox1.Width = this.Width - 20;
            this.richTextBox1.Height = this.Height - 40;
            this.TextBoxFont = new Font("Consolas", 9);

            try
            {
                StreamReader sr = new StreamReader("CSKeyWords.ini", Encoding.Default); //读取关键字
                String t;
                while ((t = sr.ReadLine()) != null)
                {
                    this.KeyWords.Push(t);
                }
            }
            catch (Exception ex) { Console.WriteLine("读取关键字配置文件失败"); }

            //  TPaint = new Thread(new ThreadStart(this.PaintLine));
            // TPaint.IsBackground = true;

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //TPaint.Abort();
            // TPaint = new Thread(this.PaintLine);
            // TPaint.Start();
            int OriSelectionStart = richTextBox1.SelectionStart;
            SendMessage(richTextBox1.Handle, 0x0B, 0, 0); //禁止组件重画，避免闪烁问题
            richTextBox1.SelectAll();
            richTextBox1.SelectionFont = this.TextBoxFont;
            richTextBox1.DeselectAll();
            richTextBox1.SelectionStart = OriSelectionStart;
            SendMessage(richTextBox1.Handle, 0x0B, 1, 0); //允许组件重画
            richTextBox1.Refresh(); //组件刷新
            SendMessage(richTextBox1.Handle, 0x0B, 0, 0); //第二轮加工，禁止组件重画，避免着色闪烁问题
            if (richTextBox1.Lines.Length == this.Lines)
            {
                this.PaintLine(richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart));
            }
            else
            {
                int CurrentLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
                int StartLine = CurrentLine - (richTextBox1.Lines.Length - this.Lines);
                for (int i = StartLine; i <= CurrentLine; i++) this.PaintLine(i);

            }

            richTextBox1.SelectionStart = OriSelectionStart;
            richTextBox1.SelectionColor = Color.Empty;
            SendMessage(richTextBox1.Handle, 0x0B, 1, 0); //允许组件重画
            this.Lines = (richTextBox1.Lines.Length==0?1:richTextBox1.Lines.Length);
            richTextBox1.Refresh();
        }

        public void PaintLine(int LineNum) //会改变SelectionStart以及SelectionColor等属性，自行恢复。
        {
       //     Console.WriteLine("行数：" + richTextBox1.Lines.Length);
       //     Console.WriteLine("当前行号：" + LineNum);
            string LineStr;
            try { LineStr = richTextBox1.Lines[LineNum]; }
            catch (Exception ex) { richTextBox1.SelectionColor = Color.Empty; return; }
            int StartIndex = richTextBox1.GetFirstCharIndexFromLine(LineNum); //本行开头的CharIndex
            int Strpos = LineStr.Length - 1; //LineStr中的pos，从该行最后一个字符开始
            string t = null;

            while (true)
            {
                t = null;
                if (Strpos < 0) break;


                while (Strpos >= 0
                    && LineStr[Strpos] != ' '
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
                    && LineStr[Strpos] != '!'
                    && LineStr[Strpos] != '|'
                    && LineStr[Strpos] != '&'
                    )
                {

                    if (LineStr[Strpos] == '/') //处理注释和除号
                    {
                        if (Strpos >= 0 && LineStr[Strpos - 1] == '/')
                        {
                            //Console.WriteLine("StartIndex + Strpos:" + (StartIndex + Strpos) + " LineStr.Length - Strpos + 1:" + (LineStr.Length - Strpos + 1));
                            richTextBox1.Select(StartIndex + Strpos - 1, LineStr.Length - Strpos + 1);
                            richTextBox1.SelectionColor = Color.Green;
                            Strpos--;
                            t = null;
                            break;
                        }
                        else { break; }
                    }

                    if (LineStr[Strpos]=='\"') //处理字符串
                    {
                        t += LineStr[Strpos];
                        if (--Strpos < 0) break;
                        int Strpos1 = Strpos;
                        while (Strpos1 >= 0 && LineStr[Strpos1] != '\"')
                        {
                            Strpos1--;
                        }
                        if (Strpos1 >= 0)
                        {
                            while (LineStr[Strpos] != '\"')
                            {
                                t += LineStr[Strpos];
                                Strpos--;
                            }
                        }
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

                if (this.KeyWords.Contains(t))
                {
                    richTextBox1.Select(StartIndex + Strpos + 1, t.Length);
                    richTextBox1.SelectionColor = Color.Blue;
                }else if(t.Length>1 && t.StartsWith("\"") && t.EndsWith("\""))
                {
                    richTextBox1.Select(StartIndex + Strpos + 1, t.Length);
                    richTextBox1.SelectionColor = Color.Green;
                }
                else
                {
                    richTextBox1.Select(StartIndex + Strpos + 1, t.Length);
                    richTextBox1.SelectionColor = Color.Black;
                }

                Strpos--;
            }

        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.richTextBox1.Width = this.Width - 20;
            this.richTextBox1.Height = this.Height - 40;
        }
    }
}
