using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib.CommandHandlers.CommandParameterObjects
{
    public class GetTablesParameters
    {
        string subjectIDs = "";
        public string SubjectIDs { get => subjectIDs; set => subjectIDs = value; }

        int upatedWithinDays = -1;
        public int UpatedWithinDays { get => upatedWithinDays; set => upatedWithinDays = value; }

        
    }
}
