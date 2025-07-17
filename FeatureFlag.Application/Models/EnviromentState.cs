namespace FeatureFlag.Application.Models;

public class EnvironmentState
{
    public required EnvironmentEnum Environment { get; init; }
    public required bool IsActive { get; set; } = false;
    public required int Percentage { get; set; } = 100;
}