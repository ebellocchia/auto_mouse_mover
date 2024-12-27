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

namespace AutoMouseMover.Utils
{
    //
    // Settings helper
    //
    class SettingsHelper
    {
        //
        // Constants
        //
        #region Constants

        // Defautl moving period
        private const int DEF_MOVING_PERIOD       = 5;
        // Default moving pixel
        private const int DEF_MOVING_PIXEL        = 5;
        // Default minimize to tray bar
        private const bool DEF_MINIMIZE_TRAY_BAR  = true;
        // Default show tray bar icon
        private const bool DEF_SHOW_TRAY_BAR_ICON = true;

        #endregion

        //
        // Members
        //
        #region Members

        // Moving period
        private int  mMovingPeriod;
        // Moving pixel
        private int  mMovingPixel;
        // Minimize to tray flag
        private bool mMinimizeToTrayBar;
        // Show tray icon flag
        private bool mShowTrayBarIcon;

        #endregion

        //
        // Public methods
        //
        #region Public methods

        // Constructor
        public SettingsHelper()
        {
            Load();
        }

        // Load settings
        public void Load()
        {
            mMovingPeriod      = Properties.Settings.Default.MovingPeriod;
            mMovingPixel       = Properties.Settings.Default.MovingPixel;
            mMinimizeToTrayBar = Properties.Settings.Default.MinimizeToTrayBar;
            mShowTrayBarIcon   = Properties.Settings.Default.ShowTrayBarIcon;
        }

        // Load default settings
        public void LoadDefault()
        {
            mMovingPeriod      = DEF_MOVING_PERIOD;
            mMovingPixel       = DEF_MOVING_PIXEL;
            mMinimizeToTrayBar = DEF_MINIMIZE_TRAY_BAR;
            mShowTrayBarIcon   = DEF_SHOW_TRAY_BAR_ICON;
        }

        // Save settings
        public void Save()
        {
            Properties.Settings.Default.MovingPeriod      = mMovingPeriod;
            Properties.Settings.Default.MovingPixel       = mMovingPixel;
            Properties.Settings.Default.MinimizeToTrayBar = mMinimizeToTrayBar;
            Properties.Settings.Default.ShowTrayBarIcon   = mShowTrayBarIcon;
            Properties.Settings.Default.Save();
        }

        #endregion

        //
        // Properties
        //
        #region Properties
        
        // Moving time property
        public int MovingTime
        {
            get { return mMovingPeriod; }
            set { mMovingPeriod = value; }
        }

        // Moving pixel property
        public int MovingPixel
        {
            get { return mMovingPixel; }
            set { mMovingPixel = value; }
        }

        // Minimize to tray bar property
        public bool MinimizeToTrayBar
        {
            get { return mMinimizeToTrayBar; }
            set { mMinimizeToTrayBar = value; }
        }

        // Show tray bar icon property
        public bool ShowTrayBarIcon
        {
            get { return mShowTrayBarIcon; }
            set { mShowTrayBarIcon = value; }
        }

        #endregion
    }
}
