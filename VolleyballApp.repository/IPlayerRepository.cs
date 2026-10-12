using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.model;

namespace VolleyballApp.repository
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllAsync();
        Task<List<Player>> GetAllPlayersAsync();
        Task<List<Club>> GetAllClubsAsync();
        Task<List<PlayerRegistration>> GetAllRegistrationsAsync();
        Task<Player?> GetByIdAsync(Guid id);
        Task<List<Player>> GetByFilterAsync(
            string? name,
            string? position,
            string? currentClub);
        Task AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
    }
}
