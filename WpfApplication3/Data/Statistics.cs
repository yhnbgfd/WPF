using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Data
{
    class Statistics
    {
        public void UpdateSurplus(int year, int month, int type)
        {
            if(year == 0 || month == 0)
            {
                return;
            }
            string startdate = Wpf.Helper.Date.Format(year+"-"+month+"-01");
            string enddate = Wpf.Helper.Date.Format(year + "-" + (month+1) + "-01");
            string SelectSql = "SELECT SUM(income)-SUM(expenses) from T_Report "
                + " WHERE datetime BETWEEN '" + startdate + "' AND datetime('" + enddate + "','-1 second')  "
                +" AND type="+type;
            Console.WriteLine(SelectSql);
            double SelectResult = Wpf.Data.Database.SelectSurplus(SelectSql); ;
            string UpdateSql = "UPDATE T_Surplus SET surplus="+SelectResult
                +" WHERE year="+year
                +" AND month="+month
                +" AND type="+type;
            Wpf.Data.Database.Update(UpdateSql);
        }
    }
}
