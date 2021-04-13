using DSTDataScraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTDataScraper.DatacleaningStrategies
{
    public class NoCleaningStrategy : IDatacleaner
    {
        public string CleanData(string data)
        {
            return data;
        }
    }
}
