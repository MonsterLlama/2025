using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MonsterLlama.KiwiSDR.Web.Logger.Data;

namespace MonsterLlama.KiwiSDR.Web.Logger.AUTH
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authenticate : ControllerBase
    {
        private readonly SignInManager<ApplicationIdentityUser> signInManager;

        public Authenticate(SignInManager<ApplicationIdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> AuthenticateUserAsync([FromBody] ApplicationIdentityUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

           var result = await this.signInManager.PasswordSignInAsync(user: user, password: user.ClientSecret, isPersistent: true, lockoutOnFailure: false);

           return Ok("result");
        }
    }
}
