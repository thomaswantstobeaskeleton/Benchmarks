param(
  [string]$Configuration = "Release",
  [string]$Runtime = "win-x64",
  [switch]$SingleFile = $true,
  [switch]$SelfContained = $true
)

$ErrorActionPreference = "Stop"
$root = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $root

$out = Join-Path $root "artifacts/publish/$Runtime"
$single = if ($SingleFile) { "true" } else { "false" }
$self = if ($SelfContained) { "true" } else { "false" }

dotnet publish "src/WinOpti.App/WinOpti.App.csproj" `
  -c $Configuration `
  -r $Runtime `
  --self-contained $self `
  /p:PublishSingleFile=$single `
  /p:IncludeNativeLibrariesForSelfExtract=true `
  -o $out

Write-Host "Published to $out"
