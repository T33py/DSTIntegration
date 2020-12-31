﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using DSTIntegrationLib.ConnectionHelpers;
using RestSharp;
using DSTIntegrationLib.SerializationObjects;

namespace DSTIntegrationLib
{
    public class DSTConnection
    {
        Dictionary<string, string> settings = new SettingsBuilder("DSTSettings.json").Variables;
        public Dictionary<string, string> Settings { get => settings; set => settings = value; }
        
        /// <summary>
        /// Retrieve subjects using the current settings
        /// </summary>
        /// <returns></returns>
        public IRestResponse RetrieveSubjects()
        {
            var request = RequestBuilder.RetrieveSubjects(settings);

            RestClient client = new RestClient(settings[SettingConstants.SubjectHTTP]);

            IRestResponse response = client.Post(request);

            Console.WriteLine(response.Content);
            
            return response;
        }

        /// <summary>
        /// Retrieve the list of tables using the current settings
        /// </summary>
        /// <returns></returns>
        public IRestResponse RetrieveTables()
        {
            var request = RequestBuilder.RetrieveTables(settings);

            RestClient client = new RestClient(settings[SettingConstants.TableHTTP]);

            IRestResponse response = client.Post(request);

            Console.WriteLine(response.Content);

            return response;
        }

        /// <summary>
        /// Retrieve metadata for a specific table
        /// </summary>
        /// <returns></returns>
        public IRestResponse RetrieveMetadata()
        {
            var request = RequestBuilder.RetrieveMetadata(settings);

            RestClient client = new RestClient(settings[SettingConstants.MetadataHTTP]);

            IRestResponse response = client.Post(request);

            Console.WriteLine(response.Content);

            return response;
        }

        /// <summary>
        /// Retrieve the table data covered by the provided metadata object
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public IRestResponse RetrieveTableData(TableMetadata metadata)
        {
            var request = RequestBuilder.RetrieveTable(settings, metadata);

            RestClient client = new RestClient(settings[SettingConstants.DataHTTP]);

            IRestResponse response = client.Post(request);

            Console.WriteLine(response.Content);

            return response;
        }
    }
}
