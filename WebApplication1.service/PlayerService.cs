using WebApplication1.model;
using WebApplication1.repository;

namespace WebApplication1.service
{

    public interface IPlayerService
    {
        IEnumerable<Player> GetAll();
        Player? GetById(int id);

        IEnumerable<Player> GetByFilter(
            string? name,
            string? position,
            string? currentClub
            );

        bool Create(Player player);
        bool Update(int id, Player updatedPlayer);
        bool Delete(int id);
    }
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Player> GetAll()
        {
            return _repository.GetAll();
        }

        public Player? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Player> GetByFilter(string? name, string? position, string? currentClub)
        {
            IEnumerable<Player> foundPlayers = _repository.GetAll();

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

            return foundPlayers;
        }

        public bool Create(Player player)
        {
            if (_repository.GetById(player.Id) != null)
            {
                return false;
            }

            _repository.Add(player);
            return true;
        }

        public bool Update(int id, Player updatedPlayer)
        {
            Player? player = _repository.GetById(id);

            if (player == null)
            {
                return false;
            }

            player.Name = updatedPlayer.Name;
            player.Position = updatedPlayer.Position;
            player.CurrentClub = updatedPlayer.CurrentClub;

            _repository.Update(player);

            return true;
        }

        public bool Delete(int id)
        {
            Player? player = _repository.GetById(id);

            if (player == null)
            {
                return false;
            }

            _repository.Delete(player);

            return true;
        }
    }
}
