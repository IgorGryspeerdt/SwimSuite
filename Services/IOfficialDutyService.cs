using SwimSuite.Models;

namespace SwimSuite.Services;

public interface IOfficialDutyService
{
    Task<OfficialDutyListViewModel?> GetListAsync(Guid clubId, CancellationToken cancellationToken = default);

    Task<OfficialDuty?> CreateAsync(OfficialDutyCreateViewModel model, CancellationToken cancellationToken = default);
}
