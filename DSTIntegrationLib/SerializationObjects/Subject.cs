using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib.SerializationObjects
{
    public class Subject
    {
        [JsonProperty]
        public string id;

        [JsonProperty]
        public string description;

        [JsonProperty]
        public bool active;

        [JsonProperty]
        public bool hasSubjects;

        [JsonProperty]
        public List<Subject> subjects;

        
        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string idt)
        {
            string rep_template = idt + "{0}({1})\n{2}";

            string subject_list = "";
            idt = idt + "  ";

            if (subjects.Count > 0)
            {
                foreach (var sub in subjects)
                {
                    subject_list = subject_list + idt + sub.ToString(idt);
                }
            }

            return string.Format(rep_template, description, id, subject_list);
        }
    }

    /// <summary>
    /// Helperclass for deserialization
    /// </summary>
    public class SubjectListContainer
    {
        [JsonProperty]
        public List<Subject> subjectList;
    }
}
