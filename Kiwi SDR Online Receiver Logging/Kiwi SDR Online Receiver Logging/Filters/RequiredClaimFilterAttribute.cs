using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredClaimFilterAttribute : ActionFilterAttribute
    {
        public RequiredClaimFilterAttribute(string claimType, string claimValue)
        {
            ClaimType  = claimType;
            ClaimValue = claimValue;
        }

        public string ClaimType { get; }
        public string ClaimValue { get; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            IEnumerable<Claim>? claims = null;

            try
            {
                claims = GetClaimsFromToken(context);
            }
            catch
            {
                context.ModelState.AddModelError("Claims", "Unable to authentication permissions.");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };

                context.Result = new UnauthorizedObjectResult(problemDetails);
            }

            if (claims is null || claims is not List<Claim>)
            {
                context.ModelState.AddModelError("Claims", "Unauthorized");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };

                context.Result = new UnauthorizedObjectResult(problemDetails);
            }
            else
            {
                var claimFound = ((List<Claim>)claims).Any(c => c.Type == ClaimType && c.Value == ClaimValue);

                if (!claimFound) 
                {
                    context.ModelState.AddModelError("Claims", "Unauthorized");

                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status401Unauthorized
                    };

                    context.Result = new UnauthorizedObjectResult(problemDetails);
                }
            }

        }

        private IEnumerable<Claim>? GetClaimsFromToken(ActionExecutingContext context)
        {
            //context.HttpContext.Items.TryGetValue("Claims", out var claims);

            var token = context.HttpContext.Request.Headers.Authorization;


            if (token.Count > 0)
            {
                var access_token = token[0];

                if (!string.IsNullOrWhiteSpace(access_token) && access_token.StartsWith("Bearer"))
                {
                    access_token = access_token.Substring(6).Trim();
                }

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var claims = handler.ReadJwtToken(access_token).Claims;

                return claims;
            }
            else
            {
                return null;
            }
        }
    }
}
