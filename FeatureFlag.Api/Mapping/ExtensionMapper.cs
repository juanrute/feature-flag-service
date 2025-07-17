using FeatureFlag.Contracts.Requests;
using FeatureFlag.Application.Models;
using FeatureFlag.Contracts.Responses;

public static class ContractMapping
{
    public static FeatureToggle MapToFeatureToggle(this CreateFeatureFlagRequest request)
    {
        return new FeatureToggle
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };
    }

    public static FeatureFlagResponse MapCreatedResponse(this FeatureToggle featureToggle)
    {
        return new FeatureFlagResponse
        {
            Id = featureToggle.Id,
            Name = featureToggle.Name,
            Description = featureToggle.Description
        };
    }


    public static FeatureFlagDetailResponse MapToSingleResponse(this FeatureToggle featureToggle)
    {
        return new FeatureFlagDetailResponse
        {
            Id = featureToggle.Id,
            Name = featureToggle.Name,
            Description = featureToggle.Description,
            EnvironmentState = featureToggle.EnvironmentStates.ToMapResponse()
        };
    }

    public static List<FeatureFlagDetail> ToMapResponse(this List<EnvironmentState> environmentStates)
    {
        return environmentStates.Select(es => new FeatureFlagDetail
        {
            Environment = es.Environment.ToString(),
            IsActive = es.IsActive,
            Percentage = es.Percentage
        }).ToList();
    }

    public static List<FeatureFlagResponse> ToFeatureListResponse(this IEnumerable<FeatureToggle> features)
    {
        return features.Select(f => new FeatureFlagResponse
        {
            Id = f.Id,
            Name = f.Name,
            Description = f.Description
        }).ToList();
    }
}