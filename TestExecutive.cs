using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ABT.TestSpace.AppConfig;
using ABT.TestSpace.SCPI_VISA_Instruments;
using ABT.TestSpace.Logging;
using ABT.TestSpace.Switching;

// TODO: Refactor TestExecutive to Microsoft's C# Coding Conventions, https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions.
// NOTE: For public methods, will deviate by using PascalCasing for parameters.  Will use recommended camelCasing for internal & private method parameters.
//  - Prefer named arguments for public methods be Capitalized/PascalCased, not uncapitalized/camelCased.
//  - Invoking public methods with named arguments is a superb, self-documenting coding technique, improved by PascalCasing.
// TODO: Update to .Net 7.0 & C# 11.0 instead of .Net FrameWork 4.8 & C# 7.3 when possible.
// NOTE: Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas instruments TIDP.SAA Fusion Library supposedly compiled to .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & WinUI 3.
//       TIDP.SAA actually appears to be compiled to .Net FrameWork 4.5, but that's still not necessarily compatible with .Net 7.0.
//  - https://www.ti.com/tool/FUSION_USB_ADAPTER_API
// TODO: Update to WinUI 3 or WPF instead of WinForms when possible.
// NOTE: Chose WinForms due to incompatibility of WinUI 3 with .Net Framework, and unfamiliarity with WPF.
// NOTE: With deep appreciation for https://learn.microsoft.com/en-us/docs/ & https://stackoverflow.com/!
//
//  References:
//  - https://github.com/Amphenol-Borisch-Technologies/TestExecutive
//  - https://github.com/Amphenol-Borisch-Technologies/TestExecutor
//  - https://github.com/Amphenol-Borisch-Technologies/TestExecutiveTests
namespace ABT.TestSpace {
    public abstract partial class TestExecutive : Form {
        public readonly AppConfigLogger ConfigLogger = AppConfigLogger.Get();
        public readonly Dictionary<String, SCPI_VISA_Instrument> SVIs = SCPI_VISA_Instrument.Get();
        public AppConfigUUT ConfigUUT { get; private set; } = AppConfigUUT.Get();
        public AppConfigTest ConfigTest { get; private set; } // Requires form; instantiated by button_click event method.
        public CancellationTokenSource CancelTokenSource { get; private set; } = new CancellationTokenSource();
        internal readonly String _appAssemblyVersion;
        internal readonly String _libraryAssemblyVersion;
        private Boolean _cancelled = false;

        protected abstract Task<String> RunTestAsync(String TestID);

        protected TestExecutive(Icon icon) {
            this.InitializeComponent();
            this._appAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            this._libraryAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Icon = icon;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio
            USB_ERB24.Set(RelayForms.C.NC);
        }

