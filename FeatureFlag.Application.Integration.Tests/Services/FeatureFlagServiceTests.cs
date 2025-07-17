using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FeatureFlag.Application.Models;
using FeatureFlag.Application.Repositories;
using FeatureFlag.Application.Services;
using Xunit;

namespace FeatureFlag.Application.Integration.Tests.Services
{
    public class FeatureFlagServiceTests
    {
        private readonly FeatureToggleRepository _repository;
        private readonly FeatureFlagService _service;

        private List<FeatureToggle> featureToggles = new();

        public FeatureFlagServiceIntegrationTests()
        {
            _repository = new FeatureToggleRepository(featureToggles);
            _service = new FeatureFlagService(_repository);
        }

        [Fact]
        public async Task CreateAndGetFeatureFlag_ShouldSucceed()
        {
            var feature = new FeatureToggle
            {
                Id = Guid.NewGuid(),
                Name = "IntegrationFeature",
                Description = "Integration test feature",
                EnvironmentStates = new List<EnvironmentState>
                {
                    new EnvironmentState { Environment = EnvironmentEnum.Prod, IsActive = true, Percentage = 100 }
                }
            };

            var created = await _service.CreateAsync(feature);
            Assert.True(created);

            var allFlags = await _service.GetAllAsync();
            Assert.Contains(allFlags, f => f.Name == "IntegrationFeature");
        }

        [Fact]
        public async Task ToggleActivationAndCheckStatus_ShouldSucceed()
        {
            var feature = new FeatureToggle
            {
                Id = Guid.NewGuid(),
                Name = "ToggleIntegration",
                Description = "Toggle integration feature",
                EnvironmentStates = new List<EnvironmentState>
                {
                    new EnvironmentState { Environment = EnvironmentEnum.Prod, IsActive = false, Percentage = 0 }
                }
            };
            await _service.CreateAsync(feature);

            await _service.ToggleActivationAsync(feature.Id, EnvironmentEnum.Prod, 100, true);
            var updated = await _service.GetByIdOrNameAsync(feature.Id.ToString());
            Assert.NotNull(updated);
            Assert.True(updated.EnvironmentStates[0].IsActive);
        }

        [Fact]
        public async Task DeleteFeatureFlag_ShouldRemoveFlag()
        {
            var feature = new FeatureToggle
            {
                Id = Guid.NewGuid(),
                Name = "DeleteIntegration",
                Description = "Delete integration feature",
                EnvironmentStates = new List<EnvironmentState>()
            };
            await _service.CreateAsync(feature);
            var deleted = await _service.DeleteByIdAsync(feature.Id);
            Assert.True(deleted);
            var allFlags = await _service.GetAllAsync();
            Assert.DoesNotContain(allFlags, f => f.Id == feature.Id);
        }
    }    
}
