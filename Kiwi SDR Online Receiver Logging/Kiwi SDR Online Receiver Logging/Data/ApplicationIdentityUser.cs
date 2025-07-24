using Microsoft.AspNetCore.Identity;

namespace MonsterLlama.KiwiSDR.Web.Logger.Data
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;
    }
}
