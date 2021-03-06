﻿using System;
using System.Collections.Generic;

namespace Manulife.DNC.MSAD.WS.Events
{
    public class OrderMessage : IOrder
    {
        public string ID { get; set; }

        public DateTime OrderTime { get; set; }

        public List<OrderItems> OrderItems { get; set; }

        public string OrderUserID { get; set; }

        public string ProductID { get; set; } // 演示字段，实际应该在OrderItems中

        public DateTime MessageTime { get; set; }
    }
}
