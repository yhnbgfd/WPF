using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace Wpf.Helper
{
    static class Secure
    {
        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckUserNameAndPassword(string username, SecureString password)
        {
            IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(password);
            string passwordstr = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            string sql = "Select count(*) from T_User "
                + " where username='" + username + "' and password='" + passwordstr + "' and status=1";
            if(Wpf.Data.Database.SelectCount(sql) == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将密码翻译成明文string
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string TranslatePassword(SecureString password)
        {
            IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(password);
            string passwordstr = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            return passwordstr;
        }

        /// <summary>
        /// 计算字符串的32位md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5_32(string str)
        {
            string result = "";
            byte[] data = Encoding.GetEncoding("utf-8").GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(data);
            for (int i = 0; i < bytes.Length; i++)
            {
                result += bytes[i].ToString("x2");
            }
            return result;
        }

        public static string GetFileMD5(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = md5.ComputeHash(file);
            string str = System.BitConverter.ToString(hash_byte);
            str = str.Replace("-", "");
            return str;
        }
        /// <summary>
        /// 保存注册信息到文件
        /// </summary>
        public static void RegistrationInformationFile()
        {
            FileStream fs = new FileStream(Properties.Settings.Default.Path + "Registration Information.key", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            StringBuilder Information = new StringBuilder();
            Information.Append("注册时间：" + Properties.Settings.Default.注册时间+"\n");
            Information.Append("注册码："+Properties.Settings.Default.注册码+"\n");
            sw.Write(Information.ToString());
            sw.Flush();//清空缓冲区  
            sw.Close();//关闭流  
            fs.Close();
        }
    }
}
