using Microsoft.AspNetCore.Mvc;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Data;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Filters;
using MonsterLlama.KiwiSDR.Web.Logger.Model;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LogEntryController : ControllerBase
    {
        private readonly KiwiSdrDbContext dbContext;

        public LogEntryController(KiwiSdrDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [RequiredClaimFilter("CanReadLogEntries", "true")]
        public IActionResult GetAllLogEntries()
        {
            var logEntries = this.dbContext.Set<LogEntry>();

            return Ok(logEntries);
        }

        [HttpGet("pfx={prefix}")]
        [RequiredClaimFilter("CanReadLogEntries", "true")]
        public IActionResult GetAllLogByPrefix(string prefix)
        {
            var logEntries = this.dbContext.Set<LogEntry>().Where(entry => entry.Callsign.ToLower().StartsWith(prefix.ToLower()));

            if (logEntries is null)
            {
                return NotFound();
            }

            return Ok(logEntries);
        }

        [HttpPost]
        [RequiredClaimFilter("CanAddLogEntry", "true")]
        public async Task<IActionResult> AddLogEntryAsync([FromBody] LogEntry logEntry)
        {
            if (!ModelState.IsValid || logEntry is null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                this.dbContext.Set<LogEntry>().Add(logEntry);

                await this.dbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                ModelState.AddModelError("Add Log Entry", e.Message);
                return BadRequest(ModelState);
            }
            
            return Ok();
        }

    }
}
