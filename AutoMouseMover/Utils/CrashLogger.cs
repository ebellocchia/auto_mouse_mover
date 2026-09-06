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
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AutoMouseMover.Utils
{
    //
    // Crash logger.
    // Diagnostic logging is disabled by default: nothing is written to disk unless the
    // application is started with the "--debug-log" command line switch.
    // See docs/Diagnosing-Crashes.md.
    //
    static class CrashLogger
    {
        //
        // Constants
        //
        #region Constants

        // Application folder inside the local application data folder
        private const string APP_FOLDER_NAME = "AutoMouseMover";
        // Log file name
        private const string LOG_FILE_NAME = "crash.log";
        // Command line switch enabling the diagnostic log
        private const string ENABLE_SWITCH = "--debug-log";

        #endregion

        //
        // Members
        //
        #region Members

        // Logging enabled flag, evaluated once at startup
        private static readonly bool mEnabled = IsEnabledOnCommandLine();

        #endregion

        //
        // Properties
        //
        #region Properties

        // Log file path property
        public static string LogFilePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                    APP_FOLDER_NAME,
                                    LOG_FILE_NAME);
            }
        }

        #endregion

        //
        // Public methods
        //
        #region Public methods

        // Log an exception
        public static void Log(string cSource,
                               Exception cException)
        {
            Write($"{cSource}: {cException}");
        }

        // Log a message
        public static void Log(string cMessage)
        {
            Write(cMessage);
        }

        // Write the session banner (application, OS and display information)
        public static void WriteSessionHeader()
        {
            // Nothing to do if disabled, avoid collecting information for no reason
            if (!mEnabled)
            {
                return;
            }

            var str_builder = new StringBuilder();
            str_builder.AppendLine("==================== SESSION START ====================");
            str_builder.AppendLine($"Log file      : {LogFilePath}");
            str_builder.AppendLine($"App version   : {Assembly.GetExecutingAssembly().GetName().Version}");
            str_builder.AppendLine($"OS            : {RuntimeInformation.OSDescription}");
            str_builder.AppendLine($".NET runtime  : {RuntimeInformation.FrameworkDescription}");
            str_builder.AppendLine($"Process       : {(Environment.Is64BitProcess ? "64-bit" : "32-bit")}");
            str_builder.AppendLine($"DPI awareness : {Application.HighDpiMode}");
            str_builder.AppendLine($"Displays      : {DescribeDisplays()}");
            Write(str_builder.ToString().TrimEnd());
        }

        // Describe the current display layout
        public static string DescribeDisplays()
        {
            try
            {
                var str_builder = new StringBuilder();
                var virtual_screen = SystemInformation.VirtualScreen;
                str_builder.Append($"count={Screen.AllScreens.Length}, virtual={virtual_screen.Width}x{virtual_screen.Height}@({virtual_screen.X},{virtual_screen.Y})");

                foreach (Screen screen in Screen.AllScreens)
                {
                    var primary_marker = screen.Primary ? "*" : string.Empty;
                    str_builder.Append($" | {primary_marker}{screen.DeviceName}: {screen.Bounds.Width}x{screen.Bounds.Height}@({screen.Bounds.X},{screen.Bounds.Y}) {screen.BitsPerPixel}bpp");
                }

                return str_builder.ToString();
            }
            catch (Exception ex)
            {
                return $"<unavailable: {ex.Message}>";
            }
        }

        #endregion

        //
        // Private methods
        //
        #region Private methods

        // Get if logging was enabled on the command line
        private static bool IsEnabledOnCommandLine()
        {
            try
            {
                foreach (var arg in Environment.GetCommandLineArgs())
                {
                    if (string.Equals(arg, ENABLE_SWITCH, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        // Append a line to the log file, if enabled
        private static void Write(string cText)
        {
            // Disabled by default: never touch the disk unless explicitly requested
            if (!mEnabled)
            {
                return;
            }

            try
            {
                var path = LogFilePath;
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.AppendAllText(path, $"[{DateTime.Now:o}] {cText}{Environment.NewLine}{Environment.NewLine}");
            }
            catch
            {
            }
        }

        #endregion
    }
}
