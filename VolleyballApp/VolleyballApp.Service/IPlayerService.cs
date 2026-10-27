using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.Model;

namespace VolleyballApp.Service
{
    public interface IPlayerService
    {
        Task<List<PlayerDto>> GetAllPlayersAsync();
        Task<List<Club>> GetAllClubsAsync();
        Task<List<PlayerRegistration>> GetAllRegistrationsAsync();
        Task<Guid> CreateRegistrationAsync(CreatePlayerRegistration request);

        Task<Player?> GetByIdAsync(Guid id);

        Task<List<Player>> GetByFilterAsync(
            string? name,
            string? position,
            string? currentClub
        );

        Task CreateAsync(Player player);

        Task UpdateAsync(Guid id, Player updatedPlayer);

        Task DeleteAsync(Guid id);
    }
}
