using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Services.Models
{
    public class ErrorDetails
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
