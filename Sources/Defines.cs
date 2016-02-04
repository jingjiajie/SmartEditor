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
        public static string Version = "V0.1.4";
        public static string XMLDirectory = "./Config/";
        public static string SettingFilePath = "./Config/Settings.xml";

        public static string CurrentLanguage = null; //当前语言
        public static List<string> KeyWords = new List<string>(); //当前语言的所有关键字
        public static List<string> SupportedLanguages = new List<string>(); //所有支持的语言
        

        public static string DefaultMainFormTitle = "SmartEditor " + Version; //默认主窗口标题
        public static Font TextBoxFont = new Font("Consolas", 9); //主窗口RichTextBox字体

        public static string NewFileName = "New File"; //新建文件默认文件名
        

    }
}
