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
        public ICollectionView Test { get; private set; }
        //public ICollectionView GroupedCustomers { get; private set; }


        public List<Model_Test> ViewModelTest()
        {
            var _customers = new List<Model_Test>
                                 {
                                     new Model_Test
                                         {
                                             姓 = "Christian",
                                             名 = "Moser",
                                             性别 = Gender.男,
                                             年龄 = "20",
                                             WebSite = "http://www.wpftutorial.net",
                                             ReceiveNewsletter = true,
                                             Image = "Images/christian.jpg"
                                         },
                                     new Model_Test
                                         {
                                             姓 = "Peter",
                                             名 = "Meyer",
                                             性别 = Gender.男,
                                             年龄 = "24",
                                             WebSite = "http://www.petermeyer.com",
                                             Image = "Images/peter.jpg"
                                         },
                                     new Model_Test
                                         {
                                             姓 = "Lisa",
                                             名 = "Simpson",
                                             性别 = Gender.女,
                                             年龄 = "18",
                                             WebSite = "http://www.thesimpsons.com",
                                             Image = "Images/lisa.jpg"
                                         },
                                     new Model_Test
                                         {
                                             姓 = "Betty",
                                             名 = "Bossy",
                                             性别 = Gender.女,
                                             年龄 = "21",
                                             WebSite = "http://www.bettybossy.ch",
                                             Image = "Images/betty.jpg"
                                         }
                                 };

            Test = CollectionViewSource.GetDefaultView(_customers);

            //分组用的
            //GroupedCustomers = new ListCollectionView(_customers);
            //GroupedCustomers.GroupDescriptions.Add(new PropertyGroupDescription("Gender"));

            return _customers;
        }
    }
}
