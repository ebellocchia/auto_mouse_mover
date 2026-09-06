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
	  - system-info.txt           (OS, monitor, sleep-state and basic machine info)

	No elevation is required to read these. Run it AFTER a crash has happened.

	Two of the inputs are opt-in:
	  - crash.log only exists if the app was started with the "--debug-log"
		switch; diagnostic logging is off by default.
	  - .dmp files only exist if tools/Enable-WerLocalDumps.ps1 has been run
		once on this machine.
	A bundle is still produced if either one is missing.

.PARAMETER Hours
	How far back to pull Event Viewer entries. Defaults to 24 hours.

.PARAMETER DumpFolder
	Folder where WER writes dumps. Defaults to %LOCALAPPDATA%\AutoMouseMover\Dumps,
	matching tools/Enable-WerLocalDumps.ps1.

.PARAMETER MaxDumps
	How many of the newest dump files to include. Defaults to 3.

.EXAMPLE
	powershell -ExecutionPolicy Bypass -File .\tools\Collect-CrashLogs.ps1

.EXAMPLE
	powershell -ExecutionPolicy Bypass -File .\tools\Collect-CrashLogs.ps1 -Hours 48 -MaxDumps 5
#>

[CmdletBinding()]
param(
	[int]$Hours = 24,
	[string]$DumpFolder = (Join-Path $env:LOCALAPPDATA 'AutoMouseMover\Dumps'),
	[int]$MaxDumps = 3
)

$ErrorActionPreference = 'Continue'

$appData = Join-Path $env:LOCALAPPDATA 'AutoMouseMover'
$logFile = Join-Path $appData 'crash.log'
$stamp   = Get-Date -Format 'yyyyMMdd-HHmmss'
$staging = Join-Path $env:TEMP "AmmDiag-$stamp"
$zipPath = Join-Path ([Environment]::GetFolderPath('Desktop')) "AutoMouseMover-diag-$stamp.zip"
$since   = (Get-Date).AddHours(-$Hours)

# Read an event log without treating "nothing matched" as a failure: Get-WinEvent
# raises a terminating error when the filter yields no events at all
function Get-EventsSince
{
	param(
		[string]$LogName,
		[datetime]$Since
	)

	try {
		return @(Get-WinEvent -FilterHashtable @{ LogName = $LogName; StartTime = $Since } -ErrorAction Stop)
	}
	catch {
		if ($_.Exception.Message -match 'No events were found') {
			return @()
		}
		throw
	}
}

# Write the selected events to a report file, or a placeholder if there are none
function Save-EventReport
{
	param(
		[object[]]$Events,
		[string]$Path,
		[string]$EmptyMessage
	)

	if ($Events -and $Events.Count -gt 0) {
		$Events |
			Select-Object TimeCreated, ProviderName, Id, LevelDisplayName, Message |
			Format-List |
			Out-File -FilePath $Path -Encoding utf8 -Width 500
	}
	else {
		$EmptyMessage | Set-Content -Path $Path -Encoding utf8
	}
}

New-Item -Path $staging -ItemType Directory -Force | Out-Null
Write-Host "Collecting diagnostics since $since ..." -ForegroundColor Cyan

# 1) Managed crash log
if (Test-Path $logFile) {
	Copy-Item $logFile (Join-Path $staging 'crash.log') -Force
	Write-Host "  [+] crash.log"
}
else {
	@(
		"crash.log was not found at $logFile."
		''
		'Diagnostic logging is disabled by default. Start the app as'
		'"AutoMouseMover.exe --debug-log" to enable it, then reproduce the crash.'
	) | Set-Content (Join-Path $staging 'crash.log.MISSING.txt') -Encoding utf8
	Write-Host "  [-] crash.log not found (start the app with --debug-log to enable it)" -ForegroundColor Yellow
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
	Write-Host "      run tools\Enable-WerLocalDumps.ps1 once so Windows writes dumps there" -ForegroundColor Yellow
}

