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
                +" ID as 序号,"
                +" strftime('%Y-%m-%d %H:%M:%S',datetime) as 时间,"
                +" unitsname as 单位名称,"
                +" use as 用途,"
                +" income as 收入,"
                +" expenses as 支出 "
                +" FROM main.T_Report order by id ");
            //Test = CollectionViewSource.GetDefaultView(_customers);

            //分组用的
            //GroupedCustomers = new ListCollectionView(_customers);
            //GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("Gender"));

            return data;
        }
    }
}
