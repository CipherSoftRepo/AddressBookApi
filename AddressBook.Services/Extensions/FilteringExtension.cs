using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddressBook.Data.Interfaces;

namespace AddressBook.Services.Extensions
{
    public static class FilteringExtension
    {
        public static IQueryable<T> FilterOutDeleted<T>(this IQueryable<T> query)
            where T : class, IDeletableEntity
        {
            if (null == query)
            {
                return query;
            }

            return query.Where(_ => _.HasBeenDeleted == null);
        }
    }
}
