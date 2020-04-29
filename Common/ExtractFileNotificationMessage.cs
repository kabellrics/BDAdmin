using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ExtractFileNotificationMessage : MessageBase
    {
        public ExtractFileNotificationMessage(string message, byte[] img = null)
        {
            Message = message;
            IMG = img;
        }
        public String Message { get; set; }
        public byte[] IMG { get; set; }
    }
}
