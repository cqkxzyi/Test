using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Test
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


            //services.AddSingleton<IConfiguration>(Configuration);//配置IConfiguration的依赖

            GetConfig();
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
                endpoints.MapControllers();
            });

            GetConfig();
        }



        #region 获取json参数配置
        /// <summary>
        /// 获取json参数配置
        /// </summary>
        public void GetConfig()
        {
  
            //直接读取字符串
            string conn = Configuration.GetConnectionString("key1");
            conn = Configuration["key"];
            conn = Configuration["key1"];
            conn = Configuration["json:key"];

            //读取GetSection
            IConfigurationSection configurationSection = Configuration.GetSection("key1");


            //添加 json 文件路径
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json");
            IConfigurationRoot configurationRoot = configurationBuilder.Build();


            //弱类型方式读取
            var key1 = configurationRoot["key1"];
             var key2 = configurationRoot.GetSection("key1");



        }
        #endregion
    }

}
