using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MonsterLlama.KiwiSDR.Web.Logger.Data
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationIdentityUser>
    {
        public ApplicationIdentityDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
    }
}
