using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAdmin.ViewModel.Model
{
    public class NotificationMessage : MessageBase
    {
        public NotificationMessage(string msg):base()
        {
            Message = msg;
        }
        public String Message { get; set; }
    }
}
