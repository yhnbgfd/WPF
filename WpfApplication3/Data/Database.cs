using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace Wpf.Data
{
    public delegate void DML(string sql);

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
        /// 增删改
        /// </summary>
        /// <param name="InSql"></param>
        /// <param name="dml"></param>
        public static void doDML(string InSql, string DMLType)
        {
            List<string> sqls = new List<string>();
            string sql = InSql;
            sqls.Add(sql);
            sqls.Add(new Wpf.ViewModel.ViewModel_操作记录().InsertLog(DMLType, sql, "", "DML"));
            Transaction(sqls);
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

        public static decimal CountDecimal(string sql)
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
