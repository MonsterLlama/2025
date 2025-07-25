using Microsoft.AspNetCore.Mvc;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Data;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Filters;
using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Model;

namespace Kiwi_SDR_Online_Receiver_Logging.Controllers
{
    [Route("api/[controller]")]
    [ValidateJwtSecurityTokenFilter]
    public class ReceiversController : ControllerBase
    {
        private readonly KiwiSdrDbContext kiwiSdrDbContext;

        public ReceiversController(KiwiSdrDbContext kiwiSdrDbContext) 
        {
            this.kiwiSdrDbContext = kiwiSdrDbContext;
        }

        [HttpGet]
        [RequiredClaimFilter("CanReadLogEntries", "true")]
        public IActionResult GetAllServers()
        {
            var receiver = this.kiwiSdrDbContext.Set<Receiver>();

            return Ok(receiver);
        }

        [HttpGet("id={id}")]
        [RequiredClaimFilter("CanReadLogEntries", "true")]
        public IActionResult GetReceiverById(int id)
        {
            var receiver = this.kiwiSdrDbContext.Set<Receiver>().FirstOrDefault(rcvr => rcvr.ReceiverId == id);

            if (receiver is null)
            {
                return NotFound();
            }

            // Test Return value
            return Ok(receiver);
        }
    }
}
