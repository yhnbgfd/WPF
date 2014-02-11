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
            sw.Write(DateTime.Now +" || "+ log + "\n");
            //清空缓冲区  
            sw.Flush();
            //关闭流  
            sw.Close();
            fs.Close();
        }
    }
}
