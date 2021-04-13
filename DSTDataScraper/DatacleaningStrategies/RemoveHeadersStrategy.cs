using DSTDataScraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTDataScraper.DatacleaningStrategies
{
    public class RemoveHeadersStrategy : IDatacleaner
    {
        public string CleanData(string data)
        {
            var split_data = data.Split(Environment.NewLine.ToCharArray());
            data = "";

            for(int i = 1; i < split_data.Length; i++)
            {
                data = data + split_data[i].Trim() + Environment.NewLine;
            }

            return data;
        }

    }
}
