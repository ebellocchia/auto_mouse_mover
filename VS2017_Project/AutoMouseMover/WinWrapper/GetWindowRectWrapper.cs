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
using System.Runtime.InteropServices;

namespace AutoMouseMover.WinWrapper
{
    //
    // Wrapper for GetWindowRect function
    //
    static class GetWindowRectWrapper
    {
        //
        // DLL imports
        //
        #region DLL imports

        // GetCursorPos API import 
        [DllImport("User32.Dll")]
        public static extern IntPtr GetDesktopWindow();

        // GetCursorPos API import
        [DllImport("User32.Dll")]
        private static extern int GetWindowRect(IntPtr cHWnd, ref WinRect rRect);

        #endregion

        //
        // Static methods
        //
        #region Static methods

        // Get window rectangle
        public static bool GetWindowRectangle(IntPtr      cHWnd, 
                                              ref WinRect rRect)
        {
            return (GetWindowRect(cHWnd, ref rRect) != WinBool.FALSE);
        }

        #endregion
    }
}
