using Microsoft.EntityFrameworkCore;
using SwimSuite.Data;
using SwimSuite.Models;

namespace SwimSuite.Services;

public class TrainingScheduleService(ApplicationDbContext context) : ITrainingScheduleService
{
    public async Task<TrainingScheduleViewModel?> GetScheduleAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        var club = await context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(club => club.Id == clubId, cancellationToken);

        if (club is null)
        {
            return null;
        }

        var groups = await context.TrainingGroups
            .AsNoTracking()
            .Where(group => group.ClubId == clubId)
            .OrderBy(group => group.Name)
            .ToListAsync(cancellationToken);

        var blocks = await context.TrainingBlocks
            .AsNoTracking()
            .Include(block => block.TrainingGroup)
            .Where(block => block.ClubId == clubId)
            .OrderBy(block => block.Date)
            .ThenBy(block => block.StartTime)
            .ToListAsync(cancellationToken);

        return new TrainingScheduleViewModel
        {
            Club = club,
            TrainingGroups = groups,
            TrainingBlocks = blocks
        };
    }

    public async Task<TrainingGroup?> GetGroupAsync(Guid clubId, Guid groupId, CancellationToken cancellationToken = default)
    {
        return await context.TrainingGroups
            .AsNoTracking()
            .FirstOrDefaultAsync(group => group.ClubId == clubId && group.Id == groupId, cancellationToken);
    }

    public async Task<TrainingBlock?> GetBlockAsync(Guid clubId, Guid blockId, CancellationToken cancellationToken = default)
    {
        return await context.TrainingBlocks
            .AsNoTracking()
            .Include(block => block.TrainingGroup)
            .FirstOrDefaultAsync(block => block.ClubId == clubId && block.Id == blockId, cancellationToken);
    }

    public async Task<TrainingGroup?> CreateGroupAsync(TrainingGroupCreateViewModel model, CancellationToken cancellationToken = default)
    {
        var clubExists = await context.Clubs
            .AnyAsync(club => club.Id == model.ClubId, cancellationToken);

        if (!clubExists)
        {
            return null;
        }

        var group = new TrainingGroup
        {
            ClubId = model.ClubId,
            Name = model.Name.Trim(),
            Description = NormalizeOptional(model.Description)
        };

        context.TrainingGroups.Add(group);
        await context.SaveChangesAsync(cancellationToken);

        return group;
    }

    public async Task<TrainingBlock?> CreateBlockAsync(TrainingBlockCreateViewModel model, CancellationToken cancellationToken = default)
    {
        var group = await context.TrainingGroups
            .AsNoTracking()
            .FirstOrDefaultAsync(group => group.ClubId == model.ClubId && group.Id == model.TrainingGroupId, cancellationToken);

        if (group is null)
        {
            return null;
        }

        var block = new TrainingBlock
        {
            ClubId = model.ClubId,
            TrainingGroupId = model.TrainingGroupId,
            Date = model.Date,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            Location = NormalizeOptional(model.Location),
            Notes = NormalizeOptional(model.Notes)
        };

        context.TrainingBlocks.Add(block);
        await context.SaveChangesAsync(cancellationToken);

        return block;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
