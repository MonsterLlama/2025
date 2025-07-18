using Microsoft.AspNetCore.Mvc;

namespace Kiwi_SDR_Online_Receiver_Logging.Controllers
{
    [Route("api/[controller]")]
    public class Receivers : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllServers()
        {
            // Test Return value
            return Ok("GetAllServers()");
        }
    }
}
