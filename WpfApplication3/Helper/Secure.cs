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
        public static void RegisterLog()
        {
            FileStream fs = new FileStream(Properties.Settings.Default.Path + "Logs\\RegisterLog.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            StringBuilder Information = new StringBuilder();
            Information.Append("注册时间：" + Properties.Settings.Default.注册时间+"\n");
            Information.Append("注册码："+Properties.Settings.Default.注册码+"\n");
            sw.Write(Information.ToString());
            sw.Flush();//清空缓冲区  
            sw.Close();//关闭流  
            fs.Close();
        }

        /// <summary>
        /// 注册程序
        /// </summary>
        private static void Register()
        {
            string time = DateTime.Now.ToString();
            string License = Wpf.Helper.Secure.GetMD5_32(time + " Power By StoneAnt");

            Properties.Settings.Default.注册时间 = time;
            Properties.Settings.Default.注册码 = License;
            Properties.Settings.Default.初始化程序 = true;
            Wpf.Helper.XmlHelper xml = new Helper.XmlHelper();
            xml.LoadXML();
            xml.WriteXML("时间", time);
            xml.WriteXML("注册码", License);
            xml.SaveXML();
            Wpf.Data.Database.doDML("Insert into T_Type(key,value) values('997','" + time + "')", "Update", "UpdateLicense");
            Wpf.Data.Database.doDML("UPDATE T_Type set value='" + License + "' where key=998", "Update", "UpdateLicense");
            Wpf.Data.Database.doDML("Insert into T_Type(key,value) values('999','false')", "Update", "UpdateLicense");
            Wpf.Helper.Secure.RegisterLog();
#if (!DEBUG)
            Wpf.Data.Database.ChangePassword(Wpf.Helper.Secure.GetMD5_32(License + "PowerByStoneAnt"));
#endif
        }

        /// <summary>
        /// 系统自检、正版检查
        /// 第一次开打软件会隐藏注册，防止拷贝
        /// 数据库如果打不开，启动时就出错了。轮不到自检。
        /// </summary>
        public static void SystemCheck()
        {
            Wpf.Helper.FileSystemCheck.CheckFolder();
            if (!Properties.Settings.Default.初始化程序)
            {
                string sql = "select value from T_Type where key=998";
                if (Wpf.Data.Database.SelectString(sql) == "md5")
                {
                    Register();
                }
            }
        }

        /// <summary>
        /// 试用期检查,大于15则过试用期
        /// </summary>
        /// <returns></returns>
        public static int CheckLicense()
        {
            string sql = "select value from T_Type where key=998";
            if (Wpf.Data.Database.SelectString(sql) != "md5")
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
