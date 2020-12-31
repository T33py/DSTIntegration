using DSTIntegrationLib;
using DSTIntegrationLib.SerializationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegration.CommandHandlers
{
    /// <summary>
    /// Retrieve the requested data as an appropriate .NET object.
    /// TODO: implement propper error handling.
    /// </summary>
    public static class Retrievers
    {
        /// <summary>
        /// Retrieve subjects using the connection provided.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static List<Subject> RetrieveSubjects(DSTConnection connection)
        {
            List<Subject> subjects = new List<Subject>();

            var response = connection.RetrieveSubjects();
            
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                subjects = JSONDeserializer.DeserializeSubjects(response.Content);
            }
            else
            {
                Console.WriteLine("HTTP error: " + response.StatusCode + " -> " + response.Content);
            }

            return subjects;
        }


        /// <summary>
        /// Retrieve tables using the connection provided.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static List<Table> RetrieveTables(DSTConnection connection)
        {
            List<Table> tables = new List<Table>();

            var response = connection.RetrieveTables();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                tables = JSONDeserializer.DeserializeTables(response.Content);
            }
            else
            {
                Console.WriteLine("HTTP error: " + response.StatusCode + " -> " + response.Content);
            }

            return tables;
        }

        /// <summary>
        /// Retrieve table metadata using the connection provided.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static TableMetadata RetrieveTableMetadata(DSTConnection connection)
        {
            TableMetadata tables = new TableMetadata();

            var response = connection.RetrieveMetadata();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                tables = JSONDeserializer.DeserializeMetadata(response.Content);
            }
            else
            {
                Console.WriteLine("HTTP error: " + response.StatusCode + " -> " + response.Content);
            }

            return tables;
        }

        /// <summary>
        /// Good boi
        /// </summary>
        public static string Golden { get => "WOOF"; }
    }
}
