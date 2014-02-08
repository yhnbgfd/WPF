using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace Wpf.Database
{
    public class DBInitialize
    {
        string datasource = Properties.Settings.Default.DataSource;
        SQLiteCommand cmd = new SQLiteCommand();
        DBconnect dbconn = new DBconnect();

        public DBInitialize()
        {
            
        }

        public void CreateDataFile()
        {
            SQLiteConnection.CreateFile(datasource);
            //dbconn.DisConnect();
        }

        public void CreateTable()
        {
            cmd.Connection = dbconn.Connect();
            string sql = "create table content(id integer,datetime timestamp,unitsname text,used text,income real,expenses real);";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            dbconn.DisConnect();
        }
    }
}
