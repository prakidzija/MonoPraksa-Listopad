using System.ComponentModel.DataAnnotations;

namespace VolleyballApp.Model;

public class CreateClub
{
    [Required]
    [StringLength(30)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(40)]
    public string Adress { get; set; } = "";
}
