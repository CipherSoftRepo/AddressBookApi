using AutoMapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AddressBook.Services.Automapper
{
    public class AutomapperConfig
    {
        public static void RegisterMappings()
        {
            
            
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(Assembly.GetExecutingAssembly());
            });
        }
    }
}
