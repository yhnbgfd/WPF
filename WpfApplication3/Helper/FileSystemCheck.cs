using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Wpf.Helper
{
    static class FileSystemCheck
    {
        private static string BasePath = Properties.Settings.Default.Path;

        /// <summary>
        /// 检查软件的关键文件夹跟文件
        /// </summary>
        /// <returns></returns>
        public static bool CheckFolder()
        {
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            if (!Directory.Exists("ExcelOutput"))
            {
                Directory.CreateDirectory("ExcelOutput");
            }
            //if (Directory.Exists("Data"))//数据库文件夹存在，则检查数据库文件
            //{
            //    if(!CheckFile(Properties.Settings.Default.DataSource))
            //    {
            //        Wpf.Helper.Log.ErrorLog("！！！！数据库文件不存在 ！！！！");
            //        return false;
            //    }
            //}
            //else
            //{
            //    Wpf.Helper.Log.ErrorLog("！！！！数据库文件夹不存在 ！！！！");
            //    return false;
            //}
            return true;
        }

        private static bool CheckFile(string FileName)
        {
            if (File.Exists(FileName))//数据库文件存在
            {
                return true;
            }
            return false;
        }
    }
}
