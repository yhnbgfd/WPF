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
                dt = DateTime.Parse(ThirteenMonth(time));
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
                dt = DateTime.Parse(ThirteenMonth(time));
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
                dt = DateTime.Parse(ThirteenMonth(time));
            }
            string format = "dd";
            return dt.ToString(format);
        }

        private string ThirteenMonth(string time)
        {
            string[] date = time.Split('-');
            if(date[1].Equals("13"))
            {
                date[0] = (int.Parse(date[0]) + 1).ToString();
                date[1] = "01";
            }
            return date[0]+"-"+date[1]+"-"+date[2];
        }
    }
}
