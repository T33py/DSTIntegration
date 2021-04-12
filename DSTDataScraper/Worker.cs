using DSTIntegration;
using DSTIntegrationLib.SerializationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSTDataScraper
{
    public class Worker
    {
        DataScraper boss;
        DSTRequestHandler requestHandler;

        List<string> my_data;
        List<TableMetadata> requests_handled;

        string current_data;
        TableMetadata current_request;

        public string Name { get; set; }

        public Worker(string name, DataScraper boss)
        {
            this.boss = boss;
            Name = name;

            requestHandler = new DSTRequestHandler(false);
            my_data = new List<string>();
        }

        public void Work()
        {
            Console.WriteLine("Work work");
            var rand = new Random();

            current_request = boss.RequestAssignment();

            //Console.WriteLine(current_request.ToString());
            while (!(current_request is null))
            {
                //var data = "tst";
                var data = requestHandler.GetTableData(current_request);
                boss.HandInAssignment(data, current_request);

                // get next assignment
                Thread.Sleep(10);
                current_request = boss.RequestAssignment();
            }

            boss.WorkersFinished[Name] = true;
        }
    }
}
