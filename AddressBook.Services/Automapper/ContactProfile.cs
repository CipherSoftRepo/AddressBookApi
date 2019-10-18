using System;
using System.Collections.Generic;
using System.Text;
using AddressBook.Data.Entities;
using AddressBook.Services.BMOModels;
using AddressBook.Services.DTOModels;
using AddressBook.Services.DTOModels.DTOLists;
using AutoMapper;

namespace AddressBook.Services.Automapper
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactDTO>();
            CreateMap<Contact, ContactListDTO>();
            CreateMap<ContactBMO, Contact>()
                // since the telephone number needs to be unique, we make sure there is no white spaces. This could be improved by RegEx and other methods.
                .ForMember(a => a.TelephoneNumber, opt => opt.MapFrom(c => c.TelephoneNumber.Replace(" ", "")));
        }
    }
}
