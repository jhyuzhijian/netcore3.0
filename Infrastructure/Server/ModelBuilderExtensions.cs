using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Core_Entity;

namespace Infrastructure.Server
{
    public static class ModelBuilderExtensions
    {
        public static void ExecuteConfigurations(this ModelBuilder modelBuilder)
        {
            var allItem = Assembly
             .GetEntryAssembly()
             .GetReferencedAssemblies()
             .Select(Assembly.Load)
             .SelectMany(c => c.ExportedTypes)
             .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
             .Where(type => type.GetTypeInfo().IsClass)
             .Where(type => type.GetTypeInfo().BaseType != null)
             ;

            var Models = allItem
                .Where(type => typeof(IEntity).IsAssignableFrom(type));

            //添加模型实体类型
            foreach (var item in Models)
            {
                if (modelBuilder.Model.FindEntityType(item) == null)
                {
                    modelBuilder.Model.AddEntityType(item);
                }
            }

            var configurationTypes = allItem
                .Where(type => typeof(IEntityTypeConfiguration<>).IsAssignableFrom(type))
                ;
            foreach (var configurationType in configurationTypes)
            {
                Activator.CreateInstance(configurationType, modelBuilder);
            }

        }
    }
}
