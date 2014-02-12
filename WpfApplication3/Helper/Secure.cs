using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Helper
{
    class Secure
    {
        public bool CheckUserNameAndPassword(string username, string password)
        {
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
