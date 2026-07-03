using SwimSuite.Models;

namespace SwimSuite.Services;

public interface ITrainerAttendanceService
{
    Task<TrainerAttendanceEditViewModel?> GetForTrainingBlockAsync(Guid clubId, Guid trainingBlockId, CancellationToken cancellationToken = default);

    Task<bool> SaveAsync(TrainerAttendanceEditViewModel model, CancellationToken cancellationToken = default);
}
