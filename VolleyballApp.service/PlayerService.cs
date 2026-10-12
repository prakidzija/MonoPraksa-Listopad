using VolleyballApp.model;
using VolleyballApp.repository;

namespace VolleyballApp.service
{
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
            return await _repository.GetAllPlayersAsync();
        }

        public async Task<List<Club>> GetAllClubs()
        {
            return await _repository.GetAllClubsAsync();
        }

        public async Task<List<PlayerRegistration>> GetAllRegistrations()
        {
            return await _repository.GetAllRegistrationsAsync();
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
            return await _repository.GetByFilterAsync(name, position, currentClub);
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
