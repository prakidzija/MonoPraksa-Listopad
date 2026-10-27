using System.ComponentModel.DataAnnotations;

namespace VolleyballApp.Model;

public class CreatePlayerRegistration
{
    public Guid PlayerId { get; set; }
    public Guid ClubId { get; set; }

    [Required]
    [RegularExpression("^(permanent|loan|dual_license)$")]
    public string RegistrationType { get; set; } = "";

    [Range(1, 99)]
    public int JerseyNumber { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
