using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Serilog; // Install Serilog via NuGet Package Manager.  Site is https://serilog.net/.
using TestLibrary.AppConfig;
using TestLibrary.Instruments;
using static System.Net.Mime.MediaTypeNames;

namespace TestLibrary.TestSupport {
    public class TestCancellationException : Exception {
        // NOTE: Only ever throw TestCancellationException from TestPrograms, never from TestLibrary.
        public TestCancellationException(String message = "") : base(message) { }
        public const String ClassName = nameof(TestCancellationException);
    }

    public static class EventCodes {
        public const String CANCEL = "CANCEL";
        public const String ERROR = "ERROR";
        public const String FAIL = "FAIL";
        public const String PASS = "PASS";
        public const String UNSET = "UNSET";

        public static Color GetColor(String eventCode) {
            Dictionary<String, Color> codesToColors = new Dictionary<String, Color>() {
                {EventCodes.CANCEL, Color.Yellow },
                {EventCodes.ERROR, Color.Aqua },
                {EventCodes.FAIL, Color.Red },
                {EventCodes.PASS, Color.Green },
                {EventCodes.UNSET, Color.Gray }
            };
            return codesToColors[eventCode];
        }
    }

    public static class TestTasks {

        public static void ISP_Connect(String isp, String connector, Dictionary<INSTRUMENTS, Instrument> instruments) {
            InstrumentTasks.SCPI99_Reset(instruments);
            _ = MessageBox.Show($"UUT now unpowered.{Environment.NewLine}{Environment.NewLine}" +
                    $"Connect '{isp}' to UUT '{connector}'.{Environment.NewLine}{Environment.NewLine}" +
                    $"AFTER connecting, click OK to continue.",
                    $"Connect '{connector}'", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ISP_DisConnect(String isp, String connector, Dictionary<INSTRUMENTS, Instrument> instruments) {
            InstrumentTasks.SCPI99_Reset(instruments);
            _ = MessageBox.Show($"UUT now unpowered.{Environment.NewLine}{Environment.NewLine}" +
                    $"Disconnect '{isp}' from UUT '{connector}'.{Environment.NewLine}{Environment.NewLine}" +
                    $"AFTER disconnecting, click OK to continue.",
                    $"Disconnect '{connector}'", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public static String ISP_ExitCode(String isp, String connector, Test test,
            Dictionary<INSTRUMENTS, Instrument> instruments, Action powerOnMethod) {
            ISP_Connect(isp, connector, instruments);
            powerOnMethod();
            TestISP testISP = (TestISP)test.ClassObject;
            String exitCode = ProcessExitCode(testISP.ISPExecutableArguments, testISP.ISPExecutable, testISP.ISPExecutableFolder);
            ISP_DisConnect(isp, connector, instruments);
            return exitCode;
        }

        public static (String StandardError, String StandardOutput, Int32 ExitCode) ISP_Redirect(String isp, String connector, Test test,
            Dictionary<INSTRUMENTS, Instrument> instruments, Action powerOnMethod) {
            ISP_Connect(isp, connector, instruments);
            powerOnMethod();
            TestISP testISP = (TestISP)test.ClassObject;
            (String StandardError, String StandardOutput, Int32 ExitCode) = ProcessRedirect(testISP.ISPExecutableArguments, testISP.ISPExecutable, testISP.ISPExecutableFolder, testISP.ISPResult);
            ISP_DisConnect(isp, connector, instruments);
            return (StandardError, StandardOutput, ExitCode);
        }

        public static String EvaluateTestResult(Test test) {
            switch (test.ClassName) {
                case TestCustomizable.ClassName:
                    return test.Result;
                case TestISP.ClassName:
                    TestISP testISP = (TestISP)test.ClassObject;
                    if (String.Equals(testISP.ISPResult, test.Measurement, StringComparison.Ordinal)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                case TestNumerical.ClassName:
                    if (!Double.TryParse(test.Measurement, NumberStyles.Float, CultureInfo.CurrentCulture, out Double dMeasurement)) throw new InvalidOperationException($"TestElement ID '{test.ID}' Measurement '{test.Measurement}' ≠ System.Double.");
                    TestNumerical testNumerical = (TestNumerical)test.ClassObject;
                    if ((testNumerical.Low <= dMeasurement) && (dMeasurement <= testNumerical.High)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                case TestTextual.ClassName:
                    TestTextual testTextual = (TestTextual)test.ClassObject;
                    if (String.Equals(testTextual.Text, test.Measurement, StringComparison.Ordinal)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                default:
                    throw new NotImplementedException($"TestElement ID '{test.ID}' with ClassName '{test.ClassName}' not implemented.");
            }
        }

        public static String EvaluateUUTResult(ConfigTest configTest) {
            if (!configTest.Group.Required) return EventCodes.UNSET;
            // 0th priority evaluation that precedes all others.
            if (GetResultCount(configTest.Tests, EventCodes.PASS) == configTest.Tests.Count) return EventCodes.PASS;
            // 1st priority evaluation (or could also be last, but we're irrationally optimistic.)
            // All test results are PASS, so overall UUT result is PASS.
            if (GetResultCount(configTest.Tests, EventCodes.ERROR) > 0) return EventCodes.ERROR;
            // 2nd priority evaluation:
            // - If any test result is ERROR, overall UUT result is ERROR.
            if (GetResultCount(configTest.Tests, EventCodes.CANCEL) > 0) return EventCodes.CANCEL;
            // 3rd priority evaluation:
            // - If any test result is CANCEL, and none were ERROR, overall UUT result is CANCEL.
            if (GetResultCount(configTest.Tests, EventCodes.UNSET) > 0) {
                // 4th priority evaluation:
                // - If any test result is UNSET, and there are no explicit ERROR or CANCEL results, it implies Test(s) didn't complete
                //   without erroring or cancelling, which shouldn't occur, but...
                String s = String.Empty;
                foreach (KeyValuePair<String, Test> test in configTest.Tests) s += $"ID: '{test.Key}' Result: '{test.Value.Result}'.{Environment.NewLine}";
                UnexpectedErrorHandler($"Encountered Test(s) with EventCodes.UNSET:{Environment.NewLine}{Environment.NewLine}{s}");
                return EventCodes.ERROR;
            }
            if (GetResultCount(configTest.Tests, EventCodes.FAIL) > 0) return EventCodes.FAIL;
            // 5th priority evaluation:
            // - If there are no ERROR, CANCEL or UNSET results, but there is a FAIL result, UUT result is FAIL.

            // Else, we're really in the Twilight Zone...
            String validEvents = String.Empty, invalidTests = String.Empty;
            foreach (FieldInfo fi in typeof(EventCodes).GetFields()) validEvents += ((String)fi.GetValue(null), String.Empty);
            foreach (KeyValuePair<String, Test> test in configTest.Tests) if (!validEvents.Contains(test.Value.Result)) invalidTests += $"ID: '{test.Key}' Result: '{test.Value.Result}'.{Environment.NewLine}";
            UnexpectedErrorHandler($"Invalid Test ID(s) to Result(s):{Environment.NewLine}{invalidTests}");
            return EventCodes.ERROR;
        }

        private static Int32 GetResultCount(Dictionary<String, Test> tests, String eventCode) {
            return (from test in tests where String.Equals(test.Value.Result, eventCode) select test).Count();
        }

        public static void UnexpectedErrorHandler(String logMessage) {
            Log.Error(logMessage);
            MessageBox.Show(Form.ActiveForm, $"Unexpected error.  Details logged for analysis & resolution.{Environment.NewLine}{Environment.NewLine}" +
                            $"If reoccurs, please contact Test Engineering.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
