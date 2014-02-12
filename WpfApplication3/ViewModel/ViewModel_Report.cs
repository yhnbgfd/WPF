using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using Wpf.Model;
using System.Data;

namespace Wpf.ViewModel
{
    public class ViewModel_Report
    {
        StringBuilder sql = new StringBuilder();
        double surplus = 0;
        double lastSurplus = 0;

        public ViewModel_Report()
        {
            sql.Append("SELECT * FROM T_Report ");
        }

        private List<Model_Report> Report()
        {
            int id = 1;//序号
            sql.Append(" ORDER BY DateTime ASC");
            DataSet data = new Wpf.Data.Database().Select(sql.ToString());
            var _report = new List<Model_Report>();
            Properties.Settings.Default.借方发生额累计 = 0;
            Properties.Settings.Default.贷方发生额累计 = 0;
            if (data.Tables[0].Rows.Count == 0)
            {
                _report.Add(new Model_Report());
            }
            else
            {
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    lastSurplus += ((double)dr[4] - (double)dr[5]);
                    Properties.Settings.Default.借方发生额累计 += (double)dr[4];
                    Properties.Settings.Default.贷方发生额累计 += (double)dr[5];
                    _report.Add(new Model_Report
                    {
                        Dbid = (long)dr[0],
                        序号 = id++,
                        //日期 = (dr[1].ToString() != "") ? new Wpf.Helper.Date().Format(dr[1].ToString()) : "",
                        月 = (dr[1].ToString() != "") ? new Wpf.Helper.Date().FormatMonth(dr[1].ToString()) : "",
                        日 = (dr[1].ToString() != "") ? new Wpf.Helper.Date().FormatDay(dr[1].ToString()) : "",
                        单位名称 = dr[2].ToString(),
                        用途 = dr[3].ToString(),
                        借方发生额 = (double)dr[4],
                        贷方发生额 = (double)dr[5],
                        结余 = lastSurplus
                    });
                }
            }
            return _report;
        }

        public List<Model_Report> Report(int type, int year, int month)
        {
            string date = new Wpf.Helper.Date().Format(year + "-" + month + "-1");
            string nextdate = new Wpf.Helper.Date().Format(year + "-" + (month + 1) + "-1");
            if(year == 0)
            {
                sql.Append(" WHERE type=" + type);
            }
            else if(month == 0)
            {
                date = new Wpf.Helper.Date().Format(year + "-01-01");
                nextdate = new Wpf.Helper.Date().Format((year + 1) + "-01-01");
                sql.Append(" WHERE datetime BETWEEN '" + date + "' AND datetime('" + nextdate + "','-1 second')");
                if (type != 0)
                {
                    sql.Append(" AND type=" + type);
                }
            }
            else
            {
                sql.Append(" WHERE datetime BETWEEN '" + date + "' AND datetime('" + nextdate + "','-1 second')");
                if (type != 0)
                {
                    sql.Append(" AND type=" + type);
                }
            }
            
            GetSurplus(year, month, type);
            return Report();
        }

        /// <summary>
        /// 获取上月结余
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public double GetSurplus(int year, int month, int type)
        {
            string sql = "";
            if(month == 0)//全部月
            {
                sql = "select surplus from T_Surplus where year=" + year + " and type=" + type;
            }
            else
            {
                sql = "select surplus from T_Surplus where year=" + year + " and month=" + (month-1) + " and type=" + type;
            }
            surplus = new Wpf.Data.Database().SelectSurplus(sql);
            lastSurplus = surplus;
            return surplus;
        }

        /// <summary>
        /// 检查是否有数据供写入结余值，没有则插入0结余的数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void CheckSurplus(int year, int month)
        {
            string sql = "SELECT count(*) from T_Surplus where year=" + year + " and month=" + month;
            if (month == 0 || year == 0)
            {
                return;
            }
            if (new Wpf.Data.Database().SelectCount(sql) == 0)
            {
                for (int i = 1; i <= 5; i++ )
                {
                    sql = "insert into T_Surplus(year,month,surplus,type) values(" + year + "," + month + ",0," + i + ")";
                    new Wpf.Data.Database().Insert(sql);
                }
            }
        }

    }
}
