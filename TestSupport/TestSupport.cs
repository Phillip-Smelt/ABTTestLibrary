using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ABTTestLibrary.AppConfig;

namespace ABTTestLibrary.TestSupport {
    public class ABTTestLibraryException : Exception {
        // NOTE: ABTTestLibrary - Currently don't throw any ABTTestLibraryExceptions, but option exists if needed.
        public ABTTestLibraryException() { }
        public ABTTestLibraryException(string message) : base(message) { }
        public ABTTestLibraryException(string message, Exception inner) : base(message, inner) { }
    }

    public static class EventCodes {
        public const String ABORT = "ABORT";
        public const String ERROR = "ERROR";
        public const String FAIL = "FAIL";
        public const String PASS = "PASS";
        public const String UNSET = "UNSET";

        public static Color GetColor(String EventCode) {
            Dictionary<String, Color> CodesToColors = new Dictionary<string, Color>() {
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
        public static void EvaluateTestResult(Test test, out String eventCode) {
            eventCode = EventCodes.UNSET;
            (String sLow, String sHigh) = (test.LimitLow, test.LimitHigh);
            //   - LimitLow = LimitHigh = String.Empty.
            if (String.Equals(sLow, String.Empty) && String.Equals(sHigh, String.Empty)) {
                eventCode = EventCodes.ERROR;
                throw new Exception($"Invalid limits; App.config TestElement ID '{test.ID}' has LimitLow = String.Empty && LimitHigh = String.Empty");
            }

            //   - LimitLow = String.Empty,	LimitHigh ≠ String.Empty, but won't parse to Double.
            if (String.Equals(sLow, String.Empty) && !String.Equals(sHigh, String.Empty) && !Double.TryParse(sHigh, out _)) {
                eventCode = EventCodes.ERROR;
                throw new Exception($"Invalid limits; App.config TestElement ID '{test.ID}' has LimitLow = String.Empty && LimitHigh ≠ String.Empty && LimitHigh ≠ System.Double");
            }

            //   - LimitHigh = String.Empty, LimitLow  ≠ String.Empty, but won't parse to Double.
            if (String.Equals(sHigh, String.Empty) && !String.Equals(sLow, String.Empty) && !Double.TryParse(sLow, out _)) {
                eventCode = EventCodes.ERROR;
                throw new Exception($"Invalid limits; App.config TestElement ID '{test.ID}' has LimitHigh = String.Empty && LimitLow ≠ String.Empty && LimitLow ≠ System.Double");
            }

            //   - LimitLow ≠ String.Empty,	LimitHigh ≠ String.Empty, neither parse to Double, & LimitLow ≠ LimitHigh.
            if (sLow != sHigh)
                if (!String.Equals(sLow, String.Empty) && sHigh != String.Empty)
                    if (!Double.TryParse(sLow, out _) && !Double.TryParse(sHigh, out _)) {
                        eventCode = EventCodes.ERROR;
                        throw new Exception($"Invalid limits; App.config TestElement ID '{test.ID}' has LimitLow ≠ LimitHigh && LimitLow ≠ String.Empty && LimitHigh ≠ String.Empty && LimitLow ≠ System.Double && LimitHigh ≠ System.Double");
            }

            Double dLow, dHigh, dMeasurement;
            //   - LimitLow & LimitHigh both parse to Doubles; both low & high limits.
            //   - LimitLow is allowed to be > LimitHigh if both parse to Double.
            //     This simply excludes a range of measurements from passing, rather than including a range from passing.
            //   - LimitLow is allowed to be = LimitHigh if both parse to Double.
            //     This simply means only one measurement passes.
            if (Double.TryParse(sLow, out dLow) && Double.TryParse(sHigh, out dHigh)) {
                if (!Double.TryParse(test.Measurement, out dMeasurement)) {
                    eventCode = EventCodes.ERROR;
                    throw new Exception($"Invalid measurement; App.config TestElement ID '{test.ID}' Measurement '{test.Measurement}' ≠ System.Double");
                }
                eventCode = EventCodes.FAIL;
                if (dLow <= dHigh) if (dLow <= dMeasurement && dMeasurement <= dHigh) eventCode = EventCodes.PASS;
                if (dLow > dHigh)  if (dMeasurement >= dLow || dMeasurement <= dHigh) eventCode = EventCodes.PASS;
            }

            //   - LimitLow parses to Double, LimitHigh = String.Empty; only low limit, no high.
            if (Double.TryParse(sLow, out dLow) && String.Equals(sHigh, String.Empty)) {
                if (!Double.TryParse(test.Measurement, out dMeasurement)) {
                    eventCode = EventCodes.ERROR;
                    throw new Exception($"Invalid measurement; App.config TestElement ID '{test.ID}' Measurement '{test.Measurement}' ≠ System.Double");
                }

                if (dLow <= dMeasurement) eventCode = EventCodes.PASS;
                else eventCode = EventCodes.FAIL;
            }

            //   - LimitLow = String.Empty, LimitHigh parses to Double; no low limit, only high.
            if (String.Equals(sLow, String.Empty) && Double.TryParse(sHigh, out dHigh)) {
                if (!Double.TryParse(test.Measurement, out dMeasurement)) {
                    eventCode = EventCodes.ERROR;
                    throw new Exception($"Invalid measurement; App.config TestElement ID '{test.ID}' Measurement '{test.Measurement}' ≠ System.Double");
                }
                if (dMeasurement <= dHigh) eventCode = EventCodes.PASS;
                else eventCode = EventCodes.FAIL;
            }
            //   - LimitLow = LimitHigh, both ≠ String.Empty, and Units are "N/A".
            //     This is to verify checksums or CRCs, or to read String contents from memory, or from a file, etc.
            if (sLow == sHigh && sLow != String.Empty && test.Units == "N/A") {
                    if (test.Measurement == sLow) eventCode = EventCodes.PASS;
                    else eventCode = EventCodes.FAIL;
            }
        }

        public static String EvaluateUUTResult(Config Config) {
            if (!Config.Group.Required) return EventCodes.UNSET;
            // 0th priority evaluation that trumps all others.
            if (GetResultCount(Config.Tests, EventCodes.PASS) == Config.Tests.Count) return EventCodes.PASS;
            // 1st priority evaluation (or could also be last, but we're irrationally optimistic.)
            // All test results are PASS, so overall UUT result is PASS.
            if (GetResultCount(Config.Tests, EventCodes.ERROR) > 0) return EventCodes.ERROR;
            // 2nd priority evaluation:
            // - If any test result is ERROR, overall UUT result is ERROR.
            if (GetResultCount(Config.Tests, EventCodes.ABORT) > 0) return EventCodes.ABORT;
            // 3rd priority evaluation:
            // - If any test result is ABORT, but none were ERROR, overall UUT result is ABORT.
            if (GetResultCount(Config.Tests, EventCodes.UNSET) > 0) return EventCodes.ERROR;
            // 4th priority evaluation:
            // - If any test result is UNSET, and there are no explicit ERROR or ABORT results, it implies the test didn't complete
            //   without erroring or aborting, which shouldn't occur.
            if (GetResultCount(Config.Tests, EventCodes.FAIL) > 0) return EventCodes.FAIL;
            // 5th priority evaluation:
            // - If there are no ERROR, ABORT or UNSET results, but there is a FAIL result, UUT result is FAIL.
            throw new Exception($"Invalid test result in Test List '{Config.Tests}'");
            // EventCodes was modified without updating EvaluateUUTResult; take exception to that.
        }

        private static Int32 GetResultCount(Dictionary<String, Test> Tests, String EventCode) {
            return (from t in Tests where t.Value.Result == EventCode select t).Count();
        }
    }
}
