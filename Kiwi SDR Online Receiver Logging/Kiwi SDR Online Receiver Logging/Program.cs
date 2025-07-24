namespace Kiwi_SDR_Online_Receiver_Logging
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

            //
            //  Configure Jwt Bearer Token Authentication
            //
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



            var identityDbConnectionString = builder.Configuration.GetConnectionString("IdentityUsersDb");

            //
            //  Configure Identity EF Core
            //
            builder.Services
                .AddDbContext<ApplicationIdentityDbContext>(options => 
                {
                    options.UseSqlServer(connectionString: identityDbConnectionString);
                })
            ;

            //
            //  Configure Identity
            //
            builder.Services
                .AddIdentity<ApplicationIdentityUser, IdentityRole>(options => 
                {
                    options.Password.RequiredLength         = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase       = false;
                    options.Password.RequireLowercase       = true;
                    options.Password.RequireDigit           = true;
                    options.Password.RequiredUniqueChars    = 5;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(60);
                })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();



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
