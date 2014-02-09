using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace Wpf.Data
{
    public class Database
    {
        private string DataSource = Properties.Settings.Default.DataSource;
        private SQLiteConnection conn = new SQLiteConnection();
        private DataSet data = new DataSet();

        private string SelectSql = "SELECT id,datetime,unitsname,use,income,expenses FROM main.T_Report";
        private string UpdateSql;
        private string InsertSql;
        private string DeleteSql;

        public Database()
        {
            this.GetConnect();
        }


        private void CreateFile()
        {
            SQLiteConnection.CreateFile(DataSource);
        }
        private void GetConnect()
        {
            SQLiteConnectionStringBuilder connBuilder = new SQLiteConnectionStringBuilder();
            connBuilder.DataSource = DataSource;
            conn.ConnectionString = connBuilder.ToString();
            conn.Open();
        }


        public DataSet Select()
        {
            SQLiteDataAdapter dAdapter = new SQLiteDataAdapter(SelectSql, conn);
            dAdapter.Fill(data, "T_Report");
            return data;
        }
    }
}
