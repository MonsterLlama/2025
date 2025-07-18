using Kiwi_SDR_Online_Receiver_Logging.Model;
using Microsoft.AspNetCore.Mvc;

namespace Kiwi_SDR_Online_Receiver_Logging.Controllers
{
    [Route("api/[controller]")]
    public class Receivers : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllServers()
        {
            var receiver = new Receiver()
            {
                UrlId    = 1,
                URL      = "http://kb6c.proxy.kiwisdr.com:8073/",
                Antenna  = "Wellbrooke Loop",
                Name     = "KB6C/6",
                Grid     = "DM04kr",
                Location = "Stauffer, California",
                ASL      = 1585
            };

            // Test Return value
            return Ok(receiver);
        }
    }
}
