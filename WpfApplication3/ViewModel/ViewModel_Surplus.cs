using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Wpf.Data.Database.Transaction(sqls);
        }
    }
}
