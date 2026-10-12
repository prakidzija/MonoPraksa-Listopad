using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.model;

namespace VolleyballApp.service
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterUser request);
        Task<User?> LoginAsync(LoginUser request);
    }
}
