using System.ComponentModel.DataAnnotations;

namespace Adventure.Models.Authentcation
{
    public class TokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
