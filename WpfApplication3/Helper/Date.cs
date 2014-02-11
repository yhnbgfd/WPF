using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Helper
{
    public class Date
    {
        public int GetYear()
        {
            return DateTime.Now.Year;
        }

        public int GetMonth()
        {
            return DateTime.Now.Month;
        }

        public string Format(DateTime time)
        {
            string format = "yyyy-MM-dd";    // Use this format
            return time.ToString(format);
        }

        public string Format(string time)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.Parse(time);
            }
            catch(Exception)
            {
                new Wpf.Helper.Log().SaveLog("Format: DateTime Exception :'" + time+"'");
                return "Exception";
            }
            return Format(dt);
        }

        public string FormatMonth(string time)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.Parse(time);
            }
            catch (Exception)
            {
                new Wpf.Helper.Log().SaveLog("Format: DateTime Exception :'" + time + "'");
                return "Exception";
            }
            string format = "MM";
            return dt.ToString(format);
        }

        public string FormatDay(string time)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.Parse(time);
            }
            catch (Exception)
            {
                new Wpf.Helper.Log().SaveLog("Format: DateTime Exception :'" + time + "'");
                return "Exception";
            }
            string format = "dd";
            return dt.ToString(format);
        }
    }
}
