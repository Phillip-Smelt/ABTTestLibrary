using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ABTTestLibrary.AppConfig;

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
            (String sLow, String sHigh) = (test.LimitLow, test.LimitHigh);
            //   - LimitLow = LimitHigh = String.Empty.
            if (String.Equals(sLow, String.Empty) && String.Equals(sHigh, String.Empty)) {
                throw new InvalidOperationException($"Invalid limits; App.config TestElement ID '{test.ID}' has LimitLow = LimitHigh = String.Empty");
            }

            //   - LimitLow = String.Empty,	LimitHigh ≠ String.Empty, but won't parse to Double.
            if (String.Equals(sLow, String.Empty) && !String.Equals(sHigh, String.Empty) && !Double_TryParse(sHigh, out _)) {
                throw new InvalidOperationException($"Invalid limits; App.config TestElement ID '{test.ID}' has LimitLow = String.Empty && LimitHigh ≠ String.Empty && LimitHigh ≠ System.Double");
            }

            //   - LimitHigh = String.Empty, LimitLow ≠ String.Empty, but won't parse to Double.
            if (String.Equals(sHigh, String.Empty) && !String.Equals(sLow, String.Empty) && !Double_TryParse(sLow, out _)) {
                throw new InvalidOperationException($"Invalid limits; App.config TestElement ID '{test.ID}' has LimitHigh = String.Empty && LimitLow ≠ String.Empty && LimitLow ≠ System.Double");
            }

            //   - LimitLow ≠ LimitHigh, LimitLow ≠ String.Empty, LimitHigh ≠ String.Empty, neither parse to Double.
            if (!String.Equals(sLow, sHigh)) {
                if (!String.Equals(sLow, String.Empty) && !String.Equals(sHigh,String.Empty)) {
                    if (!Double_TryParse(sLow, out _) && !Double_TryParse(sHigh, out _)) {
                        throw new InvalidOperationException($"Invalid limits; App.config TestElement ID '{test.ID}' has LimitLow ≠ LimitHigh && LimitLow ≠ String.Empty && LimitHigh ≠ String.Empty && LimitLow ≠ System.Double && LimitHigh ≠ System.Double");
                    }
                }
            }

            //   - LimitLow = LimitHigh ≠ String.Empty ≠ CUSTOM, neither parse to Double.
            //     This is to verify String checksums or CRCs, or to verify Strings from RAM/ROM or from files, etc.
            const String _CUSTOM = "CUSTOM";
            if (String.Equals(sLow, sHigh) && !String.Equals(sLow, String.Empty) && !String.Equals(sLow, _CUSTOM) && !Double_TryParse(sLow, out _)) {
                if (String.Equals(sLow, test.Measurement)) return EventCodes.PASS;
                else return EventCodes.FAIL;
            }

            //   - LimitLow = LimitHigh = CUSTOM.
            //     For custom Tests.
            if (String.Equals(sLow, sHigh) && String.Equals(sLow, _CUSTOM)) {
                switch (test.Measurement) {
                    case EventCodes.ABORT:
                    case EventCodes.ERROR:
                    case EventCodes.FAIL:
                    case EventCodes.PASS:
                    case EventCodes.UNSET:
                        return test.Measurement;
                    default:
                        throw new InvalidOperationException($"Invalid CUSTOM measurement; App.config TestElement ID '{test.ID}' Measurement '{test.Measurement}' didn't return valid EventCode.");
                }
            }

            // NOTE: ATBTestLibary - Below code depends upon above code preceding it; it assumes none of above conditions occurred.
            // Remaining cases have numerical limits, so test.Measurement must also be numerical.
            if (!Double_TryParse(test.Measurement, out Double dMeasurement)) {
                throw new InvalidOperationException($"Invalid measurement; App.config TestElement ID '{test.ID}' Measurement '{test.Measurement}' ≠ System.Double");
            }

            //   - LimitLow & LimitHigh both parse to Doubles; both low & high limits.
            //   - LimitLow is allowed to be > LimitHigh if both parse to Double.
            //     This simply excludes a range of measurements from passing, rather than including a range from passing.
            if (Double_TryParse(sLow, out Double dLow) & Double_TryParse(sHigh, out Double dHigh)) {
                // Evaluate both above Double_TryParse() functions with & versus && to declare both dLow and dHigh.
                if ((dLow <= dHigh) && (dLow <= dMeasurement) && (dMeasurement <= dHigh)) return EventCodes.PASS;
                else if ((dLow > dHigh) && (dMeasurement >= dLow || dMeasurement <= dHigh)) return EventCodes.PASS;
                else return EventCodes.FAIL;
            }

            //   - LimitLow parses to Double, LimitHigh = String.Empty; only low limit, no high.
            if (Double_TryParse(sLow, out dLow) && String.Equals(sHigh, String.Empty)) {
                if (dLow <= dMeasurement) return EventCodes.PASS;
                else return EventCodes.FAIL;
            }

            //   - LimitLow = String.Empty, LimitHigh parses to Double; no low limit, only high.
            if (String.Equals(sLow, String.Empty) && Double_TryParse(sHigh, out dHigh)) {
                if (dMeasurement <= dHigh) return EventCodes.PASS;
                else return EventCodes.FAIL;
            }

            // If none of the above conditions occur, something went wrong.  Below code shouldn't ever execute, but...
            throw new InvalidOperationException($"App.config TestElement ID '{test.ID}', Measurement '{test.Measurement}', LimitLow '{test.LimitLow}', LimitHigh '{test.LimitHigh}.");
        }

        private static Boolean Double_TryParse(String s, out Double d) {
            return Double.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out d);
            // Convenience wrapper method to add NumberStyles.Float & CultureInfo.CurrentCulture to Double.TryParse().
            // NumberStyles.Float best for parsing floating point decimal values, including scientific/exponential notation.
        }

        public static String EvaluateUUTResult(Config config) {
            if (!config.Group.Required) return EventCodes.UNSET;
            // 0th priority evaluation that precedes all others.
            if (GetResultCount(config.Tests, EventCodes.PASS) == config.Tests.Count) return EventCodes.PASS;
            // 1st priority evaluation (or could also be last, but we're irrationally optimistic.)
            // All test results are PASS, so overall UUT result is PASS.
            if (GetResultCount(config.Tests, EventCodes.ERROR) > 0) return EventCodes.ERROR;
            // 2nd priority evaluation:
            // - If any test result is ERROR, overall UUT result is ERROR.
            if (GetResultCount(config.Tests, EventCodes.ABORT) > 0) return EventCodes.ABORT;
            // 3rd priority evaluation:
            // - If any test result is ABORT, and none were ERROR, overall UUT result is ABORT.
            if (GetResultCount(config.Tests, EventCodes.UNSET) > 0) throw new InvalidOperationException("One or more Tests didn't execute!");
            // 4th priority evaluation:
            // - If any test result is UNSET, and there are no explicit ERROR or ABORT results, it implies the test didn't complete
            //   without erroring or aborting, which shouldn't occur, but...
            if (GetResultCount(config.Tests, EventCodes.FAIL) > 0) return EventCodes.FAIL;
            // 5th priority evaluation:
            // - If there are no ERROR, ABORT or UNSET results, but there is a FAIL result, UUT result is FAIL.

            // Else, handles oopsies!
            String validEvents = String.Empty, invalidTests = String.Empty;
            foreach (FieldInfo fi in typeof(EventCodes).GetFields()) validEvents += ((String)fi.GetValue(null), String.Empty);
            foreach (KeyValuePair<String, Test> t in config.Tests) {
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
