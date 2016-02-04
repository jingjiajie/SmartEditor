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
            FileExplorer.Filter = "所有文件|*.*";
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
                FileExplorer.Filter = "所有文件|*.*";
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


        private void ChangeCurrentLanguage(string LanguageName)
        {
            try
            {
                (new Files()).LoadLanguageConf(LanguageName);
                (new Painter()).Repaint(this.richTextBox1);
            }
            catch(Exception ex) { MessageBox.Show(this, "读取语言配置文件失败,无法对语法进行着色。错误信息：\n" + ex.Message, "错误提示"); }
        }
        public MainForm()
        {
            InitializeComponent();
            this.Text = " New File * - "+Defines.DefaultMainFormTitle;
            //   Control.CheckForIllegalCrossThreadCalls = false;
            for (int i = 0; i < Defines.SupportedLanguages.Count; i++)
            {
                string t = null;
                switch (t = Defines.SupportedLanguages[i])
                {
                    case "CSharp": { this.LanguageBox.Items.Add("C#"); break; }
                    case "CPP": { this.LanguageBox.Items.Add("C++"); break; }
                    default: { this.LanguageBox.Items.Add(t); break; }
                }
            }
            if(Defines.SupportedLanguages.Count != 0)
            {
                this.LanguageBox.SelectedIndex = 0;
            }
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
            if(CurrentLine>=this.SelectionStartLine) P.Paint(this.SelectionStartLine,CurrentLine,this.richTextBox1);
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
                DialogResult DR;
                if (Files.CurrentFile != null) DR = MessageBox.Show(this, "是否将更改保存到 " + Files.CurrentFile + " ？", "提示消息", MessageBoxButtons.YesNoCancel);
                else DR = MessageBox.Show(this, "是否将更改保存到 " + Defines.NewFileName + " ？", "提示消息", MessageBoxButtons.YesNoCancel);
                switch (DR)
                {
                    case DialogResult.Yes: { this.SaveFile(); break; }
                    case DialogResult.No: break;
                    case DialogResult.Cancel: return;
                }
            }
            OpenFileDialog FileExplorer = new OpenFileDialog();
            FileExplorer.Filter = "所有文件|*.*";
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
            DialogResult DR;
            if (this.IfSaved == false)
            {
                if (Files.CurrentFile == null)
                {
                    DR = MessageBox.Show(this, " 是否保存更改到 New File？", "提示消息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (DR == DialogResult.Yes) { this.SaveFileAs(); }
                    else if (DR == DialogResult.Cancel) e.Cancel = true;
                }
                else
                {
                    DR = MessageBox.Show(this, " 是否保存更改到" + Files.CurrentFile + "？", "提示消息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (DR == DialogResult.Yes) { this.SaveFile(); }
                    else if (DR == DialogResult.Cancel) { e.Cancel = true; }
                }
            }

        }

        private void FindMenuItem_Click(object sender, EventArgs e)
        {
            Form_Find F = new Form_Find(this);
            F.Show(this);
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            (new Form_About(this)).Show(this);
        }

        private void LanguageBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string LanguageName = null;
            switch (this.LanguageBox.Text)
            {
                case "C#": { LanguageName = "CSharp"; break; }
                case "C++": { LanguageName = "CPP"; break; }
                default: { LanguageName = this.LanguageBox.Text; break; }
            }
            this.ChangeCurrentLanguage(LanguageName);
        }
    }
}
