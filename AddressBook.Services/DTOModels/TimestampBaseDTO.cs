using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Services.DTOModels
{
    public abstract class TimestampBaseDTO
    {
        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}
