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
        public void excelTest(string path)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.Visible = true;
            //Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Workbook wb = xlApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory+"test.xls");
            Worksheet ws = (Worksheet)wb.Worksheets[1];

            // Select the Excel cells, in the range c1 to c7 in the worksheet.
            Range aRange = ws.get_Range("C1", "C7");//获取Excel多个单元格区域
            aRange.NumberFormatLocal = "@";//设置单元格格式为文本 
            aRange.ColumnWidth = 20;
            // Fill the cells in the C1 to C7 range of the worksheet with the args[0].
            Object[] args = new Object[1];
            args[0] = "18520223331";
            aRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, aRange, args);

        }
    }
}
