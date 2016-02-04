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
            Files F = new Files();
            try
            {
                F.LoadSettings(Defines.SettingFilePath);
            }catch(Exception ex) { MessageBox.Show("读取配置文件失败。错误信息：\n" + ex.Message, "错误提示"); }
            Application.Run(new MainForm());
        }

    }
}