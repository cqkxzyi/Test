using Dapper;
using DotNetCore.CAP;
using Exceptionless.Json;
using Manulife.DNC.MSAD.Common;
using Manulife.DNC.MSAD.WS.Events;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Manulife.DNC.MSAD.WS.DeliveryService.Services
{
    public class OrderSubscriberService : IOrderSubscriberService, ICapSubscribe
    {
        private readonly string _connStr;

        public OrderSubscriberService(string connStr)
        {
            _connStr = connStr;
        }

        [CapSubscribe(EventConstants.EVENT_NAME_CREATE_ORDER)]
        public async Task ConsumeOrderMessage(OrderMessage message)
        {
            try
            {
                await Console.Out.WriteLineAsync($"[DeliveryService] 接收到消息 : {JsonHelper.SerializeObject(message)}");

                await AddDeliveryRecordAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        private async Task<bool> AddDeliveryRecordAsync(OrderMessage order)
        {
            //throw new Exception("自定义异常"); // just for demo use
            using (var conn = new SqlConnection(_connStr))
            {
                string sqlCommand = @"INSERT INTO [dbo].[Deliveries] (ID, OrderID, ProductID, OrderUserID, CreatedTime)
                                                            VALUES (@ID, @OrderID, @ProductID, @OrderUserID, @CreatedTime)";

                int count = await conn.ExecuteAsync(sqlCommand, param: new
                {
                    ID = Guid.NewGuid().ToString(),
                    OrderID = order.ID,
                    OrderUserID = order.OrderUserID,
                    ProductID = order.ProductID,
                    CreatedTime = DateTime.Now
                });

                return count > 0;
            }
        }
    }
}
