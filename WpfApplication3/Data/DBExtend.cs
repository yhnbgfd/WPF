using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Data
{
    public class DBExtend
    {

        public string GenerateInsertSQL(Wpf.Model.Model_Report data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into main.T_Report(datetime,unitsname,use,income,expenses) values('");
            sql.Append((data.日期 == null ? "1989-03-17 00:00:00":data.日期)).Append("','")
                .Append((data.单位名称 == null ? "无" : data.单位名称)).Append("','")
                .Append((data.用途 == null ? "无" : data.用途)).Append("',")
                .Append(data.借方发生额).Append(",")
                .Append(data.贷方发生额);
            sql.Append(")");
            Console.WriteLine(sql.ToString());
            return sql.ToString();
        }

        public string GenerateUpdateSQL(Wpf.Model.Model_Report data)
        {
            StringBuilder sql = new StringBuilder();



            return sql.ToString();
        }
    }
}