        private void Form_Shown(Object sender, EventArgs e) {
            this.FormReset();
            this.Text = $"{this.ConfigUUT.Number}, {this.ConfigUUT.Description}";
            if (!String.Equals(String.Empty, this.ConfigUUT.DocumentationFolder)) {
                if (Directory.Exists(this.ConfigUUT.DocumentationFolder)) {
                    ProcessStartInfo psi = new ProcessStartInfo {
                        FileName = "explorer.exe",
                        WindowStyle = ProcessWindowStyle.Minimized,
                        Arguments = $"\"{this.ConfigUUT.DocumentationFolder}\""
                    };
                    Process.Start(psi);
                    // Paths with embedded spaces require enclosing double-quotes (").
                    // Even then, simpler 'System.Diagnostics.Process.Start("explorer.exe", path);' invocation fails - must use ProcessStartInfo class.
                    // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
                } else MessageBox.Show(Form.ActiveForm, $"Path {this.ConfigUUT.DocumentationFolder} invalid.", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.ButtonSelectTests.Enabled = true;
        }

        private void ButtonSelectTests_Click(Object sender, EventArgs e) {
            this.ConfigTest = AppConfigTest.Get();
            this.Text = $"{this.ConfigUUT.Number}, {this.ConfigUUT.Description}, {this.ConfigTest.TestElementID}";
            this.FormReset();
            this.ButtonSelectTests.Enabled = true;
            this.ButtonStartReset(enabled: true);
        }

        private async void ButtonStart_Clicked(Object sender, EventArgs e) {
            String serialNumber = Interaction.InputBox(Prompt: "Please enter UUT Serial Number", Title: "Enter Serial Number", DefaultResponse: this.ConfigUUT.SerialNumber);
            if (String.Equals(serialNumber, String.Empty)) return;
            this.ConfigUUT.SerialNumber = serialNumber;
            await this.RunAsync();
        }

        private void ButtonCancel_Clicked(Object sender, EventArgs e) {
            #region Long Test Cancellation Comment
            // NOTE: Two types of TestExecutive/Operator initiated cancellations possible, proactive & reactive:
            //  1)  Proactive:
            //      - Microsoft's recommended CancellationTokenSource technique, which can proactively
            //        cancel currently executing Test, *if* implemented.
            //      - Implementation is the Test Developer's responsibility.
            //      - Implementation necessary if the *currently* executing Test must be cancellable during
            //        execution.
            //  2)  Reactive:
            //      - TestExecutive's already implemented, always available & default reactive "Cancel before next Test" technique,
            //        which simply sets this._cancelled Boolean to true, checked at the end of RunTest()'s foreach loop.
            //      - If this._cancelled is true, RunTest()'s foreach loop is broken, causing reactive cancellation
            //        prior to the next Test's execution.
            //      - Note: This doesn't proactively cancel the *currently* executing Test, which runs to completion.
            //  Summary:
            //      - If necessary to deterministically cancel a specific Test's execution, Microsoft's
            //        CancellationTokenSource technique *must* be implemented by the Test Developer.
            //      - If only necessary to deterministically cancel overall Test Program execution,
            //        TestExecutive's basic "Cancel before next Test" technique is already available without
            //        any Test Developer implementation needed.
            //      - Note that some Tests may not be safely cancellable mid-execution.
            //          - For such, simply don't implement Microsoft's CancellationTokenSource technique.
            //  https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
            //  https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation
            //  https://learn.microsoft.com/en-us/dotnet/standard/threading/canceling-threads-cooperatively
            //
            //  NOTE: TestExecutor/Test Developer initiated cancellations also possible:
            //      - Any TestExecutor's Test can initiate a cancellation programmatically by simply
            //        throwing a TestCancellationException:
            //        - Let's say we want to abort if specific conditions occur in a Test, for example if
            //          power application fails.
            //        - We don't want to continue testing if the UUT's applied power busses fail,
            //          because any subsequent failures are likely due to the UUT not being powered
            //          correctly.
            //        - So, simply throw a TestCancellationException if an applied power bus fails.
            //        - This is simulated in T01 in https://github.com/Amphenol-Borisch-Technologies/TestExecutor/blob/master/TestProgram/T-Common.cs
            //        - Test Developer must set TestCancellationException's message to the Measured
            //          value for it to be Logged, else default String.Empty or Double.NaN values are Logged.
            #endregion Long Test Cancellation Comment
            this.CancelTokenSource.Cancel();
            this._cancelled = true;
            this.ButtonCancel.Text = "Cancelling..."; // Here's to British English spelling!
            this.ButtonCancel.Enabled = false;
            this.ButtonCancel.UseVisualStyleBackColor = false;
            this.ButtonCancel.BackColor = Color.Red;
        }

        private void ButtonStartReset(Boolean enabled) {
            if (enabled) {
                this.ButtonStart.UseVisualStyleBackColor = false;
                this.ButtonStart.BackColor = Color.Green;
            } else {
                this.ButtonStart.BackColor = SystemColors.Control;
                this.ButtonStart.UseVisualStyleBackColor = true;
            }
            this.ButtonStart.Enabled = enabled;
        }

        private void ButtonCancelReset(Boolean enabled) {
            if (enabled) {
                this.ButtonCancel.UseVisualStyleBackColor = false;
                this.ButtonCancel.BackColor = Color.Yellow;
            } else {
                this.ButtonCancel.BackColor = SystemColors.Control;
                this.ButtonCancel.UseVisualStyleBackColor = true;
            }
            this.ButtonCancel.Text = "Cancel";
            if (this.CancelTokenSource.IsCancellationRequested) {
                this.CancelTokenSource.Dispose();
                this.CancelTokenSource = new CancellationTokenSource();
            }
            this._cancelled = false;
            this.ButtonCancel.Enabled = enabled;
        }

        private void FormReset() {
            this.ButtonSelectTests.Enabled = false;
            this.ButtonStartReset(enabled: false);
            this.ButtonCancelReset(enabled: false);
            this.TextUUTResult.Text = String.Empty;
            this.TextUUTResult.BackColor = Color.White;
            if (this.ConfigTest != null) {
                this.ButtonSaveOutput.Enabled = !this.ConfigTest.IsOperation;
                this.ButtonOpenTestDataFolder.Enabled = (this.ConfigTest.IsOperation && this.ConfigLogger.FileEnabled);
            } else {
                this.ButtonSaveOutput.Enabled = false;
                this.ButtonOpenTestDataFolder.Enabled = false;
            }
            this.ButtonEmergencyStop.Enabled = true;
            this.rtfResults.Text = String.Empty;
        }

        private void ButtonEmergencyStop_Clicked(Object sender, EventArgs e) {
            SCPI99.ResetAll(this.SVIs);
            USB_ERB24.Set(RelayForms.C.NC);
            if (this.ButtonCancel.Enabled) this.ButtonCancel_Clicked(this, null);
       }

        private void ButtonSaveOutput_Click(Object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Title = "Save Test Results",
                Filter = "Rich Text Format|*.rtf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"{this.ConfigUUT.Number}_{this.ConfigTest.TestElementID}_{this.ConfigUUT.SerialNumber}",
                DefaultExt = "rtf",
                CreatePrompt = false,
                OverwritePrompt = true
            };
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if ((dialogResult == DialogResult.OK) && !String.Equals(saveFileDialog.FileName, String.Empty)) this.rtfResults.SaveFile(saveFileDialog.FileName);
        }

        private void ButtonOpenTestDataFolder_Click(Object sender, EventArgs e) {
            ProcessStartInfo psi = new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"\"{this.ConfigLogger.FilePath}\"" };
            Process.Start(psi);
            // Will fail if this.ConfigLogger.FilePath is invalid.  Don't catch resulting Exception though; this has to be fixed in App.config.
        }

