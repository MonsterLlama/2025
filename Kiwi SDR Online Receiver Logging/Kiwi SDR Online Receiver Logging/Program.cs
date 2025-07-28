using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth.Model;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Data;
using System.Text;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Adds services for controllers to the services collection..
            builder.Services.AddControllers();


            //
            //  Add DbContext for the SDR Kiwi DB where Receivers and LogEntries are persisted
            //
            var sdrKiwiDbConnectionString = builder.Configuration.GetConnectionString("KiwiSdrDb");

            builder.Services.AddDbContext<KiwiSdrDbContext>(options =>
            {
                options.UseSqlServer(sdrKiwiDbConnectionString);
            });

            //
            //  Add Authentication DbContext
            //
            var authenticationDbConnectionString = builder.Configuration.GetConnectionString("AuthenticationDb");

            builder.Services.AddDbContext<AuthenticationDbContext>(options => 
            {
                options.UseSqlServer(authenticationDbConnectionString);
            });

            //
            //  Configure Jwt Bearer Token Authentication
            //
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
