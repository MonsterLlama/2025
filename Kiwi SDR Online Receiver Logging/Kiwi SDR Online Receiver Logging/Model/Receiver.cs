using System.ComponentModel.DataAnnotations;

namespace MonsterLlama.KiwiSDR.Web.Logger.Model
{
    public class Receiver
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ReceiverId { get; set; }

        [Url]
        public string URL { get; set; } = String.Empty;

        public string? Location { get; set; }

        public string? Grid { get; set; }

        public double ASL { get; set; }

        public string? Antenna { get; set; }

        [Required]
        public string Name { get; set; } = String.Empty;

    }
}
