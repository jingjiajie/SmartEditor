using System;
using System.Xml;
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
        public static string CurrentFileExt = null;

        public byte[] OpenFile(string FilePath) //打开文件
        {
            try
            {
                FileStream FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read); //创建文件流
                FileInfo FI = new FileInfo(FilePath);
                int FLength = (int)FS.Length;
                byte[] FileContent = new byte[FLength];
                FS.Read(FileContent, 0, FLength);
                Files.CurrentFile = FilePath;
                Files.CurrentFileExt = FI.Extension;
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

       /* public void LoadLanguageConf(string LanguageName)
        {
            string FilePath = Defines.XMLDirectory + LanguageName + ".xml";
            Defines.CurrentLanguage = LanguageName;
            Defines.KeyWords.Clear();
            Defines.Operators.Clear();
            XmlDocument XmlDoc = new XmlDocument();
            try
            {
                XmlDoc.Load(FilePath);
                XmlNode RootNode = XmlDoc.SelectSingleNode("/Configuration");
                XmlNodeList KeyWordList = RootNode.SelectNodes("KeyWords/KeyWord");
                XmlNodeList OperatorList = RootNode.SelectNodes("Operators/Operator");
                XmlNode Comment = RootNode.SelectSingleNode("Comment");
                for (int i = 0; i < KeyWordList.Count; i++)
                {
                    Defines.KeyWords.Add(KeyWordList[i].InnerText);
                }
                for (int i = 0; i < OperatorList.Count; i++)
                {
                    Defines.Operators.Add(OperatorList[i].InnerText);
                }
                Defines.Comment = Comment.InnerText;
            }
            catch(Exception ex) { throw ex; }
            
        } */

        public void LoadSettings(string SettingFilePath)
        {
            XmlDocument XmlDoc = new XmlDocument();
            try
            {
                XmlDoc.Load(SettingFilePath);
                XmlNode RootNode = XmlDoc.SelectSingleNode("/Settings");
                XmlNodeList SupportedLanguageList = RootNode.SelectNodes("SupportedLanguages/Language");
                Defines.SupportedLanguages.Clear();
                for (int i = 0; i < SupportedLanguageList.Count; i++)
                {
                    Defines.SupportedLanguages.Add(SupportedLanguageList[i].InnerText);
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
