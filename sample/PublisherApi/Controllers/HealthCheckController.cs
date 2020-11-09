using Microsoft.AspNetCore.Mvc;

namespace PublisherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("OK");
        }
    }
}