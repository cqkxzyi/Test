using DotNetCore.CAP;
using Manulife.DNC.MSAD.WS.DeliveryService.Models;
using Manulife.DNC.MSAD.WS.DeliveryService.Services;
using Manulife.DNC.MSAD.WS.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Manulife.DNC.MSAD.WS.DeliveryService
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

            // Subscriber
            services.AddScoped<IOrderSubscriberService, OrderSubscriberService>();
            //services.AddTransient<IOrderSubscriberService, OrderSubscriberService>();

            // Dapper-ConnString
            services.AddSingleton(Configuration["DB:OrderDB"]);

            // EF DbContext
            services.AddDbContext<DeliveryDbContext>();

            // CAP
            services.AddCap(x =>
            {
                //x.UseEntityFramework<DeliveryDbContext>(); // EF

                x.UseSqlServer(c =>
                {
                    c.ConnectionString = Configuration["DB:OrderDB"];
                    //c.Schema = "cap_D";
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
                x.Version = "Order-v1";

                // Below settings is just for demo
                x.FailedRetryCount = 1;
                x.FailedRetryInterval = 5;
                //x.SucceedMessageExpiredAfter = 60*2;//2分钟
            });

            // CAP
            services.AddCap(x =>
            {
                //x.UseEntityFramework<DeliveryDbContext>(); // EF

                x.UseSqlServer(c =>
                {
                    c.ConnectionString = Configuration["DB:OrderDB"];
                    //c.Schema = "cap_D";
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
                x.Version = "D-v1";

                // Below settings is just for demo
                x.FailedRetryCount = 1;
                x.FailedRetryInterval = 5;
                //x.SucceedMessageExpiredAfter = 60*2;//2分钟
            });

            // Swagger
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
                s.IncludeXmlComments(xmlPath);
            });
        }

      
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // CAP
            //app.UseCap();

            // Swagger
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "doc/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint($"/doc/{Configuration["Service:DocName"]}/swagger.json",
                    $"{Configuration["Service:Name"]} {Configuration["Service:Version"]}");
                s.RoutePrefix = string.Empty;//根目录处使用
            });
        }
    }
}
