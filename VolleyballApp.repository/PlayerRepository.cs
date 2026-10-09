using Microsoft.EntityFrameworkCore;
using VolleyballApp.model;

namespace VolleyballApp.repository
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllAsync();
        Task<List<Player>> GetAllPlayers();
        Task<List<Club>> GetAllClubs();
        Task<List<PlayerRegistration>> GetAllRegistrations();
        Task<Player?> GetByIdAsync(Guid id);
        Task AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
    }

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

        public async Task<List<Player>> GetAllPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<List<Club>> GetAllClubs()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<List<PlayerRegistration>> GetAllRegistrations()
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

        public async Task AddAsync(Player player)
        {
            await _context.Players.AddAsync(player);
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
