using System;

namespace Comm
{
    public interface IDelivery
    {
        string ID { get; set; }
        string OrderID { get; set; }
        string OrderUserID { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime UpdatedTime { get; set; }
    }
}
