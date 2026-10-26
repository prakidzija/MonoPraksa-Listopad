﻿using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.Model;

namespace VolleyballApp.Service
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
