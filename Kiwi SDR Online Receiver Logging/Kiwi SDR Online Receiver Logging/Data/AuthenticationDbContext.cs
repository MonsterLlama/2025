using Microsoft.EntityFrameworkCore;

namespace MonsterLlama.KiwiSDR.Web.Logger.Data
{
    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
    }
}
