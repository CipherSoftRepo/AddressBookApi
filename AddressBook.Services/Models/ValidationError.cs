using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Services.Models
{
    public class ValidationError
    {
        public string ErrorMessage { get; set; }

        public List<string> PropertyNames { get; }
    }
}
