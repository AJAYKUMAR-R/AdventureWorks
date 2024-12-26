
using Adventure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Adventure.Model.Authentcation;

public class Register
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Email address is missing or invalid.")]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password, ErrorMessage = "Incorrect or missing password.")]
    public string? Password { get; set; }

    [Required] 
    public Role Role { get; set; }

    [Required] 
    public ClaimsPermissions Claim { get; set; }

}