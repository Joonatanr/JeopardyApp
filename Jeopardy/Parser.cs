using System;
using System.Collections.Generic;
using System.Drawing;
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

        private int FieldColumn = 0;
        private int FieldFirstRow = 0;
        private int QuestionColumn = 0;
        private int AnswerColumn = 0;
        private int PictureColumn = 0;

        public Parser()
        {
            xlApp = new Excel.Application();
        }

        public bool LoadQuestionsFromFile(string filePath, out List<QuestionField> res)
        {
            if (OpenFile(filePath))
            {
                Excel.Worksheet workSheet = xlWorkbook.Sheets[1];
                Excel.Range rng = workSheet.UsedRange;
                mainTable = GetTableFromRange(rng);

                for (int x = 1; x <= mainTable.LastRow; x++)
                {
                    for (int y = 1; y <= mainTable.LastColumn; y++)
                    {
                        if (mainTable.Cells[x, y] == null)
                        {
                            continue;
                        }

                        if (mainTable.Cells[x, y].ToString() == "Valdkond->")
                        {
                            FieldColumn = y;
                            FieldFirstRow = x + 1;
                        }

                        if (mainTable.Cells[x, y].ToString() == "Küsimus")
                        {
                            QuestionColumn = y;
                        }

                        if (mainTable.Cells[x, y].ToString() == "Vastus")
                        {
                            AnswerColumn = y;
                        }

                        if (mainTable.Cells[x, y].ToString() == "Pilt")
                        {
                            PictureColumn = y;
                        }
                    }

                    // We have found them all... 
                    if (FieldColumn > 0 && AnswerColumn > 0 && QuestionColumn > 0 && PictureColumn > 0)
                    {
                        Console.WriteLine("FieldColumn : " + FieldColumn + " AnswerColumn : " + AnswerColumn + "QuestionColumn : " + QuestionColumn);
                        break;
                    }
                }

                //Lets move forward in the file.
                int currentRow = FieldFirstRow;

                List<QuestionField> parsedList = new List<QuestionField>();

                while (currentRow < mainTable.LastRow)
                {
                    if (mainTable.Cells[currentRow, FieldColumn] != null)
                    {
                        //Lets do this in a simple way, we just assume we found a field.
                        QuestionField field = parseQuestionField(currentRow, FieldColumn);
                        parsedList.Add(field);
                        currentRow += 6;
                    }
                    else
                    {
                        currentRow++;
                    }
                }


                Console.WriteLine("Parsed {0} fields from excel file", parsedList.Count);
                res = parsedList;
                return true;
               
            }
            else
            {
                res = null;
                return false;
            }
        }


        private QuestionField parseQuestionField(int x , int y)
        {
            QuestionField res;
            string name = mainTable.Cells[x, y].ToString();
            res = new QuestionField(name);

            for (int ix = 0; ix < 5; ix++)
            {
                object QuestionCell = mainTable.Cells[ix + x + 1, QuestionColumn];
                object AnswerCell = mainTable.Cells[ix + x + 1, AnswerColumn];
                object PictureCell = mainTable.Cells[ix + x + 1, PictureColumn];

                string qString = "blank";
                string aString = "blank";

                if (QuestionCell != null && AnswerCell != null)
                {
                    qString = QuestionCell.ToString();
                    aString = AnswerCell.ToString();
                }

                if (PictureCell != null)
                {
                    string picturePath = PictureCell.ToString();
                    Image img;
                    try
                    {
                        img = Bitmap.FromFile(picturePath);
                        res.questionArray[ix] = new QuestionPicture(qString, aString, img);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed to load image : " + picturePath);
                    }
                }
                else
                {
                    res.questionArray[ix].QuestionString = qString;
                    res.questionArray[ix].AnswerString = aString;
                }
            }

            return res;
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
