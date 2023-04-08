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
using TestLibrary.AppConfig;
using TestLibrary.SCPI_VISA_Instruments;
using TestLibrary.Logging;
using TestLibrary.Switching;

// TODO: Refactor TestLibrary to Microsoft's C# Coding Conventions, https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions.
// TODO: Update to .Net 7.0 & C# 11.0 instead of .Net FrameWork 4.8 & C# 7.0 when possible.
// NOTE: Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas instruments TIDP.SAA Fusion Library is compiled to .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & UWP.
//  - https://www.ti.com/tool/FUSION_USB_ADAPTER_API
// TODO: Update to UWP instead of WinForms when possible.
// NOTE: Chose WinForms due to incompatibility of UWP with .Net Framework, and unfamiliarity with WPF.
// NOTE: With deep appreciation for https://learn.microsoft.com/en-us/docs/ & https://stackoverflow.com/!
//
//  References:
//  - https://github.com/Amphenol-Borisch-Technologies/TestLibrary
//  - https://github.com/Amphenol-Borisch-Technologies/TestProgram
//  - https://github.com/Amphenol-Borisch-Technologies/TestLibraryTests
namespace TestLibrary {
    public abstract partial class TestExecutive : Form {
        public readonly AppConfigLogger ConfigLogger = AppConfigLogger.Get();
        public readonly Dictionary<String, SCPI_VISA_Instrument> SVIs = SCPI_VISA_Instrument.Get(); // TODO: May havw to revert to { get; private set; } if Keysight's SCPI classes contain state, thus must be writeable.
        public AppConfigUUT ConfigUUT { get; private set; } = AppConfigUUT.Get();
        public AppConfigTest ConfigTest { get; private set; } // Requires form; instantiated by form button click event method.
        public CancellationTokenSource CancelTokenSource { get; private set; } = new CancellationTokenSource();
        private readonly String _appAssemblyVersion;
        private readonly String _libraryAssemblyVersion;
        private Boolean _cancelled = false;

        protected abstract Task<String> RunTestAsync(String testID);

        protected TestExecutive(Icon icon) {
            this.InitializeComponent();
            this._appAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            this._libraryAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Icon = icon;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio
            USB_ERB24.ResetAll();
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
            this.ButtonSelectGroup.Enabled = true;
        }

        private void ButtonSelectGroup_Click(Object sender, EventArgs e) {
            this.ConfigTest = AppConfigTest.Get();
            this.Text = $"{this.ConfigUUT.Number}, {this.ConfigUUT.Description}, {this.ConfigTest.Group.ID}";
            this.FormReset();
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStartReset(enabled: true);
        }

        private async void ButtonStart_Clicked(Object sender, EventArgs e) {
            this.ConfigUUT.SerialNumber = Interaction.InputBox(Prompt: "Please enter UUT Serial Number", Title: "Enter Serial Number", DefaultResponse: this.ConfigUUT.SerialNumber);
            if (String.Equals(this.ConfigUUT.SerialNumber, String.Empty)) return;
            await this.RunAsync();
        }

        private void ButtonCancel_Clicked(Object sender, EventArgs e) {
            #region Long Test Cancellation Comment
            // NOTE: Two types of TestLibrary/Operator initiated cancellations possible, proactive & reactive:
            //  1)  Proactive:
            //      - Microsoft's recommended CancellationTokenSource technique, which can proactively
            //        cancel currently executing Test, *if* implemented.
            //      - Implementation is the Test Developer's responsibility.
            //      - Implementation necessary if the *currently* executing Test must be cancellable during
            //        execution.
            //  2)  Reactive:
            //      - TestLibrary's already implemented, always available & default reactive "Cancel before next Test" technique,
            //        which simply sets this._cancelled Boolean to true, checked at the end of RunTest()'s foreach loop.
            //      - If this._cancelled is true, RunTest()'s foreach loop is broken, causing reactive cancellation
            //        prior to the next Test's execution.
            //      - Note: This doesn't proactively cancel the *currently* executing Test, which runs to completion.
            //  Summary:
            //      - If it's necessary to deterministically cancel a specific Test's execution, Microsoft's
            //        CancellationTokenSource technique *must* be implemented by the Test Developer.
            //      - If it's only necessary to deterministically cancel overall Test Program execution,
            //        TestLibrary's basic "Cancel before next Test" technique is already available without
            //        any Test Developer implementation needed.
            //      - Note: Some Tests may not be safely cancellable mid-execution.
            //          - For such, simply don't implement Microsoft's CancellationTokenSource technique.
            //  https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
            //  https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation
            //  https://learn.microsoft.com/en-us/dotnet/standard/threading/canceling-threads-cooperatively
            //
            //  NOTE: TestProgram/Test Developer initiated cancellations also possible:
            //      - Any TestProgram's Test can initiate a cancellation programmatically by simply
            //        throwing a TestCancellationException:
            //        - Let's say we want to abort if specific conditions occur in a Test, for example if
            //          power application fails.
            //        - We don't want to continue testing if the UUT's applied power busses fail,
            //          because any downstream failures are likely due to the UUT not being powered
            //          correctly.
            //        - So, simply throw a TestCancellationException if an applied power bus fails.
            //        - This is simulated in T01 in https://github.com/Amphenol-Borisch-Technologies/TestProgram/blob/master/TestProgram.T-Shared.cs
            //        - Test Developer must set TestCancellationException's message to the Measured
            //          value for it to be Logged, else default String.Empty or Double.NaN values are Logged.
        #endregion Long Test Cancellation Comment
            this.CancelTokenSource.Cancel();
            this._cancelled = true;
            this.ButtonCancel.Text = "Cancelling..."; // Here's to British English spelling!
            this.ButtonCancel.Enabled = false;  this.ButtonCancel.UseVisualStyleBackColor = false; this.ButtonCancel.BackColor = Color.Red;
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
            this.ButtonSelectGroup.Enabled = false;
            this.ButtonStartReset(enabled: false);
            this.ButtonCancelReset(enabled: false);
            this.TextUUTResult.Text = String.Empty;
            this.TextUUTResult.BackColor = Color.White;
            if (this.ConfigTest != null) {
                this.ButtonSaveOutput.Enabled = !this.ConfigTest.Group.Required;
                this.ButtonOpenTestDataFolder.Enabled = (this.ConfigTest.Group.Required && this.ConfigLogger.FileEnabled);
            } else {
                this.ButtonSaveOutput.Enabled = false;
                this.ButtonOpenTestDataFolder.Enabled = false;
            }
            this.ButtonEmergencyStop.Enabled = true;
            this.rtfResults.Text = String.Empty;
        }

