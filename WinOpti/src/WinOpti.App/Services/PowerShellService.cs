using System.Diagnostics;

namespace WinOpti.App.Services;

public sealed class PowerShellService
{
    public string Run(string command)
    {
        var psi = new ProcessStartInfo("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            UseShellExecute = false
        };

        using var p = Process.Start(psi)!;
        var stdout = p.StandardOutput.ReadToEnd();
        var stderr = p.StandardError.ReadToEnd();
        p.WaitForExit();
        return string.IsNullOrWhiteSpace(stderr) ? stdout.Trim() : $"ERROR: {stderr.Trim()}";
    }
}
