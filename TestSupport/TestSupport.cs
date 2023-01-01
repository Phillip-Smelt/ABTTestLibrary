using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ABTTestLibrary.Config;

namespace ABTTestLibrary.TestSupport {
    public class ABTAbortException : Exception {
        public ABTAbortException() { }
        public ABTAbortException(String message) : base(message) { }
        public ABTAbortException(String message, Exception inner) : base(message, inner) { }
    }

    public static class EventCodes {
        public const String ABORT = "ABORT";
        public const String ERROR = "ERROR";
        public const String FAIL = "FAIL";
        public const String PASS = "PASS";
        public const String UNSET = "UNSET";

        public static Color GetColor(String EventCode) {
            Dictionary<String, Color> CodesToColors = new Dictionary<String, Color>() {
                {EventCodes.ABORT, Color.Yellow },
                {EventCodes.ERROR, Color.Aqua },
                {EventCodes.FAIL, Color.Red},
                {EventCodes.PASS, Color.Green },
                {EventCodes.UNSET, Color.Gray }
            };
            return CodesToColors[EventCode];
        }
    }

    public static class TestTasks {
        public static String EvaluateTestResult(Test test) {
            // NOTE: Sequence of below if conditions blocks interdependent.  That is, if reordered, they may fail.
            (String sLow, String sHigh) = (test.LimitLow, test.LimitHigh);
            Double dLow, dHigh, dMeasurement;

            if (String.Equals(sLow, String.Empty) || String.Equals(sHigh, String.Empty)) {
                // If either Limit is String.Empty, non-empty Limit & Measurement must parse to numeric.
                if (String.Equals(sLow, String.Empty) && String.Equals(sHigh, String.Empty)) {
                    //   - LimitLow = LimitHigh = String.Empty.
                    // One or both Limits must be numeric, or TestElement is erroneous.
                    throw new InvalidOperationException($"Invalid limits; App.config TestElement ID '{test.ID}' has LimitLow = LimitHigh = String.Empty.");
                }

                if (!Double_TryParse(sLow, out _) && !Double_TryParse(sHigh, out _)) {
                    // One or both Limits must be numeric, or TestElement is erroneous.
                    if (String.Equals(sLow, String.Empty)) {
                        throw new InvalidOperationException($"Invalid Limit; App.config TestElement ID '{test.ID}' LimitHigh '{sHigh}' ≠ System.Double.");
                    } else throw new InvalidOperationException($"Invalid Limit; App.config TestElement ID '{test.ID}' LimitLow '{sLow}' ≠ System.Double.");
                }

                if (!Double_TryParse(test.Measurement, out dMeasurement)) {
                    // Measurement must be numeric, or Test is erroneous.
                    throw new InvalidOperationException($"Invalid measurement; App.config TestElement ID " +
                        $"'{test.ID}' Measurement '{test.Measurement}' ≠ System.Double.");
                }

                if (Double_TryParse(sLow, out dLow) && String.Equals(sHigh, String.Empty)) {
                    //   - LimitLow parses to Double, LimitHigh = String.Empty; only low limit, no high.
                    if (dLow <= dMeasurement) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                }

                if (String.Equals(sLow, String.Empty) && Double_TryParse(sHigh, out dHigh)) {
                    //   - LimitLow = String.Empty, LimitHigh parses to Double; no low limit, only high.
                    if (dMeasurement <= dHigh) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                }
            }

            if (Double_TryParse(sLow, out dLow) && Double_TryParse(sHigh, out dHigh)) {
                //   - LimitLow & LimitHigh both parse to Doubles; both low & high limits.
                if (!Double_TryParse(test.Measurement, out dMeasurement)) {
                    // Measurement must be numeric, or Test is erroneous.
                    throw new InvalidOperationException($"Invalid measurement; App.config TestElement ID " +
                        $"'{test.ID}' Measurement '{test.Measurement}' ≠ System.Double.");
                }
                if (dLow <= dHigh) {
                    if ((dLow <= dMeasurement) && (dMeasurement <= dHigh)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                }
                if (dMeasurement >= dLow || dMeasurement <= dHigh) return EventCodes.PASS;
                //   - LimitLow is allowed to be > LimitHigh if both parse to Double.
                //     This simply excludes a range of measurements from passing, rather than including a range from passing.
                else return EventCodes.FAIL;
            }

            if (String.Equals(sLow, sHigh)) {
                // Either CUSTOM test or CRC/String comparison test.
                if (String.Equals(sLow, "CUSTOM")) {
                    //   - LimitLow = LimitHigh = CUSTOM, for custom Tests.
                    switch (test.Measurement) {
                        case EventCodes.ABORT:
                        case EventCodes.ERROR:
                        case EventCodes.FAIL:
                        case EventCodes.PASS:
                        case EventCodes.UNSET:
                            return test.Measurement;
                        default:
                            throw new InvalidOperationException($"Invalid CUSTOM measurement; App.config TestElement ID " +
                                $"'{test.ID}' Measurement '{test.Measurement}' didn't return valid EventCode.");
                    }
                }
                if (String.Equals(sLow, test.Measurement)) return EventCodes.PASS;
                // CRC or String comparison Test.
                else return EventCodes.FAIL;
            } else throw new InvalidOperationException($"Invalid Limits; App.config TestElement ID '{test.ID}' LimitLow '{sLow}' ≠ LimitHigh '{sHigh}'.");
                // Limits are non-numeric & non-empty Strings, but LimitLow ≠ LimitHigh, therefore invalid.

            // If none of the above conditions occur, there's a logic bug buried in them; report it.
            throw new InvalidOperationException($"I'm so embarrassed!{Environment.NewLine}" +
                $"App.config TestElement ID '{test.ID}', Measurement '{test.Measurement}', LimitLow '{test.LimitLow}', LimitHigh '{test.LimitHigh}{Environment.NewLine}" +
                $"inexplicably escaped my impeccable logic & didn't evaluate correctly.");
        }

        private static Boolean Double_TryParse(String s, out Double d) {
            return Double.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out d);
            // Convenience wrapper method to add NumberStyles.Float & CultureInfo.CurrentCulture to Double.TryParse().
            // NumberStyles.Float best for parsing floating point decimal values, including scientific/exponential notation.
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
            if (GetResultCount(configTest.Tests, EventCodes.ABORT) > 0) return EventCodes.ABORT;
            // 3rd priority evaluation:
            // - If any test result is ABORT, and none were ERROR, overall UUT result is ABORT.
            if (GetResultCount(configTest.Tests, EventCodes.UNSET) > 0) throw new InvalidOperationException("One or more Tests didn't execute!");
            // 4th priority evaluation:
            // - If any test result is UNSET, and there are no explicit ERROR or ABORT results, it implies the test didn't complete
            //   without erroring or aborting, which shouldn't occur, but...
            if (GetResultCount(configTest.Tests, EventCodes.FAIL) > 0) return EventCodes.FAIL;
            // 5th priority evaluation:
            // - If there are no ERROR, ABORT or UNSET results, but there is a FAIL result, UUT result is FAIL.

            // Else, handle oopsies!
            String validEvents = String.Empty, invalidTests = String.Empty;
            foreach (FieldInfo fi in typeof(EventCodes).GetFields()) validEvents += ((String)fi.GetValue(null), String.Empty);
            foreach (KeyValuePair<String, Test> t in configTest.Tests) {
                if (!validEvents.Contains(t.Value.Result)) invalidTests += $"ID: '{t.Key}' Result: '{t.Value.Result}'.{Environment.NewLine}";
            }
            if (!String.Equals(invalidTests, String.Empty)) throw new NotImplementedException($"Invalid Test ID(s) to Result(s):{Environment.NewLine}{invalidTests}");
            else throw new NotImplementedException("Sigh...unexpected Error.  Am clueless why.");
            // EventCodes was modified without updating EvaluateUUTResult; take Exception to that.
        }

        private static Int32 GetResultCount(Dictionary<String, Test> tests, String eventCode) {
            return (from t in tests where String.Equals(t.Value.Result, eventCode) select t).Count();
        }
    }
}
