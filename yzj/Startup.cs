using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using yzj.Server;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Infrastructure;
using Infrastructure.Model;
using AutoMapper;

//将所有controller定义为webapi，且无法针对某个控制器退出webapi的定义
//[assembly:ApiController]
namespace yzj
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//应用程序所在目录
            string xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            //项目生成的xml文档
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            services.AddControllers();
            services.AddDbContext<BasicDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });
            #region 通过dll注入服务
            //services.AddDataService();
            #endregion
            #region json.net注入
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // 设置时区为 UTC
                });
            #endregion
            #region swagger文档注入
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "MyApi",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = string.Empty,
                        Email = string.Empty,
                        Url = new Uri("http://www.baidu.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Nothing",
                        Url = new Uri("https://example.com/license")
                    }
                }); ;
                //具有冲突（重复命名方法）取第一个
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IncludeXmlComments(xmlPath);
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(name: "areaRoute", "{area:exists}/{controller=Home}/{action=Index}"); // 区域路由
                //endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); // 默认路由
            });
        }
    }
}
