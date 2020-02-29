using Microsoft.AspNetCore.Mvc;

namespace ServiceB.Controllers
{
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("/healthCheck")]
        public IActionResult Check() => Ok("ok");
    }
}
