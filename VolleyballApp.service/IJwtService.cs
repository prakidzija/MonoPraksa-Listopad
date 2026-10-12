using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.model;

namespace VolleyballApp.service
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
