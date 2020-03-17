using Manulife.DNC.MSAD.WS.Events;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Push.Models;
using Push.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Push.Controllers
{
    //[Produces("application/json")]
    [Route("api/Order/[action]")]
    public class OrderController : Controller
    {
        public IOrderRepository OrderRepository { get; }

        public OrderController(IOrderRepository OrderRepository)
        {
            this.OrderRepository = OrderRepository;
        }

        [HttpGet]
        public string Get1(string ID)
        {
            if (ID == null)
                return "解析参数异常";

            return "成功！！";
        }

        
        [HttpGet]
        public string Get2(OrderDTO2 orderDTO)
        {
            if (orderDTO==null ||orderDTO.ID == null)
                return "解析参数异常";


            return "成功！！";
        }

        //Content-Type:application/json
        [HttpGet]
        public string FromBody([FromBody]OrderDTO2 orderDTO)
        {
            if (orderDTO == null || orderDTO.ID == null)
                return "解析参数异常";

            return "成功！！";
        }

        [HttpGet]
        public string FromForm([FromForm]OrderDTO2 orderDTO)
        {
            if (orderDTO == null || orderDTO.ID == null)
                return "解析参数异常";

            return "成功！！";
        }

        /// <summary>
        /// 这是一个PostBySql函数
        /// </summary>
        /// <remarks>
        /// 例子:Get api/Test/Test3/zhangyi
        /// </remarks>
        /// <param name="orderDTO">请求实体</param>
        /// <returns>是否成功</returns> 
        /// <response code="200">成功啦 哈哈</response>
        /// <response code="201">201错误</response>
        /// <response code="400">400错误</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<string> PostBySql([FromBody]OrderDTO orderDTO)
        {
            try
            {
                if (orderDTO == null || orderDTO.ID == null)
                    return "解析参数异常";

                // var result = OrderRepository.CreateOrderByDapper(orderDTO).GetAwaiter().GetResult();
                var result = await OrderRepository.CreateOrderByDapper(orderDTO);

                return result ? "Post Order Success" : "Post Order Failed";
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
           
        }

        [HttpPost]
        public string PostByEf([FromBody]OrderDTO orderDTO)
        {
            List<OrderItems> list = new List<OrderItems> {
                new OrderItems(){ ID="1"},
                new OrderItems(){ ID="2" }
            };
            OrderDTO a = new OrderDTO();
            a.ID = "11";
            a.OrderItems = list;
            string bb=JsonConvert.SerializeObject(a);



            if (orderDTO == null || orderDTO.ID == null)
                return "解析参数异常";

            var result = OrderRepository.CreateOrderByEF(orderDTO).GetAwaiter().GetResult();

            return result ? "Post Order Success" : "Post Order Failed";
        }
    }
}