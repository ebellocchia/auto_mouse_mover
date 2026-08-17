<#
.SYNOPSIS
	Collects AutoMouseMover crash diagnostics into a single zip file.

.DESCRIPTION
	Gathers everything needed to investigate a crash and bundles it into one
	timestamped .zip on your Desktop:
	  - crash.log                 (managed log written by the app)
	  - the most recent .dmp files (native dumps captured by WER)
	  - application-errors.txt    (Windows "Application Error" / .NET runtime events)
	  - system-power-display.txt  (Kernel-Power sleep/resume + display events)
	  - system-info.txt           (OS, monitor and basic machine info)

	No elevation is required to read these. Run it AFTER a crash has happened.

.PARAMETER Hours
	How far back to pull Event Viewer entries. Defaults to 24 hours.

.PARAMETER DumpFolder
	Folder where WER writes dumps. Defaults to %LOCALAPPDATA%\AutoMouseMover\Dumps,
	matching tools/Enable-WerLocalDumps.ps1.

.PARAMETER MaxDumps
	How many of the newest dump files to include. Defaults to 3.

.EXAMPLE
	pwsh -ExecutionPolicy Bypass -File .\tools\Collect-CrashLogs.ps1

.EXAMPLE
	pwsh -ExecutionPolicy Bypass -File .\tools\Collect-CrashLogs.ps1 -Hours 48 -MaxDumps 5
#>

[CmdletBinding()]
param(
	[int]$Hours = 24,
	[string]$DumpFolder = (Join-Path $env:LOCALAPPDATA 'AutoMouseMover\Dumps'),
	[int]$MaxDumps = 3
)

$ErrorActionPreference = 'Continue'

$appData   = Join-Path $env:LOCALAPPDATA 'AutoMouseMover'
$logFile   = Join-Path $appData 'crash.log'
$stamp     = Get-Date -Format 'yyyyMMdd-HHmmss'
$staging   = Join-Path $env:TEMP "AmmDiag-$stamp"
$zipPath   = Join-Path ([Environment]::GetFolderPath('Desktop')) "AutoMouseMover-diag-$stamp.zip"
$since     = (Get-Date).AddHours(-$Hours)

New-Item -Path $staging -ItemType Directory -Force | Out-Null
Write-Host "Collecting diagnostics since $since ..." -ForegroundColor Cyan

# 1) Managed crash log
if (Test-Path $logFile) {
	Copy-Item $logFile (Join-Path $staging 'crash.log') -Force
	Write-Host "  [+] crash.log"
}
else {
	"crash.log was not found at $logFile (the app may not have logged anything yet)." |
		Set-Content (Join-Path $staging 'crash.log.MISSING.txt')
	Write-Host "  [-] crash.log not found" -ForegroundColor Yellow
}

# 2) Newest native dumps
if (Test-Path $DumpFolder) {
	$dumps = Get-ChildItem -Path $DumpFolder -Filter *.dmp -ErrorAction SilentlyContinue |
		Sort-Object LastWriteTime -Descending | Select-Object -First $MaxDumps
	foreach ($d in $dumps) {
		Copy-Item $d.FullName (Join-Path $staging $d.Name) -Force
		Write-Host ("  [+] {0} ({1:N1} MB)" -f $d.Name, ($d.Length / 1MB))
	}
	if (-not $dumps) { Write-Host "  [-] no .dmp files in $DumpFolder yet" -ForegroundColor Yellow }
}
else {
	Write-Host "  [-] dump folder $DumpFolder does not exist yet" -ForegroundColor Yellow
}

# 3) Application Error / .NET Runtime events
try {
	Get-WinEvent -FilterHashtable @{ LogName = 'Application'; StartTime = $since } -ErrorAction Stop |
		Where-Object {
			($_.ProviderName -in 'Application Error', '.NET Runtime', 'Windows Error Reporting') -or
			($_.Message -match 'AutoMouseMover')
		} |
		Select-Object TimeCreated, ProviderName, Id, LevelDisplayName, Message |
		Format-List | Out-File (Join-Path $staging 'application-errors.txt')
	Write-Host "  [+] application-errors.txt"
}
catch { "Failed to read Application log: $_" | Out-File (Join-Path $staging 'application-errors.txt') }

# 4) Power (sleep/resume/Modern Standby) and display/dock events
try {
	Get-WinEvent -FilterHashtable @{ LogName = 'System'; StartTime = $since } -ErrorAction Stop |
		Where-Object {
			# Keep meaningful power, Modern Standby, monitor and dock/PnP events...
			($_.ProviderName -match 'Kernel-Power|Kernel-Processor-Power|Kernel-PnP|UserPnp|Pdc|Win32k|Display|Dwm|MonitorReplacement|001') -or
			($_.Message -match 'monitor|display|dock|standby|lid')
		} |
		# ...but drop the high-volume policy-scheme reset noise that drowns out the signal
		Where-Object { -not ($_.ProviderName -eq 'Microsoft-Windows-UserModePowerService' -and $_.Id -eq 12) } |
		Select-Object TimeCreated, ProviderName, Id, LevelDisplayName, Message |
		Format-List | Out-File (Join-Path $staging 'system-power-display.txt')
	Write-Host "  [+] system-power-display.txt"
}
catch { "Failed to read System log: $_" | Out-File (Join-Path $staging 'system-power-display.txt') }

# 5) Basic system + monitor info
try {
	$info = [ordered]@{
		Timestamp   = (Get-Date)
		ComputerName= $env:COMPUTERNAME
		OS          = (Get-CimInstance Win32_OperatingSystem).Caption
		OSVersion   = [Environment]::OSVersion.Version.ToString()
		Monitors    = (Get-CimInstance Win32_DesktopMonitor | Select-Object Name, ScreenWidth, ScreenHeight)
		VideoCtrls  = (Get-CimInstance Win32_VideoController |
						Select-Object Name, CurrentHorizontalResolution, CurrentVerticalResolution, DriverVersion)
	}
	$info.GetEnumerator() | ForEach-Object {
		"=== $($_.Key) ==="; $_.Value | Out-String; ''
	} | Out-File (Join-Path $staging 'system-info.txt')
	Write-Host "  [+] system-info.txt"
}
catch { "Failed to gather system info: $_" | Out-File (Join-Path $staging 'system-info.txt') }

# Zip it up
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path (Join-Path $staging '*') -DestinationPath $zipPath -Force
Remove-Item $staging -Recurse -Force

Write-Host ''
Write-Host "Done. Diagnostics bundle created at:" -ForegroundColor Green
Write-Host "  $zipPath"
Write-Host ''
Write-Host 'Attach that zip when reporting the issue. NOTE: full dumps can contain'
Write-Host 'memory contents - review before sharing externally.'
