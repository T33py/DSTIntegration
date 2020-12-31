using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib.CommandHandlers.CommandParameterObjects
{
    public class GetSubjectsParameters
    {
        bool recursive = false;
        public bool Recursive { get => recursive; set => recursive = value; }

        string subjectIDs = "";
        public string SubjectIDs { get => subjectIDs; set => subjectIDs = value; }
    }
}
