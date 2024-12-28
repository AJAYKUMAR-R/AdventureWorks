
﻿using System.ComponentModel.DataAnnotations;

namespace Adventure.Model.Authentcation;

public class Login
{
    [Required]
    [DataType(DataType.EmailAddress, ErrorMessage = "Email address is missing or invalid.")]
    public string? UserName { get; set; }

    [Required]
    [DataType(DataType.Password, ErrorMessage = "Incorrect or missing password.")]
    public string? Password { get; set; }

    [Required]
    public bool RememberMe { get; set; } = false;

}