using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SmartEditor
{
    class Defines
    {
        public static string Version = "V0.1.6";
        public static string CompilerFilePath = ".\\Compilers\\";

        public static string CurrentLanguage = null; //当前语言
        public static List<string> SupportedLanguages = new List<string>() { "CPP", "CSharp" };

        public static string DefaultMainFormTitle = "SmartEditor "; //默认主窗口标题
        public static Font TextBoxFont = new Font("Consolas", 9); //主窗口RichTextBox字体

        public static string NewFileName = "New File"; //新建文件默认文件名

    }
}
