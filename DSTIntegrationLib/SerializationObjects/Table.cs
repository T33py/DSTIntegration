using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib.SerializationObjects
{
    public class Table
    {
        [JsonProperty]
        public string id;

        [JsonProperty]
        public string text;

        [JsonProperty]
        public string unit;

        [JsonProperty]
        public string updated;

        [JsonProperty]
        public string firstPeriod;

        [JsonProperty]
        public string latestPeriod;

        [JsonProperty]
        public bool active;

        [JsonProperty]
        public List<string> variables;

        public override string ToString()
        {
            string rep_template = "{0}({1})\n{2}";

            string variable_list = "";
            
            if (variables.Count > 0)
            {
                foreach (var variable in variables)
                {
                    variable_list = variable_list + "  " + variable.ToString() + "\n";
                }
            }

            return string.Format(rep_template, text, id, variable_list);
        }
    }

    /// <summary>
    /// Helperclass for deserialization
    /// </summary>
    public class TableListContainer
    {
        [JsonProperty]
        public List<Table> tableList;
    }
}
