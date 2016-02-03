using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartEditor
{
    class Files
    {
        public static string CurrentFile = null;

        public byte[] OpenFile(string FilePath) //打开文件
        {
            try
            {
                FileStream FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read); //创建文件流
                int FLength = (int)FS.Length;
                byte[] FileContent = new byte[FLength];
                FS.Read(FileContent, 0, FLength);
                Files.CurrentFile = FilePath;
                FS.Close();
                return FileContent;
            }
            catch (Exception ex) { throw ex;}
        }

        public void SaveFile(string FilePath, byte[] FileContent) //保存文件
        {
            try
            {
                FileStream FS = new FileStream(FilePath,FileMode.Create,FileAccess.Write);
                FS.Write(FileContent, 0, FileContent.Length);
                Files.CurrentFile = FilePath;
                FS.Close();
                return;
            }catch(Exception ex) { throw ex; }
        }
    }
}