        private void ButtonEmergencyStop_Clicked(Object sender, EventArgs e) {
            SCPI99.ResetAll(this.SVIs);
            USB_ERB24.ResetAll();
            if (this.ButtonCancel.Enabled) ButtonCancel_Clicked(this, null);
       }

        private void ButtonSaveOutput_Click(Object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Title = "Save Test Results",
                Filter = "Rich Text Format|*.rtf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"{this.ConfigUUT.Number}_{this.ConfigTest.Group.ID}_{this.ConfigUUT.SerialNumber}",
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
            }
            this.ConfigUUT.EventCode = EventCodes.UNSET;
            USB_ERB24.ResetAll();
            SCPI99.ResetAll(this.SVIs);
            Logger.Start(this.ConfigUUT, this.ConfigLogger, this.ConfigTest, this._appAssemblyVersion, this._libraryAssemblyVersion, ref this.rtfResults);
            this.ButtonCancelReset(enabled: true);
        }

        private async Task RunAsync() {
            PreRun();
            foreach (KeyValuePair<String, Test> kvp in this.ConfigTest.Tests) {
                try {
                    kvp.Value.Measurement = await Task.Run(() => RunTestAsync(kvp.Value.ID));
                    kvp.Value.Result = EvaluateTestResult(kvp.Value);
                } catch (Exception e) {
                    if (e.ToString().Contains(TestCancellationException.ClassName)) {
                        kvp.Value.Result = EventCodes.CANCEL;
                        while (!(e is TestCancellationException) && (e.InnerException != null)) e = e.InnerException;
                        if ((e is TestCancellationException) && !String.IsNullOrEmpty(e.Message)) kvp.Value.Measurement = e.Message;
                    } else {
                        StopRun(kvp, e.ToString());
                    }
                    break;
                } finally {
                    Logger.LogTest(kvp.Value);
                }
                if (this._cancelled) {
                    kvp.Value.Result = EventCodes.CANCEL;
                    break;
                }
            }
            PostRun();
        }

        private void StopRun(KeyValuePair<String, Test> kvp, String exceptionString) {
            SCPI99.ResetAll(this.SVIs);
            USB_ERB24.ResetAll();
            kvp.Value.Result = EventCodes.ERROR;
            Logger.UnexpectedErrorHandler(exceptionString.ToString());
        }

        private void PostRun() {
            SCPI99.ResetAll(this.SVIs);
            USB_ERB24.ResetAll();
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStartReset(enabled: true);
            this.ButtonCancelReset(enabled: false);
            this.ConfigUUT.EventCode = EvaluateUUTResult(this.ConfigTest);
            this.TextUUTResult.Text = this.ConfigUUT.EventCode;
            this.TextUUTResult.BackColor = EventCodes.GetColor(this.ConfigUUT.EventCode);
            Logger.Stop(this.ConfigUUT, this.ConfigLogger, this.ConfigTest.Group, ref this.rtfResults);
        }

        private String EvaluateTestResult(Test test) {
            switch (test.ClassName) {
                case TestCustomizable.ClassName:
                    return test.Result;
                case TestISP.ClassName:
                    TestISP testISP = (TestISP)test.ClassObject;
                    if (String.Equals(testISP.ISPExpected, test.Measurement, StringComparison.Ordinal)) return EventCodes.PASS;
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

        private String EvaluateUUTResult(AppConfigTest configTest) {
            if (!configTest.Group.Required) return EventCodes.UNSET;
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

        private Int32 GetResultCount(Dictionary<String, Test> tests, String eventCode) {
            return (from test in tests where String.Equals(test.Value.Result, eventCode) select test).Count();
        }
    }
    
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