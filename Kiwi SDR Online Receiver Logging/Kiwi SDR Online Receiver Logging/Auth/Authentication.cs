using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth
{
    public static class Authentication
    {

        public static async Task<bool> VerifyJsonWebToken(string? jwtToken, string secretKey)
        {
            if (String.IsNullOrWhiteSpace(jwtToken) || String.IsNullOrWhiteSpace(secretKey)) { return false; }

            // Needed for IssuerSigningKey
            if (jwtToken.StartsWith("Bearer "))
                jwtToken = jwtToken.Substring("Bearer ".Length).Trim();

            var keyBytes    = Encoding.UTF8.GetBytes(secretKey);
            var securityKey = new SymmetricSecurityKey(keyBytes);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = securityKey,
                ValidateLifetime         = true,
                ValidateIssuer           = false,
                ValidateAudience         = false,
                ClockSkew                = TimeSpan.Zero
            };

            var result = await new JsonWebTokenHandler().ValidateTokenAsync(jwtToken, validationParameters);

            return result.IsValid;
        }

        public static bool VerifyJwtToken(string? jwtToken, string secretKey)
        {
            if (String.IsNullOrWhiteSpace(jwtToken)) return false;

            // Needed for IssuerSigningKey
            if (jwtToken.StartsWith("Bearer"))
                jwtToken = jwtToken.Substring(6).Trim();

            var key         = Encoding.ASCII.GetBytes(secretKey);
            var securityKey = new SymmetricSecurityKey(key);


            SecurityToken validatedToken;

            try
            {
                // Validate Token
                new JwtSecurityTokenHandler()
                .ValidateToken
                (
                    token:jwtToken,
                    validationParameters: new TokenValidationParameters
                    {
                        ClockSkew                = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer           = false,
                        ValidateAudience         = false,
                        ValidateLifetime         = true,
                        IssuerSigningKey         = securityKey
                    },
                    validatedToken: out validatedToken
                );

            }
            catch (SecurityTokenException) { return false; }
            catch { throw; }

            return validatedToken is not null;
        }

        public static string CreateJsonWebToken(WebApiCredentials creds, string secretKey, out DateTime ValidTo)
        {
            // Create and populate the Claims Dictionary<string, object>
            var claims = new Dictionary<string, object>
            {
                ["ClientName"]          = creds.ClientName,
                ["CanReadReceivers"]    = creds.CanReadReceivers,
                ["CanReadLogEntries"]   = creds.CanReadLogEntries,
                ["CanAddReceiver"]      = creds.CanAddReceiver,
                ["CanAddLogEntry"]      = creds.CanAddLogEntry
            };

            var keyBytes           = Encoding.UTF8.GetBytes(secretKey);
            var securityKey        = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Create Security Token Descriptor needed by the JsonWebToken constructor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials  = signingCredentials,
                Claims              = claims,
                NotBefore           = DateTime.UtcNow,
                Expires             = DateTime.UtcNow.AddDays(1) 
            };

            ValidTo = tokenDescriptor.Expires.Value;

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }


        public static string CreateJwtToken(WebApiCredentials creds, string secretKey, out DateTime ValidTo)
        {
            // Create and populate the IEnumerable<Claim> object for the Jwt SecurityToken
            var claims = new List<Claim>();

            claims.Add(new Claim("ClientName",        creds.ClientName));
            claims.Add(new Claim("CanReadReceivers",  creds.CanReadReceivers));
            claims.Add(new Claim("CanReadLogEntries", creds.CanReadLogEntries));
            claims.Add(new Claim("CanAddReceiver",    creds.CanAddReceiver));
            claims.Add(new Claim("CanAddLogEntry",    creds.CanAddLogEntry));

            // Create the SigningCredentials object for the Jwt SecurityToken
            var securityKey        = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey ?? string.Empty));
            var signingCredentials = new SigningCredentials(key: securityKey, algorithm: SecurityAlgorithms.HmacSha256Signature);

            // Create Jwt Security Token
            var jwt = new JwtSecurityToken(
                claims:             claims,
                notBefore:          DateTime.UtcNow,
                expires:            DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            ValidTo = jwt.ValidTo;

            return token;
        }
    }
}
