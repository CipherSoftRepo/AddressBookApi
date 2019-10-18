using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using AddressBook.Data.Entities;
using AddressBook.Data.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBook.Data
{
    public class Context : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public Context() : base()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            _loggerFactory = _loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
#if DEBUG
                .UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=AddressBook;Trusted_Connection=True;MultipleActiveResultSets=True")
#elif RELEASE
                .UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=AddressBook;Trusted_Connection=True;MultipleActiveResultSets=True")
#endif
                .EnableSensitiveDataLogging().UseLoggerFactory(_loggerFactory);

            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            // define Contact
            ContactConfiguration.Configure(builder);
            
        }

        public DbSet<Contact> Contacts { get; set; }

    }
}
