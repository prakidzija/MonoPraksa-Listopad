using Microsoft.AspNetCore.Mvc;
using VolleyballApp.Model;
using VolleyballApp.Service;
using Microsoft.AspNetCore.Authorization;

namespace VolleyballApp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }


        [Authorize]
        [HttpGet("player")]
        public async Task<IActionResult> GetAllPlayersAsync()
        {
            var players = await _playerService.GetAllPlayersAsync();
            return Ok(players);
        }

        [Authorize]
        [HttpGet("club")]
        public async Task<IActionResult> GetAllClubsAsync()
        {
            var clubs = await _playerService.GetAllClubsAsync();
            return Ok(clubs);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("club")]
        public async Task<IActionResult> CreateClubAsync([FromBody] CreateClub request)
        {
            try
            {
                var id = await _playerService.CreateClubAsync(request);
                return StatusCode(201, new { id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("registration")]
        public async Task<IActionResult> GetAllRegistrationsAsync()
        {
            var registrations = await _playerService.GetAllRegistrationsAsync();
            return Ok(registrations);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("registration")]
        public async Task<IActionResult> CreateRegistrationAsync([FromBody] CreatePlayerRegistration request)
        {
            try
            {
                var id = await _playerService.CreateRegistrationAsync(request);
                return StatusCode(201, new { id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            Player? player = await _playerService.GetByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [Authorize]
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilterAsync(
            [FromQuery] string? name,
            [FromQuery] string? position,
            [FromQuery] string? currentClub)
        {
            var players = await _playerService.GetByFilterAsync(
                name,
                position,
                currentClub);


            return Ok(players);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] Player player)
        {
            await _playerService.CreateAsync(player);

            return StatusCode(201);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] Player updatedPlayer)
        {
            try
            {
                await _playerService.UpdateAsync(id, updatedPlayer);
                return Ok(updatedPlayer);
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

            
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _playerService.DeleteAsync(id);

                return StatusCode(200, "Player deleted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }
}
