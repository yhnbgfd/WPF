using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wpf.Helper
{
    public static class Date
    {
        public static int GetYear()
        {
            return DateTime.Now.Year;
        }

        public static int GetMonth()
        {
            return DateTime.Now.Month;
        }

        public static string Format(DateTime time)
        {
            string format = "yyyy-MM-dd";    // Use this format
            return time.ToString(format);
        }

        public static string Format(string time)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.Parse(time);
            }
            catch(Exception)
            {
                new Wpf.Helper.Log().SaveLog("Format: DateTime Exception :'" + time+"'");
                dt = DateTime.Parse(ThirteenMonth(time));
            }
            return Format(dt);
        }

        public static string FormatMonth(string time)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.Parse(time);
            }
            catch (Exception)
            {
                new Wpf.Helper.Log().SaveLog("Format: DateTime Exception :'" + time + "'");
                dt = DateTime.Parse(ThirteenMonth(time));
            }
            string format = "MM";
            return dt.ToString(format);
        }

        public static string FormatDay(string time)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.Parse(time);
            }
            catch (Exception)
            {
                new Wpf.Helper.Log().SaveLog("Format: DateTime Exception :'" + time + "'");
                dt = DateTime.Parse(ThirteenMonth(time));
            }
            string format = "dd";
            return dt.ToString(format);
        }

        private static string ThirteenMonth(string time)
        {
            string[] date = time.Split('-');
            if(date[1].Equals("13"))
            {
                date[0] = (int.Parse(date[0]) + 1).ToString();
                date[1] = "01";
            }
            return date[0]+"-"+date[1]+"-"+date[2];
        }

        /// <summary>
        /// 验证字符串是否为日
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsStringOfDay(string input)
        {
            if (Regex.Match(input, @"^[0-3]{0,1}[0-9]{1,1}$").Success)
            {
                Console.WriteLine("true");
                return true;
            }
            Console.WriteLine("false");
            return false;
        }
        /// <summary>
        /// 验证字符串是否为浮点数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsStringOfDouble(string input)
        {
            if (Regex.Match(input, @"^[0-9]+\.?[0-9]*$").Success)
            {
                Console.WriteLine("true");
                return true;
            }
            Console.WriteLine("false");
            return false;
        }
    }
}
