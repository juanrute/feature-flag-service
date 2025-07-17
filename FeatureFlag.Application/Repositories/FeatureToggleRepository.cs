using FeatureFlag.Application.Models;

namespace FeatureFlag.Application.Repositories;

public class FeatureToggleRepository(List<FeatureToggle> featureToggles) : IFeatureToggleRepository
{

    public Task<bool> CreateAsync(FeatureToggle featureToggle, CancellationToken token = default)
    {
        featureToggles.Add(featureToggle);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        var removed = featureToggles.RemoveAll(f=>f.Id == id);
        return Task.FromResult(removed > 0);
    }

    public Task<IEnumerable<FeatureToggle>> GetAllAsync(CancellationToken token = default)
    {
        return Task.FromResult(featureToggles.AsEnumerable());
    }

    public Task<FeatureToggle?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var toogle = featureToggles.SingleOrDefault(f => f.Id == id);
        return Task.FromResult(toogle);
    }

    public Task<FeatureToggle?> GetByNameAsync(string name, CancellationToken token = default)
    {
        var toogle = featureToggles.SingleOrDefault(f => f.Name == name);
        return Task.FromResult(toogle);
    }

    public Task<bool> SetPartialActivation(Guid id, EnvironmentEnum environment, int percentage, bool isActive, CancellationToken token = default)
    {
        var featureIndex = featureToggles.FindIndex(x => x.Id == id);
        if (featureIndex == -1)
        {
            return Task.FromResult(false);
        }

        var environmentToupdate = featureToggles[featureIndex].EnvironmentStates
            .FirstOrDefault(x => x.Environment == environment);
            
        environmentToupdate!.Percentage = percentage;
        environmentToupdate!.IsActive = isActive;
        
        return Task.FromResult(true);
    }
}