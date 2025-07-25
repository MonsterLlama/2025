using Microsoft.EntityFrameworkCore;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Model;
using MonsterLlama.KiwiSDR.Web.Logger.Model;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Data
{
    public class KiwiSdrDbContext : DbContext
    {
        public KiwiSdrDbContext(DbContextOptions<KiwiSdrDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Receiver>().HasKey("ReceiverId");
            modelBuilder.Entity<LogEntry>().HasKey("LogEntryId");
        }
    }
}
