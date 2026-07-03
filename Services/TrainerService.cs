using Microsoft.EntityFrameworkCore;
using SwimSuite.Data;
using SwimSuite.Models;

namespace SwimSuite.Services;

public class TrainerService(ApplicationDbContext context) : ITrainerService
{
    public async Task<TrainerListViewModel?> GetListAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        var club = await context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(club => club.Id == clubId, cancellationToken);

        if (club is null)
        {
            return null;
        }

        var trainers = await context.Trainers
            .AsNoTracking()
            .Where(trainer => trainer.ClubId == clubId)
            .OrderBy(trainer => trainer.LastName)
            .ThenBy(trainer => trainer.FirstName)
            .ToListAsync(cancellationToken);

        return new TrainerListViewModel
        {
            Club = club,
            Trainers = trainers
        };
    }

    public async Task<Trainer?> CreateAsync(TrainerCreateViewModel model, CancellationToken cancellationToken = default)
    {
        var clubExists = await context.Clubs
            .AnyAsync(club => club.Id == model.ClubId, cancellationToken);

        if (!clubExists)
        {
            return null;
        }

        var trainer = new Trainer
        {
            ClubId = model.ClubId,
            FirstName = model.FirstName.Trim(),
            LastName = model.LastName.Trim(),
            Email = NormalizeOptional(model.Email),
            PhoneNumber = NormalizeOptional(model.PhoneNumber),
            IsActive = model.IsActive
        };

        context.Trainers.Add(trainer);
        await context.SaveChangesAsync(cancellationToken);

        return trainer;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
