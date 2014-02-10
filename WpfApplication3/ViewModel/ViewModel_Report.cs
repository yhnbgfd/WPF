﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using Wpf.Model;
using System.Data;

namespace Wpf.ViewModel
{
    public class ViewModel_Report
    {
        //public ICollectionView Model_Report { get; private set; }

        public List<Model_Report> Report()
        {
            int id = 1;//序号
            
            DataSet data = new Wpf.Data.Database().Select();
            var _report = new List<Model_Report>();
            
            foreach(DataRow dr in data.Tables[0].Rows)
            {
                    _report.Add(new Model_Report 
                                    { 
                                        Dbid = (long)dr[0],
                                        序号 = id++,
                                        日期 = dr[1].ToString(),
                                        单位名称 = dr[2].ToString(),
                                        用途 = dr[3].ToString(),
                                        收入 = (double)dr[4],
                                        支出 = (double)dr[5],
                                        结余 = (double)dr[4] - (double)dr[5]
                                    }
                    );
            }
            //Model_Report = CollectionViewSource.GetDefaultView(_report);
            return _report;
        }
    }
}
