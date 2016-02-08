using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartEditor.Lexers
{
    abstract class Lexer
    {
        abstract public bool IsKeyWord(string str);
        abstract public bool IsOperator(string str);
        abstract public bool IsString(string str);
        abstract public bool IsComment(string str);
        abstract public string Next(ref int StartPos, string LineStr);

    }
}
