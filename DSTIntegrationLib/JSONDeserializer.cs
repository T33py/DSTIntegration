using DSTIntegrationLib.SerializationObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib
{
    public static class JSONDeserializer
    {
        /// <summary>
        /// build a list of subjects based on the json provided
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public static List<Subject> DeserializeSubjects(string serialized)
        {
            serialized = "{\"subjectList\":" + serialized + "}";
            JsonSerializer serializer = new JsonSerializer();

            JObject subject_list = JObject.Parse(serialized);

            SubjectListContainer subject_lc = subject_list.ToObject<SubjectListContainer>();

            List<Subject> subjects = subject_lc.subjectList;

            return subjects;
        }

        /// <summary>
        /// build a list of subjects based on the json provided
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public static List<Table> DeserializeTables(string serialized)
        {
            serialized = "{\"tableList\":" + serialized + "}";
            JsonSerializer serializer = new JsonSerializer();

            JObject subject_list = JObject.Parse(serialized);

            TableListContainer table_lc = subject_list.ToObject<TableListContainer>();

            List<Table> tables = table_lc.tableList;

            return tables;
        }

        /// <summary>
        /// Build metadata object for a table from json
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public static TableMetadata DeserializeMetadata(string serialized)
        {
            JsonSerializer serializer = new JsonSerializer();

            JObject metadataJobj = JObject.Parse(serialized);

            TableMetadata metadata = metadataJobj.ToObject<TableMetadata>();

            return metadata;
        }
    }
}
