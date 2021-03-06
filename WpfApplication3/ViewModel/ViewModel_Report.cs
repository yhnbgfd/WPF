﻿using System;
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
        private int cb_year = 0;
        private int cb_month = 0;

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
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                surplus += ((decimal)dr[4] - (decimal)dr[5]);
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
            //更新承上月结余=这个月最后一天的结余
            Wpf.Data.Database.doDML("update T_Surplus set surplus=" + surplus 
                + " where year=" + cb_year
                + " and month=" + cb_month
                + " and type=" + type, "Update", "Update承上月结余");
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
            this.cb_year = year;
            this.cb_month = month;
            string date;
            string nextdate;
            if(year == 0)//年=0即查看所有该type的结果
            {
                sql.Append(" WHERE type=" + type);
            }
            else if(month == 0)//月=0即查看该type当年所有结果
            {
                date = year + "-01-01";
                nextdate = (year + 1) + "-01-01";
                sql.Append(" WHERE datetime BETWEEN '" + date + "' AND datetime('" + nextdate + "','-1 second')");
                sql.Append(" AND type=" + type);
            }
            else//查看该type该年该月结果
            {
                date = Wpf.Helper.Date.Format(year + "-" + month + "-1");
                nextdate = Wpf.Helper.Date.Format(year + "-" + (month + 1) + "-1");
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
            if (month == 0 || year == 0)
            {
                return;
            }
            string sql2010 = "SELECT count(*) from T_Surplus where year=2010 and month=12";
            if (Wpf.Data.Database.SelectCount(sql2010) == 0)
            {
                new Wpf.ViewModel.ViewModel_Surplus().InsertSurplus(2010, 12);
            }
            string sql = "SELECT count(*) from T_Surplus where year=" + year + " and month=" + month;
            if (Wpf.Data.Database.SelectCount(sql) == 0)
            {
                new Wpf.ViewModel.ViewModel_Surplus().InsertSurplus(year, month);
            }
        }

    }
}
