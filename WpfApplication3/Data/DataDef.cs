using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wpf.Helper;

namespace Wpf.Data
{
    public static class DataDef
    {
        public static bool isDBconnect = false;
        public static string SignInUserName = "";
        public static string DbPassword = Wpf.Helper.Secure.GetMD5_32("PowerByStoneAntasdasd");
        public static XmlHelper xmh = new XmlHelper();

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
            xmh.ReadXML("Tag1"),
            xmh.ReadXML("Tag2"),
            xmh.ReadXML("Tag3"),
            xmh.ReadXML("Tag4"),
            xmh.ReadXML("Tag5"),
            xmh.ReadXML("Tag6")
            //{"预算内户"},
            //{"预算外户"},
            //{"周转金户"},
            //{"计生专户"},
            //{"政粮补贴资金专户"},
            //{"土地户"}
        };

        public static List<string> ChineseMonth = new List<string>
        {
            {"一月"},
            {"二月"},
            {"三月"},
            {"四月"},
            {"五月"},
            {"六月"},
            {"七月"},
            {"八月"},
            {"九月"},
            {"十月"},
            {"十一月"},
            {"十二月"}
        };

        public static List<string> Month = new List<string>();
        public static List<string> Year = new List<string>();
        public static int perYear = 3, afterYear = 5;

        static DataDef()
        {
            Month.Add("全部");
            for (int i = 1; i <= 12;i++ )
            {
                Month.Add(i.ToString());
            }
            Year.Add("全部");
            for (int i = Wpf.Helper.Date.GetYear() - perYear; i < Wpf.Helper.Date.GetYear() + afterYear; i++)
            {
                Year.Add(i.ToString());
            }
        }
    }
}
