using Comm;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Delivery.Models;
using Push.Repositories;
using Delivery.Services;

namespace Delivery
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
            services.AddControllers();

            // send
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();

            // Subscriber
            services.AddScoped<IDeliverySubscriberService, DeliverySubscriberService>();
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

                x.UseDashboard(); // 启动仪表盘
                x.Version = "Order-v1";

                // Below settings is just for demo
                x.FailedRetryCount = 1;
                x.FailedRetryInterval = 5;
                //x.SucceedMessageExpiredAfter = 60*2;//2分钟
            });


            // Swagger
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc(Configuration["Service:DocName"], new OpenApiInfo
                {
                    Title = Configuration["Service:Title"],
                    Version = Configuration["Service:Version"],
                    Description = Configuration["Service:Description"],
                    Contact = new OpenApiContact
                    {
                        Name = Configuration["Service:Contact:Name"],
                        Email = Configuration["Service:Contact:Email"]
                    }
                });

                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                var xmlPath = Path.Combine(basePath, Configuration["Service:XmlFile"]);
                s.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                //RESTful API方式
                //endpoints.MapControllers();

                //自定义模式
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Home}/{action=Index}/{id?}");
            });



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
