using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Infrastructure.AutoMapper
{
    public static class MappingRegister
    {
        public static Type[] MapTypes()
        {
            var allItem = Assembly
                .GetEntryAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .SelectMany(c => c.ExportedTypes)
                .Where(type => type.GetInterfaces().Contains(typeof(IProfile)));
            
            return allItem.ToArray();
        }
    }
}
