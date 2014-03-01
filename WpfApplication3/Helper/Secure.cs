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

        public static void Save初始金额()
        {
            FileStream fs = new FileStream(Properties.Settings.Default.Path + "Registration Information.key", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            StringBuilder Information = new StringBuilder();
            Information.Append("初始金额_预算内户：" + Properties.Settings.Default.初始金额_预算内户 + "\n");
            Information.Append("初始金额_预算外户：" + Properties.Settings.Default.初始金额_预算外户 + "\n");
            Information.Append("初始金额_周转金户：" + Properties.Settings.Default.初始金额_周转金户 + "\n");
            Information.Append("初始金额_计生专户：" + Properties.Settings.Default.初始金额_计生专户 + "\n");
            Information.Append("初始金额_政粮补贴资金专户：" + Properties.Settings.Default.初始金额_政粮补贴资金专户 + "\n");
            Information.Append("初始金额_土地户：" + Properties.Settings.Default.初始金额_土地户 + "\n");
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
            //Wpf.Data.Database.Update("UPDATE T_Type set value='" + License + "' where key=998");
            Wpf.Data.Database.doDML("UPDATE T_Type set value='" + License + "' where key=998","Update");
            Wpf.Helper.Secure.RegistrationInformationFile();
            if (Properties.Settings.Default.正式版)
            {
                Wpf.Data.Database.ChangePassword(Wpf.Helper.Secure.GetMD5_32(License + "PowerByStoneAnt"));
            }
        }

        /// <summary>
        /// 系统自检、正版检查
        /// 第一次开打软件会隐藏注册，防止拷贝
        /// 数据库如果打不开，启动时就出错了。轮不到自检。
        /// </summary>
        public static void SystemCheck()
        {
            if (!Properties.Settings.Default.初始化程序)//还没初始化
            {
                //可能遇到的问题：别人修改了user.config文件初始化程序=false，那么就会重新注册一遍
                Register();
            }
            Wpf.Helper.FileSystemCheck.CheckFolder();
        }
    }
}
