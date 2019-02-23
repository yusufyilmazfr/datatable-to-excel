using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace kizilay
{
    public class WriteDataToExcel
    {
        public DataTable dataTable { get; set; }

        public WriteDataToExcel(DataTable dataTable)
        {
            this.dataTable = dataTable;
        }

        public bool WriteToExcel()
        {
            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Sheets excelSheet;
                Excel.Workbooks excelWorkBooks;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                excelWorkBooks = xlApp.Workbooks;
                xlWorkBook = excelWorkBooks.Add(misValue);
                excelSheet = xlWorkBook.Worksheets;
                xlWorkSheet = (Excel.Worksheet)excelSheet.get_Item(1);

                var data = new object[dataTable.Rows.Count, dataTable.Columns.Count];

                //basliklari ekliyelim 
                for (int x = 0; x < dataTable.Columns.Count; x++)
                {
                    data[0, x] = dataTable.Columns[x].ColumnName;
                }
                //basliklari kalin ve sutünun ortasina pozisyonliyalim
                var boldformat = xlWorkSheet.get_Range("A1", "T1");
                var m_objfont = boldformat.Font;
                m_objfont.Bold = true;

                var verformat = xlWorkSheet.get_Range("A1", "T1");
                verformat.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                //satirlari objemize ekliyelim
                for (var row = 1; row < dataTable.Rows.Count; row++)
                {
                    for (var column = 1; column <= dataTable.Columns.Count; column++)
                    {
                        data[row, column - 1] = dataTable.Rows[row - 1][column - 1].ToString();
                    }
                }

                //K sütünün Text olarak formatliyalim 
                string endcelltelephone = "K" + dataTable.Rows.Count.ToString();
                var writeFormat = xlWorkSheet.get_Range("K1", endcelltelephone);
                writeFormat.NumberFormat = "@";

                //Objeyi ekliyecegimiz Range i belirliyelim
                string endcell = "T" + dataTable.Rows.Count.ToString();
                var writeR = xlWorkSheet.get_Range("A1", endcell);
                //objeyi hazirladigimiz range e ekliyelim
                writeR.Value2 = data;
                //kayit edelim
                xlWorkBook.SaveAs("data", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                data = null;
                xlWorkBook.Close(false, misValue, misValue);
                excelWorkBooks.Close();
                xlApp.Quit();

                //objeleri yok edelim
                System.Runtime.InteropServices.Marshal.ReleaseComObject(verformat);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(boldformat);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objfont);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(writeFormat);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(writeR);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkBooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                verformat = null;
                boldformat = null;
                m_objfont = null;
                writeR = null;
                writeFormat = null;
                xlWorkSheet = null;
                excelSheet = null;
                excelWorkBooks = null;
                xlWorkBook = null;
                xlApp = null;

                //GC calistiralim
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
