using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Data
{
    public static class DataDef
    {
        public static bool isDBconnect = false;

        public static Dictionary<string, string> dict = new Dictionary<string, string>
        {
            {"id","DBId"},
            {"month","月"},
            {"day","日"},
            {"unitsname","单位名称"},
            {"use","用途"},
            {"income","借方发生额"},
            {"expenses","贷方发生额"}
        };

        public static List<string> CustomerType = new List<string>
        {
            {"预算内户"},
            {"预算外户"},
            {"周转金户"},
            {"计生专户"},
            {"政粮补贴资金专户"}
        };

        public static List<object> Month = new List<object>();
        public static List<object> Year = new List<object>();
        public static int perYear = 3, afterYear = 5;

        static DataDef()
        {
            Month.Add("全部");
            for (int i = 1; i <= 12;i++ )
            {
                Month.Add(i);
            }
            Year.Add("全部");
            for (int i = Wpf.Helper.Date.GetYear() - perYear; i < Wpf.Helper.Date.GetYear() + afterYear; i++)
            {
                Year.Add(i);
            }
        }
    }
}
