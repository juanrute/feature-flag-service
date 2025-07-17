using System.ComponentModel.DataAnnotations;

namespace FeatureFlag.Contracts.Requests;

public class UpdatePartiallyRequest
{
    public required Guid FeatureFlagId { get; init; }
    public required string Environment { get; init;  }
    [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100")]
    public required int Percentage { get; init; }
}