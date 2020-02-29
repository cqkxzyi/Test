namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Consul;

    public interface IServiceDiscoveryProvider
    {
        Task<List<string>> GetConsulServiceAsync();
    }

    /// <summary>
    /// 获取服务列表
    /// </summary>
    public class ConsulServiceProvider : IServiceDiscoveryProvider
    {
        private string consulAddres;
        public ConsulServiceProvider(string url) {
            consulAddres = url;
        }
        public async Task<List<string>> GetConsulServiceAsync()
        {
            var consulClient = new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(consulAddres);
            });

            var queryResult = await consulClient.Health.Service("Service", string.Empty, true);

            var services = consulClient.Catalog.Service("Service").Result.Response;

            //while (queryResult.Response.Length == 0)
            //{
            //    Console.WriteLine("No services found, wait 1s....");
            //    await Task.Delay(1000);
            //    queryResult = await consulClient.Health.Service("Service", string.Empty, true);
            //}

            var result = new List<string>();
            foreach (var serviceEntry in queryResult.Response)
            {
                result.Add(serviceEntry.Service.Address + ":" + serviceEntry.Service.Port);
            }
            return result;
        }
    }

    /// <summary>
    /// 实时同步获取Consul端的服务列表名称
    /// </summary>
    public class PollingConsulServiceProvider : IServiceDiscoveryProvider
    {
        private List<string> _services = new List<string>();
        private bool _polling;
        private string consulAddres;
        /// <summary>
        /// 实时同步获取Consul端的服务列表名称
        /// </summary>
        /// <param name="url"></param>
        public PollingConsulServiceProvider(string url)
        {
            consulAddres = url;
            var _timer = new Timer(async _ =>
            {
                if (_polling)
                {
                    return;
                }

                _polling = true;
                await Poll();
                _polling = false;

            }, null, 0, 10000);
        }
     
        public async Task<List<string>> GetConsulServiceAsync()
        {
            if (_services.Count == 0)
                await Poll();
            return _services;
        }

        private async Task Poll()
        {
            _services = await new ConsulServiceProvider(consulAddres).GetConsulServiceAsync();
        }
    }
}
