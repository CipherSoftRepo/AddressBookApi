using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBook.Services.BMOModels;
using AddressBook.Services.DTOModels;
using AddressBook.Services.DTOModels.DTOLists;
using AddressBook.Services.Enums;
using AddressBook.Services.Interfaces;
using AddressBook.Services.Models;
using AddressBook.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : BaseController<ContactDTO,ContactListDTO, ContactBMO>
    {
        
        public ContactController(ContactService entityService) : base(entityService)
        {
        }

        [HttpGet]
        [Route("")]
        [ProducesDefaultResponseType(typeof(PaginationList<IEnumerable<ContactListDTO>>))]
        public async Task<IActionResult> GetContacts(string column = null, int? skip = null, int? take = null, Ordering ordering = Ordering.desc)
        {
          
            return await GetAllAsync().ConfigureAwait(false);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ProducesDefaultResponseType(typeof(ContactDTO))]
        public async Task<IActionResult> GetContact(Guid id)
        {
            return await GetAsync(id).ConfigureAwait(false);
        }

        [HttpPut]
        [Route("")]
        [ProducesDefaultResponseType(typeof(ContactDTO))]
        public async Task<IActionResult> PutContact([FromBody] ContactBMO model)
        {


            return await UpdateAsync(model.Id, model).ConfigureAwait(false);

        }

        [HttpPost]
        [Route("")]
        [ProducesDefaultResponseType(typeof(ContactDTO))]
        public async Task<IActionResult> PostContact([FromBody] ContactBMO model)
        {
            return await CreateAsync(model).ConfigureAwait(false);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            return await DeleteAsync(id).ConfigureAwait(false);
        }
    }
}