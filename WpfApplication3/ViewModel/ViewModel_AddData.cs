using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wpf.Model;

namespace Wpf.ViewModel
{
    public class ViewModel_AddData
    {
        public List<Model_AddData> addData()
        {
            List<Model_AddData> data = new List<Model_AddData>();
            //Model_AddData mdata = new Model_AddData();
            //mdata.时间 = DateTime.Now.ToString("yyyy-MM-dd");
            for(int i=0; i<10; i++)
            {
                data.Add(new Model_AddData());
            }
            return data;
        }
    }
}
