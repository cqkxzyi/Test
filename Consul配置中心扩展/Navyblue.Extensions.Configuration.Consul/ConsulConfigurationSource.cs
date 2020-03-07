
using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading;

namespace Extension.Configuration.Consul
{
    /// <summary>
    /// 2数据源
    /// </summary>
    internal sealed class ConsulConfigurationSource : IConsulConfigurationSource
    {
        public CancellationToken CancellationToken { get; }

        public Action<ConsulClientConfiguration> ConsulClientConfiguration { get; set; }

        public Action<HttpClient> ConsulHttpClient { get; set; }

        public Action<HttpClientHandler> ConsulHttpClientHandler { get; set; }

        public string ServiceKey { get; }

        public bool Optional { get; set; } = false;

        public QueryOptions QueryOptions { get; set; }

        public int ReloadDelay { get; set; } = 250;

        public bool ReloadOnChange { get; set; } = false;

        /// <summary>
        /// 2数据源
        /// </summary>
        /// <param name="serviceKey"></param>
        /// <param name="cancellationToken"></param>
        public ConsulConfigurationSource(string serviceKey, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(serviceKey))
            {
                throw new ArgumentNullException(nameof(serviceKey));
            }

            this.ServiceKey = serviceKey;
            this.CancellationToken = cancellationToken;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            ConsulParser consulParser = new ConsulParser(this);

            return new ConsulConfigurationProvider(this, consulParser);
        }

    }
}