using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using Serilog;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TestLibrary.Config;
using TestLibrary.Instruments;

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

        public static Color GetColor(String EventCode) {
            Dictionary<String, Color> CodesToColors = new Dictionary<String, Color>() {
                {EventCodes.CANCEL, Color.Yellow },
                {EventCodes.ERROR, Color.Aqua },
                {EventCodes.FAIL, Color.Red },
                {EventCodes.PASS, Color.Green },
                {EventCodes.UNSET, Color.Gray }
            };
            return CodesToColors[EventCode];
        }
    }

    public static class TestTasks {
        public static String EvaluateTestResult(Test test) {
            switch (test.ClassName) {
                case TestCustomizable.ClassName:
                    return test.Measurement;
                case TestISP.ClassName:
                    TestISP tisp = (TestISP)test.ClassObject;
                    if (String.Equals(tisp.ISPResult, test.Measurement, StringComparison.Ordinal)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                case TestNumerical.ClassName:
                    if (!Double.TryParse(test.Measurement, NumberStyles.Float, CultureInfo.CurrentCulture, out Double dMeasurement)) throw new InvalidOperationException($"TestElement ID '{test.ID}' Measurement '{test.Measurement}' ≠ System.Double.");
                    TestNumerical tn = (TestNumerical)test.ClassObject;
                    if ((tn.Low <= dMeasurement) && (dMeasurement <= tn.High)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                case TestTextual.ClassName:
                    TestTextual tt = (TestTextual)test.ClassObject;
                    if (String.Equals(tt.Text, test.Measurement, StringComparison.Ordinal)) return EventCodes.PASS;
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
                foreach (KeyValuePair<String, Test> t in configTest.Tests) s += $"ID: '{t.Key}' Result: '{t.Value.Result}'.{Environment.NewLine}";
                UnexpectedErrorHandler($"Encountered Test(s) with EventCodes.UNSET:{Environment.NewLine}{Environment.NewLine}{s}");
                return EventCodes.ERROR;
            }
            if (GetResultCount(configTest.Tests, EventCodes.FAIL) > 0) return EventCodes.FAIL;
            // 5th priority evaluation:
            // - If there are no ERROR, CANCEL or UNSET results, but there is a FAIL result, UUT result is FAIL.

            // Else, we're really in the Twilight Zone...
            String validEvents = String.Empty, invalidTests = String.Empty;
            foreach (FieldInfo fi in typeof(EventCodes).GetFields()) validEvents += ((String)fi.GetValue(null), String.Empty);
            foreach (KeyValuePair<String, Test> t in configTest.Tests) if (!validEvents.Contains(t.Value.Result)) invalidTests += $"ID: '{t.Key}' Result: '{t.Value.Result}'.{Environment.NewLine}";
            UnexpectedErrorHandler($"Invalid Test ID(s) to Result(s):{Environment.NewLine}{invalidTests}");
            return EventCodes.ERROR;
        }

        private static Int32 GetResultCount(Dictionary<String, Test> tests, String eventCode) {
            return (from t in tests where String.Equals(t.Value.Result, eventCode) select t).Count();
        }

        public static void UnexpectedErrorHandler(String logMessage) {
            Log.Error(logMessage);
            MessageBox.Show($"Unexpected error.  Details logged for analysis & resolution.{Environment.NewLine}{Environment.NewLine}" +
                            $"If reoccurs, please contact Test Engineering.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
