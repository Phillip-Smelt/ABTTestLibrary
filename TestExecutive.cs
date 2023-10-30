using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using ABT.TestSpace.TestExec.AppConfig;
using ABT.TestSpace.TestExec.SCPI_VISA_Instruments;
using ABT.TestSpace.TestExec.Logging;
using ABT.TestSpace.TestExec.Switching.USB_ERB24;
using static ABT.TestSpace.TestExec.Switching.RelayForms;
using System.Text.RegularExpressions;
using System.Xml.Linq;

/// <para>
/// TODO: Refactor TestExecutive to Microsoft's C# Coding Conventions, https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions.
/// NOTE: For public methods, will deviate by using PascalCasing for parameters.  Will use recommended camelCasing for internal & private method parameters.
///  - Prefer named arguments for public methods be Capitalized/PascalCased, not uncapitalized/camelCased.
///  - Invoking public methods with named arguments is a superb, self-documenting coding technique, improved by PascalCasing.
/// TODO: Add documentation per https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments.
/// TODO: Update to .Net 7.0 & C# 11.0 instead of .Net FrameWork 4.8 & C# 7.3 when possible.
/// NOTE: Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas instruments TIDP.SAA Fusion Library supposedly compiled to .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & WinUI 3.
///       TIDP.SAA actually appears to be compiled to .Net FrameWork 4.5, but that's still not necessarily compatible with .Net 7.0.
///  - https://www.ti.com/tool/FUSION_USB_ADAPTER_API
/// TODO: Update to WinUI 3 or WPF instead of WinForms when possible.
/// TODO: Add/Debug TestExecutive control access keys; embedded '&' in control's Text properties for Alt-key quick-keys.
///       - Had them for all controls, worked as quick-keys with their Alt-keys, but didn't display as underlined characters except in Designer view.
///       - Removed access keying '&'s from all control Test properties until resolved.
/// https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-create-access-keys-for-windows-forms-controls?view=netframeworkdesktop-4.8.
/// NOTE: Chose WinForms due to incompatibility of WinUI 3 with .Net Framework, and unfamiliarity with WPF.
/// With deep appreciation for https://learn.microsoft.com/en-us/docs/ & https://stackoverflow.com/!
///
///  References:
///  - https://github.com/Amphenol-Borisch-Technologies/TestExecutive
///  - https://github.com/Amphenol-Borisch-Technologies/TestExecutor
///  </para>

