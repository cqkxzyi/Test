using Manulife.DNC.MSAD.WS.Events;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manulife.DNC.MSAD.WS.StorageService.Models
{
    [Table("Storage")]
    public class Storage : IStorage
    {
        public string ID { get; set; }

        public int StorageNumber { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
