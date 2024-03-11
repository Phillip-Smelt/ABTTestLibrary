﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABT.TestSpace.TestExec.AppConfig;

namespace ABT.TestSpace.TestExec.Processes {
    public enum PROCESS_METHOD { ExitCode, Redirect }

    public static class ProcessExternal {
        [DllImport("kernel32.dll")] static extern Boolean GetConsoleMode(IntPtr hConsoleHandle, out UInt32 lpMode);
        [DllImport("kernel32.dll")] static extern Boolean SetConsoleMode(IntPtr hConsoleHandle, UInt32 dwMode);

        public static void Connect(String Description, String Connector, Action PreConnect, Action PostConnect, Boolean AutoContinue = false) {
            PreConnect?.Invoke();
            String message = $"UUT unpowered.{Environment.NewLine}{Environment.NewLine}" +
                             $"Connect '{Description}' to UUT '{Connector}'.{Environment.NewLine}{Environment.NewLine}" +
                             $"AFTER connecting, click OK to continue.";
            if (AutoContinue) _ = MessageBox.Show(FormInterconnectGet(), message, $"Connect '{Connector}'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else _ = MessageBox.Show(message, $"Connect '{Connector}'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PostConnect?.Invoke();
        }

        public static void DisConnect(String Description, String Connector, Action PreDisconnect, Action PostDisconnect, Boolean AutoContinue = false) {
            PreDisconnect?.Invoke();
            String message = $"UUT unpowered.{Environment.NewLine}{Environment.NewLine}" +
                             $"Disconnect '{Description}' from UUT '{Connector}'.{Environment.NewLine}{Environment.NewLine}" +
                             $"AFTER disconnecting, click OK to continue.";
            if (AutoContinue) _ = MessageBox.Show(FormInterconnectGet(), message, $"Disconnect '{Connector}'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else _ = MessageBox.Show(message, $"Disconnect '{Connector}'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (!AutoContinue) _ = MessageBox.Show(message, $"Disconnect '{Connector}'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PostDisconnect?.Invoke();
        }

        public static String ExitCode(MeasurementProcess MP) { return ProcessExitCode(MP.ProcessArguments, MP.ProcessExecutable, MP.ProcessFolder); }

        private static Form FormInterconnectGet() {
            Form form = new Form() { Size = new Size(0, 0) };
            Task.Delay(TimeSpan.FromSeconds(1.0)).ContinueWith((t) => form.Close(), TaskScheduler.FromCurrentSynchronizationContext());
            return form;
        }

        public static String ProcessExitCode(String arguments, String fileName, String workingDirectory) {
            Int32 exitCode = -1;
            using (Process process = new Process()) {
                ProcessStartInfo psi = new ProcessStartInfo {
                    Arguments = arguments,
                    FileName = workingDirectory + fileName,
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Maximized,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = false
                };
                process.StartInfo = psi;
                process.Start();
                DisableUserInput(process.Handle);
                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            return exitCode.ToString();
        }

        public static (String StandardError, String StandardOutput, Int32 ExitCode) ProcessRedirect(String arguments, String fileName, String workingDirectory, String expectedResult) {
            String standardError, standardOutput;
            Int32 exitCode = -1;
            using (Process process = new Process()) {
                ProcessStartInfo psi = new ProcessStartInfo {
                    Arguments = arguments,
                    FileName = workingDirectory + fileName,
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };
                process.StartInfo = psi;
                process.Start();
                DisableUserInput(process.Handle);
                process.WaitForExit();
                StreamReader se = process.StandardError;
                standardError = se.ReadToEnd();
                StreamReader so = process.StandardOutput;
                standardOutput = so.ReadToEnd();
                exitCode = process.ExitCode;
            }
            if (standardOutput.Contains(expectedResult)) return (standardError, expectedResult, exitCode);
            else return (standardError, standardOutput, exitCode);
        }

        public static (String StandardError, String StandardOutput, Int32 ExitCode) Redirect(MeasurementProcess MP) { return ProcessRedirect(MP.ProcessArguments, MP.ProcessExecutable, MP.ProcessFolder, MP.ProcessExpected); }

        private static void DisableUserInput(IntPtr processHandle) {
            const UInt32 ENABLE_QUICK_EDIT = 0x0040;
            GetConsoleMode(processHandle, out UInt32 consoleMode);
            consoleMode &= ~ENABLE_QUICK_EDIT; // Clear the ENABLE_QUICK_EDIT_MODE flag bit.
            SetConsoleMode(processHandle, consoleMode);
        }
    }
}
