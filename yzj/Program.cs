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
                NLogUtil.WriteDBLog(NLog.LogLevel.Trace, LogType.Web, "��վ�����ɹ�");
                host.Run();
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
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
    }
}
