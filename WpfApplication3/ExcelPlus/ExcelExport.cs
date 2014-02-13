using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection; 
using Microsoft.Office.Interop.Excel;
using System.Data;

namespace Wpf.ExcelPlus
{
    public class ExcelExport
    {
        //class variable
        private List<Object[]> contentArray = new List<Object[]>();
        private int exportYear = 2014;
        private int exportMonth = 1;
        private int exportType = 1;
        private int surplusCount = 1;
        private double countIncome = 0;
        private double countExpenses = 0;

        //Com variable
        private Application xlApp;
        private Workbook wb;

        public ExcelExport()
        {
            //测试用
            Export(exportYear, exportMonth, exportType);
            //正式版用
           // ExportExcel(exportYear, exportMonth, exportType);
        }

        /*
        public void ExportExcel(int year, int month, int type)
        {
            Init();

            xlApp = new Application();
            xlApp.Visible = true;

            wb = xlApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "templt.xls");
            Worksheet ws = (Worksheet)wb.Worksheets[1];
            GetAndProcessData();

        }

        private void Init()
        {
            exportYear = 0;
            exportMonth = 0;
            exportType = 0;
        }

        private void UnInit()
        {
 
        }

        private void GetAndProcessData()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM T_Report where type =" + exportType);
            DataSet data = new Wpf.Data.Database().Select(sql.ToString());

            int surplusYear = exportYear;
            int surplusMont = exportMonth - 1;
            if (exportMonth == 1)
            {
                surplusMont = 12;
                surplusYear = surplusYear - 1;
            }

            string surplusql = "SELECT * FROM surplus where year =" + surplusYear + " and month=" + surplusMont + " and type=" + exportType;
            DataSet surplusData = new Wpf.Data.Database().Select(surplusql);
            foreach (DataRow dr in data.Tables[0].Rows)
            {

            }
            
            foreach (DataRow dr in data.Tables[0].Rows)
            {
                if (dr[1].ToString() != "")
                {
                    DateTime drTime = (DateTime)dr[1];

                    if (drTime.Year == exportYear)
                    {
                        if (drTime.Month == exportMonth)
                        {
                            Object[] args = new Object[7];
                            args[0] = drTime.Month;
                            args[1] = drTime.Day;
                            args[2] = dr[2].ToString();
                            args[3] = dr[3].ToString();
                            args[4] = (double)dr[4];
                            args[5] = (double)dr[5];
                            args[6] = (double)dr[4] - (double)dr[5];
                            contentArray.Add(args);
                        }
                    }
                }
            }
        }

         */

        public void Export(int year, int month, int type)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM T_Report where type =" + exportType);
            DataSet data = Wpf.Data.Database.Select(sql.ToString());

            int surplusYear = exportYear;
            int surplusMont = exportMonth - 1;
            if (exportMonth == 1)
            {
                surplusMont = 12;
                surplusYear = surplusYear - 1;
            }

            string surplusql = "SELECT surplus FROM T_Surplus where year =" + surplusYear + " and month=" + surplusMont + " and type=" + exportType;
            double surplus = Wpf.Data.Database.SelectSurplus(surplusql);

            foreach (DataRow dr in data.Tables[0].Rows)
            {
                if (dr[1].ToString() != "")
                {
                    DateTime drTime = (DateTime)dr[1];
                    if (drTime.Year == year)
                    {
                        if (drTime.Month == month)
                        {
                            if (surplusCount % 26 == 1)
                            {
                                Object[] conclusionCell = new Object[7];
                                if (surplusCount == 1)
                                {
                                    conclusionCell[2] = "承上年总结";
                                }
                                else
                                {
                                    conclusionCell[2] = "承上页";
                                }
                                conclusionCell[0] = drTime.Month;
                                conclusionCell[1] = drTime.Day;
                                conclusionCell[3] = "";
                                conclusionCell[4] = "";
                                conclusionCell[5] = "";
                                conclusionCell[6] = surplus;
                                contentArray.Add(conclusionCell);
                            }
                            Object[] args = new Object[7];
                            args[0] = drTime.Month;
                            args[1] = drTime.Day;
                            args[2] = dr[2].ToString();
                            args[3] = dr[3].ToString();
                            args[4] = (double)dr[4];
                            args[5] = (double)dr[5];
                            args[6] = surplus;
                            contentArray.Add(args);

                            surplus = (double)dr[4] - (double)dr[5] + surplus;
                            countIncome += (double)dr[4];
                            countExpenses += (double)dr[5];
                            surplusCount++;
                        }
                    }
                }
            }

            int countT = contentArray.Count + 3;
            Application xlApp = new Application();
            xlApp.Visible = true;

            Workbook wb = xlApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "templt.xls");
            Worksheet ws = (Worksheet)wb.Worksheets[1];
            ws.Cells[2, 1] = String.Format("{0}年", year);
            
            for (int i = 3; i < countT; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    string textAbc = contentArray[i - 3][j].ToString();
                    ws.Cells[i + 1, j + 1] = textAbc;
                }
            }

            string endCellCall = "G" + countT;
            Range textRange = ws.get_Range("A3", endCellCall);
            //textRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            textRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            switch (type)
            {
                case 1:
                    ws.Cells[1, 5] = String.Format("(预算内户)", year);
                    break;
                case 2:
                    ws.Cells[1, 5] = String.Format("(预算外户)", year);
                    break;
                case 3:
                    ws.Cells[1, 5] = String.Format("(周转金户)", year);
                    break;
                case 4:
                    ws.Cells[1, 5] = String.Format("(计生专户)", year);
                    break;
                case 5:
                    ws.Cells[1, 5] = String.Format("(政粮补贴资金专户)", year);
                    break;
                default:
                    break;
            }
        }
    }
}
