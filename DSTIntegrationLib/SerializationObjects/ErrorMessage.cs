using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTIntegrationLib.SerializationObjects
{
    public class ErrorMessage
    {
        [JsonProperty]
        public string errorTypeCode;

        [JsonProperty]
        public string message;
    }
}
