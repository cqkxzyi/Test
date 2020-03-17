using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Extension.Configuration.Consul;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            return Host.CreateDefaultBuilder(args)
                

            //.ConfigureAppConfiguration((hostingContext, configBuilder) =>
            // {
            //     configBuilder.AddJsonFile("config01.json");
            // })


            //添加自定义配置
            .ConfigureAppConfiguration(
                (hostingContext, builder) =>
                {
                    builder.AddConsul("json", cancellationTokenSource.Token, source =>
                    {
                        source.ConsulClientConfiguration = (
                                cco => cco.Address = new Uri("http://192.168.3.6:8500")
                        );
                        source.Optional = true;
                        source.ReloadOnChange = true;
                        source.ReloadDelay = 1000;
                        source.QueryOptions = new QueryOptions
                        {
                            WaitIndex = 0
                        };
                    });
                })

             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             });
        }

    }
}
