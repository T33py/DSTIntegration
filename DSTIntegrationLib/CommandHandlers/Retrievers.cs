﻿using DSTIntegrationLib;
using DSTIntegrationLib.SerializationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegration.CommandHandlers
{
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
    }
}