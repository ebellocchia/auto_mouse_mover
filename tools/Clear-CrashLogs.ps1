<#
.SYNOPSIS
	Clears AutoMouseMover diagnostic logs and dumps for a clean capture.

.DESCRIPTION
	Removes the managed crash log and (optionally) the native WER dump files so
	that the next run of AutoMouseMover.exe produces a fresh, easy-to-read data
	set. The app recreates crash.log automatically on its next launch.

	This only touches per-user diagnostic files, so no elevation is required. It
	does NOT remove the WER registry setting (use Enable-WerLocalDumps.ps1 -Remove
	for that) and does NOT delete the diagnostic zips on your Desktop.

.PARAMETER DumpFolder
	Folder where WER writes dumps. Defaults to %LOCALAPPDATA%\AutoMouseMover\Dumps,
	matching the other tools scripts.

.PARAMETER KeepDumps
	Clear only crash.log and leave any existing .dmp files in place.

.PARAMETER Backup
	Before clearing, archive the current crash.log and dumps into a timestamped
	zip on your Desktop, so nothing is lost if you haven't reviewed it yet.

.EXAMPLE
	pwsh -ExecutionPolicy Bypass -File .\tools\Clear-CrashLogs.ps1

.EXAMPLE
	pwsh -ExecutionPolicy Bypass -File .\tools\Clear-CrashLogs.ps1 -Backup

.EXAMPLE
	pwsh -ExecutionPolicy Bypass -File .\tools\Clear-CrashLogs.ps1 -KeepDumps
#>

[CmdletBinding()]
param(
	[string]$DumpFolder = (Join-Path $env:LOCALAPPDATA 'AutoMouseMover\Dumps'),
	[switch]$KeepDumps,
	[switch]$Backup
)

$ErrorActionPreference = 'Stop'

$appData = Join-Path $env:LOCALAPPDATA 'AutoMouseMover'
$logFile = Join-Path $appData 'crash.log'

# Warn if the app is running: old entries will be cleared, but the current
# session keeps logging, so restart the exe afterwards for a truly clean run
$running = Get-Process -Name 'AutoMouseMover' -ErrorAction SilentlyContinue
if ($running) {
	Write-Host 'AutoMouseMover is currently running.' -ForegroundColor Yellow
	Write-Host 'Close and relaunch it after this script for a clean session.' -ForegroundColor Yellow
	Write-Host ''
}

# Optional backup before deleting anything
if ($Backup) {
	$stamp   = Get-Date -Format 'yyyyMMdd-HHmmss'
	$staging = Join-Path $env:TEMP "AmmBackup-$stamp"
	$zipPath = Join-Path ([Environment]::GetFolderPath('Desktop')) "AutoMouseMover-logs-backup-$stamp.zip"
	New-Item -Path $staging -ItemType Directory -Force | Out-Null

	if (Test-Path $logFile) {
		Copy-Item $logFile (Join-Path $staging 'crash.log') -Force
	}
	if ((Test-Path $DumpFolder) -and (Get-ChildItem -Path $DumpFolder -Filter *.dmp -ErrorAction SilentlyContinue)) {
		Copy-Item (Join-Path $DumpFolder '*.dmp') $staging -Force
	}

	if (Get-ChildItem -Path $staging -ErrorAction SilentlyContinue) {
		Compress-Archive -Path (Join-Path $staging '*') -DestinationPath $zipPath -Force
		Write-Host "Backed up existing logs to:" -ForegroundColor Cyan
		Write-Host "  $zipPath"
	}
	else {
		Write-Host 'Nothing to back up (no existing logs or dumps).' -ForegroundColor Cyan
	}
	Remove-Item $staging -Recurse -Force
	Write-Host ''
}

# Clear the managed crash log
if (Test-Path $logFile) {
	Remove-Item $logFile -Force
	Write-Host "  [x] Removed crash.log"
}
else {
	Write-Host "  [ ] crash.log not present"
}

# Clear native dumps unless asked to keep them
if ($KeepDumps) {
	Write-Host "  [ ] Dumps left in place (-KeepDumps)"
}
elseif (Test-Path $DumpFolder) {
	$dumps = Get-ChildItem -Path $DumpFolder -Filter *.dmp -ErrorAction SilentlyContinue
	if ($dumps) {
		$dumps | Remove-Item -Force
		Write-Host ("  [x] Removed {0} dump file(s)" -f $dumps.Count)
	}
	else {
		Write-Host "  [ ] No dump files to remove"
	}
}
else {
	Write-Host "  [ ] Dump folder not present"
}

Write-Host ''
Write-Host 'Cleared. Launch AutoMouseMover.exe (standalone, no debugger/profiler)' -ForegroundColor Green
Write-Host 'to start a fresh capture.'
