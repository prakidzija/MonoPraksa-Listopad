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
        public IActionResult GetAll()
        {
            return Ok(_playerService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Player? player = _playerService.GetById(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        [HttpGet("filter")]
        public IActionResult GetByFilter([FromQuery] string? name, [FromQuery] string? position, [FromQuery] string? currentClub)
        {
            var players = _playerService.GetByFilter(name, position, currentClub);

            return Ok(players);
        }
    
        [HttpPost]

        public IActionResult Create([FromBody] Player player)
        {
            bool created = _playerService.Create(player);

            if (!created)
                {
                    return Conflict("Player with this Id already exists!");
                }
            return Ok(player);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Player updatePlayer)
        {
            bool updated = _playerService.Update(id, updatePlayer);

            if (!updated)
            {
                return NotFound();
            }

            return Ok(updatePlayer);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = _playerService.Delete(id);

            if(!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
