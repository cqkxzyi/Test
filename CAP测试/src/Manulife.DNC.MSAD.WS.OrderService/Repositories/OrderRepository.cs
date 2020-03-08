using Dapper;
using DotNetCore.CAP;
using Manulife.DNC.MSAD.WS.Events;
using Manulife.DNC.MSAD.WS.OrderService.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Manulife.DNC.MSAD.WS.OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public OrderDbContext DbContext { get; }
        public ICapPublisher CapPublisher { get; }
        public string ConnStr { get; } // For Dapper use

        public OrderRepository(OrderDbContext DbContext, ICapPublisher CapPublisher, string ConnStr)
        {
            this.DbContext = DbContext;
            this.CapPublisher = CapPublisher;
            this.ConnStr = ConnStr;
        }

        public async Task<bool> CreateOrderByEF(IOrder order)
        {
            using (var trans = DbContext.Database.BeginTransaction())
            {
                var orderEntity = new Orders()
                {
                    ID = GenerateOrderID(),
                    OrderUserID = order.OrderUserID,
                    OrderTime = DateTime.Now,
                    ProductID = order.ProductID // For demo use
                };

                DbContext.Orders.Add(orderEntity);
                await DbContext.SaveChangesAsync();

                var orderMessage = new OrderMessage()
                {
                    ID = orderEntity.ID,
                    OrderUserID = orderEntity.OrderUserID,
                    OrderTime = orderEntity.OrderTime,
                    ProductID = orderEntity.ProductID // For demo use
                };
                
                await CapPublisher.PublishAsync(EventConstants.EVENT_NAME_CREATE_ORDER, orderMessage);

                Console.WriteLine("发送MQ成功！");

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<bool> CreateOrderByDapper(IOrder order)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    // business code here
                    string sqlCommand = @"INSERT INTO [dbo].[Order](ID, OrderTime, OrderUserID, ProductID)
                                                                VALUES(@ID, @OrderTime, @OrderUserID, @ProductID)";

                    order.ID = GenerateOrderID();
                    await conn.ExecuteAsync(sqlCommand, param: new
                    {
                        ID = order.ID,
                        OrderTime = DateTime.Now,
                        OrderUserID = order.OrderUserID,
                        ProductID = order.ProductID
                    }, transaction: trans);

                    // For Dapper/ADO.NET, need to pass transaction
                    var orderMessage = new OrderMessage()
                    {
                        ID = order.ID,
                        OrderUserID = order.OrderUserID,
                        OrderTime = order.OrderTime,
                        OrderItems = null,
                        MessageTime = DateTime.Now,
                        ProductID = order.ProductID // For demo use
                    };

                    await CapPublisher.PublishAsync(EventConstants.EVENT_NAME_CREATE_ORDER, orderMessage, trans);

                    //throw new Exception("异常了");

                    trans.Commit();
                }
            }

            return true;
        }

        private string GenerateOrderID()
        {
            // TODO: Some business logic to generate Order ID
            return Guid.NewGuid().ToString();
        }

        private string GenerateEventID()
        {
            // TODO: Some business logic to generate Order ID
            return Guid.NewGuid().ToString();
        }
    }
}
