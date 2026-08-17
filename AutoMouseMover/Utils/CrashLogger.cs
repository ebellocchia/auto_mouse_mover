// AutoMouseMover, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// AutoMouseMover.Utils.CrashLogger
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

internal static class CrashLogger
{
    private const string APP_FOLDER_NAME = "AutoMouseMover";

    private const string LOG_FILE_NAME = "crash.log";

    public static string LogFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AutoMouseMover", "crash.log");

    public static void Log(string source, Exception exception)
    {
        Write($"{source}: {exception}");
    }

    public static void Log(string message)
    {
        Write(message);
    }

    public static void WriteSessionHeader()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("==================== SESSION START ====================");
        sb.AppendLine($"Log file      : {LogFilePath}");
        sb.AppendLine($"App version   : {Assembly.GetExecutingAssembly().GetName().Version}");
        sb.AppendLine($"OS            : {RuntimeInformation.OSDescription}");
        sb.AppendLine($".NET runtime  : {RuntimeInformation.FrameworkDescription}");
        sb.AppendLine($"Process       : {(Environment.Is64BitProcess ? "64-bit" : "32-bit")}");
        sb.AppendLine($"DPI awareness : {Application.HighDpiMode}");
        sb.AppendLine($"Displays      : {DescribeDisplays()}");
        Write(sb.ToString().TrimEnd());
    }

    public static string DescribeDisplays()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Rectangle virtualScreen = SystemInformation.VirtualScreen;
            sb.Append($"count={Screen.AllScreens.Length}, virtual={virtualScreen.Width}x{virtualScreen.Height}@({virtualScreen.X},{virtualScreen.Y})");

            foreach (Screen screen in Screen.AllScreens)
            {
                string primaryMarker = screen.Primary ? "*" : string.Empty;
                sb.Append($" | {primaryMarker}{screen.DeviceName}: {screen.Bounds.Width}x{screen.Bounds.Height}@({screen.Bounds.X},{screen.Bounds.Y}) {screen.BitsPerPixel}bpp");
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"<unavailable: {ex.Message}>";
        }
    }

    private static void Write(string text)
    {
        try
        {
            string path = LogFilePath;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.AppendAllText(path, $"[{DateTime.Now:o}] {text}{Environment.NewLine}{Environment.NewLine}");
        }
        catch
        {
        }
    }
}
