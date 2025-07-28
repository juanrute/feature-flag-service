using FeatureFlag.Domain.Audit;

namespace FeatureFlag.Application.Services;

public interface IAuditService
{
    Task<Audit?> GetByNameAsync(string name, CancellationToken token = default);
}