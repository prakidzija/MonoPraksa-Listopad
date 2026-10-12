using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.model;

namespace VolleyballApp.repository
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
