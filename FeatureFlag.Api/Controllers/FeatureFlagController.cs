using Microsoft.AspNetCore.Mvc;
using FeatureFlag.Contracts.Requests;
using FeatureFlag.Application.Services;

namespace FeatureFlag.Api.Controllers;

[ApiController]
public class FeatureFlagController(IFeatureFlagService _featureFlagService) : ControllerBase
{


    [HttpPost(Endpoints.FeatureFlag.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateFeatureFlagRequest request,
        CancellationToken token)
    {
        var featureToggle = request.MapToFeatureToggle();
        await _featureFlagService.CreateAsync(featureToggle, token);

        var featureFlagResponse = featureToggle.MapCreatedResponse();
        return CreatedAtAction(nameof(Get), new { idOrName = featureToggle.Id }, featureFlagResponse);
    }

    [HttpGet(Endpoints.FeatureFlag.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var featureFlags = await _featureFlagService.GetAllAsync(token);
        var response = featureFlags.ToFeatureListResponse();
        return Ok(response);
    }

    [HttpGet(Endpoints.FeatureFlag.Get)]
    public async Task<IActionResult> Get(
        [FromRoute] string idOrName,
        CancellationToken token)
    {
        var featureFlag = await _featureFlagService.GetByIdOrNameAsync(idOrName, token);
        if (featureFlag is null)
        {
            return NotFound();
        }

        return Ok(featureFlag.MapToSingleResponse());
    }

    [HttpPut(Endpoints.FeatureFlag.Update)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateFeatureFlagRequest request,
        CancellationToken token)
    {
        if (!IsValidEnvironment(request.Environment, out var envEnum))
        {
            return BadRequest(new
            {
                Error = $"Invalid environment. Valid values: {string.Join(", ", Enum.GetNames<EnvironmentEnum>())}"
            });
        }

        var updatedFeatureFlag = await _featureFlagService.ToggleActivationAsync(
            id,
            envEnum,
            request.Percentage,
            request.IsEnabled,
            token);

        if (updatedFeatureFlag is null)
        {
            return NotFound();
        }

        return Ok();



    }

    [HttpDelete(Endpoints.FeatureFlag.Delete)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken token)
    {
        var deleted = await _featureFlagService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpGet(Endpoints.FeatureFlag.GetActive)]
    public async Task<IActionResult> IsActiveByEnvironment(
        [FromRoute] Guid id,
        [FromRoute] string environment,
        [FromRoute] string clientId,
        CancellationToken token)
    {
        if (!IsValidEnvironment(environment, out var envEnum))
        {
            return BadRequest(new
            {
                Error = $"Invalid environment. Valid values: {string.Join(", ", Enum.GetNames<EnvironmentEnum>())}"
            });
        }
        var featureFlag = await _featureFlagService.IsFeatureEnabled(id, clientId, envEnum, token);

        return Ok(featureFlag);
    }
    

    private bool IsValidEnvironment(string environment, out EnvironmentEnum envEnum)
    {
        return Enum.TryParse<EnvironmentEnum>(environment, ignoreCase: true, out envEnum) &&
            Enum.IsDefined(typeof(EnvironmentEnum), envEnum);
    }
}
