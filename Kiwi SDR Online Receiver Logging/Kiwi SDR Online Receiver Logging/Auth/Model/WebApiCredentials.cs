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
        public string ClientSecret { get; set; }= string.Empty;

        [Required]
        public string ClientName {  get; set; } = string.Empty;
    }
}
