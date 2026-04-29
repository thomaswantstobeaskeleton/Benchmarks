using System.Text.Json;
using WinOpti.App.Models;

namespace WinOpti.App.Services;

public sealed class LoggerService
{
    private readonly string _logPath;
    public LoggerService(string logDirectory)
    {
        Directory.CreateDirectory(logDirectory);
        _logPath = Path.Combine(logDirectory, $"winopti-{DateTime.UtcNow:yyyyMMdd}.log");
    }

    public void Log(ChangeRecord record)
    {
        var line = JsonSerializer.Serialize(record);
        File.AppendAllText(_logPath, line + Environment.NewLine);
    }
}
