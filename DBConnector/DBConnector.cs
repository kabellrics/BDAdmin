using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBConnector
{
    public class DBConnector
    {
        protected MySqlConnection connection;
        public DBConnector()
        {
            //connection = new MySqlConnection(string.Format("server={0};port={1};user={2};password={3};database={4}", "http://192.168.1.13/","3307", "root", "flechel@m9a7d2", "BDComics"));
            connection = new MySqlConnection("Server=192.168.1.13;Port=3307;Database=BDComics;Uid=root;Pwd=flechel@m9a7d2;Allow User Variables=true");
        }
    }
}
