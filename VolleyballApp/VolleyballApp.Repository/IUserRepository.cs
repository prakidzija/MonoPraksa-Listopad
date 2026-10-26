﻿using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.Model;

namespace VolleyballApp.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
