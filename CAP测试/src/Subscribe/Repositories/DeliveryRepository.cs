using Dapper;
using DotNetCore.CAP;
using Comm;
using System;
using System.Threading.Tasks;
using Delivery.Models;

namespace Push.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        public DeliveryDbContext DbContext { get; }
        public ICapPublisher CapPublisher { get; }
        public string ConnStr { get; } // For Dapper use

        public DeliveryRepository(DeliveryDbContext DbContext, ICapPublisher CapPublisher, string ConnStr)
        {
            this.DbContext = DbContext;
            this.CapPublisher = CapPublisher;
            this.ConnStr = ConnStr;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<bool> PublishAsync()
        {
            await CapPublisher.PublishAsync(EventConstants.EVENT_ByDelivery,"来自Delivery系统的消息："+ DateTime.Now);

            return true;
        }
    }
}
