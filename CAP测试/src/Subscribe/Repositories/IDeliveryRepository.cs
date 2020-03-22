using Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Push.Repositories
{
    public interface IDeliveryRepository
    {
        Task<bool> PublishAsync();
    }
}
