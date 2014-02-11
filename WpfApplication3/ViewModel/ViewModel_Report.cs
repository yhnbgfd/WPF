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
            if (data.Tables[0].Rows.Count == 0)
            {
                _report.Add(new Model_Report());
            }
            else
            {
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    _report.Add(new Model_Report
                    {
                        Dbid = (long)dr[0],
                        序号 = id++,
                        日期 = (dr[1].ToString() != "") ? new Wpf.Helper.Date().Format(dr[1].ToString()) : "",
                        单位名称 = dr[2].ToString(),
                        用途 = dr[3].ToString(),
                        借方发生额 = (double)dr[4],
                        贷方发生额 = (double)dr[5],
                        结余 = (double)dr[4] - (double)dr[5]
                    }
                    );
                }
            }
            return _report;
        }

        public List<Model_Report> Report(int type, int year, int month)
        {
            if (year == 0)//全部年份
            {
                if (type != 0)
                {
                    sql.Append(" WHERE type=" + type);
                }
                return Report();
            }

            string date;
            string nextdate;
            if (month == 0)
            {
                date = new Wpf.Helper.Date().Format(year + "-01-01");
                nextdate = new Wpf.Helper.Date().Format((year + 1) + "-01-01");
            }
            else
            {
                date = new Wpf.Helper.Date().Format(year + "-" + month + "-1");
                nextdate = new Wpf.Helper.Date().Format(year + "-" + (month + 1) + "-1");
            }
            sql.Append(" WHERE datetime BETWEEN '" + date + "' AND datetime('" + nextdate + "','-1 second')");

            if (type != 0)
            {
                sql.Append(" AND type=" + type);
            }
            return Report();
        }
    }
}
