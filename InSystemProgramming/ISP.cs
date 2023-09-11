using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABT.TestSpace.TestExec.AppConfig;

namespace ABT.TestSpace.TestExec.InSystemProgramming {
    public enum ISP_METHOD { ExitCode, Redirect }
    
    public static class ISP {
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

        public static String ExitCode(Measurement measurement) {
            MeasurementISP measurementISP = (MeasurementISP)measurement.ClassObject;
            String exitCode = ProcessExitCode(measurementISP.ISPExecutableArguments, measurementISP.ISPExecutable, measurementISP.ISPExecutableFolder);
            return exitCode;
        }

        public static (String StandardError, String StandardOutput, Int32 ExitCode) Redirect(Measurement measurement) {
            MeasurementISP measurementISP = (MeasurementISP)measurement.ClassObject;
            (String StandardError, String StandardOutput, Int32 ExitCode) = ProcessRedirect(measurementISP.ISPExecutableArguments, measurementISP.ISPExecutable, measurementISP.ISPExecutableFolder, measurementISP.ISPExpected);
            return (StandardError, StandardOutput, ExitCode);
        }
    }
}
