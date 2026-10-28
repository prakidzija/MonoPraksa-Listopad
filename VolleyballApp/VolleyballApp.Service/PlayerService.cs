using VolleyballApp.Model;
using VolleyballApp.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;

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


        public async Task<List<PlayerDto>> GetAllPlayersAsync()
        {
            var players = await _repository.GetAllAsync();
            var output = players.Select(p => new PlayerDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Position = p.Position,
                Clubs = p.PlayerRegistrations.Where(r => r.Club != null).Select(r => r.Club!.Name).ToList()
            }).ToList();

            return output;
        }

        public Task<List<Club>> GetAllClubsAsync()
        {
            return _repository.GetAllClubsAsync();
        }

        public async Task<Guid> CreateClubAsync(CreateClub request)
        {
            var name = request.Name?.Trim();
            var adress = request.Adress?.Trim();
            if (string.IsNullOrWhiteSpace(name) || name.Length > 30 ||
                string.IsNullOrWhiteSpace(adress) || adress.Length > 40)
                throw new ArgumentException("Enter a name (up to 30 characters) and address (up to 40 characters).");

            var club = new Club { Name = name, Adress = adress };
            await _repository.CreateClubAsync(club);
            return club.Id;
        }

        public Task<List<PlayerRegistration>> GetAllRegistrationsAsync()
        {
            return _repository.GetAllRegistrationsAsync();
        }


        public async Task<Guid> CreateRegistrationAsync(CreatePlayerRegistration request)
        {
            if (request.PlayerId == Guid.Empty || request.ClubId == Guid.Empty)
                throw new ArgumentException("Select a player and a club.");

            if (request.RegistrationType is not ("permanent" or "loan" or "dual_license"))
                throw new ArgumentException("Select a valid registration type.");

            if (request.StartDate == default || request.EndDate < request.StartDate)
                throw new ArgumentException("A start date is required and the end date must not precede it.");

            if (request.JerseyNumber is < 1 or > 99)
                throw new ArgumentException("Jersey number must be between 1 and 99.");

            if (await _repository.GetByIdAsync(request.PlayerId) == null ||
                !await _repository.ClubExistsAsync(request.ClubId))
                throw new KeyNotFoundException("The selected player or club does not exist.");

            if (await _repository.JerseyNumberExistsAsync(request.ClubId, request.JerseyNumber, request.StartDate, request.EndDate))
                throw new InvalidOperationException("This jersey number is already registered with the selected club during the selected dates.");

            var registration = new PlayerRegistration
            {
                PlayerId = request.PlayerId,
                ClubId = request.ClubId,
                RegistrationType = request.RegistrationType,
                JerseyNumber = request.JerseyNumber,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
            await _repository.CreateRegistrationAsync(registration);
            return registration.Id;
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
