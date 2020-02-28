

namespace ServiceB
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseUrls("http://192.168.3.8:6003")
                .UseStartup<Startup>();
    }
}
