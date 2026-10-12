﻿using System;
using System.Collections.Generic;
using System.Text;
using VolleyballApp.Model;

namespace VolleyballApp.Service
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAllAsync();
        Task<List<Player>> GetAllPlayersAsync();
        Task<List<Club>> GetAllClubsAsync();
        Task<List<PlayerRegistration>> GetAllRegistrationsAsync();

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
}
