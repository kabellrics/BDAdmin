using System;
using System.Collections.Generic;
using System.Text;

namespace DBConnector
{
    public class DBFichier
    {
        public int ID;
        public string Name;
        public Nullable<int> Order;
        public byte[] Image;
        public Nullable<int> ParentID;
        public string Collection;
    }
}
