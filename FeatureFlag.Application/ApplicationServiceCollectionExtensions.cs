using FeatureFlag.Application.Repositories;
using FeatureFlag.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using FeatureFlag.Application.Models;

namespace FeatureFlag.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IFeatureFlagService, FeatureFlagService>();
        services.AddSingleton<IFeatureToggleRepository>(provider => 
        {
            var testData = GetHardCodedData();
            return new FeatureToggleRepository(testData);
        });

        return services;
    }

    private static List<FeatureToggle> GetHardCodedData()
    {
        return new() {
            new FeatureToggle
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "DisabledFeature",
                Description = "Feature completely disabled in all environments",
                EnvironmentStates = new List<EnvironmentState>
                {
                    new() { Environment = EnvironmentEnum.Development, IsActive = false, Percentage = 0 },
                    new() { Environment = EnvironmentEnum.Testing, IsActive = false, Percentage = 0 },
                    new() { Environment = EnvironmentEnum.Staging, IsActive = false, Percentage = 0 },
                    new() { Environment = EnvironmentEnum.Production, IsActive = false, Percentage = 0 }
                }
            },

            new FeatureToggle
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "EnabledFeature",
                Description = "Feature fully enabled in all environments",
                EnvironmentStates = new List<EnvironmentState>
                {
                    new() { Environment = EnvironmentEnum.Development, IsActive = true, Percentage = 100 },
                    new() { Environment = EnvironmentEnum.Testing, IsActive = true, Percentage = 100 },
                    new() { Environment = EnvironmentEnum.Staging, IsActive = true, Percentage = 100 },
                    new() { Environment = EnvironmentEnum.Production, IsActive = true, Percentage = 100 }
                }
            }
        };
    }
}