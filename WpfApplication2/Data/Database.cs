using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace Wpf.Data
{
    public static class Database
    {
        private static string DataSource = Properties.Settings.Default.DataSource;
        private static SQLiteConnection conn = new SQLiteConnection();
        private static DataSet data = new DataSet();

        private static string SelectSql = "SELECT id,datetime,unitsname,use,income,expenses FROM main.T_Report";
        private static string UpdateSql;
        private static string InsertSql;
        private static string DeleteSql;

        static Database()
        {
            GetConnect();
        }


        private static void CreateFile()
        {
            SQLiteConnection.CreateFile(DataSource);
        }
        private static void GetConnect()
        {
            SQLiteConnectionStringBuilder connBuilder = new SQLiteConnectionStringBuilder();
            connBuilder.DataSource = DataSource;
            conn.ConnectionString = connBuilder.ToString();
            conn.Open();
        }


        public static DataSet Select()
        {
            SQLiteDataAdapter dAdapter = new SQLiteDataAdapter(SelectSql, conn);
            dAdapter.Fill(data,"T_Report");
            return data;
        }
    }
}
