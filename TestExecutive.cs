using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ABT.TestSpace.TestExec.AppConfig;
using ABT.TestSpace.TestExec.SCPI_VISA_Instruments;
using ABT.TestSpace.TestExec.Logging;
using ABT.TestSpace.TestExec.Switching;
using ABT.TestSpace.TestExec.Switching.USB_ERB24;
using System.Runtime.CompilerServices;

// TODO: Refactor TestExecutive to Microsoft's C# Coding Conventions, https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions.
// NOTE: For public methods, will deviate by using PascalCasing for parameters.  Will use recommended camelCasing for internal & private method parameters.
//  - Prefer named arguments for public methods be Capitalized/PascalCased, not uncapitalized/camelCased.
//  - Invoking public methods with named arguments is a superb, self-documenting coding technique, improved by PascalCasing.
// TODO: Add documentation per https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments.
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
namespace ABT.TestSpace.TestExec {
    public abstract partial class TestExecutive : Form {
        public readonly AppConfigLogger ConfigLogger = AppConfigLogger.Get();
        public readonly Dictionary<SCPI_VISA_Instrument.Alias, SCPI_VISA_Instrument> SVIs = SCPI_VISA_Instrument.Get();
        public AppConfigUUT ConfigUUT { get; private set; } = AppConfigUUT.Get();
        public AppConfigTest ConfigTest { get; private set; } // Requires form; instantiated by button_click event method.
        public CancellationTokenSource CancelTokenSource { get; private set; } = new CancellationTokenSource();
        internal readonly String _appAssemblyVersion;
        internal readonly String _libraryAssemblyVersion;
        private Boolean _cancelled = false;

        protected abstract Task<String> MeasurementRun(String MeasurementID);

        protected TestExecutive(Icon icon) {
            this.InitializeComponent();
            this._appAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            this._libraryAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Icon = icon;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio
            UE24.Set(RelayForms.C.S.NO); // Ensure they'll switch.
            UE24.Set(RelayForms.C.S.NC);
        }

        private void Form_Shown(Object sender, EventArgs e) {
            this.FormReset();
            this.Text = $"{this.ConfigUUT.Number}, {this.ConfigUUT.Description}";
#if !DEBUG
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
#endif
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
            this.MeasurementsPreRun();
            await this.MeasurementsRun();
            this.MeasurementsPostRun();
        }

        private void ButtonCancel_Clicked(Object sender, EventArgs e) {
            #region Long Measurement Cancellation Comment
            // NOTE: Two types of TestExecutor Cancellations possible, each having two sub-types resulting in 4 altogether:
            // A) Spontaneous Operator Initiated Cancellations:
            //      1)  Operator Proactive:
            //          - Microsoft's recommended CancellationTokenSource technique, permitting Operator to proactively
            //            cancel currently executing Measurement.
            //          - Requires TestExecutor implementation by the Test Developer, but is initiated by Operator, so is categorized as such.
            //          - Implementation necessary if the *currently* executing Measurement must be cancellable during execution by the Operator.
            //          - https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
            //          - https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation
            //          - https://learn.microsoft.com/en-us/dotnet/standard/threading/canceling-threads-cooperatively
            //      2)  Operator Reactive:
            //          - TestExecutive's already implemented, always available & default reactive "Cancel before next Test" technique,
            //            which simply sets this._cancelled Boolean to true, checked at the end of TestExecutive.MeasurementsRun()'s foreach loop.
            //          - If this._cancelled is true, TestExecutive.MeasurementsRun()'s foreach loop is broken, causing reactive cancellation
            //            prior to the next Measurement's execution.
            //          - Note: This doesn't proactively cancel the *currently* executing Measurement, which runs to completion.
            // B) PrePlanned Developer Programmed Cancellations:
            //      3)  TestExecutor/Test Developer initiated Cancellations:
            //          - Any TestExecutor's Measurement can initiate a Cancellation programmatically by simply throwing a CancellationException:
            //          - Permits immediate Cancellation if specific condition(s) occur in a Measurement; perhaps to prevent UUT or equipment damage,
            //            or simply because futher execution is pointless.
            //          - Simply throw a CancellationException if the specific condition(s) occcur.
            //          - This is simulated in T01 in https://github.com/Amphenol-Borisch-Technologies/TestExecutor/blob/master/TestProgram/T-Common.cs
            //          - Test Developer must set CancellationException's message to the Measured Value for it to be Logged, else default String.Empty or Double.NaN values are Logged.
            //      4)  App.Config's CancelOnFailure:
            //          - App.Config's TestMeasurement element has a Boolean "CancelOnFailure" field:
            //          - If the current TestExecutor.MeasurementRun() has CancelOnFailure=true and it's resulting EvaluateResultMeasurement() returns EventCodes.FAIL,
            //            TestExecutive.MeasurementsRun() will break/exit, stopping further testing.
            //		    - Do not pass Go, do not collect $200, go directly to TestExecutive.MeasurementsPostRun().
            //
            // NOTE: The Operator Proactive & TestExecutor/Test Developer initiated Cancellations both occur while the currently executing TestExecutor.MeasurementRun() conpletes, via 
            //       thrown CancellationExceptions.
            // NOTE: The Operator Reactive & App.Config's CancelOnFailure Cancellations both occur after the currently executing TestExecutor.MeasurementRun() completes, via checks
            //       inside the TestExecutive.MeasurementsRun() loop.
            #endregion Long Measurement Cancellation Comment
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
            UE24.Set(RelayForms.C.S.NC);
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
            ProcessStartInfo psi = new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"\"{Logger.GetFilePath(this)}\"" };
            Process.Start(psi);
            // Will fail if this.ConfigLogger.FilePath is invalid.  Don't catch resulting Exception though; this has to be fixed in App.config.
        }

