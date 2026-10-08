using Microsoft.AspNetCore.Mvc;
using WebApplication1.model;
using WebApplication1.service;

namespace WebApplication1.controller
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

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var players = await _playerService.GetAllAsync();

            return Ok(players);
        }

        [HttpGet("player")]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _playerService.GetAllPlayers();
            return Ok(players);
        }

        [HttpGet("club")]
        public async Task<IActionResult> GetAllClubs()
        {
            var clubs = await _playerService.GetAllClubs();
            return Ok(clubs);
        }

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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromBody] Player player)
        {
            bool created = await _playerService.CreateAsync(player);

            if (!created)
            {
                return Conflict();
            }

            return Ok(player);
        }

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
