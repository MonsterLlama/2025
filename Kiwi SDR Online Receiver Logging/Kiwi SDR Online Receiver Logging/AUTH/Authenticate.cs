using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth.Model;
using MonsterLlama.KiwiSDR.Web.Logger.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonsterLlama.KiwiSDR.Web.Logger.AUTH
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authenticate : ControllerBase
    {
        private readonly AuthenticationDbContext db;
        private readonly IConfiguration configuration;

        public Authenticate(AuthenticationDbContext authenticationDbContext, IConfiguration configuration)
        {
            this.db = authenticationDbContext;
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult AuthenticateClient([FromBody] WebApiCredentials creds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (creds is null || String.IsNullOrWhiteSpace(creds.ClientId) || String.IsNullOrWhiteSpace(creds.ClientSecret))
            {
                return BadRequest("You're unauthorized to access this resource.");
            }

            // Validate Credentials
            var credsFound = db.Set<WebApiCredentials>().Any(c => c.ClientId == creds.ClientId && c.ClientSecret == creds.ClientSecret);

            if(!credsFound)
            {
                return Unauthorized("You're unauthorized to access this resource.");
            }

            // Create and populate the IEnumerable<Claim> object for the Jwt SecurityToken
            var claims = new List<Claim>();

            claims.Add(new Claim("ClientName",       creds.ClientName));
            claims.Add(new Claim("CanReadReceivers", creds.CanReadReceivers.ToString()));
            claims.Add(new Claim("CanReadReceivers", creds.CanReadLogEntries.ToString()));
            claims.Add(new Claim("CanReadReceivers", creds.CanAddReceiver.ToString()));
            claims.Add(new Claim("CanReadReceivers", creds.CanAddLogEntry.ToString()));

            // Create the SigningCredentials object for the Jwt SecurityToken
            var secretKey          = this.configuration.GetValue<string>("SecretKey");
            var securityKey        = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey ?? String.Empty));
            var signingCredentials = new SigningCredentials(key: securityKey, algorithm: SecurityAlgorithms.HmacSha256Signature);

            // Create Jwt Security Token
            var jwt = new JwtSecurityToken(
                claims:             claims,
                notBefore:          DateTime.UtcNow,
                expires:            DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials
                );

            try
            {
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);

                return Ok(new {token, expires_at = jwt.ValidTo });
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
