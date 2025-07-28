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


        [HttpPost]
        [RequiredClaimFilter("CanAddReceiver", "true")]
        public IActionResult AddReceiver([FromBody] Receiver newReceiver)
        {
            // Validate ModelState
            if (!ModelState.IsValid)
            {
                return BadRequest("Failed to add Receiver. Invalid ModelState!");
            }

            // Validate Input (Receiver object)
            if (newReceiver is null || newReceiver is not Receiver)
            {
                ModelState.AddModelError("Receiver", "Failed to add Receiver. Invalid Input!");
                return BadRequest(ModelState);
            }

            // Trim trailing forward slash. This is a business logic decision to better avoid duplicate names.
            newReceiver.URL = newReceiver.URL.TrimEnd('/').Trim();

            // Validate that the Receiver Doesn't already exist
            if(this.kiwiSdrDbContext.Set<Receiver>().Any(r => r.URL == newReceiver.URL))
            {
                ModelState.AddModelError("Receiver", $"Cannot add a new Receiver w/ the URL {newReceiver.URL}! Receiver already exists!");
                return BadRequest(ModelState);
            }

            // Add new Receiver
            try
            {
                this.kiwiSdrDbContext.Add(newReceiver);
                this.kiwiSdrDbContext.SaveChanges();
            }
            catch(Exception e)
            {
                ModelState.AddModelError("Receiver", e.Message);
                return BadRequest(ModelState);
            }

            return Ok(newReceiver);
        }

    }
}
