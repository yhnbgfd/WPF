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
        decimal surplus = 0m;
        private int type = 0;

        public ViewModel_Report()
        {
            sql.Append("SELECT * FROM T_Report ");
        }

        private List<Model_Report> Report()
        {
            int id = 1;//序号
            sql.Append(" AND DeleteTime IS NULL ORDER BY DateTime ASC");
            DataSet data = Wpf.Data.Database.Select(sql.ToString());
            var _report = new List<Model_Report>();
            Properties.Settings.Default.借方发生额合计 = 0;
            Properties.Settings.Default.贷方发生额合计 = 0;
            //if (data.Tables[0].Rows.Count == 0)//无结果
            //{
            //    _report.Add(new Model_Report());
            //}
            //else
            {
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    surplus += ((decimal)dr[4] - (decimal)dr[5]);
                    Properties.Settings.Default.借方发生额合计 += (decimal)dr[4];
                    Properties.Settings.Default.贷方发生额合计 += (decimal)dr[5];
                    _report.Add(new Model_Report
                    {
                        Dbid = (long)dr[0],
                        序号 = id++,
                        月 = int.Parse(Wpf.Helper.Date.FormatMonth(dr[1].ToString())),
                        日 = int.Parse(Wpf.Helper.Date.FormatDay(dr[1].ToString())),
                        单位名称 = dr[2].ToString(),
                        用途 = dr[3].ToString(),
                        借方发生额 = (decimal)dr[4],
                        贷方发生额 = (decimal)dr[5],
                        结余 = surplus
                    });
                }
            }
            //更新承上月结余=这个月最后一天的结余
            Wpf.Data.Database.Update("update T_Surplus set surplus=" + surplus 
                + " where year="+Properties.Settings.Default.下拉框_年
                + " and month=" + Properties.Settings.Default.下拉框_月
                + " and type=" + type);
            Wpf.Data.Database.Count借方发生额累计(type, Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            Wpf.Data.Database.Count贷方发生额累计(type, Properties.Settings.Default.下拉框_年, Properties.Settings.Default.下拉框_月);
            return _report;
        }

        /// <summary>
        /// 获取表的数据
        /// 已处理年、月=0
        /// </summary>
        /// <param name="type"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public List<Model_Report> Report(int inputtype, int year, int month)
        {
            this.type = inputtype;
            string date;
            string nextdate;
            if(year == 0)//年=0即查看所有该type的结果
            {
                sql.Append(" WHERE type=" + type);
            }
            else if(month == 0)//月=0即查看该type当年所有结果
            {
                date = Wpf.Helper.Date.Format(year + "-01-01");
                nextdate = Wpf.Helper.Date.Format((year + 1) + "-01-01");
                sql.Append(" WHERE datetime BETWEEN '" + date + "' AND datetime('" + nextdate + "','-1 second')");
                sql.Append(" AND type=" + type);
            }
            else//查看该type该年该月结果
            {
                date = Wpf.Helper.Date.Format(year + "-" + month + "-1");
                nextdate = Wpf.Helper.Date.Format(year + "-" + (month + 1) + "-1");//有13月的问题
                sql.Append(" WHERE datetime BETWEEN '" + date + "' AND datetime('" + nextdate + "','-1 second')");
                sql.Append(" AND type=" + type);
            }
            surplus = GetSurplus(year, month, type);
            return Report();
        }

        /// <summary>
        /// 获取月结余
        /// 将上月结余赋值到承上月结余用于这个月结余数据的初始数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public decimal GetSurplus(int year, int month, int type)
        {
            string sql = "";
            if (year == 0)
            {
                return 0;
            }
            else if (month == 0 || month == 1)//全部月
            {
                sql = "select surplus from T_Surplus where year=" + (year - 1) + " and month=12 and type=" + type;
            }
            //else if(month == 1)//
            //{
            //    sql = "select surplus from T_Surplus where year=" + (year-1) + " and month=12 and type=" + type;
            //}
            else
            {
                sql = "select surplus from T_Surplus where year=" + year + " and month=" + (month-1) + " and type=" + type;
            }
            return Wpf.Data.Database.SelectSurplus(sql);
        }

        /// <summary>
        /// 检查是否有数据供写入结余值，没有则插入0结余的surplus表数据项
        /// 已处理年、月=0
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
            if (Wpf.Data.Database.SelectCount(sql) == 0)
            {
                Wpf.Data.Database.InsertSurplus(year, month);
            }
        }

    }
}
