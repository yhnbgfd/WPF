using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.Data;

using Wpf.Model;

namespace Wpf.ViewModel
{
    public class ViewModel_Test
    {
        public ICollectionView Model_Test { get; private set; }
        //public ICollectionView GroupedCustomers { get; private set; }

        public DataSet ViewModelTest()
        {
            DataSet data = new Wpf.Database.DML().Select("SELECT "
                +" ID,"
                +" strftime('%Y-%m-%d %H:%M:%S',datetime) as datetime,"
                +" unitsname,"
                +" use,"
                +" income,"
                +" expenses "
                +" FROM main.T_Report order by id ");
            //Test = CollectionViewSource.GetDefaultView(_customers);

            //分组用的
            //GroupedCustomers = new ListCollectionView(_customers);
            //GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("Gender"));
            data.Tables[0].Columns["ID"].ColumnName = "序号";
            data.Tables[0].Columns["datetime"].ColumnName = "日期";
            data.Tables[0].Columns["unitsname"].ColumnName = "单位名称";
            data.Tables[0].Columns["use"].ColumnName = "用途";
            data.Tables[0].Columns["income"].ColumnName = "收入";
            data.Tables[0].Columns["expenses"].ColumnName = "支出";

            return data;
        }
    }
}
