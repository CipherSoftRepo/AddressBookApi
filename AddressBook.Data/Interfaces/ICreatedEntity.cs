using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBook.Data.Interfaces
{
    public interface ICreatedEntity
    {
        DateTime Created { get; set; }
    
    }
}