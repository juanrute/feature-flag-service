using FeatureFlag.Application.Models;
using FeatureFlag.Application.Repositories;
using FeatureFlag.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FeatureFlag.Application.Integration.Tests
{
    public class FeatureFlagServiceIntegrationTests
    {
        private readonly IFeatureFlagService _service;
        private readonly IFeatureToggleRepository _repository;

        public FeatureFlagServiceIntegrationTests()
        {
            var services = new ServiceCollection();
            services.AddApplication();
            var provider = services.BuildServiceProvider();

            _service = provider.GetRequiredService<IFeatureFlagService>();
            _repository = provider.GetRequiredService<IFeatureToggleRepository>();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddFeatureToRepository()
        {
            // Arrange
            var feature = new FeatureToggle
            {
                Id = Guid.NewGuid(),
                Name = "TestFeature",
                Description = "Test Description",
                EnvironmentStates = new List<EnvironmentState>
                {
                    new() { Environment = EnvironmentEnum.Development, IsActive = true, Percentage = 100 }
                }
            };

            // Act
            var result = await _service.CreateAsync(feature);
            var retrieved = await _service.GetByIdOrNameAsync(feature.Id.ToString());

            // Assert
            Assert.True(result);
            Assert.NotNull(retrieved);
            Assert.Equal(feature.Name, retrieved!.Name);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldRemoveFeature()
        {
            // Arrange
            var feature = new FeatureToggle { Id = Guid.NewGuid(), Name = "ToDelete" };
            await _service.CreateAsync(feature);

            // Act
            var deleteResult = await _service.DeleteByIdAsync(feature.Id);
            var retrieved = await _service.GetByIdOrNameAsync(feature.Id.ToString());

            // Assert
            Assert.True(deleteResult);
            Assert.Null(retrieved);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllFeatures()
        {
            // Arrange
            var initialCount = (await _service.GetAllAsync()).Count();
            var feature1 = new FeatureToggle { Id = Guid.NewGuid(), Name = "Feature1" };
            var feature2 = new FeatureToggle { Id = Guid.NewGuid(), Name = "Feature2" };
            await _service.CreateAsync(feature1);
            await _service.CreateAsync(feature2);

            // Act
            var result = (await _service.GetAllAsync()).ToList();

            // Assert
            Assert.Equal(initialCount + 2, result.Count);
            Assert.Contains(result, f => f.Id == feature1.Id);
            Assert.Contains(result, f => f.Id == feature2.Id);
        }

        [Fact]
        public async Task GetByIdOrNameAsync_ShouldFindByBothIdAndName()
        {
            // Arrange
            var feature = new FeatureToggle 
            { 
                Id = Guid.NewGuid(), 
                Name = "FindMeFeature",
                EnvironmentStates = new List<EnvironmentState>
                {
                    new() { Environment = EnvironmentEnum.Development, IsActive = true, Percentage = 100 }
                }
            };
            await _service.CreateAsync(feature);

            // Act
            var byId = await _service.GetByIdOrNameAsync(feature.Id.ToString());
            var byName = await _service.GetByIdOrNameAsync(feature.Name);

            // Assert
            Assert.NotNull(byId);
            Assert.NotNull(byName);
            Assert.Equal(feature.Id, byId!.Id);
            Assert.Equal(feature.Id, byName!.Id);
        }

        [Fact]
        public async Task ToggleActivationAsync_ShouldUpdateFeatureState()
        {
            // Arrange
            var feature = new FeatureToggle
            {
                Id = Guid.NewGuid(),
                Name = "ToggleFeature",
                EnvironmentStates = new List<EnvironmentState>
                {
                    new() { Environment = EnvironmentEnum.Development, IsActive = false, Percentage = 0 }
                }
            };
            await _service.CreateAsync(feature);

            // Act
            var updated = await _service.ToggleActivationAsync(
                feature.Id, 
                EnvironmentEnum.Development, 
                50, 
                true);

            // Assert
            Assert.NotNull(updated);
            var envState = updated!.EnvironmentStates.First(e => e.Environment == EnvironmentEnum.Development);
            Assert.True(envState.IsActive);
            Assert.Equal(50, envState.Percentage);

            // Verify the change persisted
            var retrieved = await _service.GetByIdOrNameAsync(feature.Id.ToString());
            var retrievedEnvState = retrieved!.EnvironmentStates.First(e => e.Environment == EnvironmentEnum.Development);
            Assert.True(retrievedEnvState.IsActive);
            Assert.Equal(50, retrievedEnvState.Percentage);
        }

        [Fact]
        public async Task IsFeatureEnabled_ShouldWorkWithHardcodedData()
        {
            // Act & Assert for disabled feature
            var disabledFeatureId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var disabledResult = await _service.IsFeatureEnabled(disabledFeatureId, "any-client", EnvironmentEnum.Development);
            Assert.False(disabledResult);

            // Act & Assert for enabled feature
            var enabledFeatureId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var enabledResult = await _service.IsFeatureEnabled(enabledFeatureId, "any-client", EnvironmentEnum.Development);
            Assert.True(enabledResult);
        }

        [Fact]
        public async Task IsFeatureEnabled_ShouldRespectPercentageRollout()
        {
            // Arrange
            var feature = new FeatureToggle
            {
                Id = Guid.NewGuid(),
                Name = "PartialRollout",
                EnvironmentStates = new List<EnvironmentState>
                {
                    new() { Environment = EnvironmentEnum.Development, IsActive = true, Percentage = 50 }
                }
            };
            await _service.CreateAsync(feature);

            // Act - Test with multiple client IDs to see different results
            var client1Result = await _service.IsFeatureEnabled(feature.Id, "client1", EnvironmentEnum.Development);
            var client2Result = await _service.IsFeatureEnabled(feature.Id, "client2", EnvironmentEnum.Development);
            var client3Result = await _service.IsFeatureEnabled(feature.Id, "client3", EnvironmentEnum.Development);

            // Assert - Not all should be the same with 50% rollout
            // Note: In reality, we can't predict the exact results due to hashing,
            // but we can verify that not all are true or all are false
            Assert.False(client1Result && client2Result && client3Result); // Not all true
            Assert.False(!client1Result && !client2Result && !client3Result); // Not all false
        }
    }
}