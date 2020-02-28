

namespace ServiceA
{
    using System;
    using Consul;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(serviceProvider =>
            //{
            //    var server = serviceProvider.GetRequiredService<IServer>();
            //    return server.Features.Get<IServerAddressesFeature>();
            //});


            //配置IConsulClient到ASP.NET Core的依赖注入系统中
            string consulAddress = "http://192.168.3.6:8500";
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(consulAddress);
            }));



            services.AddSingleton<IHostedService, ConsulHostedService>();

            services.AddHealthChecks();//自带的健康检查

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //自带的健康检查
            app.UseHealthChecks("/healthz");
            app.UseMvc();
        }
    }
}
