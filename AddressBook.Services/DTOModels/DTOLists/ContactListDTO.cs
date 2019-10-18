using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Services.DTOModels.DTOLists
{
    public class ContactListDTO : ModelBaseDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string TelephoneNumber { get; set; }
    }
}

