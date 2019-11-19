using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infrastructure
{
    //根据interface的实现类进行注入
    public static class DataServiceExtension
    {
        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referenceAssemblies = System.IO.Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            var types = referenceAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .ToArray();
            //.Where(x=>x!=baseType)
            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                    services.AddScoped(interfaceType, implementType);
            }
            return services;
        }
    }
}
