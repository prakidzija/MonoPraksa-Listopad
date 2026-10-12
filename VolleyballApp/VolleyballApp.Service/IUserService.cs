﻿using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.Model;

namespace VolleyballApp.Service
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterUser request);
        Task<User?> LoginAsync(LoginUser request);
    }
}
