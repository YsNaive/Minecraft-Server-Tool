using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MCServerTool.Utils
{
    public static class DialogUtils
    {
        public static string? OpenFolderDialog()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // A simple PowerShell script to open a folder dialog.
                    // This avoids bringing in WinForms or WPF dependencies just for a dialog.
                    string psScript = @"
                        Add-Type -AssemblyName System.windows.forms;
                        $f = New-Object System.Windows.Forms.FolderBrowserDialog;
                        $f.ShowNewFolderButton = $true;
                        if ($f.ShowDialog() -eq 'OK') { $f.SelectedPath }
                    ";
                    var processInfo = new ProcessStartInfo("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"{psScript}\"")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(processInfo))
                    {
                        if (process != null)
                        {
                            process.WaitForExit();
                            string result = process.StandardOutput.ReadToEnd().Trim();
                            return string.IsNullOrEmpty(result) ? null : result;
                        }
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    var processInfo = new ProcessStartInfo("zenity", "--file-selection --directory")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(processInfo))
                    {
                        if (process != null)
                        {
                            process.WaitForExit();
                            string result = process.StandardOutput.ReadToEnd().Trim();
                            return string.IsNullOrEmpty(result) ? null : result;
                        }
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    var processInfo = new ProcessStartInfo("osascript", "-e 'POSIX path of (choose folder)'")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(processInfo))
                    {
                        if (process != null)
                        {
                            process.WaitForExit();
                            string result = process.StandardOutput.ReadToEnd().Trim();
                            return string.IsNullOrEmpty(result) ? null : result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening folder dialog: {ex.Message}");
            }

            return null;
        }
    }
}