        private void MeasurementsPreRun() {
            this.FormReset();
            foreach (KeyValuePair<String, Measurement> kvp in this.ConfigTest.Measurements) {
                if (String.Equals(kvp.Value.ClassName, MeasurementNumerical.ClassName)) kvp.Value.Value = Double.NaN.ToString();
                else kvp.Value.Value = String.Empty;
                kvp.Value.Result = EventCodes.UNSET;
#if DEBUG
                kvp.Value.DebugMessage = String.Empty;
#endif
            }
            this.ConfigUUT.EventCode = EventCodes.UNSET;
            UE24.Set(RelayForms.C.S.NC);
            SCPI99.ResetAll(this.SVIs);
            Logger.Start(this, ref this.rtfResults);
            this.ButtonCancelReset(enabled: true);
        }

        private async Task MeasurementsRun() {
            foreach (String groupID in this.ConfigTest.GroupIDsSequence) {
                foreach (String measurementID in this.ConfigTest.GroupIDsToMeasurementIDs[groupID]) {
                    try {
                        this.ConfigTest.Measurements[measurementID].Value = await Task.Run(() => this.MeasurementRun(measurementID));
                        this.ConfigTest.Measurements[measurementID].Result = this.EvaluateResultMeasurement(this.ConfigTest.Measurements[measurementID]);
                    } catch (Exception e) {
                        this.MeasurementsRunExceptionHandler(measurementID, e);
                        break;
                    } finally {
                        Logger.LogTest(this.ConfigTest.IsOperation, this.ConfigTest.Measurements[measurementID], ref this.rtfResults);
                    }
                    if (this._cancelled) {
                        this.ConfigTest.Measurements[measurementID].Result = EventCodes.CANCEL;
                        break;
                    }
                    if (String.Equals(this.ConfigTest.Measurements[measurementID].Result, EventCodes.FAIL) && this.ConfigTest.Measurements[measurementID].CancelOnFailure) break;
                }
                if (this.ConfigTest.IsOperation) {

                }
                Dictionary<String, Measurement> groupIDMeasurements = this.ConfigTest.Measurements.Where(m => this.ConfigTest.GroupIDsToMeasurementIDs.ContainsKey(groupID)).ToDictionary(m => m.Key, m => m.Value);
                if (!String.Equals(this.EvaluateResults(groupIDMeasurements), EventCodes.PASS) && this.ConfigTest.Groups[groupID].CancelOnFailure) break;
            }
        }

        private void MeasurementsRunExceptionHandler(String ID, Exception e) {
            if (e.ToString().Contains(CancellationException.ClassName)) {
                while (!(e is CancellationException) && (e.InnerException != null)) e = e.InnerException;
                if ((e is CancellationException) && !String.IsNullOrEmpty(e.Message)) this.ConfigTest.Measurements[ID].Value = e.Message;
                this.ConfigTest.Measurements[ID].Result = EventCodes.CANCEL;
            } else {
                SCPI99.ResetAll(this.SVIs);
                UE24.Set(RelayForms.C.S.NC);
                Logger.LogError(e.ToString());
                this.ConfigTest.Measurements[ID].Result = EventCodes.ERROR;
            }
        }

        private void MeasurementsPostRun() {
            SCPI99.ResetAll(this.SVIs);
            UE24.Set(RelayForms.C.S.NC);
            this.ButtonSelectTests.Enabled = true;
            this.ButtonStartReset(enabled: true);
            this.ButtonCancelReset(enabled: false);
            this.ConfigUUT.EventCode = this.EvaluateResults(this.ConfigTest.Measurements);
            this.TextUUTResult.Text = this.ConfigUUT.EventCode;
            this.TextUUTResult.BackColor = EventCodes.GetColor(this.ConfigUUT.EventCode);
            Logger.Stop(this, ref this.rtfResults);
        }

