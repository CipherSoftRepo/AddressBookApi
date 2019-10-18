using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Data.Entities.Configuration
{
    public class ContactConfiguration
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Contact>()
                .HasIndex(c => c.TelephoneNumber).IsUnique();
        }
    }
}
