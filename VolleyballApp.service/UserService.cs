using Microsoft.AspNetCore.Identity;
using VolleyballApp.model;
using VolleyballApp.repository;

namespace VolleyballApp.service
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterUser request);
        Task<User?> LoginAsync(LoginUser request);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task RegisterAsync(RegisterUser request)
        {
            var existingUsername = await _userRepository
                .GetByUsernameAsync(request.Username);

            if (existingUsername != null)
            {
                throw new InvalidOperationException(
                    "Username already exists.");
            }

            var existingEmail = await _userRepository
                .GetByEmailAsync(request.Email);

            if (existingEmail != null)
            {
                throw new InvalidOperationException(
                    "Email already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                Role = "USER"
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                request.Password
            );

            await _userRepository.AddAsync(user);
        }

        public async Task<User?> LoginAsync(LoginUser request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
    );

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;
        }
    }
}
