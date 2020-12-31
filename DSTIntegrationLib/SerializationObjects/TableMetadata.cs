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

        public override string ToString()
        {
            string to_string_template = "{0}({1}) - Unit: {2}\n{3}";
            string variables = PrintVariables("  ");
            return string.Format(to_string_template, text, id, unit, variables);
        }

        public string PrintVariables(string idt)
        {
            Console.WriteLine("VariableIDT='" + idt + "'");
            string vars = "";

            foreach (var variable in variables)
            {
                vars = vars + idt + variable.ToString(idt) + "\n";
            }

            return vars;
        }
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

        /// <summary>
        /// Indented ToString() impl
        /// </summary>
        /// <param name="idt"></param>
        /// <returns></returns>
        public string ToString(string idt)
        {
            string template = "{0}({1})\n{2}";
            string values = PrintValues(idt + "  ");
            return string.Format(template, text, id, values);
        }

        public override string ToString()
        {
            string template = "{0}({1})\n{2}";
            var values = PrintValues("  ");
            return string.Format(template, text, id, values);
        }

        public string PrintValues(string idt)
        {
            Console.WriteLine("ValueIDT='" + idt + "'");
            string vals = "";

            foreach (var val in values)
            {
                vals = vals + idt + string.Format("{0}({1})\n", val.text, val.id);
            }

            return vals;
        }
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
