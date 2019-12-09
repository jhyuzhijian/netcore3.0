using AutoMapper;
using Infrastructure;
using Infrastructure.AutoMapper;
using Infrastructure.DbContextCore;
using Infrastructure.DBContextCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Reflection;


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
            //项目生成的xml文档
            string xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            string xmlEntityFile = Path.Combine(basePath, "Core_Entity.xml");
            string xmlEntityPath = Path.Combine(AppContext.BaseDirectory, xmlEntityFile);
            services.AddControllers();
            //注入DBContext
            services.AddDbContextToService<SqlServerDbContext>(DataBaseTypeEnum.SqlServer, Configuration.GetConnectionString("SqlConnection"));
            #region 通过dll注入服务
            //services.AddDataService();
            #endregion
            // json.net注入
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // 设置时区为 UTC
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
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
                //c.CustomSchemaIds(type => type.FullName);//如有相同类名则取消此行注释
                //具有冲突（重复命名方法）取第一个
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IncludeXmlComments(xmlPath);
                c.IncludeXmlComments(xmlEntityPath);
            });
            #endregion
            // 自动注入AutoMapper映射规则
            services.AddAutoMapper(MappingRegister.MapTypes());
            services.AddHttpClient();
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
                c.InjectJavascript("/swagger/ui/zh_CN.js");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;//swagger页面默认页
            });

            app.UseStateAutoMapper();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            //app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(name: "areaRoute", "{area:exists}/{controller=Home}/{action=Index}"); // 区域路由
            });
        }
    }
}
