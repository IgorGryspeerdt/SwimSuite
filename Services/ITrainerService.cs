using SwimSuite.Models;

namespace SwimSuite.Services;

public interface ITrainerService
{
    Task<TrainerListViewModel?> GetListAsync(Guid clubId, CancellationToken cancellationToken = default);

    Task<Trainer?> CreateAsync(TrainerCreateViewModel model, CancellationToken cancellationToken = default);
}
