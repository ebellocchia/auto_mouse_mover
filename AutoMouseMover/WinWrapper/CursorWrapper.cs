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
 
using System.Runtime.InteropServices;

namespace AutoMouseMover.WinWrapper
{
    //
    // Window cursor APIs wrapper
    //
    static class CursorWrapper
    {
        //
        // DLL imports
        //
        #region DLL imports

        // GetCursorPos API import
        [DllImport("User32.Dll")]
        private static extern int GetCursorPos(ref WinPoint pPoint);

        // SetCursorPos API import
        [DllImport("User32.Dll")]
        private static extern int SetCursorPos(int pX, int pY);

        #endregion

        //
        // Static methods
        //
        #region Static methods

        // Get cursor position 
        public static bool GetCursorPosition(ref WinPoint rPoint)
        {
            return (GetCursorPos(ref rPoint) == WinBool.TRUE);
        }

        // Set cursor position
        public static bool SetCursorPosition(WinPoint cPoint)
        {
            return (SetCursorPos(cPoint.mX, cPoint.mY) == WinBool.TRUE);
        }

        #endregion
    }
}
