using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonsterLlama.KiwiSDR.Web.Logger.Model
{
    public class LogEntry
    {
        [Required]
        [Range(1, long.MaxValue)]
        [Key]
        public long LogEntryId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public string Callsign { get; set; } = string.Empty;

        [Required]
        [Range (1, double.MaxValue)]
        public double Frequency { get; set; }

        [Required]
        public string Band { get; set; } = string.Empty;

        [Required]
        public string Mode {  get; set; } = string.Empty;

        public string? RST { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly LogDate_UTC { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeOnly LogTime_UTC { get; set; }

        public string City { get; set; } = String.Empty;

        public string State { get; set; } = String.Empty;

        public string Country { get; set; } = String.Empty;

        public string Continent { get; set; } = String.Empty;

        public string Comment { get; set; } = String.Empty;
    }
}
