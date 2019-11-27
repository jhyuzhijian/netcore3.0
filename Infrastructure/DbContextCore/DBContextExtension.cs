using Infrastructure.IDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DBContextCore
{
    public static class DBContextExtension
    {
        public static IServiceCollection AddDbContextToService<TDbContext>(this IServiceCollection Services, DataBaseTypeEnum DataBaseType, string connectionString)
            where TDbContext : DbContext, IDbContextCore
        {
            switch (DataBaseType)
            {
                case DataBaseTypeEnum.SqlServer:
                    Services.AddDbContext<TDbContext>(options =>
                    {
                        options.UseSqlServer(connectionString);
                    });
                    break;
                case DataBaseTypeEnum.MySql:
                    Services.AddDbContext<TDbContext>(options =>
                    {
                        options.UseMySQL(connectionString);
                    });
                    break;
                case DataBaseTypeEnum.Oracle:
                    throw new Exception("暂不支持该数据库!");
            }
            Services.AddScoped<IDbContextCore, TDbContext>();
            return Services;

        }
    }
}
