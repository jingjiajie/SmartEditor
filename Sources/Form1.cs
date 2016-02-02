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
    
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private Painter P; //Painter类用来给RichTextBox着色
        private int TextBoxLines = 1; //RichTextBox行数
        private bool IfSaved = false; //是否已经保存文件
   //   private Thread TPaint;
        public MainForm()
        {
            InitializeComponent();
            this.Text = Defines.DefaultMainFormTitle;
            //   Control.CheckForIllegalCrossThreadCalls = false;
            this.richTextBox1.Width = this.Width - 16;
            this.richTextBox1.Height = this.Height - 62;

            this.P = new Painter();
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.IfSaved = false;
            //RichTextBox的行号从0开始。
            int CurrentLine = this.richTextBox1.GetLineFromCharIndex(this.richTextBox1.SelectionStart);
            int StartLine;
            if ((StartLine = CurrentLine - (this.richTextBox1.Lines.Length - this.TextBoxLines)) < 0)
            {
                this.TextBoxLines = (richTextBox1.Lines.Length == 0 ? 1 : richTextBox1.Lines.Length);
                return;
            }
            this.TextBoxLines = (richTextBox1.Lines.Length == 0 ? 1 : richTextBox1.Lines.Length);
            P.Paint(StartLine,CurrentLine,ref this.richTextBox1);
        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.richTextBox1.Width = this.Width - 16;
            this.richTextBox1.Height = this.Height - 62;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void SubmitMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this," 真的要退出吗？请确认你的更改已经保存。", "提示消息", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileExplorer = new OpenFileDialog();
            FileExplorer.ShowDialog();
            string FilePath = FileExplorer.FileName;
            try
            {
                StreamReader sr = new StreamReader(FilePath, Encoding.Default); //读取文件
                string t = null, r = null;
                
                while ((t = sr.ReadLine()) != null)
                {
                    r += t+'\n';
                }
                this.richTextBox1.Clear();
                this.richTextBox1.AppendText(r);
                this.IfSaved = true; //打开文件等效于已经保存
            }
            catch (Exception ex) { MessageBox.Show("打开文件失败。", " 错误提示", MessageBoxButtons.OK); }

        }
    }
}
