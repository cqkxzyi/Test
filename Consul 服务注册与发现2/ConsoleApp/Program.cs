
namespace ConsoleApp
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    class Program
    {
        static void  Main(string[] args)
        {
            TestConsul().ConfigureAwait(false);
            Console.ReadKey();
        }

        static async Task TestConsul() {
            string url = "http://192.168.3.6:8500";
            ILoadBalancer balancer = new RoundRobinLoadBalancer(new PollingConsulServiceProvider(url));
            var client = new HttpClient();

            Console.WriteLine("Request by RoundRobinLoadBalancer....");
            for (int i = 0; i < 10; i++)
            {
                var service = await balancer.GetServiceAsync();

                Console.WriteLine(DateTime.Now.ToString() + "-RoundRobin:" +
                await client.GetStringAsync("http://" + service + "/api/values") + " --> " + "结果来至于 " + service);
            }


            //Console.WriteLine("Request by RandomLoadBalancer....");
            //balancer = new RandomLoadBalancer(new PollingConsulServiceProvider(url));
            //for (int i = 0; i < 10; i++)
            //{
            //    var service = await balancer.GetServiceAsync();

            //    Console.WriteLine(DateTime.Now.ToString() + "-Random:" +
            //        await client.GetStringAsync("http://" + service + "/api/values") + " --> " + "结果来至于" + service);
            //}
        }
    }
}
