using Infrastructure.DbContextCore;
using Infrastructure.Tool.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace yzj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var host = CreateHostBuilder(args).Build();//.Run();

            try
            {
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    string sqlString = configuration.GetSection("ConnectionStrings:MySqlConnection").Value;
                    NLogUtil.EnsureNlogConfig("Nlog.config", sqlString);
                }
                logger.Info("网站启动成功");
                host.Run();
            }
            catch (Exception ex)
            {
                string errorMessage = "网站启动初始化数据异常";
                NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Web, errorMessage, new Exception(errorMessage, ex));
                NLogUtil.WriteDBLog(NLog.LogLevel.Error, LogType.Web, errorMessage, new Exception(errorMessage, ex));
                throw;
            }
            finally
            {
                //确保在应用程序退出之前停止内部计时器/线程(避免在Linux上出现分段错误)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                //自个儿的配置文件
                config.AddJsonFile("selfConfig.json", optional: true, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                //移除其他所有绑定的log插件
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);//设置日志输出最低级别(appsetting.json中的logging配置会覆盖这里的minilevel,和default节点一致)
            })
            .UseNLog()//注册NLog到core默认DI
            ;
        /// <summary>
        /// 若不存在对应上下文的数据库，则创建对应数据库和架构
        /// </summary>
        /// <param name="host"></param>
        //private static void CreateDbIfNotExists(IHost host)
        //{
        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var services = scope.ServiceProvider;
        //        try
        //        {
        //            var context = services.GetRequiredService<BaseDbContext>();
        //            context.Database.EnsureCreated();
        //        }
        //        catch (Exception ex)
        //        {
        //            var logger = services.GetRequiredService<ILogger>();
        //            var logger = services.GetRequiredService<ILogger<Program>>();
        //            logger.LogError(ex, "An error occurred creating the DB.");
        //        }
        //    }
        //}
    }
}
