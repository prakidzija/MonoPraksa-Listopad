﻿using System.ComponentModel.DataAnnotations;

namespace VolleyballApp.Model;

public class RegisterUser
{
    [Required(ErrorMessage = "Username is required!")]
    [StringLength(100, MinimumLength = 1)]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress(ErrorMessage = "Invalid email format!")]
    [MaxLength(120)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Username is required!")]
    [MinLength(8, ErrorMessage = "Password must contain at least 8 characters!")]
    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*[0-9]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter and one number!")]
    public string Password { get; set; } = null!;
}
