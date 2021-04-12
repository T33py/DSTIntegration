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
            DSTRequestHandler execute = new DSTRequestHandler(true);
            TableMetadata currentMetadata = GenTMD();
            TableMetadata dataRequestMetadata = GenTMD();

            Console.WriteLine("Welcome to the CLI for DSTIntegrationLibrary. While the -help command and error handling remains on the todolist please consult sourcecode and readme for further information.");
            Console.WriteLine("Missing functionality of note:");
            Console.WriteLine("  - parsing strings in the commandline (arguments surrounded by \" \")");
            Console.WriteLine("  - -help menu/command");
            Console.WriteLine("  - -better errorhandling");
            //Console.WriteLine("  - table metadata");
            //Console.WriteLine("  - building limited scope metadata for use with getting table data");
            //Console.WriteLine("  - table data");
            //Console.WriteLine("  - keeping track of data retrieved, so it can be written to files");
            Console.WriteLine("  - writing data to files");
            Console.WriteLine("  - ");
            Console.WriteLine("To end interaction type \"exit\" or \"-e\".");

            while (!stop)
            {
                var request = Console.ReadLine();

                // try to understand what the user is trieing to do
                try
                {

                    if (GetSubjects(request).requested)
                    {
                        Console.WriteLine("Getting subjects.");
                        var parameters = GetSubjects(request).parameters;
                        List<Subject> subjects = execute.GetSubjects(parameters);


                        Console.WriteLine();
                        foreach (var subject in subjects)
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
                        currentMetadata = metadata;
                        dataRequestMetadata = GenTMD(currentMetadata);

                        Console.WriteLine();
                        Console.WriteLine(metadata);
                        Console.WriteLine();
                    }
                    else if (GetData(request).requested)
                    {
                        var parameters = GetData(request);
                        Console.WriteLine("Getting data requested");
                        var data = execute.GetTableData(dataRequestMetadata);
                        data = data.Replace("INDHOLD", dataRequestMetadata.unit);

                        if (parameters.print)
                        {
                            string dir;
                            string file;
                            if (GetData(request).print_to.Length > 0)
                            {
                                file = parameters.print_to;
                                dir = new FileInfo(file).DirectoryName;
                            }
                            else
                            {
                                dir = Directory.GetCurrentDirectory() + @"\output";
                                file = dir + @"\" + dataRequestMetadata.text + ".csv";
                            }

                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }

                            File.WriteAllText(file, data);
                            Console.WriteLine("Data saved as: " + file);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine(data);
                            Console.WriteLine();
                        }

                    }
                    else if (ResetData(request).requested)
                    {
                        string reset = ResetData(request).reset;

                        if (reset.Equals("metadata"))
                        {
                            currentMetadata = new TableMetadata();
                        }
                        else if (reset.Equals("datarequest"))
                        {
                            dataRequestMetadata = GenTMD(currentMetadata);
                        }
                        else if (reset.Equals("settings"))
                        {
                            execute.ResetSettings(true);
                        }
                    }
                    else if (PrintData(request).requested)
                    {
                        var parameters = PrintData(request);
                        var thing = parameters.thing;

                        if (thing.Equals("currentmetadata"))
                        {
                            Console.WriteLine("\n" + currentMetadata);
                        }
                        else if (thing.Equals("datarequest"))
                        {
                            Console.WriteLine("\n" + dataRequestMetadata);
                        }
                        else if (thing.Equals("variable"))
                        {
                            foreach (var variable in currentMetadata.Variables.Keys)
                            {
                                if (variable.Equals(parameters.arg))
                                {
                                    Console.WriteLine(currentMetadata.Variables[variable]);
                                }
                            }
                        }
                    }
                    else if (AddToDatarequest(request).requested)
                    {
                        var parameters = AddToDatarequest(request);


                        // if add value requested
                        if (parameters.value.Length > 0)
                        {
                            var variable = currentMetadata.Variables[parameters.variable];
                            // if the variable isn't allready in the list of variables -> add it
                            if (!dataRequestMetadata.Variables.Keys.Contains(parameters.variable))
                            {
                                if (parameters.all_values)
                                {
                                    dataRequestMetadata.variables.Add(variable);
                                }
                                else
                                {
                                    dataRequestMetadata.variables.Add(CopyVar(variable));
                                }
                            }

                            // if the variable doesn't allready contain the value -> add it
                            if (!dataRequestMetadata.Variables[parameters.variable].Values.Keys.Contains(parameters.value))
                            {
                                dataRequestMetadata.Variables[parameters.variable].values.Add(currentMetadata.Variables[parameters.variable].Values[parameters.value]);
                            }
                        }
                        // if add variable requested
                        else if (parameters.variable.Length > 0)
                        {
                            var variable = currentMetadata.Variables[parameters.variable];
                            // if the variable isn't allready in the list of variables -> add it
                            if (!dataRequestMetadata.Variables.Keys.Contains(parameters.variable))
                            {
                                if (parameters.all_values)
                                {
                                    dataRequestMetadata.variables.Add(variable);
                                }
                                else
                                {
                                    dataRequestMetadata.variables.Add(CopyVar(variable));
                                }
                            }
                        }

                        Variable CopyVar(Variable variable)
                        {
                            var nvar = new Variable();
                            nvar.elimination = variable.elimination;
                            nvar.id = variable.id;
                            nvar.map = variable.map;
                            nvar.text = variable.text;
                            nvar.time = variable.time;
                            nvar.values = new List<Value>();

                            return nvar;
                        }
                    }
                    else if (RemoveFromDatarequest(request).requested)
                    {
                        var parameters = RemoveFromDatarequest(request);
                        // if add value requested
                        if (parameters.value.Length > 0)
                        {
                            var variable = dataRequestMetadata.Variables[parameters.variable];
                            var value = variable.Values[parameters.value];
                            variable.values.Remove(value);
                        }
                        // if add variable requested
                        else if (parameters.variable.Length > 0)
                        {
                            var variable = dataRequestMetadata.Variables[parameters.variable];
                            dataRequestMetadata.variables.Remove(variable);
                        }
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
                catch (Exception e)
                {
                    Console.WriteLine("Unable to execute the instruction. \nError: \n" + e);
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
            var args = request.Split(' ');
            var parameters = new GetSubjectsParameters();
            
            if (args[0].Contains("-subjects") || args[0].Contains("-subs") || args[0].Contains("subjects"))
            {

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
                    else if (arg.Length > 0 && i > 0)
                    {
                        parameters.SubjectIDs = args[i];
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
            var args = request.Split(' ');
            var parameters = new GetTablesParameters();

            if (args[0].Contains("-tables") || args[0].Contains("-ts") || args[0].Contains("tables"))
            {

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
                    else if (arg.Length > 0 && i > 0)
                    {
                        parameters.SubjectIDs = arg;
                    }

                }

                return (true, parameters);
            }

            return (false, parameters);
        }


        public static (bool requested, GetTableMetadataParameters parameters) GetTableMetadata(string request)
        {
            request = request.ToLower();
            var args = request.Split(' ');
            GetTableMetadataParameters parameters = new GetTableMetadataParameters();

            if (args[0].Contains("-metadata") || args[0].Contains("-md") || args[0].Contains("metadata"))
            {

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
                    else if (arg.Length > 0 && i > 0)
                    {
                        parameters.TableID = args[i];
                    }
                }

                return (true, parameters);
            }

            return (false, parameters);
        }

        public static (bool requested, bool print, string print_to) GetData(string request)
        {
            var args = request.ToLower().Split(' ');
            if (args[0].Contains("getdata"))
            {
                if (args.Length > 2)
                {
                    if (args[1].Equals("-print"))
                    {
                        return (true, true, args[2]);
                    }
                }
                else if (args.Length > 1)
                {
                    if (args[1].Equals("-print"))
                    {
                        return (true, true, "");
                    }
                }
                return (true, false, "");
            }

            return (false, false, "") ;
        }

        public static (bool requested, string reset) ResetData(string request)
        {
            var args = request.ToLower().Split(' ');
            if (args[0].Contains("reset"))
            {
                return (true, args[1]);
            }
            
            return (false, "");
        }

        public static (bool requested, string thing, string arg) PrintData(string request)
        {
            var args = request.ToLower().Split(' ');
            if (args[0].Contains("print"))
            {
                if(args.Length > 2)
                {
                    return (true, args[1], args[2]);
                }
                return (true, args[1], "");
            }

            return (false, "", "");
        }
        
        public static (bool requested, string variable, string value, bool all_values) AddToDatarequest(string request)
        {
            var args = request.ToLower().Split(' ');

            if (args[0].Equals("add"))
            {
                if (args[1].Contains("variable"))
                {
                    if (args.Length > 3) return (true, args[2], "", true);
                    else                 return (true, args[2], "", false);
                }
                else if (args[1].Contains("value"))
                {
                    return (true, args[2], args[3], false);
                }
            }

            return (false, "", "", false);
        }

        public static (bool requested, string variable, string value) RemoveFromDatarequest(string request)
        {
            var args = request.ToLower().Split(' ');

            if (args[0].Equals("remove"))
            {
                if (args[1].Contains("variable"))
                {
                    return (true, args[2], "");
                }
                else if (args[1].Contains("value"))
                {
                    return (true, args[2], args[3]);
                }
            }

            return (false, "", "");
        }

        public static bool CheckStop(string request)
        {
            if (request.ToLower().Contains("exit") || request.ToLower().Contains("-e"))
                return true;
            return false;
        }

        public static TableMetadata GenTMD()
        {
            var mdata = new TableMetadata();

            mdata.active = false;
            mdata.contacts = new List<Contact>();
            mdata.description = "";
            mdata.documentation = new Documentation();
            mdata.footnote = new Footnote();
            mdata.id = "";
            mdata.suppressedDataValue = "";
            mdata.text = "";
            mdata.unit = "";
            mdata.updated = "";
            mdata.variables = new List<Variable>();

            mdata.documentation.id = "";
            mdata.documentation.url = "";

            mdata.footnote.mandatory = false;
            mdata.footnote.text = "";

            return mdata;
        }

        public static TableMetadata GenTMD(TableMetadata template)
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
    }
}
