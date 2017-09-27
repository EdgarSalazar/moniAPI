using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moni.Attributes;
using System.Reflection;
using Moni.Extensions;
using Moni.Repository;
using System.Linq.Dynamic.Core;

namespace Moni.Controllers
{
    public abstract class BalanceBaseController<T> : Controller
        where T : class
    {
        public static void TreatFields(ref string fields)
        {
            // Scape method if user wants everything.
            if(fields == "*")
                return;

            // If there's no fields informed, get the fields from the Type properties with 
            // [DefaultField] attribute on them.
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = string.Join(',', typeof(T).GetProperties()
                    .Where(x => x.GetCustomAttributes().Any(a => a is DefaultFieldAttribute))
                    .OrderBy(x => x.GetCustomAttribute<DefaultFieldAttribute>().Order)
                    .Select(x => x.Name));
            }

            fields = string.Join(',', fields.Split(',').Select(x => x.ToPascalCase()));
        }

        public static IQueryable ExtractQueryFields(string fields, IQueryable<T> query)
        {
            return fields == "*"
                    ? query
                    : query.Select($"new ({fields})");
        }
    }
}