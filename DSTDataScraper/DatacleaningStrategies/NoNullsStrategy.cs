using DSTDataScraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTDataScraper.DatacleaningStrategies
{
    public class NoNullsStrategy : IDatacleaner
    {
        IDatacleaner no_nulls = new RemoveRowsWithNullValuesStrategy();
        IDatacleaner no_header = new RemoveHeadersStrategy();
        IDatacleaner no_ws = new RemoveWhitespaceStrategy();


        public string CleanData(string data)
        {
            data = no_nulls.CleanData(data);
            //data = no_header.CleanData(data);
            data = no_ws.CleanData(data);
            return data;
        }
    }
}
