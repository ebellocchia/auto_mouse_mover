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

using AutoMouseMover.WinWrapper;

namespace AutoMouseMover.WinHelper
{
    // 
    // Resolution info structure
    //
    class ResolutionInfo
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
        
        // Height
        private int mHeight;
        // Width
        private int mWidth;

        #endregion

        //
        // Public methods
        //
        #region Public methods

        // Constructor - Empty
        public ResolutionInfo()
        {
            Invalidate();
        }

        // Constructor - From dimensions
        public ResolutionInfo(int cHeight,
                              int cWidth)
        {
            mHeight = cHeight;
            mWidth = cWidth;
        }

        // Constructor - From Windows RECT structure
        public ResolutionInfo(WinRect pRect)
        {
            mHeight = pRect.mBottom;
            mWidth = pRect.mRight;
        }

        // Invalidate 
        public void Invalidate()
        {
            mHeight = INVALID_VALUE;
            mWidth = INVALID_VALUE;
        }

        // Get if invalidated
        public bool IsInvalidated()
        {
            return ((mHeight == INVALID_VALUE) && (mWidth == INVALID_VALUE));
        }

        #endregion

        //
        // Properties
        //
        #region Properties

        // Height property
        public int Height
        {
            get { return mHeight; }
        }

        // Width property
        public int Width
        {
            get { return mWidth; }
        }

        #endregion
    }
}
