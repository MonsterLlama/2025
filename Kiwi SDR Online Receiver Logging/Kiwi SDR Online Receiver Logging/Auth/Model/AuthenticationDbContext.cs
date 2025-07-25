using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth.Model
{
    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WebApiCredentials>().HasKey("ClientId");
        }

    }
}
