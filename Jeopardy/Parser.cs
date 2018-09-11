using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace Jeopardy
{

    class Parser
    {
        /// <summary>
        /// Unfortunately directly accessing the Excel interop is very slow. It is more efficient to keep
        /// a local copy of the data that is accessed.
        /// </summary>
        private class ConvertedDatatable
        {
            public Object[,] Cells;
            public int LastRow;
            public int LastColumn;

            public ConvertedDatatable()
            {

            }
        }

        private Excel.Application   xlApp;
        private Excel.Workbook      xlWorkbook;

        private ConvertedDatatable mainTable;

        public Parser()
        {
            xlApp = new Excel.Application();
        }

        public bool LoadQuestionsFromFile(string filePath)
        {
            if (OpenFile(filePath))
            {
                Excel.Worksheet workSheet = xlWorkbook.Sheets[1];
                Excel.Range rng = workSheet.UsedRange;
                mainTable = GetTableFromRange(rng);

                return true;
               
            }
            else
            {
                return false;
            }
        }


        private ConvertedDatatable GetTableFromRange(Excel.Range range)
        {
            Excel.Worksheet ws = range.Worksheet;

            //Lets discover the end range.
            int ColumnCount = ws.UsedRange.Columns.Count;
            int RowCount = ws.UsedRange.Rows.Count;

            Excel.Range endRange = ws.Cells[RowCount, ColumnCount];
            Excel.Range testRange = ws.Range[range, endRange];
            ConvertedDatatable res = new ConvertedDatatable
            {
                Cells = testRange.Value2,
                LastRow = testRange.Rows.Count,
                LastColumn = testRange.Columns.Count
            };

            return res;
        }

        private Boolean OpenFile(String filepath)
        {
            Boolean res = true;

            if (!System.IO.Path.IsPathRooted(filepath))
            {
                //Construct the absolute location from the string.
                filepath = System.IO.Path.GetFullPath(filepath);
            }

            try
            {
                xlWorkbook = xlApp.Workbooks.Open(filepath, ReadOnly: true);
            }
            catch (Exception ex)
            {
                //writer.WriteLine(ex.ToString());
                Console.WriteLine(ex.ToString());
                res = false;
            }
            return res;
        }
    }
}
