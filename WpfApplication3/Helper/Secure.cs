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
        /// 注册程序
        /// </summary>
        private static void Register()
        {
            Wpf.Helper.XmlHelper xml = new Helper.XmlHelper();
            xml.WriteXML("注册时间", DateTime.Now.ToString());
            xml.WriteXML("注册码", Wpf.Helper.Secure.GetMD5_32("StoneAnt"));
            //997:第一次打开的时间
            //998:根据时间算出来的序列号
            //999:是否注册
            if (Wpf.Data.Database.SelectCount("select count(*) from T_Type where key=997") == 0)
            {
                Wpf.Data.Database.doDML("Insert into T_Type(key,value) values('997','"+DateTime.Now+"')", "Insert", "UpdateLicense");
            }
            if (Wpf.Data.Database.SelectCount("select count(*) from T_Type where key=999") == 0)
            {
                Wpf.Data.Database.doDML("Insert into T_Type(key,value) values('999','false')", "Insert", "UpdateLicense");
            }
#if (!DEBUG)
            Wpf.Data.Database.ChangePassword(Wpf.Helper.Secure.GetMD5_32("PowerByStoneAntasdasd"));
#endif
        }

        public static void SystemReg()
        {
            Wpf.Helper.XmlHelper xml = new Helper.XmlHelper();
            if (xml.ReadXML("注册码") == "")
            {
                Register();
            }
        }

        /// <summary>
        /// 试用期检查,大于15则过试用期
        /// </summary>
        /// <returns></returns>
        public static int CheckLicense()
        {
            string sql = "select value from T_Type where key=999";
            if (Wpf.Data.Database.SelectString(sql) == "false")
            {
                DateTime RegisterDate = Convert.ToDateTime(Wpf.Helper.Date.Format(Wpf.Data.Database.SelectString("select value from T_Type where key=997")));
                DateTime now = DateTime.Now;
                TimeSpan ts = now.Subtract(RegisterDate);
                return 15 - ts.Days;
            }
            return 15;
        }
    }
}
