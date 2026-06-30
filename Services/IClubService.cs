using SwimSuite.Models;

namespace SwimSuite.Services;

public interface IClubService
{
    Task<IReadOnlyList<Club>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Club?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Club> CreateAsync(ClubCreateViewModel model, CancellationToken cancellationToken = default);
}
