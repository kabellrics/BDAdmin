using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Common
{
    public class ConsoleNotif
    {
        public String Message;
        public Color color;
        public ConsoleNotif() { }
        public ConsoleNotif(String msg, Color textcolor)
        {
            this.Message = msg;
            this.color = textcolor;
        }
    }
}
