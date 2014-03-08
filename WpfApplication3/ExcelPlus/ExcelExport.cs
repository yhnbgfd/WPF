using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Data;
using xls = Microsoft.Office.Interop.Excel;
using System.IO;

namespace Wpf.ExcelPlus
{
    public class ExcelExport
    {
        //class variable
        private List<Object[]> contentArray = new List<Object[]>();
        private int rowCount = 1;
        private double countIncome = 0;
        private double countExpenses = 0;
        object misValue = System.Reflection.Missing.Value;
        private int LastMonth = 0;
        private decimal LastSurplus = 0;

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
                    CheckNeedInsertTitleRow(drTime.Month, surplus);
                    if (drTime.Month > LastMonth && LastMonth != 0)
                    {
                        Object[] args_合记 = new Object[7];
                        args_合记[0] = ""; 
                        args_合记[1] = ""; 
                        args_合记[2] = "本月合计";
                        args_合记[3] = "";
                        args_合记[4] = new ViewModel.ViewModel_Surplus().Count借贷方发生额合计("income", type, year, LastMonth);
                        args_合记[5] = new ViewModel.ViewModel_Surplus().Count借贷方发生额合计("expenses", type, year, LastMonth);
                        args_合记[6] = "";
                        rowCount++;
                        contentArray.Add(args_合记);
                        CheckNeedInsertTitleRow(drTime.Month, surplus);
                        if (LastMonth != 1)
                        {
                            Object[] args_累记 = new Object[7];
                            args_累记[0] = ""; 
                            args_累记[1] = ""; 
                            args_累记[2] = "本月累计";
                            args_累记[3] = "";
                            args_累记[4] = new ViewModel.ViewModel_Surplus().Count借贷方发生额累计("income", type, year, LastMonth);
                            args_累记[5] = new ViewModel.ViewModel_Surplus().Count借贷方发生额累计("expenses", type, year, LastMonth);
                            args_累记[6] = "";
                            rowCount++;
                            contentArray.Add(args_累记);
                        }
                    }
                    CheckNeedInsertTitleRow(drTime.Month, surplus);
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
                    rowCount++;
                    LastSurplus = surplus;
                    LastMonth = drTime.Month;
                }
            }
            {
                Object[] args_合记 = new Object[7];
                args_合记[0] = ""; 
                args_合记[1] = ""; 
                args_合记[2] = "本月合计";
                args_合记[3] = "";
                args_合记[4] = new ViewModel.ViewModel_Surplus().Count借贷方发生额合计("income", type, year, LastMonth);
                args_合记[5] = new ViewModel.ViewModel_Surplus().Count借贷方发生额合计("expenses", type, year, LastMonth);
                args_合记[6] = "";
                contentArray.Add(args_合记);
                rowCount++;
                CheckNeedInsertTitleRow(LastMonth, surplus);
                if (LastMonth != 1)
                {
                    Object[] args_累记 = new Object[7];
                    args_累记[0] = ""; 
                    args_累记[1] = ""; 
                    args_累记[2] = "本月累计";
                    args_累记[3] = "";
                    args_累记[4] = new ViewModel.ViewModel_Surplus().Count借贷方发生额累计("income", type, year, LastMonth);
                    args_累记[5] = new ViewModel.ViewModel_Surplus().Count借贷方发生额累计("expenses", type, year, LastMonth);
                    args_累记[6] = "";
                    contentArray.Add(args_累记);
                }
            }

            int countT = contentArray.Count + 3;
            Application xlApp = null;
            try
            {
                xlApp = new Application();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("找不到Office Excel软件", "出错了");
                return;
            }
            Workbook wb = null;
            string NewFileName = Wpf.Data.DataDef.CustomerType[type - 1] + "_" + year + "-" + month;
            try
            {
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "Data\\templt.xls", AppDomain.CurrentDomain.BaseDirectory + "ExcelOutput\\" + NewFileName + ".xls", true);
                wb = xlApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "ExcelOutput\\" + NewFileName + ".xls");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("找不到导出模板，请重新设置模板", "出错了");
                xlApp.Quit();
                return;
            }

            Worksheet ws = (Worksheet)wb.Worksheets[1];
            ws.Cells[2, 1] = String.Format("{0}年", year);

            for (int i = 3; i < countT; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    string textAbc = "";// contentArray[i - 3][j].ToString();
                    try
                    {
                        textAbc = contentArray[i - 3][j].ToString();
                    }
                    catch(Exception)
                    {
                        textAbc = "";
                    }
                    try
                    {
                        ws.Cells[i + 1, j + 1] = textAbc;
                    }
                    catch (Exception ee)
                    {
                        Wpf.Helper.DebugOnly.Output("出错内容" + textAbc+ "============="+ee.ToString());
                    }
                }
            }

            string endCellCall = "G" + (countT);
            Range textRange = ws.get_Range("A3", endCellCall);
            textRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            textRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            ws.Cells[1, 5] = "（" + Wpf.Data.DataDef.CustomerType[type-1] + "）";
            xlApp.Visible = true;
            Wpf.Data.Database.Log("Export", NewFileName, "", "Excel");
        }

        /// <summary>
        /// 检查是否需要插入“承上页/承上年”
        /// </summary>
        private void CheckNeedInsertTitleRow(int month, decimal surplus)
        {
            if (rowCount % 26 == 1)
            {
                Object[] conclusionCell = new Object[7];
                if (rowCount == 1)
                {
                    if (month == 0 || month == 1)
                    {
                        conclusionCell[2] = "承上年结余";
                    }
                    else
                    {
                        conclusionCell[2] = "承上月结余";
                    }
                }
                else
                    conclusionCell[2] = "承上页";
                conclusionCell[0] = "";
                conclusionCell[1] = "";
                conclusionCell[3] = "";
                conclusionCell[4] = "";
                conclusionCell[5] = "";
                conclusionCell[6] = surplus;
                rowCount++;
                contentArray.Add(conclusionCell);
            }
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
                    + year.ToString() + "-" + curMonth + "-01' AND Type = " + type + " ORDER BY DateTime ASC;";
            }
            else
            {
                sql = "SELECT * FROM T_Report WHERE substr(date(DateTime),0,5) LIKE '" + year.ToString()
                + "' AND Type = " + type + " ORDER BY DateTime ASC;";
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

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Wpf.Helper.DebugOnly.Output("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
