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
        private CursorPosition    mLastCursorPos;
        // Moving pixels amount
        private int               mMovingPixel;
        // Moving direction
        private eMovingDirections mMovingDir;

        #endregion

        //
        // Public methods
        //
        #region Public methods

        // Constructor
        public AutomaticMouseMover()
        {
            Initialize(DEFAULT_MOVING_PIXEL);
        }

        // Initialize
        public void Initialize(int cMovingPixel)
        {
            mMovingDir     = eMovingDirections.FORWARD;
            mMovingPixel   = cMovingPixel;
            mLastCursorPos = CursorHelper.GetCurrentPosition();
        }

        // Move mouse
        public void MoveMouse()
        {
            // Get current position
            var curr_pos = CursorHelper.GetCurrentPosition();
            
            // Move cursor only if position is not changed, in order to not disturb if the user is working
            if (curr_pos == mLastCursorPos)
            {
                // Get pixel movement depending on the direction
                int mov_pixel_rel = (mMovingDir == eMovingDirections.BACKWARD) ? (-1 * mMovingPixel) : mMovingPixel;
                // Move cursor
                MoveCursor(mov_pixel_rel);
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
        private void MoveCursor(int cDeltaPixel)
        {
            // adjust the deltas in the opposite direction if they are outside of bounds. This means we won't
            // try to move the cursor outside the available display area.


            // scan through the available screens
            var position = CursorHelper.GetCurrentPosition();
            var xDelta = cDeltaPixel;
            var yDelta = cDeltaPixel;
            var newX = position.X + xDelta;
            var newY = position.Y + yDelta;
            foreach (var screen in Screen.AllScreens)
            {
                // is this the screen the cursor is on
                var bounds = screen.Bounds;
                if (position.X >= bounds.X && position.X <= (bounds.X + bounds.Width))
                {
                    // this is our screen check if the delta will put us out
                    if (newX < bounds.X || newX > bounds.Right)
                    {
                        xDelta = -xDelta;
                    }
                    if (newY < bounds.Y || newY > bounds.Bottom)
                    {
                        yDelta = -yDelta;
                    }

                    // we don't need to scan through the rest
                    break;
                }
            }

            CursorHelper.SetPositionRelative(xDelta, yDelta);
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
