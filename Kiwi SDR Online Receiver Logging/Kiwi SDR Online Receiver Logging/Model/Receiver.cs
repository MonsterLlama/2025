using System.ComponentModel.DataAnnotations;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Model
{
    public class Receiver
    {
        [Required]
        [Range(1, int.MaxValue)]
        [Key]
        public int ReceiverId { get; set; }

        [Url]
        public string URL { get; set; } = String.Empty;

        public string Location { get; set; } = String.Empty;

        public string City { get; set; } = String.Empty;

        public string State { get; set; } = String.Empty;

        public string Country { get; set; } = String.Empty;

        public string Continent  { get; set; } = String.Empty;

        public string Grid { get; set; } = String.Empty;

        public double ASL { get; set; }

        public string Antenna { get; set; } = String.Empty;

        [Required]
        public string Name { get; set; } = String.Empty;

        public string Comment { get; set; } = String.Empty;

    }
}
