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
            {"income","收入"},
            {"expenses","支出"}
        };
        
    }
}
