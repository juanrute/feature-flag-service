namespace FeatureFlag.Contracts.Responses;


public class FeatureFlagDetailResponse : FeatureFlagResponse
{
    public required List<FeatureFlagDetail> EnvironmentState { get; init; }
}

public class FeatureFlagDetail
{    
    public required string Environment { get; init; }
    public required bool IsActive { get; init; }
    public required int Percentage { get; init; }
}