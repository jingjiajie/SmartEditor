using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SmartEditor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program.ReadKeyWords();
            Application.Run(new MainForm());
        }

        public static void ReadKeyWords()
        {
                try
                {
                    StreamReader sr = new StreamReader("CSKeyWords.ini", Encoding.Default); //读取关键字
                    String t;
                    while ((t = sr.ReadLine()) != null)
                    {
                        Defines.KeyWords.Push(t);
                    }
                }
                catch (Exception ex) { MessageBox.Show("读取关键字配置文件失败，无法对关键字进行着色。", " 错误提示", MessageBoxButtons.OK); }
        }
    }
}
