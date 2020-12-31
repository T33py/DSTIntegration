using DSTIntegrationLib;
using DSTIntegrationLib.CommandHandlers.CommandParameterObjects;
using DSTIntegrationLib.ConnectionHelpers;
using DSTIntegrationLib.SerializationObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            bool stop = false;
            //DSTConnection conn = new DSTConnection();
            //DSTRequestHandler execute = new DSTRequestHandler(conn);
            DSTRequestHandler execute = new DSTRequestHandler();

            Console.WriteLine("Welcome to the CLI for DSTIntegrationLibrary. While the -help command and error handling remains on the todolist please consult sourcecode and readme for further information.");
            Console.WriteLine("Missing functionality of note:");
            Console.WriteLine("  - parsing strings in the commandline (arguments surrounded by \" \")");
            //Console.WriteLine("  - table metadata");
            Console.WriteLine("  - building limited scope metadata for use with getting table data");
            Console.WriteLine("  - table data");
            Console.WriteLine("  - keeping track of data retrieved, so it can be written to files");
            Console.WriteLine("  - writing data to files");
            Console.WriteLine("  - ");
            Console.WriteLine("To end interaction type \"exit\" or \"-e\".");

            while (!stop)
            {
                var request = Console.ReadLine();

                if (GetSubjects(request).requested)
                {
                    Console.WriteLine("Getting subjects.");
                    var parameters = GetSubjects(request).parameters;
                    List<Subject> subjects = execute.GetSubjects(parameters);


                    Console.WriteLine();
                    foreach(var subject in subjects)
                    {
                        Console.WriteLine(subject);
                    }
                    Console.WriteLine();

                }
                else if (GetTables(request).requested)
                {
                    Console.WriteLine("Getting tables.");
                    var parameters = GetTables(request).parameters;
                    List<Table> tables = execute.GetTables(parameters);
                    

                    Console.WriteLine();
                    foreach (var table in tables)
                    {
                        Console.WriteLine(table);
                    }
                    Console.WriteLine();

                }
                else if (GetTableMetadata(request).requested)
                {
                    Console.WriteLine("Getting table-metadata.");
                    var parameters = GetTableMetadata(request).parameters;
                    TableMetadata metadata = execute.GetTableMetadata(parameters);

                    Console.WriteLine();
                    Console.WriteLine(metadata);
                    Console.WriteLine();
                }
                else if (CheckStop(request))
                {
                    stop = true;
                    Console.WriteLine("Exiting");
                }
                else
                {
                    Console.WriteLine("\nUnrecognized command.\n");
                }
            }

        }
        
        /// <summary>
        /// Handle the user asking for subjects
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static (bool requested, GetSubjectsParameters parameters) GetSubjects(string request)
        {
            request = request.ToLower();
            var parameters = new GetSubjectsParameters();
            
            if (request.Contains("-subjects") || request.Contains("-subs"))
            {
                var args = request.Split(' ');

                for(int i = 0; i < args.Length; i++)
                {
                    string arg = args[i];

                    if (arg.Equals("-id") || arg.Equals("-ids"))
                    {
                        i++; 
                        if (i >= args.Length)
                        {
                            parameters.SubjectIDs = "";
                        }
                        else
                        {
                            parameters.SubjectIDs = args[i];
                        }
                    }
                    else if (arg.Equals("-rec"))
                    {
                        parameters.Recursive = true;
                    }
                        
                }

                return (true, parameters);
            }
            return (false, parameters);

        }

        /// <summary>
        /// Handle the user asking for parameters
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static (bool requested, GetTablesParameters parameters) GetTables(string request)
        {
            request = request.ToLower();
            var parameters = new GetTablesParameters();

            if (request.Contains("-tables") || request.Contains("-ts"))
            {
                var args = request.Split(' ');

                for (int i = 0; i < args.Length; i++)
                {
                    string arg = args[i];

                    if (arg.Equals("-id") || arg.Equals("-ids"))
                    {
                        i++;
                        if (i >= args.Length) 
                        { 
                            parameters.SubjectIDs = ""; 
                        }
                        else 
                        {
                            Console.WriteLine("Sbuject: " + args[i]);
                            parameters.SubjectIDs = args[i]; 
                        }
                    }
                    else if (arg.Equals("-days") || arg.Equals("-dslu") || arg.Equals("-d") || arg.Equals("-ds"))
                    {
                        i++;
                        parameters.UpatedWithinDays = int.Parse(args[i]);
                    }

                }

                return (true, parameters);
            }

            return (false, parameters);
        }


        public static (bool requested, GetTableMetadataParameters parameters) GetTableMetadata(string request)
        {
            request = request.ToLower();
            GetTableMetadataParameters parameters = new GetTableMetadataParameters();

            if (request.Contains("-metadata") || request.Contains("-md"))
            {
                var args = request.Split(' ');

                for(int i = 0; i < args.Length; i++)
                {
                    string arg = args[i];

                    if (arg.Equals("-id"))
                    {
                        i++;
                        if(i >= args.Length)
                        {
                            parameters.TableID = "";
                        }
                        else
                        {
                            parameters.TableID = args[i];
                        }
                    }
                }

                return (true, parameters);
            }

            return (false, parameters);
        }

        public static bool CheckStop(string request)
        {
            if (request.ToLower().Contains("exit") || request.ToLower().Contains("-e"))
                return true;
            return false;
        }
    }
}
