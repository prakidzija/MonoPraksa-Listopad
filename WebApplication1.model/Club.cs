using System;
using System.Collections.Generic;

namespace WebApplication1.model;

public partial class Club
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public virtual ICollection<PlayerRegistration> PlayerRegistrations { get; set; } = new List<PlayerRegistration>();
}
