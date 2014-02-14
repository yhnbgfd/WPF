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
        private static SQLiteCommand cmd = new SQLiteCommand();
        private static SQLiteDataReader reader;
        private static DataSet data = new DataSet();

        static Database()
        {
            GetConnect();
        }

        private static void GetConnect()
        {
            SQLiteConnectionStringBuilder connBuilder = new SQLiteConnectionStringBuilder();
            connBuilder.DataSource = DataSource;
            conn.ConnectionString = connBuilder.ToString();
            conn.Open();
            cmd.Connection = conn;
        }
        /// <summary>
        /// 关闭、销毁连接
        /// </summary>
        public static void Disconnect()
        {
            conn.Close();
            conn.Dispose();
        }

        /// <summary>
        /// Select结果fill到DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet Select(string sql)
        {
            new Wpf.Helper.Log().DBLog("SELECT SQL:" + sql);
            SQLiteDataAdapter dAdapter = new SQLiteDataAdapter(sql, conn);
            data.Clear();
            dAdapter.Fill(data, "T_Report");
            return data;
        }

        /// <summary>
        /// 执行普通的update语句
        /// </summary>
        /// <param name="sql"></param>
        public static void Update(string sql)
        {
            new Wpf.Helper.Log().DBLog("UPDATE SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行普通的insert语句
        /// </summary>
        /// <param name="sql"></param>
        public static void Insert(string sql)
        {
            new Wpf.Helper.Log().DBLog("INSERT SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public static void BatchInsertDatabase(List<string> sqlArray)
        {
            foreach (string sql in sqlArray)
            {
                new Wpf.Helper.Log().DBLog("Inser SQL:" + sql);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 执行普通的delete语句
        /// </summary>
        /// <param name="sql"></param>
        public static void Delete(string sql)
        {
            new Wpf.Helper.Log().DBLog("DELETE SQL:" + sql);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 查询T_Surplus特定年月类型的结余
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static double SelectSurplus(string sql)
        {
            double result = 0;
            new Wpf.Helper.Log().DBLog("SelectSurplus SQL:" + sql);
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    result = reader.GetDouble(0);
                }
                catch (Exception)
                {
                    
                }
            }
            reader.Close();
            return result;
        }

        /// <summary>
        /// 查询count(*)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int SelectCount(string sql)
        {
            int result = 0;
            new Wpf.Helper.Log().DBLog("SelectCount SQL:" + sql);
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    result = reader.GetInt32(0);
                }
                catch(Exception)
                {

                }
            }
            reader.Close();
            return result;
        }

        /// <summary>
        /// 统计借方发生额累计（当年到当前月）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static void Count借方发生额累计(int type, int year, int month)
        {
            string sql = "SELECT total(income) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format(year + "-" + (month + 1) + "-01") + "','-1 second')";

            if(month == 0)
            {
                sql = "SELECT total(income) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format((year+1) + "-01-01") + "','-1 second')";
            }
            new Wpf.Helper.Log().DBLog("Count借方发生额累计 SQL:" + sql);
            double result = 0;
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetDouble(0);
            }
            reader.Close();
            Properties.Settings.Default.借方发生额累计 = result;
        }

        /// <summary>
        /// 统计贷方发生额累计（当年到当前月）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static void Count贷方发生额累计(int type, int year, int month)
        {
            string sql = "SELECT total(expenses) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format(year + "-" + (month + 1) + "-01") + "','-1 second')";
            if(month == 0)
            {
                sql = "SELECT total(expenses) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format((year+1) + "-01-01") + "','-1 second')";
            }
            new Wpf.Helper.Log().DBLog("Count贷方发生额累计 SQL:" + sql);
            double result = 0;
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetDouble(0);
            }
            reader.Close();
            Properties.Settings.Default.贷方发生额累计 = result;
        }
        /// <summary>
        /// 如果T_Surplus没特定的年月条目，则插入该年月5个type5条数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public static void InsertSurplus(int year, int month)
        {
            string sql = "";
            for (int i = 1; i <= 5; i++)
            {
                sql = "insert into T_Surplus(year,month,surplus,type) values(" + year + "," + month + ",0," + i + ")";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
