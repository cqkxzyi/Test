
namespace ServiceB
{
    using Consul;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public class ConsulHostedService : IHostedService
    {
        private readonly IConsulClient _consulClient;
        private readonly ILogger _logger;
        private readonly IServer _server;

        public ConsulHostedService(IConsulClient consulClient, ILogger<ConsulHostedService> logger, IServer server)
        {
            _consulClient = consulClient;
            _logger = logger;
            _server = server;
        }

        private CancellationTokenSource _cts;
        private string _serviceId;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Create a linked token so we can trigger cancellation outside of this token's cancellation
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            var features = _server.Features;
            var address = features.Get<IServerAddressesFeature>().Addresses.First();
            var uri = new Uri(address);

            _serviceId = "Service-B-" + Dns.GetHostName() + "-" + uri.Authority;

            var registration = new AgentServiceRegistration()
            {
                ID = _serviceId,
                Name = "Service",
                Address = uri.Host,
                Port = uri.Port,
                Tags = new[] { "api" },
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}/api/Health/Status",//健康检查地址
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            _logger.LogInformation("Registering in Consul");

            // 首先移除服务，避免重复注册
            await _consulClient.Agent.ServiceDeregister(registration.ID, _cts.Token);
            await _consulClient.Agent.ServiceRegister(registration, _cts.Token);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _logger.LogInformation("注销 from Consul");
            try
            {
                await _consulClient.Agent.ServiceDeregister(_serviceId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"注销 失败");
            }
        }
    }
}
