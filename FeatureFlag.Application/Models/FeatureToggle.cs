namespace FeatureFlag.Application.Models;

public class FeatureToggle
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public DateTime CreatedAt { get; set; }
    public List<EnvironmentState> EnvironmentStates { get; init; } = GetAllEnvironmentsWithDefaults();
   
    private static List<EnvironmentState> GetAllEnvironmentsWithDefaults()
    {
        return Enum.GetValues(typeof(EnvironmentEnum))
            .Cast<EnvironmentEnum>()
            .Select(env => new EnvironmentState
            {
                Environment = env,
                IsActive = false,
                Percentage = 0
            })
            .ToList();
    }
}