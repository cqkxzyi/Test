using Comm;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Subscribe.Models
{
    [Table("Deliveries")]
    public class Delivery : IDelivery
    {
        [Column("ID")]
        public string ID { get; set; }

        [Column("OrderID")]
        public string OrderID { get; set; }

        [Column("OrderUserID")]
        public string OrderUserID { get; set; }

        [Column("CreatedTime")]
        public DateTime CreatedTime { get; set; }

        [Column("UpdatedTime")]
        public DateTime UpdatedTime { get; set; }
    }
}
