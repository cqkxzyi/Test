using Manulife.DNC.MSAD.WS.OrderService.Models;
using Manulife.DNC.MSAD.WS.OrderService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using DotNetCore.CAP;
using Manulife.DNC.MSAD.WS.Events;

namespace Manulife.DNC.MSAD.WS.OrderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Repository
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Dapper-ConnString
            services.AddSingleton(Configuration["DB:OrderDB"]);

            // EF DbContext
            services.AddDbContext<OrderDbContext>();

            // CAP
            services.AddCap(x =>
            {
                //x.UseEntityFramework<OrderDbContext>(); // EF  只能二选一

                x.UseSqlServer(Configuration["DB:OrderDB"]); // SQL Server 只能二选一
                x.UseSqlServer(c =>
                {
                    c.ConnectionString = Configuration["DB:OrderDB"];
                    //c.Schema = "cap_Order";
                });

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = Configuration["MQ:Host"];
                    cfg.VirtualHost = Configuration["MQ:VirtualHost"];
                    cfg.Port = Convert.ToInt32(Configuration["MQ:Port"]);
                    cfg.UserName = Configuration["MQ:UserName"];
                    cfg.Password = Configuration["MQ:Password"];
                    cfg.ExchangeName = Configuration["MQ:ExchangeName"];
                }); // RabbitMQ

                //x.UseDashboard(); // 启动仪表盘
                x.Version= "Order-v1";

                x.FailedRetryCount = 2;//设置失败重试次数
                x.FailedRetryInterval = 5;
                //x.SucceedMessageExpiredAfter = 60*2;//2分钟 //设置处理成功的数据在数据库中保存的时间（秒），为保证系统新能，数据会定期清理。
            });

            //注册Swagger生成器，定义一个和多个Swagger 文档
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc(Configuration["Service:DocName"], new Info { Title = "My API", Version = "v1" });
            //    c.DescribeAllEnumsAsStrings();
            //});

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc(Configuration["Service:DocName"], new Info
                {
                    Title = Configuration["Service:Title"],
                    Version = Configuration["Service:Version"],
                    Description = Configuration["Service:Description"],
                    Contact = new Contact
                    {
                        Name = Configuration["Service:Contact:Name"],
                        Email = Configuration["Service:Contact:Email"]
                    }
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, Configuration["Service:XmlFile"]);
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            });
        }



        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // CAP
            //app.UseCap();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "doc/{documentName}/swagger.json";
            });
            
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint($"/doc/{Configuration["Service:DocName"]}/swagger.json",
                    $"{Configuration["Service:Name"]} 版本：{Configuration["Service:Version"]}");
                s.RoutePrefix = string.Empty;//根目录处使用
            });

        }
    }
}
