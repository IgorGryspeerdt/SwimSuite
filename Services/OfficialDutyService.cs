using Microsoft.EntityFrameworkCore;
using SwimSuite.Data;
using SwimSuite.Models;

namespace SwimSuite.Services;

public class OfficialDutyService(ApplicationDbContext context) : IOfficialDutyService
{
    public async Task<OfficialDutyListViewModel?> GetListAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        var club = await context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(club => club.Id == clubId, cancellationToken);

        if (club is null)
        {
            return null;
        }

        var duties = await context.OfficialDuties
            .AsNoTracking()
            .Include(duty => duty.Official)
            .Where(duty => duty.ClubId == clubId)
            .OrderByDescending(duty => duty.Date)
            .ThenBy(duty => duty.MeetName)
            .ToListAsync(cancellationToken);

        return new OfficialDutyListViewModel
        {
            Club = club,
            Duties = duties
        };
    }

    public async Task<OfficialDuty?> CreateAsync(OfficialDutyCreateViewModel model, CancellationToken cancellationToken = default)
    {
        var officialExists = await context.Officials
            .AnyAsync(official => official.ClubId == model.ClubId && official.Id == model.OfficialId, cancellationToken);

        if (!officialExists)
        {
            return null;
        }

        var duty = new OfficialDuty
        {
            ClubId = model.ClubId,
            OfficialId = model.OfficialId,
            Date = model.Date,
            MeetName = model.MeetName.Trim(),
            Role = model.Role.Trim(),
            Location = NormalizeOptional(model.Location),
            Notes = NormalizeOptional(model.Notes)
        };

        context.OfficialDuties.Add(duty);
        await context.SaveChangesAsync(cancellationToken);

        return duty;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
