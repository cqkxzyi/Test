using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comm;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Push.Repositories;

namespace Delivery.Controllers
{
    //[Produces("application/json")]
    [Route("api/Send/[action]")]
    public class SendController : Controller
    {
        public IDeliveryRepository SendRepository { get; }

        public SendController(IDeliveryRepository SendRepository)
        {
            this.SendRepository = SendRepository;
        }


        [HttpPost]
        public string Send()
        {
            var result = SendRepository.PublishAsync().GetAwaiter().GetResult();

            return result ? "发送成功" : "Post Order Failed";
        }
    }
}