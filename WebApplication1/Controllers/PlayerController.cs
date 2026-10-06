using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private static List<Player> players = new List<Player>
        {
            new Player { Id = 1, Name = "Ana", Position = "Outside", CurrentClub = "Porto"},
            new Player { Id = 2, Name = "Lana", Position = "Opposite", CurrentClub = "Porto"},
            new Player { Id = 3, Name = "Mina", Position = "Setter", CurrentClub = "Porto"},
            new Player { Id = 4, Name = "Leona", Position = "Outside", CurrentClub = "Porto"},
            new Player { Id = 5, Name = "Diana", Position = "Setter", CurrentClub = "Porto"},
            new Player { Id = 6, Name = "Lu", Position = "Libero", CurrentClub = "Porto"},
            new Player { Id = 7, Name = "Enea", Position = "Outside", CurrentClub = "Chelto"},
            new Player { Id = 8, Name = "Rona", Position = "Opposite", CurrentClub = "Chelto"},
            new Player { Id = 9, Name = "Zena", Position = "Setter", CurrentClub = "Chelto"},
            new Player { Id = 10, Name = "Emina", Position = "Outside", CurrentClub = "Lisbin"},
            new Player { Id = 11, Name = "Tia", Position = "Libero", CurrentClub = "Lisbin"}
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
            return Ok(player);
        }

        [HttpGet("filter")]
        public IActionResult GetByFilter([FromQuery] string? name, [FromQuery] string? position, [FromQuery] string? currentClub)
        {
            IEnumerable<Player> foundPlayers = players;

            if (!string.IsNullOrEmpty(name))
            {
                foundPlayers = foundPlayers.Where(p => p.Name == name);
            }
            
            if (!string.IsNullOrEmpty(position))
            {
                foundPlayers = foundPlayers.Where(p => p.Position == position);
            }

            if (!string.IsNullOrEmpty(currentClub))
            {
                foundPlayers = foundPlayers.Where(p => p.CurrentClub == currentClub);
            }

            if(!foundPlayers.Any())
            {
                return NotFound();
            }

            return Ok(foundPlayers);
        }


        [HttpPost]
        public IActionResult Create([FromBody] Player player)
        {
            if (players.Any(p => p.Id == player.Id))
            {
                return Conflict("Id already exists!");
            }
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
            player.Position = updatePlayer.Position;
            player.CurrentClub = updatePlayer.CurrentClub;

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
