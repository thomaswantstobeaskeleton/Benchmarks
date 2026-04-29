using WinOpti.App.Models;

namespace WinOpti.App.Services;

public sealed class SystemInfoService
{
    private readonly PowerShellService _ps = new();

    public SystemSnapshot GetSnapshot()
    {
        var cpu = _ps.Run("(Get-CimInstance Win32_Processor | Select-Object -First 1 -ExpandProperty Name)");
        var ramGb = _ps.Run("[math]::Round(((Get-CimInstance Win32_ComputerSystem).TotalPhysicalMemory / 1GB),1)");
        var gpu = _ps.Run("(Get-CimInstance Win32_VideoController | Select-Object -First 1 -ExpandProperty Name)");
        var win = _ps.Run("(Get-ComputerInfo -Property WindowsProductName,WindowsVersion | Format-Table -HideTableHeaders | Out-String)");
        var disk = _ps.Run("(Get-PSDrive C | ForEach-Object { '{0:N1}% free' -f (($_.Free/$_.Used)*100) })");
        var startup = int.TryParse(_ps.Run("(Get-CimInstance Win32_StartupCommand | Measure-Object).Count"), out var s) ? s : 0;

        var health = 100;
        if (startup > 20) health -= 20;
        if (startup > 35) health -= 20;

        return new SystemSnapshot
        {
            Cpu = cpu,
            Ram = $"{ramGb} GB",
            Gpu = gpu,
            WindowsVersion = win.Trim(),
            DiskUsage = disk,
            StartupApps = startup,
            HealthScore = Math.Max(0, health)
        };
    }
}
