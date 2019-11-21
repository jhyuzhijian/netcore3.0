using AutoMapper;
using Infrastructure;
using Infrastructure.AutoMapper;
using Infrastructure.Server;
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


//������controller����Ϊwebapi�����޷����ĳ���������˳�webapi�Ķ���
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

            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//Ӧ�ó�������Ŀ¼
            string xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            //��Ŀ���ɵ�xml�ĵ�
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            services.AddControllers();
            //ע��DBContext
            services.AddDbContextToService<BasicDbContext>(DataBaseTypeEnum.SqlServer, Configuration.GetConnectionString("SqlConnection"));
            #region ͨ��dllע�����
            //services.AddDataService();
            #endregion
            #region json.netע��
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // ����ʱ��Ϊ UTC
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });
            #endregion
            #region swagger�ĵ�ע��
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
                //���г�ͻ���ظ�����������ȡ��һ��
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IncludeXmlComments(xmlPath);
            });
            #endregion
            #region �Զ�ע��Mapperӳ�����
            services.AddAutoMapper(MappingRegister.MapTypes());
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

            app.UseStateAutoMapper();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(name: "areaRoute", "{area:exists}/{controller=Home}/{action=Index}"); // ����·��
            });
        }
    }
}