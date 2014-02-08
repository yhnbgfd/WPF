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


            _customers.Add(new Model_Test 
            {
                单位名称 = new Wpf.Database.DML().Select(),
                用途 = new Wpf.Database.DML().Select()
            });




            //Test = CollectionViewSource.GetDefaultView(_customers);

            //分组用的
            //GroupedCustomers = new ListCollectionView(_customers);
            //GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("Gender"));

            return _customers;
        }
    }
}
