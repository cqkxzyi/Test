using System;
using System.Collections.Generic;

namespace Comm
{
    public class OrderMessage : IOrder
    {
        public string ID { get; set; }

        public DateTime OrderTime { get; set; }

        public string OrderUserID { get; set; }

        public string ProductID { get; set; } // 演示字段，实际应该在OrderItems中
    }
}
