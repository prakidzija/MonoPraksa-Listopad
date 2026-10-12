﻿using VolleyballApp.Model;
using VolleyballApp.Repository;

namespace VolleyballApp.Service
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;
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
