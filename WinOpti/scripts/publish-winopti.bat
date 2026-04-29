@echo off
setlocal
set ROOT=%~dp0..\
cd /d "%ROOT%"

set RID=win-x64
set CONF=Release
set OUT=artifacts\publish\%RID%

echo [WinOpti] Publishing self-contained executable...
dotnet publish src\WinOpti.App\WinOpti.App.csproj -c %CONF% -r %RID% --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o %OUT%
if errorlevel 1 exit /b 1

echo [WinOpti] Output: %OUT%
endlocal
