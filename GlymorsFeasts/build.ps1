# Build GlymorsFeasts. Looks up Valheim + Jotunn/BepInEx refs on this machine.
# Requires the .NET SDK. Does not copy game DLLs into the git repo.
$ErrorActionPreference = "Stop"
$Root = $PSScriptRoot
$Refs = Join-Path $Root "refs"
$Game = "C:\Program Files (x86)\Steam\steamapps\common\Valheim"
$Profile = Join-Path $env:APPDATA "Thunderstore Mod Manager\DataFolder\Valheim\profiles\GLYMOR"
if ($env:VALHEIM_PATH -and (Test-Path $env:VALHEIM_PATH)) { $Game = $env:VALHEIM_PATH }
if (-not (Test-Path "$Game\valheim_Data\Managed\assembly_valheim.dll")) {
  throw "Valheim assemblies not found at $Game. Set VALHEIM_PATH."
}
New-Item -ItemType Directory -Force -Path "$Refs\managed","$Refs\bepinex","$Refs\jotunn" | Out-Null
Copy-Item "$Game\valheim_Data\Managed\assembly_valheim.dll","$Game\valheim_Data\Managed\assembly_utils.dll","$Game\valheim_Data\Managed\UnityEngine*.dll" -Destination "$Refs\managed" -Force
$be = "$Profile\BepInEx\core"
if (-not (Test-Path "$be\BepInEx.dll")) { throw "BepInEx not found in $Profile" }
Copy-Item "$be\BepInEx.dll","$be\0Harmony.dll" -Destination "$Refs\bepinex" -Force
$jo = Get-ChildItem "$Profile\BepInEx\plugins" -Recurse -Filter "Jotunn.dll" | Select-Object -First 1
if (-not $jo) { throw "Jotunn.dll not found in the GLYMOR profile plugins folder." }
Copy-Item $jo.FullName -Destination "$Refs\jotunn\Jotunn.dll" -Force
$jx = Join-Path $jo.DirectoryName "Jotunn.xml"
if (Test-Path $jx) { Copy-Item $jx -Destination "$Refs\jotunn\Jotunn.xml" -Force }
$dotnet = $null
$dotnetCandidates = @()
if ($env:DOTNET_ROOT) { $dotnetCandidates += (Join-Path $env:DOTNET_ROOT "dotnet.exe") }
$dotnetCandidates += @(
  "E:\Codex-Mods\tools\dotnet\dotnet.exe",
  "$env:ProgramFiles\dotnet\dotnet.exe",
  "$env:USERPROFILE\.dotnet\dotnet.exe",
  "$env:LOCALAPPDATA\Microsoft\dotnet\dotnet.exe"
)
foreach ($c in $dotnetCandidates) {
  if ($c -and (Test-Path $c)) { $dotnet = $c; break }
}
if (-not $dotnet) {
  $cmd = Get-Command dotnet -ErrorAction SilentlyContinue
  if ($cmd) { $dotnet = $cmd.Source }
}
if ($dotnet) {
  & $dotnet build "$Root\GlymorsFeasts.csproj" -c Release -p:ValheimRefs="$Refs"
  if ($LASTEXITCODE -ne 0) { throw "dotnet build failed with exit $LASTEXITCODE" }
} else {
  throw "dotnet SDK not found. Install the .NET SDK or set DOTNET_ROOT."
}