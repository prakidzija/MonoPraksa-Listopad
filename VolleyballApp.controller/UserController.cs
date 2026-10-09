using Microsoft.AspNetCore.Mvc;
using VolleyballApp.model;
using VolleyballApp.service;

namespace VolleyballApp.controller
{
    [ApiController]
    [Route("auth")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(
            IUserService userService,
            IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUser request)
        {
            try
            {
                await _userService.RegisterAsync(request);

                return StatusCode(201, "User registered successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginUser request)
        {
            var user = await _userService.LoginAsync(request);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                Token = token
            });
        }

    }
}
