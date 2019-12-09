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
                logger.Info("��վ�����ɹ�");
                host.Run();
            }
            catch (Exception ex)
            {
                string errorMessage = "��վ������ʼ�������쳣";
                NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Web, errorMessage, new Exception(errorMessage, ex));
                NLogUtil.WriteDBLog(NLog.LogLevel.Error, LogType.Web, errorMessage, new Exception(errorMessage, ex));
                throw;
            }
            finally
            {
                //ȷ����Ӧ�ó����˳�֮ǰֹͣ�ڲ���ʱ��/�߳�(������Linux�ϳ��ֶַδ���)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                //�Ը����������ļ�
                config.AddJsonFile("selfConfig.json", optional: true, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                //�Ƴ��������а󶨵�log���
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);//������־�����ͼ���(appsetting.json�е�logging���ûḲ�������minilevel,��default�ڵ�һ��)
            })
            .UseNLog()//ע��NLog��coreĬ��DI
            ;
        /// <summary>
        /// �������ڶ�Ӧ�����ĵ����ݿ⣬�򴴽���Ӧ���ݿ�ͼܹ�
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
