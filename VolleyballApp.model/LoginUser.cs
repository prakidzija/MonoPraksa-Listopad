using System.ComponentModel.DataAnnotations;

namespace VolleyballApp.model;

public class LoginUser
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}