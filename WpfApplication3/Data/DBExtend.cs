using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wpf.Data
{
    public class DBExtend
    {
        public bool CheckDataIsExist(long DBId)
        {
            if (new Database().SelectOne(DBId) == 1)
            {
                return true;
            }
            return false;
        }

        public string GenerateInsertSQL(Wpf.Model.Model_Report data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into main.T_Report(datetime,unitsname,use,income,expenses) values('");
            sql.Append((data.日期 == null ? "1989-03-17 00:00:00":data.日期)).Append("','")
                .Append((data.单位名称 == null ? "无" : data.单位名称)).Append("','")
                .Append((data.用途 == null ? "无" : data.用途)).Append("',")
                .Append(data.收入).Append(",")
                .Append(data.支出);
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
