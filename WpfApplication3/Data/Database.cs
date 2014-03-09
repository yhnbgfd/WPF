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
        private static string DataSource = "Data\\Data.db";
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
#if (!DEBUG)
            Wpf.Helper.XmlHelper xml = new Helper.XmlHelper();
            if (xml.ReadXML("注册码") != null && xml.ReadXML("注册码") != "")
            {
                conn.SetPassword(Wpf.Helper.Secure.GetMD5_32("PowerByStoneAntasdasd"));
            }
#endif
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
        public static bool doDML(string InSql, string DMLType, string remark)
        {
            List<string> sqls = new List<string>();
            string sql = InSql;
            sqls.Add(sql);
            sqls.Add(new Wpf.ViewModel.ViewModel_操作记录().InsertLog(DMLType, sql, remark, "DML"));
            return Transaction(sqls);
        }
        public static bool doDMLs(List<string> InSqls, string DMLType, string remark)
        {
            List<string> sqls = new List<string>();
            foreach(string sql in InSqls)
            {
                sqls.Add(sql);
                sqls.Add(new Wpf.ViewModel.ViewModel_操作记录().InsertLog(DMLType, sql, remark, "DML"));
            }
            return Transaction(sqls);
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

        public static string SelectString(string sql)
        {
            string result = "";
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    result = reader.GetString(0);
                }
                catch (Exception)
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

        /// <summary>
        /// 批量事务
        /// </summary>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        private static bool Transaction(List<string> sqlList)
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
                Wpf.Helper.DebugOnly.Output("异常:" + e.Message);
                Wpf.Helper.DebugOnly.Output(sqlList[1].ToString());
            }
            return flag;
        }

        public static bool Log(string title, string content, string remark, string type)
        {
            List<string> sqls = new List<string>();
            sqls.Add(new Wpf.ViewModel.ViewModel_操作记录().InsertLog(title, content, remark, type));
            return Transaction(sqls);
        }
    }
}
