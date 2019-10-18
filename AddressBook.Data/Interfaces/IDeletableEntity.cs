using System;

namespace AddressBook.Data.Interfaces
{
    public interface IDeletableEntity
    {
        DateTime? HasBeenDeleted { get; set; }
    }
}