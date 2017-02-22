using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace DailyStatusReporter
{
    public class ExcelWorkSheetService
    {
        public Worksheet CreateStatusExcel()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if(xlApp == null)
            {
                Console.WriteLine("xl application could not be started");
            }
            xlApp.Visible = true;

            Workbook workBook = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet workSheet = (Worksheet)workBook.Worksheets[1];

            if (workSheet == null)
            {
                Console.WriteLine("Worksheet could not be created. Check that your office installation and project references are correct.");
            }

            // Select the Excel cells, in the range c1 to c7 in the worksheet.
            Range aRange = workSheet.get_Range("C1", "C7");

            if (aRange == null)
            {
                Console.WriteLine("Could not get a range. Check to be sure you have the correct versions of the office DLLs.");
            }

            // Fill the cells in the C1 to C7 range of the worksheet with the number 6.
            Object[] args = new Object[1];
            args[0] = 6;
            aRange.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, aRange, args);

            // Change the cells in the C1 to C7 range of the worksheet to the number 8.
            aRange.Value2 = 8;

            return workSheet;


        }
        



    }
}