using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.ViewModel
{
    class RefreshSurplus
    {
        /// <summary>
        /// 
        /// </summary>
        public void RefreshMonth()
        {
            int year = Properties.Settings.Default.下拉框_年;
            List<string> sqls = new List<string>();
            for (int i = 1; i < 13; i++ )
            {
                for (int j = 1; j < 11; j++ )
                {
                    if(i==1)
                    {
                        sqls.Add("UPDATE T_Surplus "
                                + " set surplus = (select total(surplus) from T_Surplus where year=" + (year - 1) + " and month=12 and type=" + j + ") "
                                + " where year=" + year + " and month=1 and type=" + j);
                    }
                    else
                    {
                        sqls.Add("UPDATE T_Surplus "
                                + " set surplus = (select total(surplus) from T_Surplus where year=" + year + " and month=" + (i - 1) + " and type=" + j + ") "
                                + " where year=" + year + " and month=" + i + " and type=" + j);
                    }
                }
            }
            Wpf.Data.Database.Transaction(sqls);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshYear()
        {

        }
    }
}
