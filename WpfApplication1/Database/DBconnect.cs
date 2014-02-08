using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace Wpf.Database
{
    class DBconnect
    {
        /// <summary>
        /// 数据库文件path+name
        /// </summary>
        private string datasource = Properties.Settings.Default.DataSource;
        private SQLiteConnection conn = new SQLiteConnection();

        public SQLiteConnection Connect()
        {
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = datasource;
            connstr.Password = "Hh123123";
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
