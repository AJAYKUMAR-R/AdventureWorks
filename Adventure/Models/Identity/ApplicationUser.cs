using Microsoft.AspNetCore.Identity;

namespace Adventure.Models.Identity
{
    //we are adding the additional column to the asp.net user
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }

}
