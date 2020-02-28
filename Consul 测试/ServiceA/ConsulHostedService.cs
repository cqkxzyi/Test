
namespace ServiceA
{
    using Consul;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public ConsulHostedService(IConsulClient consulClient, ILogger<ConsulHostedService> logger, IServer server, IConfiguration configuration)
        {
            _consulClient = consulClient;
            _logger = logger;
            _server = server;
            _configuration = configuration;
        }

        private CancellationTokenSource _cts;
        private string _serviceId;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Create a linked token so we can trigger cancellation outside of this token's cancellation
                _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                //string address = _serverAddressesFeature.Addresses.FirstOrDefault();
                //string address = _server.Features.Get<IServerAddressesFeature>().Addresses.First();
                string address = _configuration["LocalServerUrl"];
                
                var uri = new Uri(address);

                _serviceId = "Service-A-" + Dns.GetHostName() + "-" + uri.Authority;

                var registration = new AgentServiceRegistration()
                {
                    ID = _serviceId,
                    Name = "Service",
                    Address = uri.Host,
                    Port = uri.Port,
                    Tags = new[] { "api" },
                    Check = new AgentServiceCheck()
                    {
                        HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}/healthz",//健康检查地址
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                        Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                        Timeout = TimeSpan.FromSeconds(5)
                    }
                };

                _logger.LogInformation("Registering in Consul");

                // 首先移除服务，避免重复注册
                await _consulClient.Agent.ServiceDeregister(registration.ID, _cts.Token);
                await _consulClient.Agent.ServiceRegister(registration, _cts.Token);
            }
            catch (Exception ex)
            {
                
            }
           
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
