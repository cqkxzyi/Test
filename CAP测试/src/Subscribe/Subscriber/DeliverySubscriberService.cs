using Comm;
using Common;
using Dapper;
using DotNetCore.CAP;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace Delivery.Services
{
    public class DeliverySubscriberService : ICapSubscribe, IDeliverySubscriberService
    {
        private readonly string _connStr;

        public DeliverySubscriberService(string connStr)
        {
            _connStr = connStr;
        }

        [CapSubscribe(EventConstants.EVENT_CreateOrder, Group = "group1")]
        public async Task Receive(OrderMessage message)
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

        //[CapSubscribe(EventConstants.EVENT_NAME_CREATE_ORDER , Group = "group2")]
        //public async Task Send2(OrderMessage message)
        //{
        //    try
        //    {
        //        await Console.Out.WriteLineAsync($"[DeliveryService] 接收到消息 : {JsonHelper.SerializeObject(message)}");

        //        await AddDeliveryRecordAsync(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[CapSubscribe(EventConstants.EVENT_NAME_CREATE_ORDER, Group = "group3")]
        //public async Task Send3(OrderMessage message)
        //{
        //    try
        //    {
        //        await Console.Out.WriteLineAsync($"[DeliveryService] 接收到消息 : {JsonHelper.SerializeObject(message)}");

        //        await AddDeliveryRecordAsync(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




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
