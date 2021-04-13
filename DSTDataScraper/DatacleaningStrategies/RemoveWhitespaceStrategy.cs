using DSTDataScraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTDataScraper.DatacleaningStrategies
{
    public class RemoveWhitespaceStrategy : IDatacleaner
    {
        public string CleanData(string data)
        {
            var split_data = data.Split(Environment.NewLine.ToCharArray());
            //Console.WriteLine("Split_data.length " + split_data.Length);
            data = "";

            for(int i = 0; i < split_data.Length; i++)
            {
                if (ValidateCSVRow(split_data[i]))
                {
                    //Console.WriteLine("Add back: " + i);
                    data = data + split_data[i].Trim() + Environment.NewLine;
                }
            }

            if (data.Length == 0) return data;

            // remove the final newline before returning
            return data.Substring(0, data.Length - 1);
        }

        bool ValidateCSVRow(string row)
        {
            // throw away remaining newline symbols and empty strings
            if (row.Equals(Environment.NewLine) || row.Equals(string.Empty))
                return false;

            return true;
        }
    }
}
