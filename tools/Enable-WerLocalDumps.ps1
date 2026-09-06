<#
.SYNOPSIS
	Enables (or removes) Windows Error Reporting local crash dumps for AutoMouseMover.exe.

.DESCRIPTION
	Creates the per-application WER "LocalDumps" registry key so Windows writes a
	dump file whenever AutoMouseMover.exe crashes. This is the only safety net
	that catches native faults (for example an access violation inside a Win32
	mouse/display API), which never surface as a .NET exception and leave nothing
	in crash.log.

	Key:
	  HKLM:\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps\AutoMouseMover.exe

	Writing under HKLM needs administrator rights, so the script re-launches
	itself elevated; accept the UAC prompt. You only need to run it once per
	machine. Use -Remove to undo it.

	Caveats:
	  - WER does NOT write a dump while a debugger or profiler is attached to the
		process. Launch the built .exe normally when you want a dump.
	  - A full dump (-DumpType 2) is a snapshot of process memory and can be
		hundreds of MB. See the privacy note in docs/Diagnosing-Crashes.md.

.PARAMETER DumpFolder
	Where Windows should write the dumps. Stored as an expandable string, so
	environment variables such as %LOCALAPPDATA% are resolved per user at crash
	time. Defaults to %LOCALAPPDATA%\AutoMouseMover\Dumps, matching the other
	tools scripts.

.PARAMETER DumpCount
	How many dumps to keep in the folder before Windows starts overwriting the
	oldest. Defaults to 5.

.PARAMETER DumpType
	0 = custom, 1 = mini dump, 2 = full dump. Defaults to 2, which is what is
	needed to see the faulting native frame.

.PARAMETER Remove
	Delete the registry key again, restoring the Windows default behaviour.

.EXAMPLE
	powershell -ExecutionPolicy Bypass -File .\tools\Enable-WerLocalDumps.ps1

.EXAMPLE
	powershell -ExecutionPolicy Bypass -File .\tools\Enable-WerLocalDumps.ps1 -Remove

.EXAMPLE
	powershell -ExecutionPolicy Bypass -File .\tools\Enable-WerLocalDumps.ps1 -DumpType 1 -DumpCount 10
#>

[CmdletBinding(SupportsShouldProcess = $true, ConfirmImpact = 'Medium')]
param(
	[string]$DumpFolder = '%LOCALAPPDATA%\AutoMouseMover\Dumps',
	[int]$DumpCount = 5,
	[ValidateSet(0, 1, 2)]
	[int]$DumpType = 2,
	[switch]$Remove
)

$ErrorActionPreference = 'Stop'

$exeName = 'AutoMouseMover.exe'
$keyPath = "HKLM:\SOFTWARE\Microsoft\Windows\Windows Error Reporting\LocalDumps\$exeName"

# Get if the current process is running elevated
function Test-IsAdmin
{
	$identity = [Security.Principal.WindowsIdentity]::GetCurrent()
	$principal = New-Object Security.Principal.WindowsPrincipal($identity)
	return $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}

# Print the settings currently stored in the registry. Reading HKLM does not
# require elevation, so this also works in the non-elevated parent process.
function Show-CurrentState
{
	Write-Host ''
	if (Test-Path $keyPath) {
		$key = Get-ItemProperty -Path $keyPath
		Write-Host "WER local dumps are ENABLED for $exeName" -ForegroundColor Green
		Write-Host "  Key        : $keyPath"
		Write-Host "  DumpFolder : $($key.DumpFolder)"
		Write-Host "  DumpCount  : $($key.DumpCount)"
		$typeNames = @{ 0 = 'custom'; 1 = 'mini'; 2 = 'full' }
		$typeName = if ($typeNames.ContainsKey([int]$key.DumpType)) { $typeNames[[int]$key.DumpType] } else { 'unknown' }
		Write-Host ("  DumpType   : {0} ({1})" -f $key.DumpType, $typeName)
	}
	else {
		Write-Host "WER local dumps are NOT configured for $exeName" -ForegroundColor Yellow
		Write-Host "  Key        : $keyPath (absent)"
	}
}

# The dump folder lives in the user profile and needs no elevation, so create it
# here: the elevated process below may run under a different administrator account
if (-not $Remove) {
	$expandedFolder = [Environment]::ExpandEnvironmentVariables($DumpFolder)
	if (-not (Test-Path $expandedFolder)) {
		New-Item -Path $expandedFolder -ItemType Directory -Force | Out-Null
		# Re-test rather than assume: under -WhatIf nothing was actually created
		if (Test-Path $expandedFolder) {
			Write-Host "Created dump folder: $expandedFolder" -ForegroundColor Cyan
		}
	}
}

# Re-launch elevated if needed, then report the resulting state from here.
# Under -WhatIf there is nothing to elevate for: fall through and let
# ShouldProcess describe the registry writes without prompting for UAC.
if (-not (Test-IsAdmin) -and -not $WhatIfPreference) {
	Write-Host 'Administrator rights are required to write under HKLM.' -ForegroundColor Yellow
	Write-Host 'Re-launching elevated, please accept the UAC prompt...' -ForegroundColor Yellow

	$hostExe = (Get-Process -Id $PID).Path
	$argList = @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', ('"{0}"' -f $PSCommandPath))
	if ($Remove) {
		$argList += '-Remove'
	}
	else {
		$argList += @('-DumpFolder', ('"{0}"' -f $DumpFolder), '-DumpCount', $DumpCount, '-DumpType', $DumpType)
	}

	try {
		Start-Process -FilePath $hostExe -ArgumentList $argList -Verb RunAs -Wait
	}
	catch {
		Write-Host ''
		Write-Host "Elevation was cancelled or failed: $_" -ForegroundColor Red
		Show-CurrentState
		return
	}

	Show-CurrentState
	return
}

# From here on we are elevated
if ($Remove) {
	if (Test-Path $keyPath) {
		if ($PSCmdlet.ShouldProcess($keyPath, 'Remove registry key')) {
			Remove-Item -Path $keyPath -Recurse -Force
			Write-Host "  [x] Removed $keyPath"
		}
	}
	else {
		Write-Host "  [ ] $keyPath was not present"
	}
}
else {
	if ($PSCmdlet.ShouldProcess($keyPath, 'Configure WER local dumps')) {
		New-Item -Path $keyPath -Force | Out-Null
		# ExpandString so %LOCALAPPDATA% resolves for whichever user crashes
		New-ItemProperty -Path $keyPath -Name 'DumpFolder' -Value $DumpFolder -PropertyType ExpandString -Force | Out-Null
		New-ItemProperty -Path $keyPath -Name 'DumpCount'  -Value $DumpCount  -PropertyType DWord -Force | Out-Null
		New-ItemProperty -Path $keyPath -Name 'DumpType'   -Value $DumpType   -PropertyType DWord -Force | Out-Null
		Write-Host "  [x] Configured $keyPath"
	}
}

Show-CurrentState
