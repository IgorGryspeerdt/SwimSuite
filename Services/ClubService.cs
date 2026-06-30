using Microsoft.EntityFrameworkCore;
using SwimSuite.Data;
using SwimSuite.Models;

namespace SwimSuite.Services;

public class ClubService(ApplicationDbContext context) : IClubService
{
    public async Task<IReadOnlyList<Club>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clubs
            .AsNoTracking()
            .OrderBy(club => club.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Club?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Clubs
            .AsNoTracking()
            .FirstOrDefaultAsync(club => club.Id == id, cancellationToken);
    }

    public async Task<Club> CreateAsync(ClubCreateViewModel model, CancellationToken cancellationToken = default)
    {
        var club = new Club
        {
            Name = model.Name.Trim(),
            RegistrationNumber = NormalizeOptional(model.RegistrationNumber),
            Email = NormalizeOptional(model.Email),
            PhoneNumber = NormalizeOptional(model.PhoneNumber),
            Address = NormalizeOptional(model.Address)
        };

        context.Clubs.Add(club);
        await context.SaveChangesAsync(cancellationToken);

        return club;
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
