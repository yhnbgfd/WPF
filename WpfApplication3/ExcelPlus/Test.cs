using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection; 
using Microsoft.Office.Interop.Excel;

namespace Wpf.ExcelPlus
{
    public class Test
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
        private int tempType = 1;
        private string Type = "";

        private void Init()
        {
            noContentIndex = 0;
            dataList.Clear();
            sqlArray.Clear();
            isContent = true;
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            contentCount = 4;
        }

        public void excelTest(string path)
        {
            Init();
            
            wb = xlApp.Workbooks.Open(path);
            ws = (Worksheet)wb.Worksheets[1];

            CountRowRang = ws.get_Range("B1", Missing.Value);
            TemptYear = Convert.ToString(CountRowRang.Value2);

            CountRowRang = ws.get_Range("B2", Missing.Value);
            Type = Convert.ToString(CountRowRang.Value2);
            switch (Type)
            {
                case "预算内户":
                    tempType = 1;
                    break;
                case "预算外户":
                    tempType = 2;
                    break;
                case "周转金户":
                    tempType = 3;
                    break;
                case "计生专户":
                    tempType = 4;
                    break;
                case "政粮补贴资金专户":
                    tempType = 5;
                    break;
                default:
                    break;
            }

            while (isContent)
            {
                CountRowRang = ws.get_Range(string.Format("C{0}", contentCount), Missing.Value);
                if (CountRowRang.Value2 != null)
                {
                    CurrentRange = ws.get_Range(string.Format("A{0}", contentCount), string.Format("I{0}", contentCount));
                    Object[,] currentContent = CurrentRange.Value2;
                    Model.Model_Report bean = new Model.Model_Report();
                    bean.单位名称 = Convert.ToString(currentContent[1, 3]);
                    bean.用途 = Convert.ToString(currentContent[1, 4]);
                    bean.借方发生额 = Convert.ToDouble(currentContent[1, 5]);
                    bean.贷方发生额 = Convert.ToDouble(currentContent[1, 7]);
                    bean.月 = Convert.ToString(currentContent[1, 1]);
                    bean.日 = Convert.ToString(currentContent[1, 2]);
                    dataList.Add(bean);
                    contentCount++;
                }
                else
                {
                    noContentIndex++;
                    if (noContentIndex > 3)
                        isContent = false;
                }
            }

            BatchInsert();
            UnInit();
        }

        private void BatchInsert()
        {
            string sql = "";
            string month = "";
            string day = "";
            string unitsname = "";
            string use = "";
            double income = 0;
            double expenses = 0;
            string dataTime = "";
            foreach (Model.Model_Report md in dataList)
            {
                dataTime = new Wpf.Helper.Date().Format(TemptYear + "-" + md.月 + "-" + md.日);
                
                unitsname = md.单位名称;
                income = md.借方发生额 ;
                use = md.用途;
                expenses = md.贷方发生额;
                sql = "insert into main.T_Report(datetime,unitsname,use,income,expenses,Type) values('"
                    + dataTime + "','" + unitsname + "','" + use + "','" + income + "','" + expenses + "'," + tempType + ")";
                sqlArray.Add(sql);
                new Data.Database().Insert(sql);
            }
        }

        private void UnInit()
        {
            ws = null;
            wb = null;
            xlApp.Quit();
            xlApp = null;
        }
    }
}
