using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using yzj.Tool.Util;

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
                NLogUtil.WriteDBLog(NLog.LogLevel.Trace, LogType.Web, "网站启动成功");
                host.Run();
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
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
    }
}
