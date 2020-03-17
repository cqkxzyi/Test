using Manulife.DNC.MSAD.WS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Push.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> CreateOrderByEF(IOrder order);
        Task<bool> CreateOrderByDapper(IOrder order);
    }
}
