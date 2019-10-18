using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AddressBook.Services.Models;

namespace AddressBook.Services.Exceptions
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException()
        {
        }

        public InvalidModelException(IEnumerable<ValidationError> errors = null)
            : base("Invalid data")
        {
            Errors = errors ?? Enumerable.Empty<ValidationError>();
        }

        public InvalidModelException(string message)
            : base(message)
        {
        }

        public InvalidModelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidModelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}