# 3) Application Error / .NET Runtime events
$appErrorsPath = Join-Path $staging 'application-errors.txt'
try {
	$appEvents = Get-EventsSince -LogName 'Application' -Since $since |
		Where-Object {
			# Anything naming our process, whichever provider reported it...
			($_.Message -match 'AutoMouseMover') -or
			# ...plus real fault reports. "Windows Error Reporting" is deliberately
			# NOT listed: it logs a steady stream of informational telemetry that
			# buries the signal, and its entries for our crash name the exe anyway.
			($_.ProviderName -in 'Application Error', '.NET Runtime')
		}
	Save-EventReport -Events $appEvents -Path $appErrorsPath -EmptyMessage "No matching Application events since $since."
	Write-Host "  [+] application-errors.txt"
}
catch {
	"Failed to read Application log: $_" | Set-Content $appErrorsPath -Encoding utf8
	Write-Host "  [-] application-errors.txt (failed)" -ForegroundColor Yellow
}

# 4) Power (sleep/resume/Modern Standby) and display/dock events
$powerPath = Join-Path $staging 'system-power-display.txt'
try {
	$sysEvents = Get-EventsSince -LogName 'System' -Since $since |
		Where-Object {
			# Keep meaningful power, Modern Standby, monitor and dock/PnP events...
			($_.ProviderName -match 'Kernel-Power|Kernel-Processor-Power|Kernel-PnP|UserPnp|Pdc|Win32k|Display|Dwm|MonitorReplacement') -or
			# ...matching whole words only, so "invalid"/"solid" do not drag in unrelated noise
			($_.Message -match '\b(monitors?|displays?|dock|docked|docking|undock|undocked|standby|lid)\b')
		} |
		# ...but drop the high-volume policy-scheme reset noise that drowns out the signal
		Where-Object { -not ($_.ProviderName -eq 'Microsoft-Windows-UserModePowerService' -and $_.Id -eq 12) }
	Save-EventReport -Events $sysEvents -Path $powerPath -EmptyMessage "No matching System events since $since."
	Write-Host "  [+] system-power-display.txt"
}
catch {
	"Failed to read System log: $_" | Set-Content $powerPath -Encoding utf8
	Write-Host "  [-] system-power-display.txt (failed)" -ForegroundColor Yellow
}

# 5) Basic system + monitor info
$infoPath = Join-Path $staging 'system-info.txt'
try {
	$info = [ordered]@{
		Timestamp    = (Get-Date)
		ComputerName = $env:COMPUTERNAME
		Machine      = (Get-CimInstance Win32_ComputerSystem | Select-Object Manufacturer, Model)
		OS           = (Get-CimInstance Win32_OperatingSystem).Caption
		OSVersion    = [Environment]::OSVersion.Version.ToString()
		Monitors     = (Get-CimInstance Win32_DesktopMonitor | Select-Object Name, ScreenWidth, ScreenHeight)
		VideoCtrls   = (Get-CimInstance Win32_VideoController |
						Select-Object Name, CurrentHorizontalResolution, CurrentVerticalResolution, DriverVersion)
		# Says whether this machine uses Modern Standby (S0) or classic S3 sleep, which
		# decides whether PowerModeChanged Suspend/Resume is raised by Windows at all
		SleepStates  = (powercfg /a | Out-String)
	}
	$info.GetEnumerator() | ForEach-Object {
		"=== $($_.Key) ==="; $_.Value | Out-String; ''
	} | Out-File -FilePath $infoPath -Encoding utf8 -Width 500
	Write-Host "  [+] system-info.txt"
}
catch {
	"Failed to gather system info: $_" | Set-Content $infoPath -Encoding utf8
	Write-Host "  [-] system-info.txt (failed)" -ForegroundColor Yellow
}

# Zip it up
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path (Join-Path $staging '*') -DestinationPath $zipPath -Force
Remove-Item $staging -Recurse -Force

$zipSize = (Get-Item $zipPath).Length / 1MB

Write-Host ''
Write-Host "Done. Diagnostics bundle created at:" -ForegroundColor Green
Write-Host ("  {0} ({1:N1} MB)" -f $zipPath, $zipSize)
Write-Host ''
Write-Host 'Attach that zip when reporting the issue. NOTE: full dumps can contain'
Write-Host 'memory contents - review before sharing externally.'
