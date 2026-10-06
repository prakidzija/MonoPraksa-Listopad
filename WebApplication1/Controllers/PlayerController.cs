using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private static List<Player> players = new List<Player>
        {
            new Player { Id = 1, Name = "Ana"}
        };

        [HttpGet]        
        public IActionResult GetAll()
        {
            if (players.Count == 0)
            {
                return NotFound();
            }
            return Ok(players);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Player? player = players.FirstOrDefault(p => p.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(players[id]);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Player player)
        {
            players.Add(player);
            return Ok(players);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Player updatePlayer)
        {
            Player? player = players.FirstOrDefault(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            player.Name = updatePlayer.Name;

            return Ok(player);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Player? player = players.FirstOrDefault(p => p.Id == id);

            if(player == null)
            {
                return NotFound();
            }

            players.Remove(player);

            return Ok(player);
        }
    }
}
