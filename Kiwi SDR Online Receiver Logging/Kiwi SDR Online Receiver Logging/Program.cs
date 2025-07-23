using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MonsterLlama.KiwiSDR.Web.Logger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Adds services for controllers to the services collection..
            builder.Services.AddControllers();

            var secretKey = builder.Configuration.GetValue<string>("SecretKey");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters 
                    { 
                        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey ?? String.Empty)),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime         = true,
                        ValidateAudience         = false,
                        ValidateIssuer           = false,
                        ClockSkew                = TimeSpan.Zero
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // Adds endpoints for controller actions..
            app.MapControllers();

            app.Run();
        }
    }
}
