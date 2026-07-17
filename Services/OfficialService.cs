using Microsoft.EntityFrameworkCore;
using SwimSuite.Data;
using SwimSuite.Models;

namespace SwimSuite.Services;

public class OfficialService(ApplicationDbContext context) : IOfficialService
{
    public async Task<OfficialListViewModel?> GetListAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        var club = await context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(club => club.Id == clubId, cancellationToken);

        if (club is null)
        {
            return null;
        }

        var officials = await context.Officials
            .AsNoTracking()
            .Where(official => official.ClubId == clubId)
            .OrderBy(official => official.LastName)
            .ThenBy(official => official.FirstName)
            .ToListAsync(cancellationToken);

        return new OfficialListViewModel
        {
            Club = club,
            Officials = officials
        };
    }

    public async Task<Official?> CreateAsync(OfficialCreateViewModel model, CancellationToken cancellationToken = default)
    {
        var clubExists = await context.Clubs
            .AnyAsync(club => club.Id == model.ClubId, cancellationToken);

        if (!clubExists)
        {
            return null;
        }

        var official = new Official
        {
            ClubId = model.ClubId,
            FirstName = model.FirstName.Trim(),
            LastName = model.LastName.Trim(),
            Email = NormalizeOptional(model.Email),
            PhoneNumber = NormalizeOptional(model.PhoneNumber),
            LicenseNumber = NormalizeOptional(model.LicenseNumber),
            IsActive = model.IsActive
        };

        context.Officials.Add(official);
        await context.SaveChangesAsync(cancellationToken);

        return official;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
