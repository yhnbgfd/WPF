using System;
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

        public bool InsertData(List<Model_AddData> data)
        {


            return false;
        }
    }
}
