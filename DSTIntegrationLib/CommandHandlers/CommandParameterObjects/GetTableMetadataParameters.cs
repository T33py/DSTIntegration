using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib.CommandHandlers.CommandParameterObjects
{
    public class GetTableMetadataParameters
    {
        string tableID = "";
        public string TableID { get => tableID; set => tableID = value; }
    }
}
