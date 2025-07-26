using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Filters
{
    public class ValidateJwtSecurityTokenFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //
            //  Check Request Headers for an "Authorization" key.
            //
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
                var securityKey   = configuration?.GetValue<string>("SecretKey") ?? String.Empty;

                if (!await Authentication.VerifyJsonWebToken(token, securityKey))
                {
                    context.Result = new UnauthorizedResult();
                }
            }

            await Task.CompletedTask;
        }
    }
}
