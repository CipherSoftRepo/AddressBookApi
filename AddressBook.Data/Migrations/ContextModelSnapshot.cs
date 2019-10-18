﻿// <auto-generated />
using System;
using AddressBook.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AddressBook.Data.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AddressBook.Data.Entities.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("Created");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<DateTime?>("HasBeenDeleted");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("Modified");

                    b.Property<string>("TelephoneNumber")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("TelephoneNumber")
                        .IsUnique();

                    b.ToTable("Contacts");
                });
#pragma warning restore 612, 618
        }
    }
}