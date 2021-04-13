using DSTDataScraper.DatacleaningStrategies;
using DSTDataScraper.Interfaces;
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
        IDatacleaner datacleaner;
        int max_requests;
        DSTRequestHandler requestHandler;

        List<string> my_data;
        public Dictionary<TableMetadata, string> raw_data;
        List<TableMetadata> requests_handled;

        string current_data;
        TableMetadata current_request;

        public string Name { get; set; }

        public Worker(string name, DataScraper boss)
        {
            init(name, boss, new NoCleaningStrategy(), int.MaxValue);
        }

        public Worker(string name, DataScraper boss, IDatacleaner datacleaner)
        {
            init(name, boss, datacleaner, int.MaxValue);
        }

        public Worker(string name, DataScraper boss, IDatacleaner datacleaner, int max_requests)
        {
            init(name, boss, datacleaner, max_requests);
        }

        void init(string name, DataScraper boss, IDatacleaner datacleaner, int max_requests)
        {
            this.boss = boss;
            this.datacleaner = datacleaner;
            Name = name;
            this.max_requests = max_requests;

            requestHandler = new DSTRequestHandler(false);
            my_data = new List<string>();
            requests_handled = new List<TableMetadata>();
            raw_data = new Dictionary<TableMetadata, string>();
        }

        public void Work()
        {
            Console.WriteLine("Work work");
            var rand = new Random();

            current_request = boss.RequestAssignment();

            //Console.WriteLine(current_request.ToString());
            while (!(current_request is null) && requests_handled.Count < max_requests)
            {
                //var data = "tst";
                current_data = requestHandler.GetTableData(current_request);
                CleanupData();

                boss.HandInAssignment(current_data, current_request);

                requests_handled.Add(current_request);
                my_data.Add(current_data);

                // get next assignment
                Thread.Sleep(10);
                current_request = boss.RequestAssignment();
            }

            boss.WorkersFinished[Name] = true;
        }

        void CleanupData()
        {
            var cp = current_data;
            raw_data.Add(current_request, current_data);
            current_data = datacleaner.CleanData(current_data);
        }
    }
}
