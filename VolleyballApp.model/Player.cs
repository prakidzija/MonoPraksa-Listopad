using System;
using System.Collections.Generic;

namespace VolleyballApp.model;

public partial class Player
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<PlayerRegistration> PlayerRegistrations { get; set; } = new List<PlayerRegistration>();
}
