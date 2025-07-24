using MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonsterLlama.KiwiSDR.Web.Logger.Model
{
    public class LogEntry
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long LogId { get; set; }

        [Required]
        [ForeignKey(nameof(Receiver.ReceiverId))]
        public Receiver? ReceiverId { get; set; }

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

        public string? Comment { get; set; }
    }
}
