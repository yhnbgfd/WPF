using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Wpf.Helper
{
    static class FileSystemCheck
    {
        private static string BasePath = AppDomain.CurrentDomain.BaseDirectory;

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
            return true;
        }
    }
}
