
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Extension.Configuration.Consul
{
    /// <summary>
    /// 3提供器
    /// </summary>
    public sealed class ConsulConfigurationProvider : ConfigurationProvider
    {
        private readonly ConsulParser consulParser;
        private readonly IConsulConfigurationSource source;

        /// <summary>
        /// 3提供器
        /// </summary>
        /// <param name="source"></param>
        /// <param name="consulParser"></param>
        public ConsulConfigurationProvider(IConsulConfigurationSource source, ConsulParser consulParser)
        {
            this.consulParser = consulParser;
            this.source = source;

            if (source.ReloadOnChange)
            {
                ChangeToken.OnChange(
                    () => this.consulParser.Watch(this.source.ServiceKey, this.source.CancellationToken),
                   
                    //async () =>
                    //{
                    //    //await this.consulParser.GetConfig(true, source).ConfigureAwait(false);
                    //    //Thread.Sleep(source.ReloadDelay);
                    //       Load();
                    //})

                    () => {  Load();  });
            }
        }

        /// <summary>
        /// 重新加载数据源
        /// </summary>
        public override void Load()
        {
            try
            {
                this.Data = this.consulParser.GetConfig(false, this.source).ConfigureAwait(false).GetAwaiter().GetResult();

                base.OnReload();
            }
            catch (AggregateException aggregateException)
            {
                throw aggregateException.InnerException;
            }
        }
    }
}