        private void PreRun() {
            this.FormReset();
            foreach (KeyValuePair<String, Test> kvp in this.ConfigTest.Tests) {
                if (String.Equals(kvp.Value.ClassName, TestNumerical.ClassName)) kvp.Value.Measurement = Double.NaN.ToString();
                else kvp.Value.Measurement = String.Empty;
                kvp.Value.Result = EventCodes.UNSET;
#if DEBUG
                kvp.Value.DebugMessage = String.Empty;
#endif
            }
            this.ConfigUUT.EventCode = EventCodes.UNSET;
            USB_ERB24.Set(RelayForms.C.NC);
            SCPI99.ResetAll(this.SVIs);
            Logger.Start(this, ref this.rtfResults);
            this.ButtonCancelReset(enabled: true);
        }

        private async Task RunAsync() {
            this.PreRun();
            foreach (String testMeasurementID in this.ConfigTest.TestMeasurementIDsSequence) {
                try {
                    this.ConfigTest.Tests[testMeasurementID].Measurement = await Task.Run(() => this.RunTestAsync(testMeasurementID));
                    this.ConfigTest.Tests[testMeasurementID].Result = EvaluateTestResult(this.ConfigTest.Tests[testMeasurementID]);
                    if (String.Equals(this.ConfigTest.Tests[testMeasurementID].Result, EventCodes.FAIL) && this.ConfigTest.Tests[testMeasurementID].CancelOnFailure) this._cancelled = true;
                } catch (Exception e) {
                    if (e.ToString().Contains(TestCancellationException.ClassName)) {
                        this.ConfigTest.Tests[testMeasurementID].Result = EventCodes.CANCEL;
                        while (!(e is TestCancellationException) && (e.InnerException != null)) e = e.InnerException;
                        if ((e is TestCancellationException) && !String.IsNullOrEmpty(e.Message)) this.ConfigTest.Tests[testMeasurementID].Measurement = e.Message;
                    } else {
                        this.StopRun(testMeasurementID, e.ToString());
                    }
                    break;
                } finally {
                    Logger.LogTest(this.ConfigTest.IsOperation, this.ConfigTest.Tests[testMeasurementID], ref this.rtfResults);
                }
                if (this._cancelled) {
                    this.ConfigTest.Tests[testMeasurementID].Result = EventCodes.CANCEL;
                    break;
                }
            }
            this.PostRun();
        }

        private void StopRun(String testMeasurementID, String exceptionString) {
            SCPI99.ResetAll(this.SVIs);
            USB_ERB24.Set(RelayForms.C.NC);
            this.ConfigTest.Tests[testMeasurementID].Result = EventCodes.ERROR;
            Logger.UnexpectedErrorHandler(exceptionString);
        }

        private void PostRun() {
            SCPI99.ResetAll(this.SVIs);
            USB_ERB24.Set(RelayForms.C.NC);
            this.ButtonSelectTests.Enabled = true;
            this.ButtonStartReset(enabled: true);
            this.ButtonCancelReset(enabled: false);
            this.ConfigUUT.EventCode = EvaluateUUTResult(this.ConfigTest);
            this.TextUUTResult.Text = this.ConfigUUT.EventCode;
            this.TextUUTResult.BackColor = EventCodes.GetColor(this.ConfigUUT.EventCode);
            Logger.Stop(this, ref this.rtfResults);
        }

