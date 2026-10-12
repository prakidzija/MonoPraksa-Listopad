using Microsoft.AspNetCore.Mvc;
using VolleyballApp.model;
using VolleyballApp.service;
using Microsoft.AspNetCore.Authorization;

namespace VolleyballApp.controller
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
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var players = await _playerService.GetAllAsync();

            return Ok(players);
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

        [Authorize]
        [HttpGet("registration")]
        public async Task<IActionResult> GetAllRegistrationsAsync()
        {
            var registrations = await _playerService.GetAllRegistrationsAsync();
            return Ok(registrations);
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
            bool created = await _playerService.CreateAsync(player);

            if (!created)
            {
                return Conflict();
            }

            return StatusCode(201);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] Player updatedPlayer)
        {
            bool updated = await _playerService.UpdateAsync(
                id,
                updatedPlayer);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(updatedPlayer);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            bool deleted = await _playerService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
