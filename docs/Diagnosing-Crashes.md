# Diagnosing AutoMouseMover Crashes

This app touches the Windows mouse/display APIs directly. When one of those native
APIs faults (for example an **access violation**), the crash happens *below* .NET,
so there is no exception dialog and nothing in a normal log — the process just
vanishes. This guide explains how to capture those crashes anyway.

There are **two** independent safety nets:

| Mechanism | Catches | Where it goes |
|---|---|---|
| **Managed crash log** (built into the app) | .NET exceptions (UI thread, background threads) + a startup banner and a timeline of sleep/resume & dock events | `%LOCALAPPDATA%\AutoMouseMover\crash.log` |
| **WER local dumps** (Windows feature) | True native faults that bypass .NET entirely | `%LOCALAPPDATA%\AutoMouseMover\Dumps\*.dmp` |

> Rule of thumb: if `crash.log` has an entry for the crash, it was a **managed**
> problem. If `crash.log` says nothing but a `.dmp` appeared, it was a **native**
> problem (the API-level crash we suspect).

---

## One-time setup

### 1. Enable WER local dumps (needs admin once)

```powershell
pwsh -ExecutionPolicy Bypass -File .\tools\Enable-WerLocalDumps.ps1
```

This creates a registry key telling Windows to save a full dump of
`AutoMouseMover.exe` whenever it crashes. You only do this once per machine.
Accept the UAC prompt if it appears. To undo it later:

```powershell
pwsh -ExecutionPolicy Bypass -File .\tools\Enable-WerLocalDumps.ps1 -Remove
```

### 2. Confirm logging works

Just launch the app. The first lines of
`%LOCALAPPDATA%\AutoMouseMover\crash.log` should be a `SESSION START` banner
listing your OS, DPI mode, and current monitors. If you see that, logging is live.

### 3. Capturing a native dump reliably

Two things will stop WER from capturing the dump we want:

- **Don't run under a debugger or profiler.** If Visual Studio (F5) or a
  profiler such as JetBrains DPA (`JetBrains.Dpa.Collector.exe`) is attached, it
  intercepts the native fault and **no `.dmp` is written**. Launch the built
  `.exe` from a shortcut/Explorer instead.
- **Make sure the mover is actually started** (timer running) and stays running
  *across* the transition you're testing.

### Modern Standby laptops

This machine uses **Modern Standby** (S0 low-power idle), where the classic
`PowerModeChanged` Suspend/Resume notifications are often **not** raised. The app
therefore also keys off **session lock/unlock** (`Win+L`), which is reliable on
these systems and usually precedes undocking and sleeping. In `crash.log` you
should see `SessionSwitch: SessionLock` / `SessionUnlock` lines around those
moments — those are the breadcrumbs to look for.

---

## Start a clean capture

So the next run yields a fresh, easy-to-read data set, clear the old log and
dumps first:

```powershell
pwsh -ExecutionPolicy Bypass -File .\tools\Clear-CrashLogs.ps1
```

Add `-Backup` to archive the current log/dumps to a zip on your Desktop before
clearing (useful if you haven't reviewed them yet), or `-KeepDumps` to wipe only
`crash.log`. The app recreates `crash.log` on its next launch. Then start
`AutoMouseMover.exe` standalone (no debugger/profiler) and reproduce.

---

## When a crash happens

1. **Note the time** and what you were doing (docking, undocking, waking from sleep…).
2. Run the collector — it bundles the log, the newest dumps, and the relevant
   Windows event-log entries into one zip on your Desktop:

   ```powershell
   pwsh -ExecutionPolicy Bypass -File .\tools\Collect-CrashLogs.ps1
   ```

   Use `-Hours 48` to look further back, or `-MaxDumps 5` for more dumps.

That zip is the single artifact to attach when reporting the issue.

---

## Reading the results yourself (optional)

### crash.log

Plain text, newest entries at the bottom. You'll see:

- `SESSION START` banners (one per launch),
- `PowerModeChanged: Suspend/Resume` lines,
- `DisplaySettingsChanged: ...` lines listing the monitor layout at that moment,
- any `... failed, Win32 error N` lines from the native API wrappers,
- managed exception dumps (`UI thread (ThreadException): ...`, `AppDomain ...`).

If the **last** thing before the app died was a `DisplaySettingsChanged` or
`PowerModeChanged` line with **no** exception after it, that strongly points at a
native fault during a dock/sleep transition — exactly the suspected cause.

### The .dmp file (Visual Studio)

1. Double-click the `.dmp` to open it in Visual Studio.
2. On the summary page, choose **Debug with Mixed** (native + managed).
3. When it breaks, open **Debug ▶ Windows ▶ Call Stack**. The top frames usually
   name the culprit — look for `user32.dll!SendInput`, `EnumDisplayMonitors`, or a
   GDI/display teardown frame.
4. **Debug ▶ Windows ▶ Exception Settings** ▶ tick **Win32 Exceptions ▶
   `0xC0000005` Access violation** if you want the debugger to stop exactly there.

### Lining up the timeline

Open the two event files from the zip side by side:

- `application-errors.txt` — find the **Application Error (Event ID 1000)** for
  `AutoMouseMover.exe`; note its timestamp.
- `system-power-display.txt` — look for **Kernel-Power** (sleep/resume) or display
  events at the *same second*.

A tight match between the crash and a power/display event confirms the
reconfiguration-race theory over a random one.

---

## Live debugging in Visual Studio (alternative to dumps)

If you can reproduce on demand while VS is attached:

1. **Tools ▶ Options ▶ Debugging ▶ General ▶ Enable native code debugging**
   (mixed-mode), so the debugger can see the API frames.
2. **Debug ▶ Windows ▶ Exception Settings ▶ Win32 Exceptions ▶** tick
   **Access violation (0xC0000005)**.
3. Run, then dock/undock or sleep/wake to trigger it. VS will break at the
   faulting native frame instead of the app silently disappearing.

---

## Privacy note

A **full** dump (`DumpType = 2`) contains a snapshot of the process memory. It's
the right choice for debugging this kind of fault, but review/limit who you share
the zip with outside your team.
