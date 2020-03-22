using System;

namespace Comm
{
    public interface IStorage
    {
        string ID { get; set; }
        int StorageNumber { get; set; }
        DateTime CreatedTime { get; set; }
    }
}
