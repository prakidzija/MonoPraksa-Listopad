using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.Model;

namespace VolleyballApp.Repository
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllAsync();
        Task<List<Player>> GetAllPlayersAsync();
        Task<List<Club>> GetAllClubsAsync();
        Task<List<PlayerRegistration>> GetAllRegistrationsAsync();
        Task<bool> ClubExistsAsync(Guid id);
        Task<bool> JerseyNumberExistsAsync(Guid clubId, int jerseyNumber, DateOnly startDate, DateOnly? endDate);
        Task CreateRegistrationAsync(PlayerRegistration registration);
        Task<Player?> GetByIdAsync(Guid id);
        Task<List<Player>> GetByFilterAsync(
            string? name,
            string? position,
            string? currentClub);
        Task CreateAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
    }
}
