/*
 * Copyright (c) 2020 Emanuele Bellocchia
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using AutoMouseMover.Logic;
using AutoMouseMover.Utils;
using Microsoft.Win32;

namespace AutoMouseMover
{
    //
    // Main form
    //
    public partial class AutoMouseMoverForm : Form
    {
        //
        // Constants
        //
        #region Constants

        private const int BALLOON_TIP_TIMEOUT = 500;

        #endregion

        //
        // Members
        //
        #region Members

        // Automatic mouse mover
        private AutomaticMouseMover mAutoMouseMover;
        // Settings
        private SettingsHelper mSettings;
        // Resource manager
        private ResourceManager mResourceMng;
        // Flag to remember whether the timer was running when the system suspended
        private bool mWasRunningBeforeSuspend;
        // Flag to remember whether the timer was running when the session was locked
        private bool mWasRunningBeforeLock;

        #endregion

        //
        // Constructor
        //
        #region Constructor

        // Constructor
        public AutoMouseMoverForm()
        {
            InitializeComponent();
            InitializeResource();
            // Create classes
            mAutoMouseMover = new AutomaticMouseMover();
            mSettings = new SettingsHelper();
            // Load settings
            LoadSettings();
            // React to sleep/resume and monitor docking/undocking, which can change
            // the display topology underneath the cursor timer
            SystemEvents.PowerModeChanged += OnPowerModeChanged;
            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
            // Session lock/unlock is a reliable signal on Modern Standby laptops,
            // where PowerModeChanged Suspend/Resume is often not raised. Locking
            // (Win+L) typically precedes undocking and sleeping in normal use.
            SystemEvents.SessionSwitch += OnSessionSwitch;
        }

        #endregion

        //
        // GUI events
        //
        #region GUI events

        // Start button
        private void StartButton_Click(object sender, EventArgs e)
        {
            // Disable GUI on start
            SetGuiEnabled(false);
            // Set status
            SetStatus(mResourceMng.GetString("Running"));
            // Minimize to tray bar if requested
            if (MinimizeToTrayBarBox.Checked)
            {
                MinimizeWindowToTrayBar();
            }

            // Initialize auto mouse mover class
            mAutoMouseMover.Initialize((int)MovingPixelBox.Value, LeftClickAfterMovingBox.Checked);
            // Set timer interval and start it
            CursorTimer.Interval = ((int)MovingPeriodBox.Value) * 1000;
            CursorTimer.Start();
        }

        // Stop button
        private void StopButton_Click(object sender, EventArgs e)
        {
            SetStatus(mResourceMng.GetString("Idle"));
            SetGuiEnabled(true);
            CursorTimer.Stop();
        }

        // Minimize to tray icon check box changed
        private void MinimizeToTrayBarBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowTrayBarIconBox.Enabled = MinimizeToTrayBarBox.Checked;
            ShowTrayBarIconBox.Checked = MinimizeToTrayBarBox.Checked;
        }

        // Tray icon double clicked
        private void TrayBarIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RestoreWindowFromTrayBar();
        }

        // About button in menu strip
        private void StripMenuAbout_Click(object sender, EventArgs e)
        {
            AboutForm about_form = new AboutForm();
            about_form.ShowDialog();
        }

        // Open button in tray bar context menu
        private void TrayBarMenuOpen_Click(object sender, EventArgs e)
        {
            RestoreWindowFromTrayBar();
        }

        // Close button in tray bar context menu
        private void TrayBarMenuClose_Click(object sender, EventArgs e)
        {
            CursorTimer.Stop();
            Close();
        }

        // English button in menu strip
        private void StripMenuEnglish_Click(object sender, EventArgs e)
        {
            SetLanguage("en");
        }

        // Italian button in menu strip
        private void StripMenuItalian_Click(object sender, EventArgs e)
        {
            SetLanguage("it");
        }

        #endregion

        //
        // Other events
        //
        #region Other events

        // Closing form event
        private void AutoMouseMoverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // SystemEvents are static and would otherwise keep this form alive
            SystemEvents.PowerModeChanged -= OnPowerModeChanged;
            SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;
            SystemEvents.SessionSwitch -= OnSessionSwitch;
            SaveSettings();
        }

        // Form resize
        private void AutoMouseMoverForm_Resize(object sender, EventArgs e)
        {
            // If window is minized when timer is enabled, minimize it to tray bar
            if ((WindowState == FormWindowState.Minimized) && (CursorTimer.Enabled))
            {
                MinimizeWindowToTrayBar();
            }
        }

        // Cursor timer elapsed
        private void CursorTimer_Tick(object sender, EventArgs e)
        {
            mAutoMouseMover.MoveMouse();
        }

        // System power mode changed (system going to sleep or waking up)
        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            // Record every transition so a later crash/dump can be lined up
            // against sleep/resume activity
            CrashLogger.Log($"PowerModeChanged: {e.Mode}");

            // SystemEvents handlers are raised on a dedicated background thread, so
            // marshal back onto the UI thread before touching any WinForms control
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnPowerModeChanged(sender, e)));
                return;
            }

            switch (e.Mode)
            {
                // Stop the timer before the machine sleeps so it cannot fire against
                // a transient/invalid display topology while the system wakes up
                case PowerModes.Suspend:
                    mWasRunningBeforeSuspend = CursorTimer.Enabled;
                    CursorTimer.Stop();
                    break;

                // On resume, re-baseline the cursor state (monitors may have been
                // docked/undocked while asleep) before restarting the timer
                case PowerModes.Resume:
                    if (mWasRunningBeforeSuspend)
                    {
                        mWasRunningBeforeSuspend = false;
                        mAutoMouseMover.Initialize((int)MovingPixelBox.Value, LeftClickAfterMovingBox.Checked);
                        CursorTimer.Start();
                    }
                    break;
            }
        }

        // Display settings changed (monitor docked/undocked, resolution or DPI change)
        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            // Record the new monitor layout so a later crash/dump can be lined up
            // against docking/undocking activity
            CrashLogger.Log($"DisplaySettingsChanged: {CrashLogger.DescribeDisplays()}");

            // SystemEvents handlers are raised on a dedicated background thread, so
            // marshal back onto the UI thread before touching any WinForms control
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnDisplaySettingsChanged(sender, e)));
                return;
            }

            // Drop any cached cursor/screen position that may now reference a
            // monitor that no longer exists
            if (CursorTimer.Enabled)
            {
                mAutoMouseMover.Initialize((int)MovingPixelBox.Value, LeftClickAfterMovingBox.Checked);
            }
        }

        // Session locked/unlocked (e.g. Win+L). On a Modern Standby laptop this is
        // a reliable signal where PowerModeChanged often is not, and it usually
        // fires before the machine is undocked and put to sleep.
        private void OnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            // Record every transition so a later crash/dump can be lined up
            // against lock/unlock activity
            CrashLogger.Log($"SessionSwitch: {e.Reason}");

            // SystemEvents handlers are raised on a dedicated background thread, so
            // marshal back onto the UI thread before touching any WinForms control
            if (IsDisposed || !IsHandleCreated)
            {
                return;
            }
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnSessionSwitch(sender, e)));
                return;
            }

            switch (e.Reason)
            {
                // Stop the timer while the session is locked so it cannot fire
                // against a display topology that is about to be torn down (undock)
                case SessionSwitchReason.SessionLock:
                    mWasRunningBeforeLock = CursorTimer.Enabled;
                    CursorTimer.Stop();
                    break;

                // On unlock, re-baseline the cursor state (monitors may have been
                // docked/undocked while locked) before restarting the timer
                case SessionSwitchReason.SessionUnlock:
                    if (mWasRunningBeforeLock)
                    {
                        mWasRunningBeforeLock = false;
                        mAutoMouseMover.Initialize((int)MovingPixelBox.Value, LeftClickAfterMovingBox.Checked);
                        CursorTimer.Start();
                    }
                    break;
            }
        }

        #endregion

        //
        // Private methods
        //
        #region Private methods

        // Load settings
        private void LoadSettings()
        {
            try
            {
                MovingPeriodBox.Value = mSettings.MovingTime;
                MovingPixelBox.Value = mSettings.MovingPixel;
                LeftClickAfterMovingBox.Checked = mSettings.LeftClickAfterMoving;
                MinimizeToTrayBarBox.Checked = mSettings.MinimizeToTrayBar;
                ShowTrayBarIconBox.Checked = mSettings.ShowTrayBarIcon;
            }
            catch
            {
                // Default settings in case of errors
                mSettings.LoadDefault();
                // Set again
                MovingPeriodBox.Value = mSettings.MovingTime;
                MovingPixelBox.Value = mSettings.MovingPixel;
                LeftClickAfterMovingBox.Checked = mSettings.LeftClickAfterMoving;
                MinimizeToTrayBarBox.Checked = mSettings.MinimizeToTrayBar;
                ShowTrayBarIconBox.Checked = mSettings.ShowTrayBarIcon;
            }
        }

        // Save settings
        private void SaveSettings()
        {
            mSettings.MovingTime = (int)MovingPeriodBox.Value;
            mSettings.MovingPixel = (int)MovingPixelBox.Value;
            mSettings.LeftClickAfterMoving = LeftClickAfterMovingBox.Checked;
            mSettings.MinimizeToTrayBar = MinimizeToTrayBarBox.Checked;
            mSettings.ShowTrayBarIcon = ShowTrayBarIconBox.Checked;
            mSettings.Save();
        }

        // Enable/Disable GUI
        private void SetGuiEnabled(bool enabled)
        {
            StripMenuAbout.Enabled = enabled;
            StripMenuLanguage.Enabled = enabled;
            MovingPeriodBox.Enabled = enabled;
            MovingPixelBox.Enabled = enabled;
            LeftClickAfterMovingBox.Enabled = enabled;
            MinimizeToTrayBarBox.Enabled = enabled;
            ShowTrayBarIconBox.Enabled = MinimizeToTrayBarBox.Checked ? enabled : false;
            StartButton.Enabled = enabled;
            StopButton.Enabled = !enabled;
        }

        // Set status
        private void SetStatus(string text)
        {
            StatusTextLabel.Text = text;
        }

        // Minimize window to tray bar
        private void MinimizeWindowToTrayBar()
        {
            ShowInTaskbar = false;
            TrayBarIcon.Visible = ShowTrayBarIconBox.Checked;
            TrayBarIcon.ShowBalloonTip(BALLOON_TIP_TIMEOUT);
            Hide();
        }

        // Restore window from tray bar
        private void RestoreWindowFromTrayBar()
        {
            TrayBarIcon.Visible = false;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Show();
        }

        // Set language
        private void SetLanguage(string cCulture)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cCulture);

            bool minimize_to_tray = MinimizeToTrayBarBox.Checked;
            bool show_tray_icon = ShowTrayBarIconBox.Checked;
            decimal moving_period = MovingPeriodBox.Value;
            decimal moving_pixel = MovingPixelBox.Value;

            this.Controls.Clear();
            InitializeComponent();
            InitializeResource();

            MinimizeToTrayBarBox.Checked = minimize_to_tray;
            ShowTrayBarIconBox.Checked = show_tray_icon;
            MovingPeriodBox.Value = moving_period;
            MovingPixelBox.Value = moving_pixel;
        }

        // Initialize resource
        private void InitializeResource()
        {
            string language =
                System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            // The application currently includes only English and Italian resources.
            // Fall back to English for all unsupported system languages.
            if (language != "en" && language != "it")
            {
                language = "en";
            }

            string res_name =
                $"AutoMouseMover.Properties.lang_{language}";
            
            mResourceMng = new ResourceManager(
                res_name,
                Assembly.GetExecutingAssembly()
            );
        }

        #endregion
    }
}
