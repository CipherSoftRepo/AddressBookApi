using System;

namespace AddressBook.Data.Interfaces
{
    public interface IModifiedEntity : ICreatedEntity
    {
        DateTime Modified { get; set; }

    }
}