using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartEditor
{
    public partial class Form_Find : Form
    {
        private MainForm Parent;
        public Form_Find(MainForm Parent)
        {
            InitializeComponent();
            this.Parent = Parent;
        }

        private void FindNextButton_Click(object sender, EventArgs e)
        {
            RichTextBoxFinds Options = RichTextBoxFinds.None;
            if (this.CaseSensitive.Checked == true) Options |= RichTextBoxFinds.MatchCase;
            if (this.DownRadioButton.Checked == true)
            {
                if (this.Parent.richTextBox1.SelectionLength > 0)
                {
                    this.Parent.richTextBox1.SelectionStart += this.Parent.richTextBox1.SelectionLength;
                    this.Parent.richTextBox1.SelectionLength = 0;
                }
                if (this.Parent.richTextBox1.Find(this.FindTextBox.Text, this.Parent.richTextBox1.SelectionStart, Options) < 0)
                { MessageBox.Show(this, "找不到\"" + this.FindTextBox.Text + "\"", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
            }else if(this.UpRadioButton.Checked == true)
            {
                Options |= RichTextBoxFinds.Reverse;
                if (this.Parent.richTextBox1.SelectionLength > 0)
                {
                    this.Parent.richTextBox1.SelectionLength = 0;
                }
                if (this.Parent.richTextBox1.Find(this.FindTextBox.Text, 0,this.Parent.richTextBox1.SelectionStart, Options) < 0)
                { MessageBox.Show(this, "找不到\"" + this.FindTextBox.Text + "\"", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Form_Find_Load(object sender, EventArgs e)
        {
            this.Location = new Point(this.Parent.Location.X + 180, this.Parent.Location.Y + 150);
        }

        private void FindTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.FindTextBox.Text != "") this.FindNextButton.Enabled = true;
            else this.FindNextButton.Enabled = false;
        }
    }
}
