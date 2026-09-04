using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SwimSuite.Models;

namespace SwimSuite.Data;

public class DevelopmentDataSeeder(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager,
    IConfiguration configuration)
{
    private static readonly Guid ClubId = Guid.Parse("7b2cccad-6ef4-4eb8-9cb9-b8a0ba5c1011");
    private static readonly DateTime SeededAtUtc = new(2026, 9, 1, 9, 0, 0, DateTimeKind.Utc);

    private static readonly Guid YouthGroupId = Guid.Parse("29b04a6f-cf66-4be0-aa60-50e725376d01");
    private static readonly Guid PerformanceGroupId = Guid.Parse("29b04a6f-cf66-4be0-aa60-50e725376d02");
    private static readonly Guid MastersGroupId = Guid.Parse("29b04a6f-cf66-4be0-aa60-50e725376d03");

    private static readonly Guid KeiraTrainerId = Guid.Parse("a561d80a-4ebd-40be-9f92-580e2615c101");
    private static readonly Guid FabianTrainerId = Guid.Parse("a561d80a-4ebd-40be-9f92-580e2615c102");
    private static readonly Guid AmaraTrainerId = Guid.Parse("a561d80a-4ebd-40be-9f92-580e2615c103");
    private static readonly Guid LewisTrainerId = Guid.Parse("a561d80a-4ebd-40be-9f92-580e2615c104");

    private static readonly Guid RowanOfficialId = Guid.Parse("4fdcb2d3-dd35-4149-9fbc-2e1455bf5101");
    private static readonly Guid TessaOfficialId = Guid.Parse("4fdcb2d3-dd35-4149-9fbc-2e1455bf5102");
    private static readonly Guid MiloOfficialId = Guid.Parse("4fdcb2d3-dd35-4149-9fbc-2e1455bf5103");

    private static readonly Guid YouthRegularBlockId = Guid.Parse("f8a81e33-4ca4-4df0-97b9-f69808408101");
    private static readonly Guid YouthChangedBlockId = Guid.Parse("f8a81e33-4ca4-4df0-97b9-f69808408102");
    private static readonly Guid PerformanceCancelledBlockId = Guid.Parse("f8a81e33-4ca4-4df0-97b9-f69808408103");
    private static readonly Guid PerformanceReplacementBlockId = Guid.Parse("f8a81e33-4ca4-4df0-97b9-f69808408104");
    private static readonly Guid MastersBlockId = Guid.Parse("f8a81e33-4ca4-4df0-97b9-f69808408105");
    private static readonly Guid PerformanceUpcomingBlockId = Guid.Parse("f8a81e33-4ca4-4df0-97b9-f69808408106");

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedIdentityUsersAsync();
        await SeedDomainDataAsync(cancellationToken);
    }

    private async Task SeedIdentityUsersAsync()
    {
        var password = configuration["DevelopmentSeed:Password"];
        if (string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        await CreateUserIfMissingAsync("development-northstar-admin", "northstar.admin@swimsuite.test", password);
        await CreateUserIfMissingAsync("development-northstar-coordinator", "northstar.coordinator@swimsuite.test", password);
    }

    private async Task CreateUserIfMissingAsync(string id, string email, string password)
    {
        if (await userManager.FindByIdAsync(id) is not null)
        {
            return;
        }

        if (await userManager.FindByNameAsync(email) is not null)
        {
            throw new InvalidOperationException($"A user with the development seed email '{email}' already exists with a different identifier.");
        }

        var result = await userManager.CreateAsync(new IdentityUser
        {
            Id = id,
            UserName = email,
            Email = email,
            EmailConfirmed = true
        }, password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(error => error.Description));
            throw new InvalidOperationException($"Could not create development seed user '{email}': {errors}");
        }
    }

    private async Task SeedDomainDataAsync(CancellationToken cancellationToken)
    {
        if (!await context.Clubs.AnyAsync(club => club.Id == ClubId, cancellationToken))
        {
            context.Clubs.Add(new Club
            {
                Id = ClubId,
                Name = "Northstar Aquatics",
                RegistrationNumber = "BE-TEST-042",
                Email = "hello@northstar-aquatics.test",
                PhoneNumber = "+32 2 555 0142",
                Address = "42 Lantern Lane, 1000 Brussels",
                CreatedAtUtc = SeededAtUtc
            });
        }

        await AddGroupsAsync(cancellationToken);
        await AddTrainersAsync(cancellationToken);
        await AddOfficialsAsync(cancellationToken);
        await AddTrainingBlocksAsync(cancellationToken);
        await AddTrainerAttendancesAsync(cancellationToken);
        await AddOfficialDutiesAsync(cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task AddGroupsAsync(CancellationToken cancellationToken)
    {
        await AddGroupIfMissingAsync(YouthGroupId, "Youth Development", "Technique and confidence for developing swimmers.", cancellationToken);
        await AddGroupIfMissingAsync(PerformanceGroupId, "Performance Squad", "Competition preparation and race-pace work.", cancellationToken);
        await AddGroupIfMissingAsync(MastersGroupId, "Masters", "Fitness and technique for adult swimmers.", cancellationToken);
    }

    private async Task AddGroupIfMissingAsync(Guid id, string name, string description, CancellationToken cancellationToken)
    {
        if (!await context.TrainingGroups.AnyAsync(group => group.Id == id, cancellationToken))
        {
            context.TrainingGroups.Add(new TrainingGroup { Id = id, ClubId = ClubId, Name = name, Description = description, CreatedAtUtc = SeededAtUtc });
        }
    }

    private async Task AddTrainersAsync(CancellationToken cancellationToken)
    {
        await AddTrainerIfMissingAsync(KeiraTrainerId, "Keira", "Morgan", "+32 2 555 0201", true, cancellationToken);
        await AddTrainerIfMissingAsync(FabianTrainerId, "Fabian", "Reed", "+32 2 555 0202", true, cancellationToken);
        await AddTrainerIfMissingAsync(AmaraTrainerId, "Amara", "Voss", "+32 2 555 0203", true, cancellationToken);
        await AddTrainerIfMissingAsync(LewisTrainerId, "Lewis", "Hale", "+32 2 555 0204", false, cancellationToken);
    }

    private async Task AddTrainerIfMissingAsync(Guid id, string firstName, string lastName, string phoneNumber, bool isActive, CancellationToken cancellationToken)
    {
        if (!await context.Trainers.AnyAsync(trainer => trainer.Id == id, cancellationToken))
        {
            context.Trainers.Add(new Trainer { Id = id, ClubId = ClubId, FirstName = firstName, LastName = lastName, Email = $"{firstName}.{lastName}@northstar-aquatics.test".ToLowerInvariant(), PhoneNumber = phoneNumber, IsActive = isActive, CreatedAtUtc = SeededAtUtc });
        }
    }

    private async Task AddOfficialsAsync(CancellationToken cancellationToken)
    {
        await AddOfficialIfMissingAsync(RowanOfficialId, "Rowan", "Ellis", "+32 2 555 0301", "OFF-TEST-101", true, cancellationToken);
        await AddOfficialIfMissingAsync(TessaOfficialId, "Tessa", "Ward", "+32 2 555 0302", "OFF-TEST-102", true, cancellationToken);
        await AddOfficialIfMissingAsync(MiloOfficialId, "Milo", "Quinn", "+32 2 555 0303", "OFF-TEST-103", false, cancellationToken);
    }

    private async Task AddOfficialIfMissingAsync(Guid id, string firstName, string lastName, string phoneNumber, string licenseNumber, bool isActive, CancellationToken cancellationToken)
    {
        if (!await context.Officials.AnyAsync(official => official.Id == id, cancellationToken))
        {
            context.Officials.Add(new Official { Id = id, ClubId = ClubId, FirstName = firstName, LastName = lastName, Email = $"{firstName}.{lastName}@northstar-aquatics.test".ToLowerInvariant(), PhoneNumber = phoneNumber, LicenseNumber = licenseNumber, IsActive = isActive, CreatedAtUtc = SeededAtUtc });
        }
    }

    private async Task AddTrainingBlocksAsync(CancellationToken cancellationToken)
    {
        await AddBlockIfMissingAsync(YouthRegularBlockId, YouthGroupId, new DateOnly(2026, 9, 1), new TimeOnly(17, 0), new TimeOnly(18, 30), "Northstar Pool", "Regular technique session.", cancellationToken);
        await AddBlockIfMissingAsync(YouthChangedBlockId, YouthGroupId, new DateOnly(2026, 9, 3), new TimeOnly(17, 15), new TimeOnly(18, 45), "Riverside Pool", "Changed from Northstar Pool because of maintenance.", cancellationToken);
        await AddBlockIfMissingAsync(PerformanceCancelledBlockId, PerformanceGroupId, new DateOnly(2026, 9, 5), new TimeOnly(18, 30), new TimeOnly(20, 0), "Northstar Pool", "Cancelled because the pool is unavailable for an emergency repair.", cancellationToken);
        await AddBlockIfMissingAsync(PerformanceReplacementBlockId, PerformanceGroupId, new DateOnly(2026, 9, 8), new TimeOnly(18, 30), new TimeOnly(20, 0), "Northstar Pool", "Race-pace set. Amara Voss covered for Fabian Reed.", cancellationToken);
        await AddBlockIfMissingAsync(MastersBlockId, MastersGroupId, new DateOnly(2026, 9, 10), new TimeOnly(20, 0), new TimeOnly(21, 15), "Northstar Pool", "Aerobic endurance session.", cancellationToken);
        await AddBlockIfMissingAsync(PerformanceUpcomingBlockId, PerformanceGroupId, new DateOnly(2026, 9, 15), new TimeOnly(18, 30), new TimeOnly(20, 0), "Northstar Pool", "Upcoming competition preparation session.", cancellationToken);
    }

    private async Task AddBlockIfMissingAsync(Guid id, Guid groupId, DateOnly date, TimeOnly startTime, TimeOnly endTime, string location, string notes, CancellationToken cancellationToken)
    {
        if (!await context.TrainingBlocks.AnyAsync(block => block.Id == id, cancellationToken))
        {
            context.TrainingBlocks.Add(new TrainingBlock { Id = id, ClubId = ClubId, TrainingGroupId = groupId, Date = date, StartTime = startTime, EndTime = endTime, Location = location, Notes = notes, CreatedAtUtc = SeededAtUtc });
        }
    }

    private async Task AddTrainerAttendancesAsync(CancellationToken cancellationToken)
    {
        await AddAttendanceIfMissingAsync(Guid.Parse("49fc8777-d11e-40f2-a0aa-931982314201"), YouthRegularBlockId, KeiraTrainerId, true, "Led the regular technique session.", cancellationToken);
        await AddAttendanceIfMissingAsync(Guid.Parse("49fc8777-d11e-40f2-a0aa-931982314202"), YouthRegularBlockId, FabianTrainerId, false, "Absent due to illness; Keira covered the session.", cancellationToken);
        await AddAttendanceIfMissingAsync(Guid.Parse("49fc8777-d11e-40f2-a0aa-931982314203"), YouthChangedBlockId, KeiraTrainerId, true, "Present at the changed location.", cancellationToken);
        await AddAttendanceIfMissingAsync(Guid.Parse("49fc8777-d11e-40f2-a0aa-931982314204"), PerformanceReplacementBlockId, FabianTrainerId, false, "Unavailable; replacement arranged.", cancellationToken);
        await AddAttendanceIfMissingAsync(Guid.Parse("49fc8777-d11e-40f2-a0aa-931982314205"), PerformanceReplacementBlockId, AmaraTrainerId, true, "Covered Fabian's session as replacement trainer.", cancellationToken);
        await AddAttendanceIfMissingAsync(Guid.Parse("49fc8777-d11e-40f2-a0aa-931982314206"), MastersBlockId, KeiraTrainerId, true, "Led the masters endurance session.", cancellationToken);
        await AddAttendanceIfMissingAsync(Guid.Parse("49fc8777-d11e-40f2-a0aa-931982314207"), MastersBlockId, AmaraTrainerId, true, "Assisted with lane feedback.", cancellationToken);
    }

    private async Task AddAttendanceIfMissingAsync(Guid id, Guid blockId, Guid trainerId, bool isPresent, string notes, CancellationToken cancellationToken)
    {
        if (!await context.TrainerAttendances.AnyAsync(attendance => attendance.TrainingBlockId == blockId && attendance.TrainerId == trainerId, cancellationToken))
        {
            context.TrainerAttendances.Add(new TrainerAttendance { Id = id, ClubId = ClubId, TrainingBlockId = blockId, TrainerId = trainerId, IsPresent = isPresent, Notes = notes, CreatedAtUtc = SeededAtUtc });
        }
    }

    private async Task AddOfficialDutiesAsync(CancellationToken cancellationToken)
    {
        await AddDutyIfMissingAsync(Guid.Parse("d08bb361-19a5-493c-b2c8-831328657301"), RowanOfficialId, new DateOnly(2026, 8, 23), "Brussels Summer Sprint Meet", "Starter", "Aqua Forum Brussels", "Morning and afternoon sessions.", cancellationToken);
        await AddDutyIfMissingAsync(Guid.Parse("d08bb361-19a5-493c-b2c8-831328657302"), TessaOfficialId, new DateOnly(2026, 8, 23), "Brussels Summer Sprint Meet", "Chief Timekeeper", "Aqua Forum Brussels", "Shared reporting paperwork with the host club.", cancellationToken);
        await AddDutyIfMissingAsync(Guid.Parse("d08bb361-19a5-493c-b2c8-831328657303"), RowanOfficialId, new DateOnly(2026, 9, 13), "Autumn Relay Invitational", "Turn Judge", "Riverside Pool", "Arrive 45 minutes before warm-up.", cancellationToken);
        await AddDutyIfMissingAsync(Guid.Parse("d08bb361-19a5-493c-b2c8-831328657304"), TessaOfficialId, new DateOnly(2026, 9, 13), "Autumn Relay Invitational", "Referee", "Riverside Pool", "Responsible for final results sign-off.", cancellationToken);
        await AddDutyIfMissingAsync(Guid.Parse("d08bb361-19a5-493c-b2c8-831328657305"), RowanOfficialId, new DateOnly(2026, 10, 4), "Provincial Distance Cup", "Timekeeper", "Aqua Forum Brussels", "Full-day assignment for future reimbursement reporting.", cancellationToken);
    }

    private async Task AddDutyIfMissingAsync(Guid id, Guid officialId, DateOnly date, string meetName, string role, string location, string notes, CancellationToken cancellationToken)
    {
        if (!await context.OfficialDuties.AnyAsync(duty => duty.OfficialId == officialId && duty.Date == date && duty.MeetName == meetName && duty.Role == role, cancellationToken))
        {
            context.OfficialDuties.Add(new OfficialDuty { Id = id, ClubId = ClubId, OfficialId = officialId, Date = date, MeetName = meetName, Role = role, Location = location, Notes = notes, CreatedAtUtc = SeededAtUtc });
        }
    }
}
