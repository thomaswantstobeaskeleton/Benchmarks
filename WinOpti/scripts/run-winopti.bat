@echo off
setlocal
set ROOT=%~dp0..\
cd /d "%ROOT%"

if not exist ".dotnet\dotnet.exe" (
  echo [WinOpti] Local .NET SDK not found in .dotnet\dotnet.exe
  echo Install .NET 8 SDK or place offline SDK in .dotnet\
)

dotnet run --project src\WinOpti.App\WinOpti.App.csproj
endlocal
