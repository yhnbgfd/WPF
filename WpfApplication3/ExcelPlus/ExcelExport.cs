using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Data;
using xls = Microsoft.Office.Interop.Excel;

namespace Wpf.ExcelPlus
{
    public class ExcelExport
    {
        //class variable
        private List<Object[]> contentArray = new List<Object[]>();
        private int surplusCount = 1;
        private double countIncome = 0;
        private double countExpenses = 0;
        object misValue = System.Reflection.Missing.Value;

        public void Export(int year, int month, int type)
        {
            string dataSql = ProcessDataSql(year, month, type);
            DataSet data = Wpf.Data.Database.Select(dataSql);
            string surplusSql = ProcessSurpluSql(year, month, type);
            decimal surplus = Wpf.Data.Database.SelectSurplus(surplusSql);

            foreach (DataRow dr in data.Tables[0].Rows)
            {
                if (dr[1].ToString() != "" && dr[7].ToString() == "")
                {
                    DateTime drTime = (DateTime)dr[1];
                    if (surplusCount % 25 == 1)
                    {
                        Object[] conclusionCell = new Object[7];
                        if (surplusCount == 1)
                            conclusionCell[2] = "承上年总结";
                        else
                            conclusionCell[2] = "承上页";
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
                    args[4] = (decimal)dr[4];
                    args[5] = (decimal)dr[5];
                    surplus = (decimal)dr[4] - (decimal)dr[5] + surplus;
                    args[6] = surplus;
                    contentArray.Add(args);
                    countIncome += Convert.ToDouble(dr[4]);
                    countExpenses +=  Convert.ToDouble(dr[5]);
                    surplusCount++;
                }
            }

            int countT = contentArray.Count + 3;
            Application xlApp = new Application();

            Workbook wb = null;
            try
            {
                wb = xlApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "Data\\templt.xls");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("找不到导出模板，请重新设置模板", "出错了");
                xlApp.Quit();
                return;
            }

            xlApp.Visible = true;
            Worksheet ws = (Worksheet)wb.Worksheets[1];
            ws.Cells[2, 1] = String.Format("{0}年", year);

            for (int i = 3; i < countT; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    string textAbc = contentArray[i - 3][j].ToString();
                    try
                    {
                        ws.Cells[i + 1, j + 1] = textAbc;
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine("出错内容" + textAbc+ "============="+ee.ToString());
                    }
                }
            }

            string endCellCall = "G" + (countT + 1);
            Range textRange = ws.get_Range("A3", endCellCall);
            textRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            textRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            ws.Cells[countT + 1, 3] = "借方发生额累计";
            ws.Cells[countT + 1, 4] = countIncome.ToString();
            ws.Cells[countT + 1, 5] = "贷方发生额累计";
            ws.Cells[countT + 1, 6] = countExpenses.ToString();

            switch (type)
            {
                case 1:
                    ws.Cells[1, 5] = "(预算内户)";
                    break;
                case 2:
                    ws.Cells[1, 5] = "(预算外户)";
                    break;
                case 3:
                    ws.Cells[1, 5] = "(周转金户)";
                    break;
                case 4:
                    ws.Cells[1, 5] = "(计生专户)";
                    break;
                case 5:
                    ws.Cells[1, 5] = "(政粮补贴资金专户)";
                    break;
                case 6:
                    ws.Cells[1, 5] = "(土地户)";
                    break;
                default:
                    break;
            }
            //wb.SaveAs(Properties.Settings.Default.Path + "ExcelOutput\\"+year+"_"+month+"_"+type+".xls", xls.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, xls.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        }

        private string ProcessDataSql(int year, int month, int type)
        {
            string sql = "";
            string curMonth = "";
            string nextMonth = "";
            string nextYear = "";
            if (month != 0)
            {
                if (month == 12)
                    nextYear = (year + 1).ToString();
                else
                    nextYear = year.ToString();
                if (month < 10)
                    curMonth = "0" + month;
                else
                    curMonth = month.ToString();
                if (month < 9)
                    nextMonth = "0" + (month + 1).ToString();
                else
                    nextMonth = (month + 1).ToString();
                sql = "SELECT * FROM T_Report WHERE DateTime < '" + nextYear + "-" + nextMonth + "-01' AND DateTime >= '"
                    + year.ToString() + "-" + curMonth + "-01' AND Type = " + type + " ORDER BY DateTime;";
            }
            else
            {
                sql = "SELECT * FROM T_Report WHERE substr(date(DateTime),0,5) LIKE '" + year.ToString()
                + "' AND Type = " + type +";";
            }
            
            return sql;
        }

        private string ProcessSurpluSql(int year, int month, int type)
        {
            string sql = "";
            int surplusYear = year;
            int surplusMont = month - 1;
            if (month == 1 || month == 0)
            {
                surplusMont = 12;
                surplusYear = surplusYear - 1;
            }
            sql = "SELECT surplus FROM T_Surplus where year =" + surplusYear + " and month="
                    + surplusMont + " and type=" + type + ";";

            return sql;
        }
    }
}
