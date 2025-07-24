using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth.Model;
using MonsterLlama.KiwiSDR.Web.Logger.Data;

namespace MonsterLlama.KiwiSDR.Web.Logger.AUTH
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authenticate : ControllerBase
    {

        [HttpGet]
        public IActionResult AuthenticateClient([FromBody] WebApiCredentials creds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Validate Credentials

            // Create Jwt Security Token

           return Ok(creds);
        }
    }
}
