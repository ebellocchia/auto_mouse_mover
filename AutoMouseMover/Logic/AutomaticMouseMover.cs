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

using AutoMouseMover.WinHelper;
using System.Windows.Forms;

namespace AutoMouseMover.Logic
{
    //
    // Automatic mouse mover class
    //
    class AutomaticMouseMover
    {
        //
        // Constants
        //
        #region Constants
        
        // Default moving pixel
        private const int DEFAULT_MOVING_PIXEL = 5;

        #endregion

        //
        // Enumeratives
        //
        #region Enumeratives

        // Moving directions
        private enum eMovingDirections
        {
            BACKWARD,
            FORWARD,
        }

        #endregion

        //
        // Members
        //
        #region Members

        // Last cursor position
        private CursorPosition mLastCursorPos;
        // Moving pixels amount
        private int mMovingPixel;
        // Moving direction
        private eMovingDirections mMovingDir;
        // Left click flag
        private bool mLeftClick;

        #endregion

        //
        // Public methods
        //
        #region Public methods

        // Constructor
        public AutomaticMouseMover()
        {
            Initialize(DEFAULT_MOVING_PIXEL, false);
        }

        // Initialize
        public void Initialize(int cMovingPixel, bool cLeftClick)
        {
            mMovingDir = eMovingDirections.FORWARD;
            mMovingPixel = cMovingPixel;
            mLastCursorPos = CursorHelper.GetCurrentPosition();
            mLeftClick = cLeftClick;
        }

        // Move mouse
        public void MoveMouse()
        {
            // Get current position
            var curr_pos = CursorHelper.GetCurrentPosition();

            // If the cursor position could not be read, skip this tick entirely
            // rather than acting on an invalid (-1,-1) position. This happens
            // briefly when the active input desktop switches (e.g. a session lock
            // / secure desktop), where GetCursorPos returns ERROR_ACCESS_DENIED
            // before our lock guard stops the timer.
            if (curr_pos.IsInvalidated())
            {
                return;
            }

            // Move cursor only if position is not changed, in order to not disturb if the user is working
            if (curr_pos == mLastCursorPos)
            {
                // Get pixel movement depending on the direction
                int mov_pixel_rel = (mMovingDir == eMovingDirections.BACKWARD) ? (-1 * mMovingPixel) : mMovingPixel;
                // Move cursor
                MoveCursor(mov_pixel_rel, mLeftClick);
                // Update moving direction
                UpdateMovingDir();
            }

            // Reset last cursor position
            mLastCursorPos = CursorHelper.GetCurrentPosition();
        }

        #endregion

        //
        // Private methods
        //
        #region Private methods

        // Move cursor
        private void MoveCursor(int cDeltaPixel,
                                bool cClick)
        {
            var position = CursorHelper.GetCurrentPosition();
            var x_delta = cDeltaPixel;
            var y_delta = cDeltaPixel;
            var new_x = position.X + x_delta;
            var new_y = position.Y + y_delta;

            // Scan through the available screens
            bool screen_found = false;
            foreach (var screen in Screen.AllScreens)
            {
                // Check if this is this the screen the cursor is on
                var bounds = screen.Bounds;
                if ((position.X >= bounds.X) && (position.X <= (bounds.X + bounds.Width)) &&
                    (position.Y >= bounds.Y) && (position.Y <= (bounds.Y + bounds.Height)))
                {
                    screen_found = true;

                    // Invert delta if it will put the cursor out of screen
                    if (new_x < bounds.X || new_x > bounds.Right)
                    {
                        x_delta = -x_delta;
                    }
                    if (new_y < bounds.Y || new_y > bounds.Bottom)
                    {
                        y_delta = -y_delta;
                    }

                    // We don't need to scan through the rest
                    break;
                }
            }

            // Reset position if the cursor screen was not found (very unlikely)
            if (!screen_found)
            {
                CursorHelper.ResetPosition();
            }
            else
            {
                CursorHelper.SetPositionRelative(x_delta, y_delta);
                if (cClick)
                {
                    CursorHelper.LeftClick();
                }
            }
        }

        // Update moving direction
        private void UpdateMovingDir()
        {
            if (mMovingDir == eMovingDirections.BACKWARD)
            {
                mMovingDir = eMovingDirections.FORWARD;
            }
            else
            {
                mMovingDir = eMovingDirections.BACKWARD;
            }
        }

        #endregion
    }
}
