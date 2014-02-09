using System;
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
        public ICollectionView Model_Report { get; private set; }

        public List<Model_Report> Report()
        {
            DataSet data = new DataSet();
            data = new Wpf.Data.Database().Select();
            var _report = new List<Model_Report>();
            /*{ 
                new Model_Report
                {
                    序号 = 1,
                    日期 = "2014-02-09",
                    单位名称 = "name",
                    用途 = "use1",
                    收入 = 123.5,
                    支出 = 23.5,
                    结余 = 100.0
                }
            };*/

            Model_Report = CollectionViewSource.GetDefaultView(_report);
            return _report;
        }
    }
}
