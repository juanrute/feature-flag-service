using FeatureFlag.Application.Models;

namespace FeatureFlag.Application.Services;

public interface IFeatureFlagService
{
    Task<bool> CreateAsync(FeatureToggle featureToogle, CancellationToken token = default);
    Task<IEnumerable<FeatureToggle>> GetAllAsync(CancellationToken token = default);
    Task<FeatureToggle?> GetByIdOrNameAsync(string name, CancellationToken token = default);
    Task<FeatureToggle?> ToggleActivationAsync(Guid id, EnvironmentEnum environment, int percentage, bool isActive, CancellationToken token = default);
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    Task<bool> IsFeatureEnabled(Guid id, string clientId, EnvironmentEnum environment, CancellationToken token = default);
}