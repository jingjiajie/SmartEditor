# SmartEditor
SmartEditor is a simple,light weight and customizable syntax highlighting editor.
To use it, you should ensure the "Config" directory is in the same directory of the main program,which includes the settings of SmartEditor and schemes of syntax highlighting.
SmartEditor is now supporting C# and C++ Languages.
To add more supported languages,please put the currect .xml files in the Config directory and edit the Settings.xml to add your new language.

SmartEditor 是一款简单，轻量级并且可定制性强的语法高亮文本编辑器。
在使用它之前，你需要确保Config目录（程序设置和配置文件夹）和主程序在相同目录中。
SmartEditor目前原生支持C++和C#的语法高亮。
如果你需要增加更多语言，请将符合格式的.xml文件拷贝到Config目录中，并编辑Settings.xml增加你的语言。


#For Developers
To view or change the program, you should open the .sln file with Visual Studio. 
After compiling, don't forget to copy the Config directory to the same directory of the "SmartEditor.exe".

如果你需要修改/开发此项目，请用 Microsoft Visual Studio 打开SmartEditor.sln工程文件。
编译完成后，不要忘记拷贝Config目录拷贝到主程序的相同目录。

#Histories

V0.1.4 	Added more supported language.
	Added the "About" Window. 2016/2/5

V0.1.3  Fixed bugs in highlighting string-like words with only one quotation mark.
	Realized Opening/Saving File function.
	Realized Finding functions. 2016/2/4 

V0.1.2  Fixed many bugs in syntax highlighting, reconstituted the project for more future functions. 2016/2/3
	
V0.1.1  Realized syntax highlighting function. 2016/2/2