using Manulife.DNC.MSAD.WS.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manulife.DNC.MSAD.WS.OrderService.Models
{
    [Table("Order")]
    public class Orders : IOrder
    {

        [Column("ID", Order = 1, TypeName = "nvarchar(100)")]
        public string ID { get; set; }

        public DateTime OrderTime { get; set; }

        public string OrderUserID { get; set; }

        public string ProductID { get; set; }
    }
}
