﻿using VolleyballApp.Model;
using VolleyballApp.Repository;
using Microsoft.Extensions.Logging;

namespace VolleyballApp.Service
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(IPlayerRepository repository, ILogger<PlayerService> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public Task<List<Player>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<List<Player>> GetAllPlayersAsync()
        {
            return _repository.GetAllPlayersAsync();
        }

        public Task<List<Club>> GetAllClubsAsync()
        {
            return _repository.GetAllClubsAsync();
        }

        public Task<List<PlayerRegistration>> GetAllRegistrationsAsync()
        {
            return _repository.GetAllRegistrationsAsync();
        }


        public Task<Player?> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }


        public Task<List<Player>> GetByFilterAsync(
            string? name,
            string? position,
            string? currentClub)
        {
            return _repository.GetByFilterAsync(name, position, currentClub);
        }


        public async Task CreateAsync(Player player)
        {
            await _repository.CreateAsync(player);

            _logger.LogInformation("Player {PlayerId} created.", player.Id);

        }


        public async Task UpdateAsync(
            Guid id,
            Player updatedPlayer)
        {

            var player = await _repository.GetByIdAsync(id);

            if (player == null)
            {
                throw new InvalidOperationException(
                    "Player doesn't exist.");
            }

            player.Name = updatedPlayer.Name;
            player.Position = updatedPlayer.Position;
            player.Age = updatedPlayer.Age;

            await _repository.UpdateAsync(player);

            _logger.LogInformation("Player {PlayerId} updated.", player.Id);

        }


        public async Task DeleteAsync(Guid id)
        {
            var player = await _repository.GetByIdAsync(id);

            if (player == null)
            {
                throw new InvalidOperationException(
                    "Player doesn't exist.");
            }

            await _repository.DeleteAsync(player);

            _logger.LogInformation("Player {PlayerId} deleted.", player.Id);

        }
    }
}
