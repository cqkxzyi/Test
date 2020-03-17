using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Push.Models;
using Push.Repositories;
using System;
using System.IO;

namespace Push
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

            // Repository
            services.AddScoped<IOrderRepository, OrderRepository>();

            // EF DbContext
            services.AddDbContext<OrderDbContext>();

            // Dapper-ConnString
            services.AddSingleton(Configuration["DB:OrderDB"]);


            // CAP
            services.AddCap(x =>
            {
                //x.UseSqlServer(Configuration["DB:OrderDB"]); // SQL Server ֻ�ܶ�ѡһ

                x.UseEntityFramework<OrderDbContext>(); // EF  ֻ�ܶ�ѡһ


                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = Configuration["MQ:Host"];
                    cfg.VirtualHost = Configuration["MQ:VirtualHost"];
                    cfg.Port = Convert.ToInt32(Configuration["MQ:Port"]);
                    cfg.UserName = Configuration["MQ:UserName"];
                    cfg.Password = Configuration["MQ:Password"];
                    cfg.ExchangeName = Configuration["MQ:ExchangeName"];
                }); // RabbitMQ

                //x.UseDashboard(); // �����Ǳ���

                x.FailedRetryCount = 2;
                x.FailedRetryInterval = 5;
                //x.SucceedMessageExpiredAfter = 60*2;//2����
            });


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

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, Configuration["Service:XmlFile"]);
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            // CAP
            //app.UseCap();

            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "doc/{documentName}/swagger.json";
            });

            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint($"/doc/{Configuration["Service:DocName"]}/swagger.json",
                    $"{Configuration["Service:Name"]} �汾��{Configuration["Service:Version"]}");
                s.RoutePrefix = string.Empty;//��Ŀ¼��ʹ��
            });
        }
    }
}
