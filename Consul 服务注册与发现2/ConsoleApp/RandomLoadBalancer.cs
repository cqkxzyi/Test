namespace ConsoleApp
{
    using System;
    using System.Threading.Tasks;
    public interface ILoadBalancer
    {
        Task<string> GetServiceAsync();
    }

    /// <summary>
    /// 负载均衡   随机算法
    /// </summary>
    public class RandomLoadBalancer : ILoadBalancer
    {
        private readonly IServiceDiscoveryProvider _sdProvider;

        public RandomLoadBalancer(IServiceDiscoveryProvider sdProvider)
        {
            _sdProvider = sdProvider;
        }

        private Random _random = new Random();

        public async Task<string> GetServiceAsync()
        {
            var services = await _sdProvider.GetConsulServiceAsync();
            return services[_random.Next(services.Count)];
        }
    }

    /// <summary>
    /// 负载均衡  轮询
    /// </summary>
    public class RoundRobinLoadBalancer : ILoadBalancer
    {
        private readonly IServiceDiscoveryProvider _sdProvider;

        public RoundRobinLoadBalancer(IServiceDiscoveryProvider sdProvider)
        {
            _sdProvider = sdProvider;
        }

        private readonly object _lock = new object();
        private int _index = 0;

        public async Task<string> GetServiceAsync()
        {
            var services = await _sdProvider.GetConsulServiceAsync();
            lock (_lock)
            {
                if (_index >= services.Count)
                {
                    _index = 0;
                }
                return services[_index++];
            }
        }
    }
}
