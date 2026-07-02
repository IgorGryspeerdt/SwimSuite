using SwimSuite.Models;

namespace SwimSuite.Services;

public interface ITrainingScheduleService
{
    Task<TrainingScheduleViewModel?> GetScheduleAsync(Guid clubId, CancellationToken cancellationToken = default);

    Task<TrainingGroup?> GetGroupAsync(Guid clubId, Guid groupId, CancellationToken cancellationToken = default);

    Task<TrainingBlock?> GetBlockAsync(Guid clubId, Guid blockId, CancellationToken cancellationToken = default);

    Task<TrainingGroup?> CreateGroupAsync(TrainingGroupCreateViewModel model, CancellationToken cancellationToken = default);

    Task<TrainingBlock?> CreateBlockAsync(TrainingBlockCreateViewModel model, CancellationToken cancellationToken = default);
}
