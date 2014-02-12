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
        private SQLiteConnection GetConnect()
        {
            SQLiteConnectionStringBuilder connBuilder = new SQLiteConnectionStringBuilder();
            connBuilder.DataSource = DataSource;
            conn.ConnectionString = connBuilder.ToString();
            conn.Open();
            return conn;
        }

        private void Disconnect(SQLiteConnection conn)
        {
            conn.Close();
        }

        public DataSet Select(string sql)
        {
            new Wpf.Helper.Log().SaveLog("SELECT SQL:" + sql);
            SQLiteDataAdapter dAdapter = new SQLiteDataAdapter(sql, conn);
            dAdapter.Fill(data, "T_Report");
            this.Disconnect(conn);
            return data;
        }

        public void Update(string sql)
        {
            new Wpf.Helper.Log().SaveLog("UPDATE SQL:"+sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect(conn);
        }

        public void Insert(string sql)
        {
            new Wpf.Helper.Log().SaveLog("INSERT SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect(conn);
        }

        public void Delete(string sql)
        {
            new Wpf.Helper.Log().SaveLog("DELETE SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect(conn);
        }

        public double SelectSurplus(string sql)
        {
            double result = 0;
            new Wpf.Helper.Log().SaveLog("SelectSurplus SQL:" + sql);
            cmd.CommandText = sql;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                try
                {
                    result = reader.GetDouble(0);
                }
                catch(Exception)
                {
                }
            }
            this.Disconnect(conn);
            return result;
        }

        /// <summary>
        /// 查询count(*)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int SelectCount(string sql)
        {
            int result = 0;
            new Wpf.Helper.Log().SaveLog("SelectCount SQL:" + sql);
            cmd.CommandText = sql;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetInt32(0);
            }
            this.Disconnect(conn);
            return result;
        }

        /// <summary>
        /// 统计借方发生额累计（当年到当前月）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public double Count借方发生额累计(int type, int year, int month)
        {
            string sql = "SELECT Sum(income) from T_Report "
                +" WHERE T_Report.DateTime IS NOT NULL "
                +" AND type=" + type + " "
                + " AND datetime < datetime('" + new Wpf.Helper.Date().Format(year + "-" + (month + 1) + "-01") + "','-1 second')";
            double result = 0;
            cmd.CommandText = sql;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    result = reader.GetDouble(0);
                }
                catch(Exception)
                {
                    return result;
                }
            }
            this.Disconnect(conn);
            return result;
        }

        /// <summary>
        /// 统计贷方发生额累计（当年到当前月）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public double Count贷方发生额累计(int type, int year, int month)
        {
            string sql = "SELECT Sum(expenses) from T_Report "
                +" WHERE T_Report.DateTime IS NOT NULL "
                +" AND type=" + type + " "
                + " AND datetime < datetime('" + new Wpf.Helper.Date().Format(year + "-" + (month + 1) + "-01") + "','-1 second')";
            double result = 0;
            cmd.CommandText = sql;
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    result = reader.GetDouble(0);
                }
                catch (Exception)
                {
                    return result;
                }
            }
            this.Disconnect(conn);
            return result;
        }
    }
}
