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

        
        [NonSerialized]
        Dictionary<string, Variable> vars;
        public Dictionary<string, Variable> Variables {
            get
            {
                if(vars is null)
                {
                    GenVars();
                }
                else if (vars.Keys.Count != variables.Count)
                {
                    GenVars();
                }
                return vars;
            }
            set => vars = value; 
        }


        void GenVars()
        {
            vars = new Dictionary<string, Variable>();
            foreach(var variable in variables)
            {
                vars[variable.id.ToLower()] = variable;
            }
        }


        public override string ToString()
        {
            string to_string_template = "{0}({1}) - Unit: {2}\n{3}";
            string variables = PrintVariables("  ");
            return string.Format(to_string_template, text, id, unit, variables);
        }

        public string PrintVariables(string idt)
        {
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


        [NonSerialized]
        Dictionary<string, Value> vals;
        public Dictionary<string, Value> Values
        {
            get
            {
                if (vals is null)
                {
                    GenVals();
                }
                else if (vals.Keys.Count != values.Count)
                {
                    GenVals();
                }
                return vals;
            }
            set => vals = value;
        }


        void GenVals()
        {
            vals = new Dictionary<string, Value>();
            foreach (var value in values)
            {
                vals[value.id.ToLower()] = value;
            }
        }

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
