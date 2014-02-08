using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace Wpf.Database
{
    class DBInitialize
    {
        string datasource = "test.db";

        public void CreateDataFile()
        {
            SQLiteConnection.CreateFile(datasource);
        }

        public void CreateTable()
        {
            SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
            string sql = "CREATE TABLE test(username varchar(20),password varchar(20))";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }
    }
}
