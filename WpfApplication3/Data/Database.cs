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
            GetConnect();
            new Wpf.Helper.Log().DBLog("GetConnect " + Wpf.Data.DataDef.isDBconnect);
        }

        private void GetConnect()
        {
            SQLiteConnectionStringBuilder connBuilder = new SQLiteConnectionStringBuilder();
            connBuilder.DataSource = DataSource;
            conn.ConnectionString = connBuilder.ToString();
            conn.Open();
            cmd.Connection = conn;
            Wpf.Data.DataDef.isDBconnect = true;
        }

        private void Disconnect(SQLiteConnection conn)
        {
            conn.Close();
            conn.Dispose();
            Wpf.Data.DataDef.isDBconnect = false;
            new Wpf.Helper.Log().DBLog("Disconnect " + Wpf.Data.DataDef.isDBconnect);
        }

        public DataSet Select(string sql)
        {
            new Wpf.Helper.Log().DBLog("SELECT SQL:" + sql);
            SQLiteDataAdapter dAdapter = new SQLiteDataAdapter(sql, conn);
            dAdapter.Fill(data, "T_Report");
            this.Disconnect(conn);
            return data;
        }

        public void Update(string sql)
        {
            new Wpf.Helper.Log().DBLog("UPDATE SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect(conn);
        }

        public void Insert(string sql)
        {
            new Wpf.Helper.Log().DBLog("INSERT SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect(conn);
        }

        public void BatchInsertDatabase(List<string> sqlArray)
        {
            //new Wpf.Helper.Log().SaveLog("Inser SQL:"
            foreach (string sql in sqlArray)
            {
                new Wpf.Helper.Log().DBLog("Inser SQL:" + sql);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            this.Disconnect(conn);
        }

        public void Delete(string sql)
        {
            new Wpf.Helper.Log().DBLog("DELETE SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            this.Disconnect(conn);
        }

        public double SelectSurplus(string sql)
        {
            double result = 0;
            new Wpf.Helper.Log().DBLog("SelectSurplus SQL:" + sql);
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
            new Wpf.Helper.Log().DBLog("SelectCount SQL:" + sql);
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
            new Wpf.Helper.Log().DBLog("Count借方发生额累计 SQL:" + sql);
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
                    this.Disconnect(conn);
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
            new Wpf.Helper.Log().DBLog("Count贷方发生额累计 SQL:" + sql);
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
                    this.Disconnect(conn);
                    return result;
                }
            }
            this.Disconnect(conn);
            return result;
        }

        public void InsertSurplus(int year, int month)
        {
            string sql = "";
            for (int i = 1; i <= 5; i++)
            {
                sql = "insert into T_Surplus(year,month,surplus,type) values(" + year + "," + month + ",0," + i + ")";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            this.Disconnect(conn);
        }
    }
}
