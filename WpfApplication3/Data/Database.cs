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

        private string SelectSql = "SELECT id,datetime,unitsname,use,income,expenses FROM main.T_Report";
        private string UpdateSql;
        private string InsertSql;
        private string DeleteSql;

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
        }

        private void Disconnect()
        {
            conn.Close();
            
        }

        public DataSet Select()
        {
            SQLiteDataAdapter dAdapter = new SQLiteDataAdapter(SelectSql, conn);
            dAdapter.Fill(data, "T_Report");
            this.Disconnect();
            return data;
        }

        public int SelectOne(long id)
        {
            int result = 0;
            SelectSql = "select count(*) from T_Report where id =" + id;
            cmd.CommandText = SelectSql;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                //Console.WriteLine(reader.GetInt32(0));
                result = reader.GetInt32(0);
            }
            this.Disconnect();
            return result;
        }

        public void Update(string sql)
        {
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect();
        }

        public void Insert(string sql)
        {
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect();
        }
    }
}