//  Barcode Scanner self-tests; open JPEG image of self-test barcode.
//      - Honeywell Voyager 1200g Barcode Scanner
//      - Open JPEG image of PAP131 USB-HID mode barcode.
//      - Open JPEG image of factory reset barcode.
//      - Open P:\Drive folder containing Voyager 1200g documentation.

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
        private readonly SerialNumberDialog _serialNumberDialog;
        private readonly String _manualFoldersBarcodeScanner;
        private readonly String _manualFoldersInstruments;
        private readonly String _manualFoldersRelays;

        protected TestExecutive(Icon icon) {
            InitializeComponent();
            _appAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            _libraryAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            _serialNumberDialog = ConfigLogger.SerialNumberDialogEnabled ? new SerialNumberDialog() : null;
            Icon = icon;
            TSMI_Administration.Enabled = String.Equals(UserPrincipal.Current.DisplayName, "Phillip Smelt") ? true : false;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio
            IEnumerable<String> manualFolders;
            manualFolders = from xe in XElement.Load("TestExecutive.config.xml").Elements("ManualFolders") select xe.Element("BarCodeScanner").Value;
            _manualFoldersBarcodeScanner = manualFolders.First();
            manualFolders = from xe in XElement.Load("TestExecutive.config.xml").Elements("ManualFolders") select xe.Element("Instruments").Value;
            _manualFoldersInstruments = manualFolders.First();
            manualFolders = from xe in XElement.Load("TestExecutive.config.xml").Elements("ManualFolders") select xe.Element("Relays").Value;
            _manualFoldersRelays = manualFolders.First();
            UE24.Set(C.S.NO); // Relays should be energized/de-energized/re-energized occasionally as preventative maintenance.
            UE24.Set(C.S.NC); // Besides, having 48 relays go "clack-clack" semi-simultaneously sounds awesome...
        }

        public static String NotImplementedMessageEnum(Type enumType) { return $"Unimplemented Enum item; switch/case must support all items in enum '{String.Join(",", Enum.GetNames(enumType))}'."; }

        public virtual void Initialize() {
            SCPI99.Reset(SVIs);
            UE24.Set(C.S.NC);
        }

        public virtual Boolean Initialized() {
            return SCPI99.Are(SVIs, STATE.off)
                && UE24.Are(C.S.NC);
        }
        /// <summary>
        /// NOTE: Two types of TestExecutor Cancellations possible, each having two sub-types resulting in 4 altogether:
        /// <para>
        /// A) Spontaneous Operator Initiated Cancellations:
        ///      1)  Operator Proactive:
        ///          - Microsoft's recommended CancellationTokenSource technique, permitting Operator to proactively
        ///            cancel currently executing Measurement.
        ///          - Requires TestExecutor implementation by the Test Developer, but is initiated by Operator, so is categorized as such.
        ///          - Implementation necessary if the *currently* executing Measurement must be cancellable during execution by the Operator.
        ///          - https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
        ///          - https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation
        ///          - https://learn.microsoft.com/en-us/dotnet/standard/threading/canceling-threads-cooperatively
        ///      2)  Operator Reactive:
        ///          - TestExecutive's already implemented, always available & default reactive "Cancel before next Test" technique,
        ///            which simply sets _cancelled Boolean to true, checked at the end of TestExecutive.MeasurementsRun()'s foreach loop.
        ///          - If _cancelled is true, TestExecutive.MeasurementsRun()'s foreach loop is broken, causing reactive cancellation
        ///            prior to the next Measurement's execution.
        ///          - Note: This doesn't proactively cancel the *currently* executing Measurement, which runs to completion.
        /// B) PrePlanned Developer Programmed Cancellations:
        ///      3)  TestExecutor/Test Developer initiated Cancellations:
        ///          - Any TestExecutor's Measurement can initiate a Cancellation programmatically by simply throwing a CancellationException:
        ///          - Permits immediate Cancellation if specific condition(s) occur in a Measurement; perhaps to prevent UUT or equipment damage,
        ///            or simply because futher execution is pointless.
        ///          - Simply throw a CancellationException if the specific condition(s) occcur.
        ///      4)  App.Config's CancelNotPassed:
        ///          - App.Config's TestMeasurement element has a Boolean "CancelNotPassed" field:
        ///          - If the current TestExecutor.MeasurementRun() has CancelNotPassed=true and it's resulting EvaluateResultMeasurement() doesn't return EventCodes.PASS,
        ///            TestExecutive.MeasurementsRun() will break/exit, stopping further testing.
        ///		    - Do not pass Go, do not collect $200, go directly to TestExecutive.MeasurementsPostRun().
        ///
        /// NOTE: The Operator Proactive & TestExecutor/Test Developer initiated Cancellations both occur while the currently executing TestExecutor.MeasurementRun() conpletes, via 
        ///       thrown CancellationExceptions.
        /// NOTE: The Operator Reactive & App.Config's CancelNotPassed Cancellations both occur after the currently executing TestExecutor.MeasurementRun() completes, via checks
        ///       inside the TestExecutive.MeasurementsRun() loop.
        /// </para>
        /// </summary>


        #region Form
        private void Form_Shown(Object sender, EventArgs e) {
            FormModeReset();
            FormModeWait();
            Text = $"{ConfigUUT.Number}, {ConfigUUT.Description}";
        }

        private void FormModeReset() {
            TextResult.Text = String.Empty;
            TextResult.BackColor = Color.White;
            rtfResults.Text = String.Empty;
        }

        private void FormModeRun() {
            ButtonCancelReset(enabled: true);
            ButtonSelectTests.Enabled = false;
            ButtonStartReset(enabled: false);
            TSMI_File.Enabled = false;
            ButtonEmergencyStop.Enabled = true; // Always enabled.
        }

        private void FormModeWait() {
            ButtonSelectTests.Enabled = true;
            ButtonStartReset(enabled: (ConfigTest != null));
            ButtonCancelReset(enabled: false);
            if (ConfigTest != null) TSMI_File.Enabled = !ConfigTest.IsOperation;
            else TSMI_File.Enabled = false;
            ButtonEmergencyStop.Enabled = true; // Always enabled.
        }

        private void FolderOpen(String FolderPath) {
            if (Directory.Exists(FolderPath)) {
                ProcessStartInfo psi = new ProcessStartInfo {
                    FileName = "explorer.exe",
                    WindowStyle = ProcessWindowStyle.Minimized,
                    Arguments = $"\"{FolderPath}\""
                };
                Process.Start(psi);
                // Paths with embedded spaces require enclosing double-quotes (").
                // Even then, simpler 'System.Diagnostics.Process.Start("explorer.exe", path);' invocation fails - must use ProcessStartInfo class.
                // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
            } else MessageBox.Show(Form.ActiveForm, $"Path {FolderPath} invalid.", "Yikes!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #region Command Buttons
        private void ButtonCancel_Clicked(Object sender, EventArgs e) {
            CancelTokenSource.Cancel();
            _cancelled = true;
            ButtonCancel.Text = "Cancelling..."; // Here's to British English spelling!
            ButtonCancel.Enabled = false;
            ButtonCancel.UseVisualStyleBackColor = false;
            ButtonCancel.BackColor = Color.Red;
        }

        private void ButtonCancelReset(Boolean enabled) {
            if (enabled) {
                ButtonCancel.UseVisualStyleBackColor = false;
                ButtonCancel.BackColor = Color.Yellow;
            } else {
                ButtonCancel.BackColor = SystemColors.Control;
                ButtonCancel.UseVisualStyleBackColor = true;
            }
            if (CancelTokenSource.IsCancellationRequested) {
                CancelTokenSource.Dispose();
                CancelTokenSource = new CancellationTokenSource();
            }
            _cancelled = false;
            ButtonCancel.Text = "Cancel";
            ButtonCancel.Enabled = enabled;
        }

        private void ButtonEmergencyStop_Clicked(Object sender, EventArgs e) {
            Initialize();
            if (ButtonCancel.Enabled) ButtonCancel_Clicked(this, null);
       }

        private void ButtonSelectTests_Click(Object sender, EventArgs e) {
            ConfigTest = AppConfigTest.Get();
            Text = $"{ConfigUUT.Number}, {ConfigUUT.Description}, {ConfigTest.TestElementID}";
            FormModeReset();
            FormModeWait();
        }

        private async void ButtonStart_Clicked(Object sender, EventArgs e) {
            String serialNumber;
            if (ConfigLogger.SerialNumberDialogEnabled) {
                _serialNumberDialog.Set(ConfigUUT.SerialNumber);
                serialNumber = _serialNumberDialog.ShowDialog(this).Equals(DialogResult.OK) ? _serialNumberDialog.Get() : String.Empty;
                _serialNumberDialog.Hide();
            } else {
                serialNumber = Interaction.InputBox(Prompt: "Please enter ABT Serial Number", Title: "Enter ABT Serial Number", DefaultResponse: ConfigUUT.SerialNumber);
            }
            if (String.Equals(serialNumber, String.Empty)) return;
            ConfigUUT.SerialNumber = serialNumber;
            FormModeReset();
            FormModeRun();
            MeasurementsPreRun();
            await MeasurementsRun();
            MeasurementsPostRun();
            FormModeWait();
        }

        private void ButtonStartReset(Boolean enabled) {
            if (enabled) {
                ButtonStart.UseVisualStyleBackColor = false;
                ButtonStart.BackColor = Color.Green;
            } else {
                ButtonStart.BackColor = SystemColors.Control;
                ButtonStart.UseVisualStyleBackColor = true;
            }
            ButtonStart.Enabled = enabled;
        }
        #endregion Command Buttons

        #region Tool Strip Menu Items
        private void TSMI_File_Save_Click(Object sender, EventArgs e) { }
        private void TSMI_File_Print_Click(Object sender, EventArgs e) { }
        private void TSMI_File_PrintPreview_Click(Object sender, EventArgs e) { }
        private void TSMI_File_Exit_Click(Object sender, EventArgs e) { }

        private void TSMI_Administration_EditAppConfig_Click(Object sender, EventArgs e) { }
        private void TSMI_Administration_EditTestExecutiveConfigXML_Click(Object sender, EventArgs e) { }
        private void TSMI_AdministrationLaunchKeysightBenchVue_Click(Object sender, EventArgs e) { }
        private void TSMI_AdministrationLaunchKeysightCommandExpert_Click(Object sender, EventArgs e) { }
        private void TSMI_AdministrationLaunchKeysightConnectionExpert_Click(Object sender, EventArgs e) { }
        private void TSMI_AdministrationLaunchKeysightMeasurementComputingInstaCal_Click(Object sender, EventArgs e) { }
        private void TSMI_AdministrationLaunchMicrosoftSQL_ServerManagementStudio_Click(Object sender, EventArgs e) { }
        private void TSMI_AdministrationLaunchMicrosoftVisualStudio_Click(Object sender, EventArgs e) { }

        private void TSMI_SystemBarcodeScannerDiscover_Click(Object sender, EventArgs e) { }
        private void TSMI_SystemBarcodeScannerProgramDefaults_Click(Object sender, EventArgs e) { }
        private void TSMI_SystemDiagnosticsBarcodeScanner_Click(Object sender, EventArgs e) { }
        private void TSMI_SystemDiagnosticsInstruments_Click(Object sender, EventArgs e) { }
        private void TSMI_SystemDiagnosticsRelays_Click(Object sender, EventArgs e) { }
        private void TSMI_SystemManualsBarcodeScanner_Click(Object sender, EventArgs e) { FolderOpen(_manualFoldersBarcodeScanner); }
        private void TSMI_SystemManualsInstruments_Click(Object sender, EventArgs e) { FolderOpen(_manualFoldersInstruments); }
        private void TSMI_SystemManualsRelays_Click(Object sender, EventArgs e) { FolderOpen(_manualFoldersRelays); }
        private void TSMI_SystemComplimentsPraiseAndPlaudits_Click(Object sender, EventArgs e) { _ = MessageBox.Show($"You are a kind person, {UserPrincipal.Current.DisplayName}.", $"Thank you!", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void TSMI_SystemComplimentsMoney_Click(Object sender, EventArgs e) { _ = MessageBox.Show($"Prefer ₿itcoin donations!", $"₿₿₿", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void TSMI_SystemCritiqueBugReport_Click(Object sender, EventArgs e) { }
        private void TSMI_SystemCritiqueImprovementRequest_Click(Object sender, EventArgs e) { }
        private void TSMI_SystemAbout_Click(Object sender, EventArgs e) {
            _ = MessageBox.Show($"{Assembly.GetEntryAssembly().GetName()}, version {_appAssemblyVersion}.{Environment.NewLine}{Environment.NewLine}" +
             $"{Assembly.GetExecutingAssembly().GetName()}, version {_libraryAssemblyVersion}.{Environment.NewLine}{Environment.NewLine}" +
             $"© 2022, Amphenol Borisch Technologies.",
            "About...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TSMI_UUT_eDocs_Click(Object sender, EventArgs e) { FolderOpen(ConfigUUT.DocumentationFolder); }
        private void TSMI_UUT_ManualsInstruments_Click(Object sender, EventArgs e) { FolderOpen(ConfigUUT.ManualsFolder); }
        private void TSMI_UUT_TestData_P_DriveTDR_Folder_Click(Object sender, EventArgs e) { FolderOpen(ConfigLogger.FilePath); }
        private void TSMI_UUT_TestDataSQL_ReportingAndQuerying_Click(Object sender, EventArgs e) { }
        #endregion Tool Strip Menu Items
        #endregion Form



        #region Measurements
        private void MeasurementsPreRun() {
            Logger.Start(this, ref rtfResults);
            foreach (KeyValuePair<String, Measurement> kvp in ConfigTest.Measurements) {
                if (String.Equals(kvp.Value.ClassName, MeasurementNumeric.ClassName)) kvp.Value.Value = Double.NaN.ToString();
                else kvp.Value.Value = String.Empty;
                kvp.Value.Result = EventCodes.UNSET;
                kvp.Value.Message = String.Empty;
            }
            ConfigUUT.EventCode = EventCodes.UNSET;
            Initialize();
        }

        private async Task MeasurementsRun() {
            foreach (String groupID in ConfigTest.GroupIDsSequence) {
                foreach (String measurementID in ConfigTest.GroupIDsToMeasurementIDs[groupID]) {
                    try {
                        ConfigTest.Measurements[measurementID].Value = await Task.Run(() => MeasurementRun(measurementID));
                        ConfigTest.Measurements[measurementID].Result = MeasurementEvaluate(ConfigTest.Measurements[measurementID]);
                    } catch (Exception e) {
                        Initialize();
                        if (e.ToString().Contains(CancellationException.ClassName)) {
                            ConfigTest.Measurements[measurementID].Result = EventCodes.CANCEL;
                            while (!(e is CancellationException) && (e.InnerException != null)) e = e.InnerException; // No fluff, just stuff.
                            ConfigTest.Measurements[measurementID].Message += $"{Environment.NewLine}  {CancellationException.ClassName}:{Environment.NewLine}  {e.Message}";
                        } else {
                            ConfigTest.Measurements[measurementID].Result = EventCodes.ERROR;
                            ConfigTest.Measurements[measurementID].Message += $"{Environment.NewLine}{e}";
                            _ = MessageBox.Show(Form.ActiveForm, $"Unexpected error.  Details logged for analysis & resolution.{Environment.NewLine}{Environment.NewLine}" +
                                "Please contact Test Engineering if assistance required.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    } finally {
                        Logger.LogTest(ConfigTest.IsOperation, ConfigTest.Measurements[measurementID], ref rtfResults);
                    }
                    if (_cancelled) {
                        ConfigTest.Measurements[measurementID].Result = EventCodes.CANCEL;
                        return;
                    }
                    if (MeasurementCancelNotPassed(measurementID)) return;
                }
                if (MeasurementsCancelNotPassed(groupID)) return;
            }
        }
        
        protected abstract Task<String> MeasurementRun(String measurementID);

        private void MeasurementsPostRun() {
            Initialize();
            ConfigUUT.EventCode = MeasurementsEvaluate(ConfigTest.Measurements);
            TextResult.Text = ConfigUUT.EventCode;
            TextResult.BackColor = EventCodes.GetColor(ConfigUUT.EventCode);
            Logger.Stop(this, ref rtfResults);
        }

        private Boolean MeasurementCancelNotPassed(String measurementID) { return !String.Equals(ConfigTest.Measurements[measurementID].Result, EventCodes.PASS) && ConfigTest.Measurements[measurementID].CancelNotPassed; }

        private Boolean MeasurementsCancelNotPassed(String groupID) { return !String.Equals(MeasurementsEvaluate(MeasurementsGet(groupID)), EventCodes.PASS) && ConfigTest.Groups[groupID].CancelNotPassed; }

        private Dictionary<String, Measurement> MeasurementsGet(String groupID) {
            Dictionary<String, Measurement> measurements = new Dictionary<String, Measurement>();
            foreach (String measurementID in ConfigTest.GroupIDsToMeasurementIDs[groupID]) measurements.Add(measurementID, ConfigTest.Measurements[measurementID]);
            return measurements;
        }

        private String MeasurementEvaluate(Measurement measurement) {
            switch (measurement.ClassName) {
                case MeasurementCustom.ClassName:
                    return measurement.Result; // Test Developer must set Result in TestExecutor, else it remains MeasurementsPreRun()'s initital EventCodes.UNSET.
                case MeasurementNumeric.ClassName:
                    if (!Double.TryParse(measurement.Value, NumberStyles.Float, CultureInfo.CurrentCulture, out Double dMeasurement)) throw new InvalidOperationException($"TestMeasurement ID '{measurement.ID}' Measurement '{measurement.Value}' ≠ System.Double.");
                    MeasurementNumeric mn = (MeasurementNumeric)measurement.ClassObject;
                    if ((mn.Low <= dMeasurement) && (dMeasurement <= mn.High)) return EventCodes.PASS;
                    return EventCodes.FAIL;
                case MeasurementProcess.ClassName:
                    MeasurementProcess mp = (MeasurementProcess)measurement.ClassObject;
                    if (String.Equals(mp.ProcessExpected, measurement.Value, StringComparison.Ordinal)) return EventCodes.PASS;
                    return EventCodes.FAIL;
                case MeasurementTextual.ClassName:
                    MeasurementTextual mt = (MeasurementTextual)measurement.ClassObject;
                    if (String.Equals(mt.Text, measurement.Value, StringComparison.Ordinal)) return EventCodes.PASS;
                    return EventCodes.FAIL;
                default:
                    throw new NotImplementedException($"TestMeasurement ID '{measurement.ID}' with ClassName '{measurement.ClassName}' not implemented.");
            }
        }

        private String MeasurementsEvaluate(Dictionary<String, Measurement> measurements) {
            if (MeasurementResultsCount(measurements, EventCodes.PASS) == measurements.Count) return EventCodes.PASS;
            // 1st priority evaluation (or could also be last, but we're irrationally optimistic.)
            // All measurement results are PASS, so overall result is PASS.
            if (MeasurementResultsCount(measurements, EventCodes.ERROR) != 0) return EventCodes.ERROR;
            // 2nd priority evaluation:
            // - If any measurement result is ERROR, overall result is ERROR.
            if (MeasurementResultsCount(measurements, EventCodes.CANCEL) != 0) return EventCodes.CANCEL;
            // 3rd priority evaluation:
            // - If any measurement result is CANCEL, and none were ERROR, overall result is CANCEL.
            if (MeasurementResultsCount(measurements, EventCodes.UNSET) != 0) return EventCodes.CANCEL;
            // 4th priority evaluation:
            // - If any measurement result is UNSET, and none were ERROR or CANCEL, then Measurement(s) didn't complete.
            // - Likely occurred because a Measurement failed that had its App.Config TestMeasurement CancelOnFail flag set to true.
            if (MeasurementResultsCount(measurements, EventCodes.FAIL) != 0) return EventCodes.FAIL;
            // 5th priority evaluation:
            // - If any measurement result is FAIL, and none were ERROR, CANCEL or UNSET, result is FAIL.

            String validEvents = String.Empty, invalidTests = String.Empty;
            foreach (FieldInfo fi in typeof(EventCodes).GetFields()) validEvents += ((String)fi.GetValue(null), String.Empty);
            foreach (KeyValuePair<String, Measurement> kvp in measurements) if (!validEvents.Contains(kvp.Value.Result)) invalidTests += $"ID: '{kvp.Key}' Result: '{kvp.Value.Result}'.{Environment.NewLine}";
            Logger.LogError($"{Environment.NewLine}Invalid Measurement ID(s) to Result(s):{Environment.NewLine}{invalidTests}", ShowMessage: true);
            return EventCodes.ERROR;
            // Above handles class EventCodes changing (adding/deleting/renaming EventCodes) without accomodating EvaluateResults() changes. 
        }

        private Int32 MeasurementResultsCount(Dictionary<String, Measurement> measurements, String eventCode) { return (from measurement in measurements where String.Equals(measurement.Value.Result, eventCode) select measurement).Count(); }
        #endregion Measurements
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

    public static class Ext{ public static Boolean In<T>(this T value, params T[] values) where T : struct { return values.Contains(value); } }
}