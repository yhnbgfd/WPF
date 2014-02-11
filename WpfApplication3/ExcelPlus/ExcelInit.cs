using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection; 
using Microsoft.Office.Interop.Excel;
using System.Data;

namespace Wpf.ExcelPlus
{
    public class ExcelInit
    {
        private List<Object[]> text = new List<Object[]>();

        public ExcelInit()
        {
            Console.WriteLine("Excel Init");
            DateTime date = new DateTime();
            date = System.DateTime.Now;
            int type = 1;
            test3(date.Year, date.Month, type);
        }

        public void test3(int year, int month, int type)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM T_Report ");
            DataSet data = new Wpf.Data.Database().Select(sql.ToString());

            foreach (DataRow dr in data.Tables[0].Rows)
            {
                if (dr[1].ToString() != "")
                {
                    DateTime drTime = (DateTime)dr[1];
                    if (drTime.Year == year)
                    {
                        if (drTime.Month == month)
                        {
                            Object[] args = new Object[7];
                            args[0] = drTime.Month;
                            args[1] = drTime.Day;
                            args[2] = dr[2].ToString();
                            args[3] = dr[3].ToString();
                            args[4] = (double)dr[4];
                            args[5] = (double)dr[5];
                            args[6] = (double)dr[4] - (double)dr[5];
                            text.Add(args);
                        }
                    }
                }
            }

            int countT = text.Count + 3;
            Application xlApp = new Application();
            xlApp.Visible = true;

            Workbook wb = xlApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "templt.xls");
            Worksheet ws = (Worksheet)wb.Worksheets[1];
            ws.Cells[2, 1] = String.Format("{0}年", year);

            for (int i = 3; i < countT; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    string textAbc = text[i - 3][j].ToString();
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
