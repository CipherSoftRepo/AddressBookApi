using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AddressBook.Services.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base("Object not found")
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
