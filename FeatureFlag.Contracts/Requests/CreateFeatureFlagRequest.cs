namespace FeatureFlag.Contracts.Requests;

public class CreateFeatureFlagRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}