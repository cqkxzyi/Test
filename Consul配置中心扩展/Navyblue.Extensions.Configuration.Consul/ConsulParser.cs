
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Extension.Configuration.Consul
{
    /// <summary>
    /// 4获取consul数据
    /// </summary>
    public sealed class ConsulParser
    {
        private readonly IConsulConfigurationSource consulConfigurationSource;
        private readonly object lastIndexLock = new object();
        private ulong lastIndex;
        private ConfigurationReloadToken reloadToken = new ConfigurationReloadToken();

        /// <summary>
        /// 4获取consul数据
        /// </summary>
        /// <param name="consulConfigurationSource"></param>
        public ConsulParser(IConsulConfigurationSource consulConfigurationSource)
        {
            this.consulConfigurationSource = consulConfigurationSource;
        }

        /// <summary>
        /// 获取并转换Consul配置信息
        /// </summary>
        /// <param name="reloading"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, string>> GetConfig(bool reloading, IConsulConfigurationSource source)
        {
            try
            {
                QueryResult<KVPair> kvPair = await this.GetKvPairs(source.ServiceKey, source.QueryOptions, source.CancellationToken).ConfigureAwait(false);
                if ((kvPair?.Response == null) && !source.Optional)
                {
                    if (!reloading)
                    {
                        //throw new FormatException(Resources.Error_InvalidService(source.ServiceKey));
                    }
                    return new Dictionary<string, string>();
                }

                if (kvPair?.Response == null)
                {
                    throw new FormatException(Resources.Error_ValueNotExist);
                }

                this.UpdateLastIndex(kvPair);

                return JsonConfigurationFileParser.Parse(source.ServiceKey, new MemoryStream(kvPair.Response.Value));
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Consul配置信息监控
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IChangeToken Watch(string key, CancellationToken cancellationToken)
        {
            Task.Run(() => this.RefreshForChanges(key, cancellationToken), cancellationToken);

            return this.reloadToken;
        }

        private async Task RefreshForChanges(string key, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (await this.IsValueChanged(key, cancellationToken).ConfigureAwait(false))
                    {
                        ConfigurationReloadToken previousToken = Interlocked.Exchange(ref this.reloadToken, new ConfigurationReloadToken());
                        previousToken.OnReload();
                        return;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        private async Task<bool> IsValueChanged(string key, CancellationToken cancellationToken)
        {
            QueryOptions queryOptions;
            lock (this.lastIndexLock)
            {
                queryOptions = new QueryOptions
                {
                    WaitIndex = this.lastIndex
                };
            }

            QueryResult<KVPair> result = await this.GetKvPairs(key, queryOptions, cancellationToken).ConfigureAwait(false);

            return result != null && this.UpdateLastIndex(result);
        }

        private bool UpdateLastIndex(QueryResult queryResult)
        {
            lock (this.lastIndexLock)
            {
                if (queryResult.LastIndex > this.lastIndex)
                {
                    this.lastIndex = queryResult.LastIndex;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 从consul服务器获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="queryOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<QueryResult<KVPair>> GetKvPairs(string key, QueryOptions queryOptions, CancellationToken cancellationToken)
        {
            using (IConsulClient consulClient = new ConsulClient(
                this.consulConfigurationSource.ConsulClientConfiguration,
                this.consulConfigurationSource.ConsulHttpClient,
                this.consulConfigurationSource.ConsulHttpClientHandler))
            {
                QueryResult<KVPair> result = await consulClient.KV.Get(key, queryOptions, cancellationToken).ConfigureAwait(false);

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.NotFound:
                        return result;

                    default:
                        throw new FormatException(Resources.Error_Request);
                }
            }
        }
    }
}