using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTDataScraper.Interfaces
{
    /// <summary>
    /// General strategy for cleaning up data workers retrieve
    /// </summary>
    public interface IDatacleaner
    {
        string CleanData(string data);
    }
}
