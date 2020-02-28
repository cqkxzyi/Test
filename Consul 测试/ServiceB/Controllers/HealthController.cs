using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class HealthController : Controller
    {
        [HttpGet("status")]
        public IActionResult Status() => Ok();
    }
}
