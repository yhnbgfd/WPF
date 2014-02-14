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

        //public static string Format(DateTime time)
        //{
        //    string format = "yyyy-MM-dd";    // Use this format
        //    return time.ToString(format);
        //}

        /// <summary>
        /// 格式化时间 “2014-02-14”
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string Format(string time)
        {
            string[] date = time.Split('-');
            if(date[1].Equals("13"))//月==13，则加一年
            {
                date[0] = (int.Parse(date[0]) + 1).ToString();
                date[1] = "01";
                date[2] = "01";
            }
            else
            {
                date[1] = FormatMonth(time);
                date[2] = FormatDay(time);
            }
            return date[0] +"-"+ date[1] +"-"+ date[2];
        }

        /// <summary>
        /// 返回月份
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatMonth(string time)
        {
            string month = time.Split('-')[1];
            if(month.Length == 1)
            {
                month = "0" + month;
            }
            return month;
        }

        /// <summary>
        /// 返回日
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatDay(string time)
        {
            string day = time.Split('-')[2];
            if(day.Length == 1)
            {
                day = "0" + day;
            }
            return day;
        }

        //private static string ThirteenMonth(string time)
        //{
        //    string[] date = time.Split('-');
        //    if(date[1].Equals("13"))
        //    {
        //        date[0] = (int.Parse(date[0]) + 1).ToString();
        //        date[1] = "01";
        //    }
        //    return date[0]+"-"+date[1]+"-"+date[2];
        //}

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
