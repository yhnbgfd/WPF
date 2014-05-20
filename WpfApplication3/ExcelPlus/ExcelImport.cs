using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection; 
using Microsoft.Office.Interop.Excel;

namespace Wpf.ExcelPlus
{
    public class ExcelImport
    {
        private Microsoft.Office.Interop.Excel.Application xlApp = null;
        private Workbook wb = null;
        private Worksheet ws = null;
        private Microsoft.Office.Interop.Excel.Range CountRowRang;
        private Microsoft.Office.Interop.Excel.Range CurrentRange;

        private List<Model.Model_Report> dataList = new List<Model.Model_Report>();
        private List<string> sqlArray = new List<string>();
        private int noContentIndex = 0;
        private bool isContent = true;
        int contentCount = 4;

        private string TemptYear = "";
        private int Type = 1;

        private void Init()
        {
            noContentIndex = 0;
            dataList.Clear();
            sqlArray.Clear();
            isContent = true;
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            contentCount = 4;
        }

        public void Import(string path)
        {
            Init();
            
            wb = xlApp.Workbooks.Open(path);
            ws = (Worksheet)wb.Worksheets[1];

            CountRowRang = ws.get_Range("B1", Missing.Value);
            TemptYear = Convert.ToString(CountRowRang.Value2);

            CountRowRang = ws.get_Range("B2", Missing.Value);
            Type = Convert.ToInt32(CountRowRang.Value2);

            while (isContent)
            {
                CountRowRang = ws.get_Range(string.Format("C{0}", contentCount), Missing.Value);
                if (CountRowRang.Value2 != null)
                {
                    try
                    {
                        CurrentRange = ws.get_Range(string.Format("A{0}", contentCount), string.Format("I{0}", contentCount));
                        Object[,] currentContent = CurrentRange.Value2;
                        Model.Model_Report bean = new Model.Model_Report();
                        bean.单位名称 = Convert.ToString(currentContent[1, 3]);
                        bean.用途 = Convert.ToString(currentContent[1, 4]);
                        bean.借方发生额 = Convert.ToDecimal(currentContent[1, 5]);
                        bean.贷方发生额 = Convert.ToDecimal(currentContent[1, 7]);
                        bean.月 = Convert.ToInt32(currentContent[1, 1]);
                        bean.日 = Convert.ToInt32(currentContent[1, 2]);
                        dataList.Add(bean);
                        contentCount++;
                    }
                    catch(Exception ee)
                    {
                        Helper.Log.ErrorLog(ee.ToString());
                        System.Windows.MessageBox.Show("导入文件数据有误，请检查是否有隐藏行并且数据格式不对", "错误");
                        return;
                    }

                }
                else
                {
                    noContentIndex++;
                    if (noContentIndex > 3)
                        isContent = false;
                }
            }

            BatchInsert();
            Wpf.Data.Database.Log("Import", path, "", "Excel");
            System.Windows.MessageBox.Show("数据导入完成", "数据导入完成");
            UnInit();
        }

        private void BatchInsert()
        {
            string sql = "";
            string unitsname = "";
            string use = "";
            decimal income = 0;
            decimal expenses = 0;
            string dataTime = "";
            foreach (Model.Model_Report md in dataList)
            {
                dataTime = Wpf.Helper.Date.Format(TemptYear + "-" + md.月 + "-" + md.日);
                unitsname = md.单位名称;
                income = md.借方发生额 ;
                use = md.用途;
                expenses = md.贷方发生额;
                sql = "insert into main.T_Report(datetime,unitsname,use,income,expenses,Type) values('"
                    + dataTime + "','" + unitsname + "','" + use + "','" + income + "','" + expenses + "'," + Type + ")";
                sqlArray.Add(sql);
            }
            Data.Database.doDMLs(sqlArray,"Insert","ImportExcel");
        }

        private void UnInit()
        {
            wb.Save();
            ws = null;
            wb = null;
            xlApp.Quit();
            xlApp = null;
        }
    }
}
