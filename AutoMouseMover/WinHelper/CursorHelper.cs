﻿/*
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
    // Cursor helper
    //
    static class CursorHelper
    {
        //
        // Static methods
        //
        #region Static methods

        // Get current cursor position
        public static CursorPosition GetCurrentPosition()
        {
            var cursor_point = new WinPoint();
            if (!CursorWrapper.GetCursorPosition(ref cursor_point))
            {
                return new CursorPosition();
            }
            else
            {
                return new CursorPosition(cursor_point);
            }
        }

        // Set cursor position absolute
        public static bool SetPositionAbsolute(CursorPosition cPos)
        {
            // Get resolution
            var res_info = DesktopHelper.GetResolution();

            // Absolute coordinates are from 0 to 65536
            int real_x = (cPos.X * 65536) / res_info.Width;
            int real_y = (cPos.Y * 65536) / res_info.Height;

            // Create INPUT structure
            var input = new SendInputWrapper.Input();
            // Fill it
            input.mType                = SendInputWrapper.eInputTypes.INPUT_MOUSE;
            input.mData.mMi.mMouseData = 0;
            input.mData.mMi.mTime      = 0;
            input.mData.mMi.mX         = real_x;
            input.mData.mMi.mY         = real_y;
            input.mData.mMi.mFlags     = SendInputWrapper.eMouseEventFlags.MOUSEEVENTF_ABSOLUTE | SendInputWrapper.eMouseEventFlags.MOUSEEVENTF_MOVE;
            // Send input
            return SendInputWrapper.SendInput(input);
        }

        // Set cursor position relative 
        public static bool SetPositionRelative(int cDeltaX,
                                               int cDeltaY)
        {
            // Create INPUT structure
            var input = new SendInputWrapper.Input();
            // Fill it
            input.mType                = SendInputWrapper.eInputTypes.INPUT_MOUSE;
            input.mData.mMi.mMouseData = 0;
            input.mData.mMi.mTime      = 0;
            input.mData.mMi.mX         = cDeltaX;
            input.mData.mMi.mY         = cDeltaY;
            input.mData.mMi.mFlags     = SendInputWrapper.eMouseEventFlags.MOUSEEVENTF_MOVE;
            // Send input
            return SendInputWrapper.SendInput(input);
        }

        // Check absolute position
        public static bool CheckAbsolutePosition(CursorPosition cPos)
        {
            ResolutionInfo res_info = DesktopHelper.GetResolution();
            return (((cPos.X >= 0) && (cPos.X <= res_info.Width )) && 
                    ((cPos.Y >= 0) && (cPos.Y <= res_info.Height)));
        }

        // Check relative position
        public static bool CheckRelativePosition(int cDeltaX,
                                                 int cDeltaY)
        {
            // Compute the new position
            CursorPosition new_pos = GetCurrentPosition();
            new_pos.MoveToRelative(cDeltaX, cDeltaY);

            return CheckAbsolutePosition(new_pos);
        }

        #endregion
    }
}