        private String EvaluateResultMeasurement(Measurement measurement) {
            switch (measurement.ClassName) {
                case MeasurementCustomizable.ClassName:
                    return measurement.Result;
                case MeasurementISP.ClassName:
                    MeasurementISP measurementISP = (MeasurementISP)measurement.ClassObject;
                    if (String.Equals(measurementISP.ISPExpected, measurement.Value, StringComparison.Ordinal)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                case MeasurementNumerical.ClassName:
                    if (!Double.TryParse(measurement.Value, NumberStyles.Float, CultureInfo.CurrentCulture, out Double dMeasurement)) throw new InvalidOperationException($"TestMeasurement ID '{measurement.ID}' Measurement '{measurement.Value}' ≠ System.Double.");
                    MeasurementNumerical measurementNumerical = (MeasurementNumerical)measurement.ClassObject;
                    if ((measurementNumerical.Low <= dMeasurement) && (dMeasurement <= measurementNumerical.High)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                case MeasurementTextual.ClassName:
                    MeasurementTextual measurementTextual = (MeasurementTextual)measurement.ClassObject;
                    if (String.Equals(measurementTextual.Text, measurement.Value, StringComparison.Ordinal)) return EventCodes.PASS;
                    else return EventCodes.FAIL;
                default:
                    throw new NotImplementedException($"TestMeasurement ID '{measurement.ID}' with ClassName '{measurement.ClassName}' not implemented.");
            }
        }

        private String EvaluateResults(Dictionary<String, Measurement> measurements) {
            if (!this.ConfigTest.IsOperation) return EventCodes.UNSET;
            // 0th priority evaluation.
            if (this.GetResultCount(measurements, EventCodes.PASS) == measurements.Count) return EventCodes.PASS;
            // 1st priority evaluation (or could also be last, but we're irrationally optimistic.)
            // All measurement results are PASS, so overall result is PASS.
            if (this.GetResultCount(measurements, EventCodes.ERROR) != 0) return EventCodes.ERROR;
            // 2nd priority evaluation:
            // - If any measurement result is ERROR, overall result is ERROR.
            if (this.GetResultCount(measurements, EventCodes.CANCEL) != 0) return EventCodes.CANCEL;
            // 3rd priority evaluation:
            // - If any measurement result is CANCEL, and none were ERROR, overall result is CANCEL.
            if (this.GetResultCount(measurements, EventCodes.UNSET) != 0) return EventCodes.CANCEL;
            // 4th priority evaluation:
            // - If any measurement result is UNSET, and none were ERROR or CANCEL, then Measurement(s) didn't complete.
            // - Likely occurred because a Measurement failed that had its App.Config TestMeasurement CancelOnFail flag set to true.
            if (this.GetResultCount(measurements, EventCodes.FAIL) != 0) return EventCodes.FAIL;
            // 5th priority evaluation:
            // - If any measurement result is FAIL, and none were ERROR, CANCEL or UNSET, result is FAIL.

            String validEvents = String.Empty, invalidTests = String.Empty;
            foreach (FieldInfo fi in typeof(EventCodes).GetFields()) validEvents += ((String)fi.GetValue(null), String.Empty);
            foreach (KeyValuePair<String, Measurement> kvp in measurements) if (!validEvents.Contains(kvp.Value.Result)) invalidTests += $"ID: '{kvp.Key}' Result: '{kvp.Value.Result}'.{Environment.NewLine}";
            Logger.LogError($"Invalid Measurement ID(s) to Result(s):{Environment.NewLine}{invalidTests}");
            return EventCodes.ERROR;
            // Above handles class EventCodes changing (adding/deleting/renaming EventCodes) without accomodating EvaluateResults() changes. 
        }

        private Int32 GetResultCount(Dictionary<String, Measurement> measurements, String eventCode) { return (from measurement in measurements where String.Equals(measurement.Value.Result, eventCode) select measurement).Count(); }

        public static String NotImplementedMessageEnum(Type enumType) { return $"Unimplemented Enum item; switch/case must support all items in enum '{{{String.Join(",", Enum.GetNames(enumType))}}}'."; }
    }

    public class CancellationException : Exception {
        // NOTE: Only ever throw CancellationException from TestExecutor, never from TestExecutive.
        public CancellationException(String message = "") : base(message) { }
        public const String ClassName = nameof(CancellationException);
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
}