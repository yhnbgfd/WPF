using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Wpf.Helper
{
    class Secure
    {
        public bool CheckUserNameAndPassword(string username, SecureString password)
        {
            IntPtr p = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(password);
            string passwordstr = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(p);
            string sql = "Select count(*) from T_User "
                +" where username='"+username+"' and password='"+password+"' and status=1";
            if(new Wpf.Data.Database().SelectCount(sql) == 1)
            {
                return true;
            }
            return false;
        }
    }
}
