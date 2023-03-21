using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseObjects
{
    public class NonJsonResponseException : Exception
    {
        public string Response { get; set; } = string.Empty;

        public NonJsonResponseException(string message, string response) : base(message)
        {
            Response = response;
        }
    }
}
