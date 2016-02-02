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
        public static string Version = "V0.1.2";

        public static Stack<string> KeyWords = new Stack<string>();

        public static string DefaultMainFormTitle = "SmartEditor " + Version; //默认主窗口标题
        public static Font TextBoxFont = new Font("Consolas", 9); //主窗口RichTextBox字体
        

    }
}
