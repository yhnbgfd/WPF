using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;

using Wpf.Model;

namespace Wpf.ViewModel
{
    public class ViewModel_Test
    {
        public ICollectionView Model_Test { get; private set; }
        //public ICollectionView GroupedCustomers { get; private set; }

        public List<Model_Test> ViewModelTest()
        {
            var _customers = new List<Model_Test>();
            List<List<object>> lists = new Wpf.Database.DML().Select("SELECT * FROM main.content order by id ");
            for (int i = 0; i < lists.Count; i++ )
            {
                _customers.Add(
                    new Model_Test
                    {
                        Id = (Int64)(lists[0][0]),
                        单位名称 = (string)(lists[0][4]),
                        用途 = (string)(lists[0][5]),
                        收入 = (double)(lists[0][6]),
                        支出 = (double)(lists[0][7])
                    }
                );
                //Console.WriteLine((Int64)(lists[0][0]) );
            }

            //Test = CollectionViewSource.GetDefaultView(_customers);

            //分组用的
            //GroupedCustomers = new ListCollectionView(_customers);
            //GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("Gender"));

            return _customers;
        }
    }
}
