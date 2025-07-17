using FeatureFlag.Application.Models;
using FeatureFlag.Application.Repositories;

namespace FeatureFlag.Application.Services;

public class FeatureFlagService(IFeatureToggleRepository _featureToggleRepository) : IFeatureFlagService
{
    public Task<bool> CreateAsync(
        FeatureToggle featureToogle,
        CancellationToken token = default)
    {
        return _featureToggleRepository.CreateAsync(featureToogle, token);         
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _featureToggleRepository.DeleteByIdAsync(id, token);
    }

    public Task<IEnumerable<FeatureToggle>> GetAllAsync(CancellationToken token = default)
    {
        return _featureToggleRepository.GetAllAsync(token);
    }

    public async Task<FeatureToggle?> GetByIdOrNameAsync(string idOrName, CancellationToken token = default)
    {
        var featureFlag = Guid.TryParse(idOrName, out var id)
            ? await _featureToggleRepository.GetByIdAsync(id, token)
            : await _featureToggleRepository.GetByNameAsync(idOrName, token);
        return featureFlag;
    }

    public Task<FeatureToggle?> ToggleActivationAsync(Guid id, EnvironmentEnum environment, int percentage, bool isActive, CancellationToken token = default)
    {
        _featureToggleRepository.SetPartialActivation(id, environment, percentage, isActive, token);
        return _featureToggleRepository.GetByIdAsync(id);
    }

    public async Task<bool> IsFeatureEnabled(Guid id, string clientId, EnvironmentEnum environment, CancellationToken token = default)
    {
        var featureFlag = await _featureToggleRepository.GetByIdAsync(id, token);
        var environmentData = featureFlag?.EnvironmentStates.SingleOrDefault(e => e.Environment == environment);
        if (environmentData is null)
        {
            return false;
        }

        return environmentData.IsActive && IsEligibleForRollout(clientId, environmentData.Percentage);
    }
    
    private bool IsEligibleForRollout(string userId, int percentage)
    {
        var hash = Math.Abs(userId.GetHashCode()) % 100;
        return hash < percentage;
    }
}