using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib.SerializationObjects
{
    public class TableMetadata
    {
        [JsonProperty]
        public string id;

        [JsonProperty]
        public string text;

        [JsonProperty]
        public string description;

        [JsonProperty]
        public string unit;

        [JsonProperty]
        public string suppressedDataValue;

        [JsonProperty]
        public string updated;

        [JsonProperty]
        public bool active;

        [JsonProperty]
        public List<Contact> contacts;


        [JsonProperty]
        public Documentation documentation;

        [JsonProperty]
        public Footnote footnote;

        [JsonProperty]
        public List<Variable> variables;

    }

    public class Variable
    {
        [JsonProperty]
        public string id;

        [JsonProperty]
        public string text;

        [JsonProperty]
        public bool elimination;

        [JsonProperty]
        public bool time;

        [JsonProperty]
        public string map;

        [JsonProperty]
        public List<Value> values;

    }

    public class Value
    {
        [JsonProperty]
        public string id;

        [JsonProperty]
        public string text;

    }

    public class Footnote
    {
        [JsonProperty]
        public string text;

        [JsonProperty]
        public bool mandatory;

    }

    public class Documentation
    {
        [JsonProperty]
        public string id;

        [JsonProperty]
        public string url;

    }

    public class Contact
    {
        [JsonProperty]
        public string name;

        [JsonProperty]
        public string phone;

        [JsonProperty]
        public string mail;
    }
}
