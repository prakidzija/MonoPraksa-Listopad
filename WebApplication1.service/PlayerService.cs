using WebApplication1.model;
using WebApplication1.repository;

namespace WebApplication1.service
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAllAsync();
        Task<List<Player>> GetAllPlayers();
        Task<List<Club>> GetAllClubs();

        Task<Player?> GetByIdAsync(Guid id);

        Task<List<Player>> GetByFilterAsync(
            string? name,
            string? position,
            string? currentClub
        );

        Task<bool> CreateAsync(Player player);

        Task<bool> UpdateAsync(Guid id, Player updatedPlayer);

        Task<bool> DeleteAsync(Guid id);
    }


    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<Player>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<Player>> GetAllPlayers()
        {
            return await _repository.GetAllPlayers();
        }

        public async Task<List<Club>> GetAllClubs()
        {
            return await _repository.GetAllClubs();
        }


        public async Task<Player?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task<List<Player>> GetByFilterAsync(
            string? name,
            string? position,
            string? currentClub)
        {
            IEnumerable<Player> foundPlayers =
                await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(name))
            {
                foundPlayers = foundPlayers
                    .Where(p => p.Name == name);
            }

            if (!string.IsNullOrEmpty(position))
            {
                foundPlayers = foundPlayers
                    .Where(p => p.Position == position);
            }

            if (!string.IsNullOrEmpty(currentClub))
            {
                foundPlayers = foundPlayers.Where(p =>
                    p.PlayerRegistrations.Any(r =>
                        r.Club != null &&
                        r.Club.Name == currentClub
                    )
                );
            }

            return foundPlayers.ToList();
        }


        public async Task<bool> CreateAsync(Player player)
        {
            await _repository.AddAsync(player);

            return true;
        }


        public async Task<bool> UpdateAsync(
            Guid id,
            Player updatedPlayer)
        {
            Player? player =
                await _repository.GetByIdAsync(id);

            if (player == null)
            {
                return false;
            }

            player.Name = updatedPlayer.Name;
            player.Position = updatedPlayer.Position;
            player.Age = updatedPlayer.Age;

            await _repository.UpdateAsync(player);

            return true;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            Player? player =
                await _repository.GetByIdAsync(id);

            if (player == null)
            {
                return false;
            }

            await _repository.DeleteAsync(player);

            return true;
        }
    }
}