        internal static String EvaluateTestResult(Test test) {
            switch (test.ClassName) {
                case TestCustomizable.ClassName:
                    return test.Result;
                case TestISP.ClassName:
                    TestISP testISP = new TestISP(test.ID, test.Arguments);
                    if (String.Equals(testISP.ISPExpected, test.Measurement, StringComparison.Ordinal)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                case TestNumerical.ClassName:
                    if (!Double.TryParse(test.Measurement, NumberStyles.Float, CultureInfo.CurrentCulture, out Double dMeasurement)) throw new InvalidOperationException($"TestMeasurement ID '{test.ID}' Measurement '{test.Measurement}' ≠ System.Double.");
                    TestNumerical testNumerical = new TestNumerical(test.ID, test.Arguments);
                    if ((testNumerical.Low <= dMeasurement) && (dMeasurement <= testNumerical.High)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                case TestTextual.ClassName:
                    TestTextual testTextual = new TestTextual(test.ID, test.Arguments);
                    if (String.Equals(testTextual.Text, test.Measurement, StringComparison.Ordinal)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                default:
                    throw new NotImplementedException($"TestMeasurement ID '{test.ID}' with ClassName '{test.ClassName}' not implemented.");
            }
        }

        internal static String EvaluateUUTResult(AppConfigTest configTest) {
            if (!configTest.IsOperation) return EventCodes.UNSET;
            // 0th priority evaluation that precedes all others.
            if (GetResultCount(configTest.Tests, EventCodes.PASS) == configTest.Tests.Count) return EventCodes.PASS;
            // 1st priority evaluation (or could also be last, but we're irrationally optimistic.)
            // All test results are PASS, so overall UUT result is PASS.
            if (GetResultCount(configTest.Tests, EventCodes.ERROR) != 0) return EventCodes.ERROR;
            // 2nd priority evaluation:
            // - If any test result is ERROR, overall UUT result is ERROR.
            if (GetResultCount(configTest.Tests, EventCodes.CANCEL) != 0) return EventCodes.CANCEL;
            // 3rd priority evaluation:
            // - If any test result is CANCEL, and none were ERROR, overall UUT result is CANCEL.
            if (GetResultCount(configTest.Tests, EventCodes.UNSET) != 0) {
                // 4th priority evaluation:
                // - If any test result is UNSET, and there are no explicit ERROR or CANCEL results, it implies Test(s) didn't complete
                //   without erroring or cancelling, which shouldn't occur, but...
                String s = String.Empty;
                foreach (KeyValuePair<String, Test> kvp in configTest.Tests) s += $"ID: '{kvp.Key}' Result: '{kvp.Value.Result}'.{Environment.NewLine}";
                Logger.UnexpectedErrorHandler($"Encountered Test(s) with EventCodes.UNSET:{Environment.NewLine}{Environment.NewLine}{s}");
                return EventCodes.ERROR;
            }
            if (GetResultCount(configTest.Tests, EventCodes.FAIL) != 0) return EventCodes.FAIL;
            // 5th priority evaluation:
            // - If there are no ERROR, CANCEL or UNSET results, but there are FAIL result(s), UUT result is FAIL.
            // Else, we're really in the Twilight Zone...
            String validEvents = String.Empty, invalidTests = String.Empty;
            foreach (FieldInfo fi in typeof(EventCodes).GetFields()) validEvents += ((String)fi.GetValue(null), String.Empty);
            foreach (KeyValuePair<String, Test> kvp in configTest.Tests) if (!validEvents.Contains(kvp.Value.Result)) invalidTests += $"ID: '{kvp.Key}' Result: '{kvp.Value.Result}'.{Environment.NewLine}";
            Logger.UnexpectedErrorHandler($"Invalid Test ID(s) to Result(s):{Environment.NewLine}{invalidTests}");
            return EventCodes.ERROR;
        }

        private static Int32 GetResultCount(Dictionary<String, Test> tests, String eventCode) { return (from test in tests where String.Equals(test.Value.Result, eventCode) select test).Count(); }
    }

    public class TestCancellationException : Exception {
        // NOTE: Only ever throw TestCancellationException from TestExecutor, never from TestExecutive.
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
                { EventCodes.CANCEL, Color.Yellow },
                { EventCodes.ERROR, Color.Aqua },
                { EventCodes.FAIL, Color.Red },
                { EventCodes.PASS, Color.Green },
                { EventCodes.UNSET, Color.Gray }
            };
            return codesToColors[eventCode];
        }
    }

    public abstract class SCPI_VISA_InstrumentElement_IDs { }
}