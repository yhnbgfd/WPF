using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace MvvmLight.Database
{
    class DBConnect
    {
        private string datasource = Properties.Settings.Default.DataSource;
        private SQLiteConnection conn = new SQLiteConnection();

        public SQLiteConnection GetConnect()
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = datasource;
            //connstr.Password = "Hh123123";
            conn.ConnectionString = connstr.ToString();
            conn.Open();
            return conn;
        }

        public void DisConnect()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
