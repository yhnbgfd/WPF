using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Wpf.Helper
{
    class Secure
    {
        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckUserNameAndPassword(string username, SecureString password)
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
    }
}
