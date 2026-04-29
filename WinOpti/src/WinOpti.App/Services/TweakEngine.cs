using WinOpti.App.Models;

namespace WinOpti.App.Services;

public sealed class TweakEngine
{
    private readonly PowerShellService _ps;
    private readonly LoggerService _logger;
    private readonly List<TweakDefinition> _applied = [];

    public TweakEngine(PowerShellService ps, LoggerService logger)
    {
        _ps = ps;
        _logger = logger;
    }

    public string CreateRestorePoint(bool dryRun)
    {
        const string cmd = "Checkpoint-Computer -Description 'WinOpti Restore Point' -RestorePointType 'MODIFY_SETTINGS'";
        return dryRun ? $"[Dry Run] {cmd}" : _ps.Run(cmd);
    }

    public string Apply(TweakDefinition tweak, bool dryRun)
    {
        Validate(tweak);
        var before = _ps.Run(tweak.BeforeValueCommand);
        var result = dryRun ? $"[Dry Run] {tweak.ApplyCommand}" : _ps.Run(tweak.ApplyCommand);
        if (!dryRun) _applied.Add(tweak);
        _logger.Log(new ChangeRecord
        {
            TweakId = tweak.Id,
            TweakName = tweak.Name,
            Category = tweak.Category,
            TimestampUtc = DateTime.UtcNow,
            DryRun = dryRun,
            BeforeValue = before,
            CommandRun = tweak.ApplyCommand,
            Result = result
        });
        return result;
    }

    public IEnumerable<string> UndoAll(bool dryRun)
    {
        foreach (var t in _applied.AsEnumerable().Reverse())
        {
            var res = dryRun ? $"[Dry Run] {t.RollbackCommand}" : _ps.Run(t.RollbackCommand);
            _logger.Log(new ChangeRecord
            {
                TweakId = t.Id,
                TweakName = t.Name,
                Category = t.Category,
                TimestampUtc = DateTime.UtcNow,
                DryRun = dryRun,
                BeforeValue = string.Empty,
                CommandRun = t.RollbackCommand,
                Result = res
            });
            yield return $"{t.Name}: {res}";
        }
        if (!dryRun) _applied.Clear();
    }

    public static void Validate(TweakDefinition tweak)
    {
        if (string.IsNullOrWhiteSpace(tweak.RollbackCommand)) throw new InvalidOperationException("Rollback command is required.");
        if (string.IsNullOrWhiteSpace(tweak.BeforeValueCommand)) throw new InvalidOperationException("Before value command is required.");
    }
}
