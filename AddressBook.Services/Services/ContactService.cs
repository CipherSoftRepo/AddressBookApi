using System;
using System.Collections.Generic;
using System.Text;
using AddressBook.Data.Entities;
using AddressBook.Data.Interfaces;
using AddressBook.Data.Repositories;
using AddressBook.Services.BMOModels;
using AddressBook.Services.DTOModels;
using AddressBook.Services.DTOModels.DTOLists;
using Microsoft.AspNetCore.Http;

namespace AddressBook.Services.Services
{
    public class ContactService : BaseService<Contact, ContactDTO, ContactListDTO, ContactBMO>
    {
        public ContactService(Repository<Contact> entityRepository) : base(entityRepository)
        {
        }
    }
}
