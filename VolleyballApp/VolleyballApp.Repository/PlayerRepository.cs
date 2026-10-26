﻿using Microsoft.EntityFrameworkCore;
using VolleyballApp.Model;

namespace VolleyballApp.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly PraksaDbContext _context;

        public PlayerRepository(PraksaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Player>> GetAllAsync()
        {
            return await _context.Players
                .Include(p => p.PlayerRegistrations)
                .ThenInclude(r => r.Club)
                .ToListAsync();
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<List<Club>> GetAllClubsAsync()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<List<PlayerRegistration>> GetAllRegistrationsAsync()
        {
            return await _context.PlayerRegistrations.ToListAsync();
        }

        public async Task<Player?> GetByIdAsync(Guid id)
        {
            return await _context.Players
                .Include(p => p.PlayerRegistrations)
                .ThenInclude(r => r.Club)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Player>> GetByFilterAsync(
                string? name,
                string? position,
                string? currentClub)
        {
            IQueryable<Player> query = _context.Players;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name == name);
            }

            if (!string.IsNullOrEmpty(position))
            {
                query = query.Where(p => p.Position == position);
            }

            if (!string.IsNullOrEmpty(currentClub))
            {
                Club? club = await _context.Clubs.FirstOrDefaultAsync(c => c.Name == currentClub);

                if (club == null)
                {
                    return new List<Player>();
                }


                var playerIds = await _context.PlayerRegistrations.Where(r => r.ClubId == club.Id).Select(r => r.PlayerId).ToListAsync();

                query = query.Where(p => playerIds.Contains(p.Id));
            }

            return await query.ToListAsync();
        }

        public async Task CreateAsync(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Player player)
        {
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
        }
    }
}
