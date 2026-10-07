using System;
using System.Data;
using WebApplication1.model;


namespace WebApplication1.repository
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetAll();
        Player? GetById(int id);
        void Add(Player player);
        void Update(Player player);
        void Delete(Player player);
    }

    public class PlayerRepository : IPlayerRepository
    {
        private readonly List<Player> players = new()
        {
            new Player
            {
                Id = 1,
                Name = "Ana",
                Position = "Outside",
                CurrentClub = "Porto"
            },

            new Player
            {
                Id = 2,
                Name = "Luna",
                Position = "Opposite",
                CurrentClub = "Porto"
            },

            new Player
            {
                Id = 3,
                Name = "Tara",
                Position = "Setter",
                CurrentClub = "Porto"
            },

            new Player
            {
                Id = 4,
                Name = "Mina",
                Position = "Libero",
                CurrentClub = "Porto"
            },

            new Player
            {
                Id = 5,
                Name = "Liana",
                Position = "Outside",
                CurrentClub = "Chervas"
            },

            new Player
            {
                Id = 6,
                Name = "Rene",
                Position = "Libero",
                CurrentClub = "Chervas"
            }
        };

        public IEnumerable<Player> GetAll()
        {
            return players;
        }

        public Player? GetById(int id)
        {
            return players.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Player player)
        {
            players.Add(player);
        }

        public void Update(Player player)
        {
            
        }

        public void Delete(Player player)
        {
            players.Remove(player);
        }
    }
}
