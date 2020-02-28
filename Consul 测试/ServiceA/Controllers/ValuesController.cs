using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "value1AAA";
        }
    }
}
