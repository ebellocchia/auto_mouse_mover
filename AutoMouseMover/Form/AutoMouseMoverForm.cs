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
using System.Windows.Forms;
using AutoMouseMover.Logic;
using AutoMouseMover.Utils;

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

        // Idle status string
        private const string STATUS_IDLE_STR    = "idle";
        // Running status string
        private const string STATUS_RUNNING_STR = "running";
        // Balloon tip timeout
        private const int  BALLOON_TIP_TIMEOUT  = 500;

        #endregion

        //
        // Members
        //
        #region Members

        // Automatic mouse mover
        private AutomaticMouseMover mAutoMouseMover;
        // Settings
        private SettingsHelper      mSettings;

        #endregion

        //
        // Constructor
        //
        #region Constructor

        // Constructor
        public AutoMouseMoverForm()
        {
            InitializeComponent();
            // Create classes
            mAutoMouseMover = new AutomaticMouseMover();
            mSettings = new SettingsHelper();
            // Load settings
            LoadSettings();
            // Set status
            SetStatus(STATUS_IDLE_STR);
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
            DisableGuiOnStart();
            // Minimize to tray bar if requested
            if (MinimizeToTrayBarBox.Checked)
            {
                MinimizeWindowToTrayBar();
            }
            // Set status
            SetStatus(STATUS_RUNNING_STR);
            // Initialize auto mouse mover class
            mAutoMouseMover.Initialize((int) MovingPixelBox.Value);
            // Set timer interval and start it
            CursorTimer.Interval = ((int) MovingPeriodBox.Value) * 1000;
            CursorTimer.Start();
        }

        // Stop button
        private void StopButton_Click(object sender, EventArgs e)
        {
            SetStatus(STATUS_IDLE_STR);
            EnableGuiOnStop();
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

        #endregion

        //
        // Other events
        //
        #region Other events

        // Closing form event
        private void AutoMouseMoverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
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
                MovingPeriodBox.Value        = mSettings.MovingTime;
                MovingPixelBox.Value         = mSettings.MovingPixel;
                MinimizeToTrayBarBox.Checked = mSettings.MinimizeToTrayBar;
                ShowTrayBarIconBox.Checked   = mSettings.ShowTrayBarIcon;
            }
            catch
            {
                // Default settings in case of errors
                mSettings.LoadDefault();
                // Set again
                MovingPeriodBox.Value        = mSettings.MovingTime;
                MovingPixelBox.Value         = mSettings.MovingPixel;
                MinimizeToTrayBarBox.Checked = mSettings.MinimizeToTrayBar;
                ShowTrayBarIconBox.Checked   = mSettings.ShowTrayBarIcon;
            }
        }

        // Save settings
        private void SaveSettings()
        {
            mSettings.MovingTime        = (int) MovingPeriodBox.Value;
            mSettings.MovingPixel       = (int) MovingPixelBox.Value;
            mSettings.MinimizeToTrayBar = MinimizeToTrayBarBox.Checked;
            mSettings.ShowTrayBarIcon   = ShowTrayBarIconBox.Checked;
            mSettings.Save();
        }

        // Disable GUI on start
        private void DisableGuiOnStart()
        {
            MovingPeriodBox.Enabled      = false;
            MovingPixelBox.Enabled       = false;
            MinimizeToTrayBarBox.Enabled = false;
            ShowTrayBarIconBox.Enabled   = false;
            StartButton.Enabled          = false;
            StopButton.Enabled           = true;
        }

        // Enable GUI on stop
        private void EnableGuiOnStop()
        {
            MovingPeriodBox.Enabled      = true;
            MovingPixelBox.Enabled       = true;
            MinimizeToTrayBarBox.Enabled = true;
            ShowTrayBarIconBox.Enabled   = true;
            StartButton.Enabled          = true;
            StopButton.Enabled           = false;
        }

        // Set status
        private void SetStatus(string cText)
        {
            StatusLabel.Text = String.Format(StatusLabel.Text, cText);
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

        #endregion
    }
}
