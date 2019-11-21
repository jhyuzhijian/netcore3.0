using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Server
{
    public static class DBContextExtension
    {
        public static IServiceCollection AddDbContextToService<DbContextRepository>(this IServiceCollection Services, DataBaseTypeEnum DataBaseType, string connectionString)
            where DbContextRepository : DbContext
        {
            switch (DataBaseType)
            {
                case DataBaseTypeEnum.SqlServer:
                    Services.AddDbContext<DbContextRepository>(options =>
                    {
                        options.UseSqlServer(connectionString);
                    });
                    break;
                case DataBaseTypeEnum.MySql:
                    Services.AddDbContext<DbContextRepository>(options =>
                    {
                        options.UseMySQL(connectionString);
                    });
                    break;
                case DataBaseTypeEnum.Oracle:
                    throw new Exception("暂不支持该数据库!");
            }
            return Services;
        }
    }
}
