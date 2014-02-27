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

        public static string FormatDate(string datestring)
        {
            DateTime dt = Convert.ToDateTime(datestring);
            string format = "yyyy-MM-dd";
            return dt.ToString(format);
        }

        /// <summary>
        /// 格式化时间 “2014-02-14”
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string Format(string time)
        {
            time = time.Split(' ')[0];//如果有时分秒，只取年月日
            string[] date = time.Split(new char[2]{'-','/'});
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
        /// 年-月-日 时:分:秒 格式化当前时间
        /// </summary>
        /// <returns></returns>
        public static string FormatNow()
        {
            DateTime now = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
            return now.ToString(format);
        }

        /// <summary>
        /// 返回月份
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatMonth(string time)
        {
            time = time.Split(' ')[0];
            string month = time.Split(new char[2] { '-', '/' })[1];
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
            time = time.Split(' ')[0];
            string day = time.Split(new char[2] { '-', '/' })[2];
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
                return true;
            }
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
                return true;
            }
            return false;
        }
    }
}
