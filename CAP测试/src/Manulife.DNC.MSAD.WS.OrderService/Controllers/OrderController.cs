using Manulife.DNC.MSAD.WS.OrderService.Models;
using Manulife.DNC.MSAD.WS.OrderService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Manulife.DNC.MSAD.WS.OrderService.Controllers
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
        /// 这是一个带参数的get请求Test3
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
            if (orderDTO == null || orderDTO.ID==null)
                return "解析参数异常";

            // var result = OrderRepository.CreateOrderByDapper(orderDTO).GetAwaiter().GetResult();
            var result = await OrderRepository.CreateOrderByDapper(orderDTO);

            return result ? "Post Order Success" : "Post Order Failed";
        }

        [HttpPost]
        public string PostByEf([FromBody]OrderDTO orderDTO)
        {
            if (orderDTO == null || orderDTO.ID == null)
                return "解析参数异常";

            // var result = OrderRepository.CreateOrderByDapper(orderDTO).GetAwaiter().GetResult();
            //var result = await OrderRepository.CreateOrderByDapper(orderDTO);

            var result = OrderRepository.CreateOrderByEF(orderDTO).GetAwaiter().GetResult();

            return result ? "Post Order Success" : "Post Order Failed";
        }
    }
}