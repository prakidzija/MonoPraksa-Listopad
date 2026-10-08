using System;
using System.Collections.Generic;

namespace WebApplication1.model;

public partial class PlayerRegistration
{
    public Guid Id { get; set; }

    public Guid PlayerId { get; set; }

    public Guid ClubId { get; set; }

    public string RegistrationType { get; set; } = null!;

    public int JerseyNumber { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Club Club { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
