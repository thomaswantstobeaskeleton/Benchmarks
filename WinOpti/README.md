# WinOpti

WinOpti is a Windows optimization/tweaking utility focused on **safe, reversible changes**.

## Why C# WPF?
- Native Windows desktop UX with low overhead.
- Strong access to Windows APIs and admin context checks.
- Easy PowerShell integration for system-level tweaks.

## Current capabilities
- Dashboard snapshot fields (CPU, RAM, GPU, OS, startup count, health score).
- Modular tweak definitions (`Tweaks/tweaks.json`) with:
  - description
  - risk level
  - before value command
  - apply command
  - rollback command
  - admin requirement
  - preset membership
  - source attribution
- Dry-run mode.
- Restore point creation before apply.
- Change logging to `Logs/`.
- Undo all applied changes in reverse order.
- Presets:
  - Safe Optimization
  - Gaming Optimization
  - Low-End PC Optimization
  - Privacy-Focused
  - Custom Mode

## Blur Busters-informed tweaks
This project adds safe, conservative, **non-destructive** options inspired by discussions in Blur Busters optimization threads:
- Fullscreen optimization compatibility toggle concept (kept as template/placeholder, not hard-coded to a dangerous global switch).
- Gaming preset grouping for low-latency-oriented options.

Reference reading:
- https://forums.blurbusters.com/viewtopic.php?f=10&t=10986
- https://forums.blurbusters.com/viewtopic.php?f=10&t=7672
- https://forums.blurbusters.com/viewtopic.php?t=11799

## Build
```bash
dotnet build WinOpti.sln
```

## Run
```bash
dotnet run --project src/WinOpti.App/WinOpti.App.csproj
```

## Test
```bash
dotnet test WinOpti.sln
```

## Safety Notes
- Some tweaks require admin privileges (`RequiresAdmin = true`).
- Cleanup tweaks may be informationally reversible only.
- Always review dry-run output before apply.
- Avoid blanket timer/HPET/scripted BCD changes without hardware/game-specific verification.

## Developer Guide
See `docs/DEVELOPER_GUIDE.md` for adding new tweaks and validations.


## Viable workaround for restricted containers (commenced)
If package feeds are blocked, use the included bootstrap scripts:

```bash
cd WinOpti
./scripts/bootstrap-dotnet.sh 8.0.204
```

If internet is blocked, switch to offline artifact mode:

```bash
DOTNET_SDK_TARBALL=/workspace/Benchmarks/WinOpti/.cache/dotnet-sdk-8.0.204-linux-x64.tar.gz ./scripts/bootstrap-dotnet.sh 8.0.204
```

Then build/test via:

```bash
./scripts/build.sh
```


## Windows one-click scripts (.bat/.ps1)
From `WinOpti/scripts`:

- `build-winopti.bat` → build + test in Release.
- `run-winopti.bat` → run app directly.
- `publish-winopti.bat` → publish self-contained single-file EXE output to `artifacts\publish\win-x64`.
- `publish-winopti.ps1` → configurable PowerShell publish script.

Example:
```bat
cd WinOpti\scripts
build-winopti.bat
publish-winopti.bat
```

After publish, launch:
```bat
artifacts\publish\win-x64\WinOpti.App.exe
```
