using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Wpf.Helper
{
    static class Log
    {
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "Logs\\";
        public static void SaveLog(string log)
        {
            FileStream fs = new FileStream(path + "Log.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            log = DateTime.Now + " || " + log + "\n";
            sw.Write(log);
            sw.Flush();//清空缓冲区  
            sw.Close();//关闭流  
            fs.Close();
        }

        public static void ErrorLog(string log)
        {
            FileStream fs = new FileStream(path + "Error.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            log = DateTime.Now + " || " + log + "\n";
            sw.Write(log);
            sw.Flush();//清空缓冲区  
            sw.Close();//关闭流  
            fs.Close();
        }

        public static void DBLog(string log)
        {
            FileStream fs = new FileStream(path + "DB.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            log = DateTime.Now + " || " + log + "\n";
            sw.Write(log);
            sw.Flush();//清空缓冲区  
            sw.Close();//关闭流  
            fs.Close();
        }

    }
}
