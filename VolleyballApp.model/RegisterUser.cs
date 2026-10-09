using System.ComponentModel.DataAnnotations;

namespace VolleyballApp.model;

public class RegisterUser
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
