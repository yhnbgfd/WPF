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

        public static void ChangePassword(string password)
        {
            conn.ChangePassword(password);
        }

        /// <summary>
        /// 清除数据库密码
        /// </summary>
        public static void ClearPassword()
        {
            conn.ChangePassword("");
        }

        private static void GetConnect()
        {
            SQLiteConnectionStringBuilder connBuilder = new SQLiteConnectionStringBuilder();
            connBuilder.DataSource = DataSource;
            conn.ConnectionString = connBuilder.ToString();
            if (Properties.Settings.Default.正式版 && Properties.Settings.Default.初始化程序)
            {
                //正式版且初始化过程序，这时候数据库有密码
                conn.SetPassword(Wpf.Helper.Secure.GetMD5_32(Properties.Settings.Default.注册码 + "PowerByStoneAnt"));
            }
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
            SQLiteDataAdapter dAdapter = new SQLiteDataAdapter(sql, conn);
            data.Clear();
            try
            {
                dAdapter.Fill(data, "T_Report");
            }
            catch(Exception)
            {

            }
            return data;
        }

        /// <summary>
        /// 执行普通的update语句
        /// </summary>
        /// <param name="sql"></param>
        public static void Update(string sql)
        {
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行普通的insert语句
        /// </summary>
        /// <param name="sql"></param>
        public static void Insert(string sql)
        {
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public static void BatchInsertDatabase(List<string> sqlArray)
        {
            //foreach (string sql in sqlArray)
            //{
            //    cmd.CommandText = sql;
            //    cmd.ExecuteNonQuery();
            //}
            Transaction(sqlArray);
        }
        /// <summary>
        /// 执行普通的delete语句
        /// </summary>
        /// <param name="sql"></param>
        public static void Delete(string sql)
        {
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 查询T_Surplus特定年月类型的结余
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static decimal SelectSurplus(string sql)
        {
            decimal result = 0m;
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    result = reader.GetDecimal(0);
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
                + " AND datetime('" + Wpf.Helper.Date.Format(year + "-" + (month + 1) + "-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";

            if(month == 0)
            {
                sql = "SELECT total(income) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format((year+1) + "-01-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";
            }
            decimal result = 0m;
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetDecimal(0);
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
                + " AND datetime('" + Wpf.Helper.Date.Format(year + "-" + (month + 1) + "-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";
            if(month == 0)
            {
                sql = "SELECT total(expenses) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format((year+1) + "-01-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";
            }
            decimal result = 0m;
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetDecimal(0);
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
            for (int i = 1; i <= 10; i++)
            {
                sql = "insert into T_Surplus(year,month,surplus,type) values(" + year + "," + month + ",0," + i + ")";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public static bool VerifyLicense()
        {
            string settingsStr = Properties.Settings.Default.注册码;
            string sql = "select value from T_Type where key=998";
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                if(reader.GetString(0) == settingsStr)
                {
                    reader.Close();
                    return true;
                }
            }
            reader.Close();
            return false;
        }

        /// <summary>
        /// 将设置里的初始金额初始化到数据库
        /// </summary>
        public static void Init初始金额()
        {
            Wpf.Data.Database.Update("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_预算内户 + " WHERE TYPE=1");
            Wpf.Data.Database.Update("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_预算外户 + " WHERE TYPE=2");
            Wpf.Data.Database.Update("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_周转金户 + " WHERE TYPE=3");
            Wpf.Data.Database.Update("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_计生专户 + " WHERE TYPE=4");
            Wpf.Data.Database.Update("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_政粮补贴资金专户 + " WHERE TYPE=5");
            Wpf.Data.Database.Update("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_土地户 + " WHERE TYPE=6");
        }

        /// <summary>
        /// 批量事务
        /// </summary>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        public static bool Transaction(List<string> sqlList)
        {
            bool flag = false;
            SQLiteTransaction strans = conn.BeginTransaction();
            try
            {

                foreach (string sql in sqlList)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                strans.Commit();
                flag = true;
            }
            catch (SQLiteException e)
            {
                strans.Rollback();
                Console.WriteLine("异常:" + e.Message);
                Console.WriteLine(sqlList[1].ToString());
            }
            return flag;
        }
    }
}
