namespace FeatureFlag.Application.Models;

public class Audit
{
    public required string Action { get; set; }
    public required string Detail { get; set; }
    public required Guid FlagId { get; set; }
    public  DateTime DateTime { get; set; }
    public string? ByUser { get; set; }
}