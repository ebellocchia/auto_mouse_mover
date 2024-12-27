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
    // Window SendInput API wrapper
    //
    static class SendInputWrapper
    {
        //
        // DLL imports
        //
        #region DLL imports

        // SendInput API import
        [DllImport("User32.dll")]
        private static extern uint SendInput(uint cNumInputs, ref Input rInputs, int cSize);

        #endregion

        //
        // Enumeratives
        //
        #region Enumeratives

        // Input types 
        public enum eInputTypes : int
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }

        // Mouse event flags
        [Flags]
        public enum eMouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }

        #endregion

        //
        // Structures
        //
        #region Structures

        // Windows INPUT equivalent structure
        [StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public eInputTypes mType;
            public InputUnion mData;
        }

        // Windows union equivalent structure
        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public HardwareInput mHi;
            [FieldOffset(0)]
            public KeyDbInput    mKi;
            [FieldOffset(0)]
            public MouseInput    mMi;
        }

        // Windows HARDWAREINPUT equivalent structure
        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public uint   mMsg;
            public ushort mParamL;
            public ushort mParamH;
        }

        // Windows KEYBDINPUT equivalent structure
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyDbInput
        {
            public ushort mVk;
            public ushort mScan;
            public uint   mFlags;
            public uint   mTime;
            public IntPtr mExtraInfo;
        }

        // Windows MOUSEINPUT equivalent structure
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int              mX;
            public int              mY;
            public uint             mMouseData;
            public eMouseEventFlags mFlags;
            public uint             mTime;
            public IntPtr           mExtraInfo;
        }

        #endregion

        //
        // Static methods
        //
        #region Static methods

        // Send input
        public static bool SendInput(Input cInput)
        {
            return (SendInput(1, ref cInput, Marshal.SizeOf(new Input())) != WinBool.FALSE);
        }

        #endregion
    }
}
