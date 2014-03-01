using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.ViewModel
{
    class ViewModel_操作记录
    {
        public string InsertLog(string title, string content, string remark, string type)
        {
            string time = DateTime.Now.ToString();
            content = content.Replace("'", "`");
            content = content.Replace("\"", "`");
            string sql = "INSERT INTO T_LOG(time,title,content,remark,type) values ('" + time + "','" + title + "','" + content + "','" + remark + "','" + type + "')";
            return sql;
        }
    }
}
