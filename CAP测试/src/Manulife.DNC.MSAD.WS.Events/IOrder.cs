using System;
using System.Collections.Generic;

namespace Manulife.DNC.MSAD.WS.Events
{
    public interface IOrder
    {
        string ID { get; set; }

        DateTime OrderTime { get; set; }

        string OrderUserID { get; set; }

        string ProductID { get; set; } 

    }
}
