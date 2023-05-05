using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ABT.TestSpace.AppConfig;

namespace ABT.TestSpace.InterfaceAdapters {
    public enum ISP_METHOD { ExitCode, Redirect }

    public static class ISP {

        public static void Connect(String Description, String Connector, Action PreConnect, Action PostConnect) {
            PreConnect?.Invoke();
            _ = MessageBox.Show($"UUT unpowered.{Environment.NewLine}{Environment.NewLine}" +
                $"Connect '{Description}' to UUT '{Connector}'.{Environment.NewLine}{Environment.NewLine}" +
                $"AFTER connecting, click OK to continue.",
                $"Connect '{Connector}'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PostConnect?.Invoke();
        }

        public static void DisConnect(String Description, String Connector, Action PreDisconnect, Action PostDisconnect) {
            PreDisconnect?.Invoke();
            _ = MessageBox.Show($"UUT unpowered.{Environment.NewLine}{Environment.NewLine}" +
                    $"Disconnect '{Description}' from UUT '{Connector}'.{Environment.NewLine}{Environment.NewLine}" +
                    $"AFTER disconnecting, click OK to continue.",
                    $"Disconnect '{Connector}'", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PostDisconnect?.Invoke();
        }

        public static String ProcessExitCode(String arguments, String fileName, String workingDirectory) {
            Int32 exitCode = -1;
            using (Process process = new Process()) {
                ProcessStartInfo psi = new ProcessStartInfo {
                    Arguments = arguments,
                    FileName = fileName,
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = false
                };
                process.StartInfo = psi;
                process.Start();
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
                    FileName = fileName,
                    WorkingDirectory = workingDirectory,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };
                process.StartInfo = psi;
                process.Start();
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

        public static String ExitCode(Test test) {
            TestISP testISP = (TestISP)test.ClassObject;
            String exitCode = ProcessExitCode(testISP.ISPExecutableArguments, testISP.ISPExecutable, testISP.ISPExecutableFolder);
            return exitCode;
        }

        public static (String StandardError, String StandardOutput, Int32 ExitCode) Redirect(Test test) {
            TestISP testISP = (TestISP)test.ClassObject;
            (String StandardError, String StandardOutput, Int32 ExitCode) = ProcessRedirect(testISP.ISPExecutableArguments, testISP.ISPExecutable, testISP.ISPExecutableFolder, testISP.ISPExpected);
            return (StandardError, StandardOutput, ExitCode);
        }
    }
}
