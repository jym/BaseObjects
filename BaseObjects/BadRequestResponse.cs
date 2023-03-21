using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseObjects
{
    public class BadRequestResponse
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; } = "";
    }
}
