﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wpf.Model;

namespace Wpf.ViewModel
{
    public class ViewModel_AddData
    {
        /// <summary>
        /// 初始化10条空数据
        /// </summary>
        /// <returns></returns>
        public List<Model_AddData> InitData()
        {
            List<Model_AddData> data = new List<Model_AddData>();
            for(int i=0; i<10; i++)
            {
                data.Add(new Model_AddData());
            }
            return data;
        }

        public bool InsertData(List<Model_AddData> datas, int type)
        {
            List<string> sqls = new List<string>();
            foreach(Model_AddData data in datas)
            {
                if (data.时间 == null || data.单位名称 == null || data.用途 == null)
                {
                    return false;
                }
                else
                {
                    sqls.Add("Insert into T_Report(DateTime,UnitsName,Use,Income,Expenses,Type) values('"+data.时间+"','"+data.单位名称+"','"+data.用途+"',"+data.贷方发生额+","+data.借方发生额+","+type+")");
                }
            }
            return Wpf.Data.Database.Transaction(sqls); ;
        }
    }
}