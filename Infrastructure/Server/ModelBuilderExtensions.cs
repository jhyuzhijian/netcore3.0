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
                .Where(type => type.GetInterfaces().Any(p => p.GetTypeInfo().IsGenericType && p.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            ;

            var method = modelBuilder.GetType().GetMethods()
                .Where(p => p.Name == "ApplyConfiguration")
                .Where(p => p.IsGenericMethod)
                .Where(p => p.ContainsGenericParameters && p.GetParameters().Any(s => s.ParameterType.Name == "IEntityTypeConfiguration`1"))
                .First()
                ;
            foreach (var configurationType in configurationTypes)
            {
                //获取泛型参数的类型
                var genericTypeArg = configurationType.GetInterfaces().Single().GenericTypeArguments.Single();

                // 获取ModelBuilder的ApplyConfiguration方法的构造函数的MethodInfo对象
                var genericEntityMethod = method.MakeGenericMethod(genericTypeArg);

                // 调用ApplyConfiguration方法，加载继承IEntityTypeConfiguration<>类中定义的配置
                var configurationInstance = Activator.CreateInstance(configurationType);
                var entityBuilder = genericEntityMethod.Invoke(modelBuilder, new object[] { configurationInstance });
            }

        }
    }
}
