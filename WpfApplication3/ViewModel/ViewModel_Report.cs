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

        double 借方发生额累计 = 0;
        double 贷方发生额累计 = 0;
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
                借方发生额累计 = 0;
                贷方发生额累计 = 0;
                foreach (DataRow dr in data.Tables[0].Rows)
                {
                    lastSurplus += ((double)dr[4] - (double)dr[5]);
                    借方发生额累计 += (double)dr[4];
                    贷方发生额累计 += (double)dr[5];
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
            string date;
            string nextdate;
            date = new Wpf.Helper.Date().Format(year + "-" + month + "-1");
            nextdate = new Wpf.Helper.Date().Format(year + "-" + (month + 1) + "-1");
            sql.Append(" WHERE datetime BETWEEN '" + date + "' AND datetime('" + nextdate + "','-1 second')");

            if (type != 0)
            {
                sql.Append(" AND type=" + type);
            }
            GetSurplus(year, month);
            return Report();
        }

        public double GetSurplus(int year, int month)
        {
            string sql = "select surplus from T_Surplus where year="+year+" and month="+month;
            surplus = new Wpf.Data.Database().SelectSurplus(sql);
            lastSurplus = surplus;
            return surplus;
        }

        public int CheckSurplus(int year, int month)
        {
            int result = 0;
            string sql = "SELECT count(*) from T_Surplus where year=" + year + " and month=" + month;
            result = new Wpf.Data.Database().SelectCount(sql);
            if(result == 0)
            {
                sql = "insert into T_Surplus(year,month,surplus) values("+year+","+month+",0)";
                new Wpf.Data.Database().Insert(sql);
            }
            return result;
        }

        public double[] GetAccumulative()
        {
            double[] result = new double[2];
            result[0] = 借方发生额累计;
            result[1] = 贷方发生额累计;
            return result;
        }
    }
}
