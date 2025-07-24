using System.ComponentModel.DataAnnotations;

namespace MonsterLlama.Kiwi_SDR_Online_Receiver_Logging.Auth.Model
{
    /// <summary>
    /// Represents the login credentials to this Web API.
    /// </summary>
    public class WebApiCredentials
    {
        [Required]
        [Length(minimumLength:36, maximumLength:36, ErrorMessage ="Client Id must have 36 characters.")]
        public string ClientId { get; set; } = string.Empty;

        [Required]
        [Length(minimumLength: 36, maximumLength: 36, ErrorMessage = "Client Secret must have 36 characters.")]
        public string ClientSecret { get; set; }= string.Empty;

        [Required]
        public string ClientName {  get; set; } = string.Empty;

        [Required]
        public bool CanReadReceivers { get; set; } = false;

        [Required]
        public bool CanAddReceiver { get;set; } = false;

        [Required]
        public bool CanAddLogEntry { get; set; } = false;

        [Required]
        public bool CanReadLogEntries { get; set; } = false;
    }
}
