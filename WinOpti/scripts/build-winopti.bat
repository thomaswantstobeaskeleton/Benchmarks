@echo off
setlocal
set ROOT=%~dp0..\
cd /d "%ROOT%"

echo [WinOpti] Building solution...
dotnet build WinOpti.sln -c Release
if errorlevel 1 exit /b 1

echo [WinOpti] Running tests...
dotnet test WinOpti.sln -c Release
if errorlevel 1 exit /b 1

echo [WinOpti] Done.
endlocal
