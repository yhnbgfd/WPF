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
        SQLiteCommand cmd = new SQLiteCommand();
        private DataSet data = new DataSet();

        public Database()
        {
            this.GetConnect();
            cmd.Connection = conn;
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
            new Wpf.Helper.Log().SaveLog("DB Connect!");
        }

        private void Disconnect()
        {
            conn.Close();
            new Wpf.Helper.Log().SaveLog("DB Disconnect!");
        }

        public DataSet Select(string sql)
        {
            new Wpf.Helper.Log().SaveLog("SELECT SQL:" + sql);
            SQLiteDataAdapter dAdapter = new SQLiteDataAdapter(sql, conn);
            dAdapter.Fill(data, "T_Report");
            this.Disconnect();
            return data;
        }

        public void Update(string sql)
        {
            new Wpf.Helper.Log().SaveLog("UPDATE SQL:"+sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect();
        }

        public void Insert(string sql)
        {
            new Wpf.Helper.Log().SaveLog("INSERT SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect();
        }

        public void Delete(string sql)
        {
            new Wpf.Helper.Log().SaveLog("DELETE SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect();
        }

        public double SelectSurplus(string sql)
        {
            double result = 0;
            new Wpf.Helper.Log().SaveLog("SelectSurplus SQL:" + sql);
            cmd.CommandText = sql;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                result = reader.GetDouble(0);
            }
            this.Disconnect();
            return result;
        }
    }
}
