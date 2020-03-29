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
using AutoMouseMover.WinWrapper;

namespace AutoMouseMover.WinHelper
{
    //
    // Position structure
    //
    class CursorPosition
    {
        //
        // Constants
        //
        #region Constants

        // Invalid value
        private const int INVALID_VALUE = -1;

        #endregion

        //
        // Members
        //
        #region Members
        
        // X
        private int mX;
        // Y
        private int mY;

        #endregion

        //
        // Public methods
        //
        #region Public methods

        // Constructor - Empty
        public CursorPosition()
        {
            Invalidate();
        }

        // Constructor - From position
        public CursorPosition(int cX, 
                              int cY)
        {
            MoveToAbsolute(cX, cY);
        }

        // Constructor - From Windows POINT structure
        public CursorPosition(WinPoint cPoint)
        {
            MoveToAbsolute(cPoint.mX, cPoint.mY);
        }

        // Invalidate
        public void Invalidate()
        {
            mX = INVALID_VALUE;
            mY = INVALID_VALUE;
        }

        // Get if invalidated
        public bool IsInvalidated()
        {
            return ((mX == INVALID_VALUE) && (mY == INVALID_VALUE));
        }

        // Move to absolute position
        public void MoveToAbsolute(int cX,
                                   int cY)
        {
            mX = cX;
            mY = cY;
        }

        // Move to relative position
        public void MoveToRelative(int cDeltaX,
                                   int cDeltaY)
        {
            mX += cDeltaX;
            mY += cDeltaY;
        }

        // Equal operator
        public static bool operator ==(CursorPosition cPos1,
                                       CursorPosition cPos2)
        {
            return ((cPos1.mX == cPos2.mX) && (cPos1.mY == cPos2.mY));
        }

        // Not equal operator
        public static bool operator !=(CursorPosition cPos1,
                                       CursorPosition cPos2)
        {
            return !(cPos1 == cPos2);
        }

        #endregion

        //
        // Properties
        //
        #region Properties

        // X property
        public int X
        {
            get { return mX; }
        }

        // Y property
        public int Y
        {
            get { return mY; }
        }

        #endregion
    }
}
