using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Wpf.Helper
{
    public class Log
    {
        string path = Properties.Settings.Default.Path;
        public void SaveLog(string log)
        {
            FileStream fs = new FileStream(path + "Log.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            log = DateTime.Now + " || " + log + "\n";
            sw.Write(log);
            sw.Flush();//清空缓冲区  
            sw.Close();//关闭流  
            fs.Close();
        }

        private void SaveDB(string log)
        {
            string sql = "";
        }
    }
}
