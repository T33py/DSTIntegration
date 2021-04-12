using DSTIntegration;
using DSTIntegrationLib;
using DSTIntegrationLib.SerializationObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSTDataScraper
{
    /// <summary>
    /// Main class for running datascraping through DSTs API
    /// </summary>
    public class DataScraper
    {
        public List<TableMetadata> Todo { get; set; }
        public List<TableMetadata> Done { get; set; }
        public List<TableMetadata> AllRequests { get; set; }
        public Dictionary<TableMetadata, string> Data { get; set; }

        List<Worker> workers = new List<Worker>();

        public Dictionary<string, bool> WorkersFinished { get; set; }


        DSTRequestHandler requestHandler;
        TableMetadata metadata;
        int exit_code = 42;

        Random rand = new Random();
        List<string> namechars = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "æ", "ø", "å" };
        Mutex GetAssignment_m = new Mutex();
        Mutex HandInAssignment_m = new Mutex();

        public DataScraper()
        {
            Todo = new List<TableMetadata>();
            Done = new List<TableMetadata>();
            AllRequests = new List<TableMetadata>();
            Data = new Dictionary<TableMetadata, string>();
            WorkersFinished = new Dictionary<string, bool>();
        }

        public int Setup()
        {
            Console.WriteLine("Getting metadata.");
            requestHandler = new DSTRequestHandler(true);
            metadata = requestHandler.GetTableMetadata("LPRIS16");
            Console.WriteLine("Available tables:\n");
            
            Console.WriteLine("building todolist.");
            GenerateTodoList();

            return exit_code;
        }

        /// <summary>
        /// Run datascraping with the current setup
        /// </summary>
        /// <returns>Exit code</returns>
        public int Run()
        {
            Console.WriteLine($"Available cores: {Environment.ProcessorCount}");
            Console.WriteLine("Scraping data.");


            // create as many workers as there is cores available to run them on.
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                SpawnWorker().Start();
            }

            Console.WriteLine("Start workers");

            // wait for workers to finish
            bool waiting = true;
            int last_done_count = 0;
            var _25 = true;
            var _50 = true;
            var _75 = true;
            while (waiting) 
            {
                Thread.Sleep(100); // we don't need 100 % wakefullness

                float _d = Done.Count;
                float _a = AllRequests.Count;
                float _p = (_d / _a) * 100;
                if (_p > 25 && _25) { Console.WriteLine($"{25}%"); _25 = false; }
                if (_p > 50 && _50) { Console.WriteLine($"{50}%"); _50 = false; }
                if (_p > 75 && _75) { Console.WriteLine($"{75}%"); _75 = false; }

                var done = true;
                foreach(var worker in workers)
                {
                    done = done && WorkersFinished[worker.Name];
                }

                if (done) waiting = false;
            }


            // print data
            var data_ = new List<string>();
            foreach(var request in AllRequests)
            {
                if (Data.Keys.Contains(request)) data_.Add(Data[request]);
            }

            File.AppendAllLines(Environment.CurrentDirectory + @"\output\testrun.csv", data_);


            exit_code = 42;
            return exit_code;
        }

        /// <summary>
        /// Create a thread that can perform work from the todo list
        /// </summary>
        /// <returns></returns>
        Thread SpawnWorker()
        {
            Worker foo = new Worker(GenerateWorkerName(), this);
            Console.WriteLine($"new worker {foo.Name}");
            workers.Add(foo);
            WorkersFinished[foo.Name] = false;

            ThreadStart ts = new ThreadStart(foo.Work);
            return new Thread(ts);
        }

        string GenerateWorkerName()
        {
            string name = "";

            for(int i = 0; i < rand.Next(3,10); i++)
            {
                name = name + namechars[rand.Next(0, namechars.Count - 1)];
            }

            return name;
        }

        /// <summary>
        /// Threadsafe access to the Todo list
        /// </summary>
        /// <returns></returns>
        public TableMetadata RequestAssignment()
        {
            GetAssignment_m.WaitOne();
            if(Todo.Count > 0)
            {
                var nxt = Todo[0];
                Todo.Remove(nxt);

                GetAssignment_m.ReleaseMutex();
                return nxt;
            }

            GetAssignment_m.ReleaseMutex();
            return null;
        }

        /// <summary>
        /// Add the retrieved data to the datatable
        /// </summary>
        /// <param name="data"></param>
        /// <param name="request"></param>
        public void HandInAssignment(string data, TableMetadata request)
        {
            HandInAssignment_m.WaitOne();
            Data[request] = data;
            Done.Add(request);
            HandInAssignment_m.ReleaseMutex();
        }

        /// <summary>
        /// Generate the todolist in a 'smart' way
        /// </summary>
        void GenerateTodoList()
        {

            if (metadata.Variables.Keys.Contains("tid") || metadata.Variables.Keys.Contains("time"))
            {
                SortDataByTime();
                exit_code = 2;
                return;
            }

            exit_code = 42;
        }

        /// <summary>
        /// Get all data sorted by date - this may not be sufficient to get small enough datasets for big tables
        /// </summary>
        void SortDataByTime()
        {
            Variable timeVariable;

            if (metadata.Variables.Keys.Contains("tid"))
            {
                timeVariable = metadata.Variables["tid"];
            }
            else
            {
                timeVariable = metadata.Variables["time"];
            }

            metadata.variables.Remove(timeVariable);

            // generate a request for each unique time
            foreach(var time in timeVariable.values)
            {
                TableMetadata request = CopyMetadata(metadata);
                
                var this_time_variable = CopyVariable(timeVariable);
                this_time_variable.values.Add(time);
                request.variables.Add(this_time_variable);

                foreach (var variable in metadata.variables)
                {
                    var _var = CopyVariable(variable);
                    foreach(var _val in variable.values)
                    {
                        _var.values.Add(_val);
                    }

                    request.variables.Add(_var);
                }
                
                Todo.Add(request);
                AllRequests.Add(request);
            }

            metadata.variables.Add(timeVariable);

            Console.WriteLine($"Number of things to request: {Todo.Count}");

            return;

        }


        /// <summary>
        /// Create an independent copy of the metadata without the variables inside
        /// </summary>
        /// <param name="template"></param>
        /// <returns>A copy without any values</returns>
        public TableMetadata CopyMetadata(TableMetadata template)
        {
            var mdata = new TableMetadata();

            mdata.active = template.active;
            mdata.contacts = template.contacts;
            mdata.description = template.description;
            mdata.documentation = template.documentation;
            mdata.footnote = template.footnote;
            mdata.id = template.id;
            mdata.suppressedDataValue = template.suppressedDataValue;
            mdata.text = template.text;
            mdata.unit = template.unit;
            mdata.updated = template.updated;
            mdata.variables = new List<Variable>();

            return mdata;
        }

        /// <summary>
        /// Create an independent copy of a variable without the values inside
        /// </summary>
        /// <param name="template"></param>
        /// <returns>A copy without any values</returns>
        Variable CopyVariable(Variable template)
        {
            var nvar = new Variable();
            nvar.elimination = template.elimination;
            nvar.id = template.id;
            nvar.map = template.map;
            nvar.text = template.text;
            nvar.time = template.time;
            nvar.values = new List<Value>();

            return nvar;
        }

        /// <summary>
        /// Create a copy of the value
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        Value CopyValue(Value template)
        {
            var nval = new Value();
            nval.id = template.id;
            nval.text = template.text;

            return nval;
        }
    }

    public enum ExitCodes
    {
        TodolistBuilt = 20,
        NotImplemented = 42
    }

}
