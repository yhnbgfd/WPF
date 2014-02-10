using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Data
{
    public static class DataDef
    {
        public static Dictionary<string, string> dict = new Dictionary<string, string>
        {
            {"id","DBId"},
            {"datetime","日期"},
            {"unitsname","单位名称"},
            {"use","用途"},
            {"income","借方发生额"},
            {"expenses","贷方发生额"}
        };

        public static List<string> CustomerType = new List<string>
        {
            {"全部"},    
            {"预算内户"},
            {"预算外户"},
            {"周转金户"},
            {"计生专户"},
            {"政粮补贴资金专户"}
        };

        public static List<object> Month = new List<object>();

        static DataDef()
        {
            Month.Add("全年");
            for (int i = 1; i <= 12;i++ )
            {
                Month.Add(i);
            }
        }
    }
}
