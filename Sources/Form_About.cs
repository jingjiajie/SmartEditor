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
    public partial class Form_About : Form
    {
        private MainForm Parent;
        public Form_About(MainForm Parent)
        {
            InitializeComponent();
            
            this.Parent = Parent;
           // this.Parent.Enabled = false;

        }

        private void Form_About_Load(object sender, EventArgs e)
        {
            this.Location = new Point(this.Parent.Location.X + 180, this.Parent.Location.Y + 150);
        }

        private void Form_About_FormClosing(object sender, FormClosingEventArgs e)
        {
           // this.Parent.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/jingjiajie/SmartEditor");
            }catch(Exception ex) { MessageBox.Show(this, "无法打开网页，请检查您的浏览器配置", "错误消息"); };
        }
    }
}
