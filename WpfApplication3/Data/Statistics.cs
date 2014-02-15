using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Data
{
    class Statistics
    {
        /// <summary>
        /// 刷新结余
        /// 已处理年、月=0
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="type"></param>
        public void UpdateSurplus(int year, int month, int type)
        {
            if(year == 0 || month == 0)
            {
                return;
            }
            string startdate = Wpf.Helper.Date.Format(year+"-"+month+"-01");
            string enddate = Wpf.Helper.Date.Format(year + "-" + (month+1) + "-01");
            string SelectSql = "SELECT total(income)-total(expenses) from T_Report "
                + " WHERE datetime BETWEEN '" + startdate + "' AND datetime('" + enddate + "','-1 second')  "
                + " AND type="+type;
            decimal SelectResult = Wpf.Data.Database.SelectSurplus(SelectSql); ;
            string UpdateSql = "UPDATE T_Surplus SET surplus="+SelectResult
                + " WHERE year="+year
                + " AND month="+month
                + " AND type="+type;
            Wpf.Data.Database.Update(UpdateSql);
        }
    }
}
