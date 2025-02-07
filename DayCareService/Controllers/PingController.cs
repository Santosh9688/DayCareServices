using Microsoft.AspNetCore.Mvc;

namespace DayCare.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public string Ping()
        {
            return "Hi, DayCare Service is up and running";

        }
    }
}
