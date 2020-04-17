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
        public byte[] Data;
        public string Extension;
        public Nullable<int> Year;
        public byte[] Image;
        public string ImgExtension;
        public Nullable<int> ParentID;
        public Nullable<int> CollectionID;
    }
}
