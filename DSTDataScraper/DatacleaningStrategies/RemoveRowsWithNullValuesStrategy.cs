using DSTDataScraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTDataScraper.DatacleaningStrategies
{
    public class RemoveRowsWithNullValuesStrategy : IDatacleaner
    {
        public string CleanData(string data)
        {
            var rows = data.Split(Environment.NewLine.ToCharArray());
            data = "";

            for(int row = 0; row < rows.Length; row++)
            {
                var keep_row = true;
                var row_content = rows[row];
                var cells = row_content.Split(';');


                for (int column = 0; column < cells.Length; column++)
                {
                    var cell = cells[column];
                    if (CellValueDenotesNull(cell))
                        keep_row = false;
                }

                if(keep_row)
                    data = data + rows[row].Trim() + Environment.NewLine;
            }

            return data;
        }

        private bool CellValueDenotesNull(string cell)
        {
            if (cell.Equals(".."))
                return true;

            return false;
        }


    }
}
