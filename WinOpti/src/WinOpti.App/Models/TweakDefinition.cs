namespace WinOpti.App.Models;

public enum RiskLevel { Safe, Moderate, Advanced }

public sealed class TweakDefinition
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required string Description { get; init; }
    public required RiskLevel RiskLevel { get; init; }
    public required string BeforeValueCommand { get; init; }
    public required string ApplyCommand { get; init; }
    public required string RollbackCommand { get; init; }
    public string? AfterExpectedValue { get; init; }
    public bool RequiresAdmin { get; init; }
    public string[] Presets { get; init; } = [];
    public string Source { get; init; } = "Internal";
}

public sealed class SystemSnapshot
{
    public string Cpu { get; init; } = "Unknown";
    public string Ram { get; init; } = "Unknown";
    public string Gpu { get; init; } = "Unknown";
    public string WindowsVersion { get; init; } = "Unknown";
    public string DiskUsage { get; init; } = "Unknown";
    public int StartupApps { get; init; }
    public int HealthScore { get; init; }
}

public sealed class ChangeRecord
{
    public required string TweakId { get; init; }
    public required string TweakName { get; init; }
    public required string Category { get; init; }
    public required DateTime TimestampUtc { get; init; }
    public required bool DryRun { get; init; }
    public required string BeforeValue { get; init; }
    public required string CommandRun { get; init; }
    public required string Result { get; init; }
}
