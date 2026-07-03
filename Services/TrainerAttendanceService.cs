using Microsoft.EntityFrameworkCore;
using SwimSuite.Data;
using SwimSuite.Models;

namespace SwimSuite.Services;

public class TrainerAttendanceService(ApplicationDbContext context) : ITrainerAttendanceService
{
    public async Task<TrainerAttendanceEditViewModel?> GetForTrainingBlockAsync(Guid clubId, Guid trainingBlockId, CancellationToken cancellationToken = default)
    {
        var block = await context.TrainingBlocks
            .AsNoTracking()
            .Include(block => block.Club)
            .Include(block => block.TrainingGroup)
            .FirstOrDefaultAsync(block => block.ClubId == clubId && block.Id == trainingBlockId, cancellationToken);

        if (block is null)
        {
            return null;
        }

        var trainers = await context.Trainers
            .AsNoTracking()
            .Where(trainer => trainer.ClubId == clubId && trainer.IsActive)
            .OrderBy(trainer => trainer.LastName)
            .ThenBy(trainer => trainer.FirstName)
            .ToListAsync(cancellationToken);

        var existingAttendance = await context.TrainerAttendances
            .AsNoTracking()
            .Where(attendance => attendance.ClubId == clubId && attendance.TrainingBlockId == trainingBlockId)
            .ToDictionaryAsync(attendance => attendance.TrainerId, cancellationToken);

        return new TrainerAttendanceEditViewModel
        {
            ClubId = clubId,
            TrainingBlockId = trainingBlockId,
            ClubName = block.Club?.Name ?? string.Empty,
            TrainingGroupName = block.TrainingGroup?.Name ?? string.Empty,
            Date = block.Date,
            StartTime = block.StartTime,
            EndTime = block.EndTime,
            Trainers = trainers
                .Select(trainer =>
                {
                    existingAttendance.TryGetValue(trainer.Id, out var attendance);

                    return new TrainerAttendanceEntryViewModel
                    {
                        TrainerId = trainer.Id,
                        TrainerName = trainer.DisplayName,
                        IsPresent = attendance?.IsPresent ?? false,
                        Notes = attendance?.Notes
                    };
                })
                .ToList()
        };
    }

    public async Task<bool> SaveAsync(TrainerAttendanceEditViewModel model, CancellationToken cancellationToken = default)
    {
        var blockExists = await context.TrainingBlocks
            .AnyAsync(block => block.ClubId == model.ClubId && block.Id == model.TrainingBlockId, cancellationToken);

        if (!blockExists)
        {
            return false;
        }

        var trainerIds = model.Trainers.Select(trainer => trainer.TrainerId).ToList();
        var validTrainerIds = await context.Trainers
            .Where(trainer => trainer.ClubId == model.ClubId && trainerIds.Contains(trainer.Id))
            .Select(trainer => trainer.Id)
            .ToListAsync(cancellationToken);

        if (validTrainerIds.Count != trainerIds.Distinct().Count())
        {
            return false;
        }

        var existingAttendance = await context.TrainerAttendances
            .Where(attendance => attendance.ClubId == model.ClubId && attendance.TrainingBlockId == model.TrainingBlockId)
            .ToDictionaryAsync(attendance => attendance.TrainerId, cancellationToken);

        foreach (var entry in model.Trainers)
        {
            if (!existingAttendance.TryGetValue(entry.TrainerId, out var attendance))
            {
                attendance = new TrainerAttendance
                {
                    ClubId = model.ClubId,
                    TrainingBlockId = model.TrainingBlockId,
                    TrainerId = entry.TrainerId
                };

                context.TrainerAttendances.Add(attendance);
            }

            attendance.IsPresent = entry.IsPresent;
            attendance.Notes = NormalizeOptional(entry.Notes);
        }

        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
