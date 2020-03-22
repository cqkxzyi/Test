using System;
using System.Collections.Generic;

namespace Comm
{
    public interface IOrder
    {
        string ID { get; set; }

        DateTime OrderTime { get; set; }

        string OrderUserID { get; set; }

        string ProductID { get; set; } 

    }
}
