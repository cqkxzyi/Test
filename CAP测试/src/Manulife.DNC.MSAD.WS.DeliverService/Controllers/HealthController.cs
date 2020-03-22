using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Manulife.DNC.MSAD.WS.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manulife.DNC.MSAD.WS.DeliveryService.Controllers
{
    [Produces("application/json")]
    [Route("api/Health")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Ok("ok");

        //[NonAction]
        //[CapSubscribe(EventConstants.EVENT_NAME_CREATE_ORDER)]
        //public void ReceiveMessage(DateTime time)
        //{
        //    Console.WriteLine("message time is:" + time);
        //}

        //[CapSubscribe(EventConstants.EVENT_NAME_CREATE_ORDER)]
        //public void TestSubscribe(string date)
        //{
        //    Console.WriteLine($"接收到订阅:{date}");
        //}
    }


}