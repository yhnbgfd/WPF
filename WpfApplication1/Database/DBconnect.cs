using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace Wpf.Database
{
    class DBconnect
    {
        string datasource = "test.db";
        
        public void Connect()
        {
            SQLiteConnection conn = new SQLiteConnection();
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = datasource;
            connstr.Password = "Hh123123";
            conn.ConnectionString = connstr.ToString();
            conn.Open();
        }

        public void DisConnect()
        {

        }
    }
}
