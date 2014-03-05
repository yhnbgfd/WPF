using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wpf.Helper;

namespace Wpf.ViewModel
{
    class ViewModel_Surplus
    {
        /// <summary>
        /// 如果T_Surplus没特定的年月条目，则插入该年月5个type5条数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public void InsertSurplus(int year, int month)
        {
            List<string> sqls = new List<string>();
            string sql = "";
            for (int i = 1; i <= 10; i++)
            {
                sql = "insert into T_Surplus(year,month,surplus,type) values(" + year + "," + month + ",0," + i + ")";
                sqls.Add(sql);
            }
            Wpf.Data.Database.doDMLs(sqls,"Insert","NewSurplus");
        }

        /// <summary>
        /// 统计借方发生额累计（当年到当前月）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public decimal Count借方发生额累计(int type, int year, int month)
        {
            string sql = "SELECT total(income) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format(year + "-" + (month + 1) + "-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";

            if (month == 0)
            {
                sql = "SELECT total(income) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format((year + 1) + "-01-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";
            }
            decimal result = 0m;
            result = Wpf.Data.Database.CountDecimal(sql);
            return result;
        }

        /// <summary>
        /// 统计贷方发生额累计（当年到当前月）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public decimal Count贷方发生额累计(int type, int year, int month)
        {
            string sql = "SELECT total(expenses) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format(year + "-" + (month + 1) + "-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";
            if (month == 0)
            {
                sql = "SELECT total(expenses) from T_Report "
                + " WHERE T_Report.DateTime IS NOT NULL "
                + " AND type=" + type + " "
                + " AND T_Report.DateTime BETWEEN  '" + year + "-01-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format((year + 1) + "-01-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";
            }
            decimal result = 0m;
            result = Wpf.Data.Database.CountDecimal(sql);
            return result;
        }

        public decimal Count借方发生额合计(int type, int year, int month)
        {
            decimal result = 0m;
            string monthstr = "0"; 
            string NextMonthstr = "0";
            if(month<10)
            {
                monthstr += month;
                NextMonthstr += (month + 1);
                if (month == 9)
                {
                    NextMonthstr = "10";
                }
            }
            string sql = "select total(income) from T_Report "
                + " where type="+type
                + " AND DateTime BETWEEN  '" + year + "-" + monthstr + "-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format(year + "-" + NextMonthstr + "-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";
            //DebugOnly.Output(sql);
            result = Wpf.Data.Database.CountDecimal(sql);
            return result;
        }
        public decimal Count贷方发生额合计(int type, int year, int month)
        {
            decimal result = 0m;
            string monthstr = "0";
            string NextMonthstr = "0";
            if (month < 10)
            {
                monthstr += month;
                NextMonthstr += (month + 1);
                if (month == 9)
                {
                    NextMonthstr = "10";
                }
            }
            string sql = "select total(expenses) from T_Report "
                + " where type=" + type
                + " AND DateTime BETWEEN  '" + year + "-" + monthstr + "-01' "
                + " AND datetime('" + Wpf.Helper.Date.Format(year + "-" + NextMonthstr + "-01") + "','-1 second')"
                + " AND DeleteTime IS NULL";
            //DebugOnly.Output(sql);
            result = Wpf.Data.Database.CountDecimal(sql);
            return result;
        }

        /// <summary>
        /// 将设置里的初始金额初始化到数据库
        /// </summary>
        public void InitSurplus()
        {
            List<string> sqls = new List<string>();
            sqls.Add("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_预算内户 + " WHERE TYPE=1");
            sqls.Add("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_预算外户 + " WHERE TYPE=2");
            sqls.Add("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_周转金户 + " WHERE TYPE=3");
            sqls.Add("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_计生专户 + " WHERE TYPE=4");
            sqls.Add("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_政粮补贴资金专户 + " WHERE TYPE=5");
            sqls.Add("UPDATE T_Surplus SET surplus=" + Properties.Settings.Default.初始金额_土地户 + " WHERE TYPE=6");



            Wpf.Data.Database.doDMLs(sqls, "Update", "Init初始金额");
        }
    }
}
