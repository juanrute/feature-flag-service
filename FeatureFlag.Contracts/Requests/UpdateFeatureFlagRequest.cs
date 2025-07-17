using System.ComponentModel.DataAnnotations;

namespace FeatureFlag.Contracts.Requests;

public class UpdateFeatureFlagRequest
{
    public required string Environment { get; init; }

    public required bool IsEnabled { get; init; }
    
    [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100")]
    public int Percentage { get; init; } = 100;
}
