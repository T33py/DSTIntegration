using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTDataScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start scraping");
            var ds = new DataScraper();
            
            
            var exitcode = ds.Setup();
            Console.WriteLine($"Setup exitcode: {exitcode}");


            exitcode = ds.Run();
            Console.WriteLine($"Run exitcode: {exitcode}");
            Console.ReadLine();
        }
    }
}
