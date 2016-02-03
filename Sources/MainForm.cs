using System;
using System.IO;
using System.Resources;
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
        private int SelectionStartLine = 0; //RichTextBox当前行号
        private bool _IfSaved = false; //是否已经保存文件
        private bool IfSaved
        {
            get { return this._IfSaved; }
            set
            {
                this.SubmitMenuItem.Enabled = !value;
                if (value == false && value != this._IfSaved) this.Text = Files.CurrentFile + " * - " + Defines.DefaultMainFormTitle;
                this._IfSaved = value;
            }
        }


        private void SaveFileAs() //另存为文件
        {
            SaveFileDialog FileExplorer = new SaveFileDialog();
            if (FileExplorer.ShowDialog() != DialogResult.OK) return;
            string FilePath = FileExplorer.FileName;
            byte[] FileContent = System.Text.Encoding.Default.GetBytes(this.richTextBox1.Text);
            try
            {
                Files F = new Files();
                F.SaveFile(FilePath, FileContent);
                this.IfSaved = true; //设置为已保存
                this.Text = Files.CurrentFile + " - " + Defines.DefaultMainFormTitle;
            }
            catch (Exception ex) { MessageBox.Show("保存文件失败：\n" + ex.Message, "错误消息", MessageBoxButtons.OK); }
        }
        private void SaveFile() //保存文件
        {
            string FilePath = null;
            if (Files.CurrentFile == null)
            {
                SaveFileDialog FileExplorer = new SaveFileDialog();
                FileExplorer.Title = "保存";
                if (FileExplorer.ShowDialog() != DialogResult.OK) return;
                FilePath = FileExplorer.FileName;
            }
            else { FilePath = Files.CurrentFile; }
            byte[] FileContent = System.Text.Encoding.Default.GetBytes(this.richTextBox1.Text);
            try
            {
                Files F = new Files();
                F.SaveFile(FilePath, FileContent);
                this.IfSaved = true; //设置为已保存
                this.Text = Files.CurrentFile + " - " + Defines.DefaultMainFormTitle;
            }
            catch (Exception ex) { MessageBox.Show("保存文件失败：\n" + ex.Message, "错误消息", MessageBoxButtons.OK); }
        }

        private void AskIfSave()
        {
            if (this.IfSaved == false)
            {
                if (Files.CurrentFile == null)
                {
                    if (MessageBox.Show(this, " 是否保存更改到 New File？", "提示消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) this.SaveFileAs();
                }
                else { if (MessageBox.Show(this, " 是否保存更改到" + Files.CurrentFile + "？", "提示消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) this.SaveFile(); }
            }
        }

        public MainForm()
        {
            InitializeComponent();
            this.Text = " New File * - "+Defines.DefaultMainFormTitle;
            //   Control.CheckForIllegalCrossThreadCalls = false;
            this.richTextBox1.Width = this.Width - 16;
            this.richTextBox1.Height = this.Height - 83;
            this.richTextBox1.AcceptsTab = true;

            this.P = new Painter();
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //RichTextBox的行号从0开始。
            this.IfSaved = false;
            int CurrentLine = this.richTextBox1.GetLineFromCharIndex(this.richTextBox1.SelectionStart);
            if(CurrentLine>=this.SelectionStartLine) P.Paint(this.SelectionStartLine,CurrentLine,ref this.richTextBox1);
            this.SelectionStartLine = CurrentLine;
        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.richTextBox1.Width = this.Width - 16;
            this.richTextBox1.Height = this.Height - 83;
        }

        private void SubmitMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveFile();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            if (this.IfSaved == false)
            {
                DialogResult DR = MessageBox.Show(this, "是否保存 " + Files.CurrentFile + " ？", "提示消息", MessageBoxButtons.YesNoCancel);
                switch (DR)
                {
                    case DialogResult.Yes: { this.SaveFile(); break; }
                    case DialogResult.No: break;
                    case DialogResult.Cancel: return;
                }
            }
            OpenFileDialog FileExplorer = new OpenFileDialog();
            if(FileExplorer.ShowDialog()!=DialogResult.OK) return;
            FileExplorer.ShowDialog();
            string FilePath = FileExplorer.FileName;
            Files F = new Files();
            try
            {
                string t = System.Text.Encoding.Default.GetString(F.OpenFile(FilePath));
                this.richTextBox1.Clear();
                this.richTextBox1.AppendText(t);
                this.Text = Files.CurrentFile + " - " + Defines.DefaultMainFormTitle;
                this.IfSaved = true; //打开文件等效于已经保存
            }
            catch(Exception ex) { MessageBox.Show("打开文件失败：\n" + ex.Message, "错误消息", MessageBoxButtons.OK); }

        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            int SelectionLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
            int StartIndex = richTextBox1.GetFirstCharIndexOfCurrentLine();
            LineNumberStrip.Text = "行号：" + SelectionLine;
            ColumnNumberStrip.Text = "列号：" + (this.richTextBox1.SelectionStart - StartIndex);
        }

        private void richTextBox1_MouseCaptureChanged(object sender, EventArgs e)
        {
            this.SelectionStartLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.SelectionStartLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveFileAs();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // DialogResult DR = MessageBox.Show("确认要退出吗？", "提示 ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (DR == DialogResult.Yes) this.AskIfSave();
            //else e.Cancel = true;
            this.AskIfSave();
        }

        private void FindMenuItem_Click(object sender, EventArgs e)
        {
            Form_Find F = new Form_Find(this);
            F.Show(this);
        }
    }
}
