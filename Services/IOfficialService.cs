using SwimSuite.Models;

namespace SwimSuite.Services;

public interface IOfficialService
{
    Task<OfficialListViewModel?> GetListAsync(Guid clubId, CancellationToken cancellationToken = default);

    Task<Official?> CreateAsync(OfficialCreateViewModel model, CancellationToken cancellationToken = default);
}
