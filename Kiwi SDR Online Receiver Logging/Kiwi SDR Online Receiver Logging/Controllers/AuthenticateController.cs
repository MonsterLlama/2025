using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth.Model;
using MonsterLlama.KiwiSDR.Web.Logger.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly AuthenticationDbContext db;
        private readonly IConfiguration configuration;

        public AuthenticateController(AuthenticationDbContext authenticationDbContext, IConfiguration configuration)
        {
            db = authenticationDbContext;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult AuthenticateClient([FromBody] WebApiCredentials creds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (creds is null || string.IsNullOrWhiteSpace(creds.ClientId) || string.IsNullOrWhiteSpace(creds.ClientSecret))
            {
                return BadRequest("You're unauthorized to access this resource.");
            }

            // Validate Credentials
            var credsFound = db.Set<WebApiCredentials>().Any(c => c.ClientId == creds.ClientId && c.ClientSecret == creds.ClientSecret);

            if(!credsFound)
            {
                return Unauthorized("You're unauthorized to access this resource.");
            }

            DateTime ValidTo; // It just looks cleaner up here.. ;)
            try
            {
                var secretKey =  configuration.GetValue<string>("SecretKey") ?? String.Empty;
                var token     = Authentication.CreateJwtToken(creds, secretKey, out ValidTo);

                return Ok(new {token, expires_at = ValidTo });
            }
            catch (SecurityTokenEncryptionFailedException e)
            {
                ModelState.AddModelError("JWT", e.Message);
                return BadRequest(ModelState);
            }
            catch(Exception e)
            {
                ModelState.AddModelError("JWT", e.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
