using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Outlook = Microsoft.Office.Interop.Outlook;
using Windows.Devices.Enumeration;
using Windows.Devices.PointOfService;
using ABT.TestSpace.TestExec.AppConfig;
using ABT.TestSpace.TestExec.SCPI_VISA_Instruments;
using ABT.TestSpace.TestExec.Logging;
using ABT.TestSpace.TestExec.Switching.USB_ERB24;
using static ABT.TestSpace.TestExec.Switching.RelayForms;

/// <para>
/// TODO:  Eventually; refactor TestExecutive to Microsoft's C# Coding Conventions, https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions.
/// NOTE:  For public methods, will deviate by using PascalCasing for parameters.  Will use recommended camelCasing for internal & private method parameters.
///  - Prefer named arguments for public methods be Capitalized/PascalCased, not uncapitalized/camelCased.
///  - Invoking public methods with named arguments is a superb, self-documenting coding technique, improved by PascalCasing.
/// TODO:  Eventually; add documentation per https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments.
/// TODO:  Eventually; update to .Net 7.0 & C# 11.0 instead of .Net FrameWork 4.8 & C# 7.3 when possible.
/// NOTE:  Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas instruments TIDP.SAA Fusion Library supposedly compiled to .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & WinUI 3.
///        TIDP.SAA actually appears to be compiled to .Net FrameWork 4.5, but that's still not necessarily compatible with .Net 7.0.
///  - https://www.ti.com/tool/FUSION_USB_ADAPTER_API
/// TODO:  Eventually; update to WinUI 3 or WPF instead of WinForms when possible.
/// TODO:  Soon; ensure Borisch Domain Group "Test - Engineers" has read & write permissions on all TestExecutor & TestExecutor folder/files.
/// TODO:  Soon; ensure Borisch Domain Groups ≠ "Test - Engineers" have only read permissions on all TestExecutor & TestExecutor folder/files.
/// NOTE:  Chose WinForms due to incompatibility of WinUI 3 with .Net Framework, and unfamiliarity with WPF.
/// With deep appreciation for https://learn.microsoft.com/en-us/docs/ & https://stackoverflow.com/!
///
///  References:
///  - https://github.com/Amphenol-Borisch-Technologies/TestExecutive
///  - https://github.com/Amphenol-Borisch-Technologies/TestExecutor
///  </para>
///  <para>
/// NOTE:  ABT's Zero Trust, Cloudflare Warp enterprise security solution inhibits GitHub's security, causing below error when sychronizing with
///       TestExecutive's GitHub repository at https://github.com/Amphenol-Borisch-Technologies/TestExecutive:
///             Opening repositories:
///             P:\Test\Engineers\repos\TestExecutor
///             Opening repositories:
///             P:\Test\Engineers\repos\TestExecutor
///             C:\Users\phils\source\repos\TestExecutive
///             Git failed with a fatal error.
///             Git failed with a fatal error.
///             unable to access 'https://github.com/Amphenol-Borisch-Technologies/TestLibrary/': schannel: CertGetCertificateChain trust error CERT_TRUST_IS_PARTIAL_CHAIN
///  - Temporarily disabling Zero Trust by "pausing" it resolves above error.
///    - Zero Trust's "pause" eventually, times out, and Zero Trust eventually, re-enables itself silently, without notifying you.
///  - https://stackoverflow.com/questions/27087483/how-to-resolve-git-pull-fatal-unable-to-access-https-github-com-empty
///  - FYI, synchronizing with TestExecutor's repository doesn't error out, as it doesn't utilize a Git server.
///  </para>

namespace ABT.TestSpace.TestExec {
    public abstract partial class TestExecutive : Form {
        public const String GlobalConfigurationFile = @"C:\Users\phils\source\repos\TestExecutive\TestExecutive.config.xml";
        public const String NONE = "NONE";
        public readonly AppConfigLogger ConfigLogger = AppConfigLogger.Get();
        public readonly Dictionary<SCPI_VISA_Instrument.Alias, SCPI_VISA_Instrument> SVIs = SCPI_VISA_Instrument.Get();
        public AppConfigUUT ConfigUUT { get; private set; } = AppConfigUUT.Get();
        public AppConfigTest ConfigTest { get; private set; } // Requires form; instantiated by button_click event method.
        public CancellationTokenSource CancelTokenSource { get; private set; } = new CancellationTokenSource();
        public String MeasurementIDPresent { get; private set; } = String.Empty;
        public Measurement MeasurementPresent { get; private set; } = null;
        public static readonly String EMailAdministratorTo = (from xe in XElement.Load(GlobalConfigurationFile).Elements("Administrators") select xe.Element("EMail").Value).ElementAt(0);
        public static readonly String EMailAdministratorCC = (from xe in XElement.Load(GlobalConfigurationFile).Elements("Administrators") select xe.Element("EMail").Value).ElementAt(1);
        private readonly String _serialNumberRegEx = null;
        private readonly SerialNumberDialog _serialNumberDialog = null;
        private readonly RegistryKey _serialNumberRegistryKey = null;
        private const String _serialNumberMostRecent = "MostRecent";
        private Boolean _cancelled = false;

        protected TestExecutive(Icon icon) {
            InitializeComponent();
            Icon = icon;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio

            if (String.Equals(ConfigUUT.SerialNumberRegExCustom, "NotApplicable")) _serialNumberRegEx = (from xe in XElement.Load(GlobalConfigurationFile).Elements() select xe.Element("SerialNumberRegExDefault").Value).ElementAt(0);
            else _serialNumberRegEx = ConfigUUT.SerialNumberRegExCustom;
            if (RegexInvalid(_serialNumberRegEx)) {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Invalid Serial Number Regular Expression '{_serialNumberRegEx}':");
                sb.AppendLine($"   Check {GlobalConfigurationFile}/SerialNumberRegExDefault or App.config/UUT_SerialNumberRegExCustom for valid Regular Expression syntax.");
                sb.AppendLine($"   Thank you & have a nice day {UserPrincipal.Current.DisplayName}!");
                throw new ArgumentException(sb.ToString());
            }

            _serialNumberRegistryKey = Registry.CurrentUser.CreateSubKey($"SOFTWARE\\{ConfigUUT.Customer}\\{ConfigUUT.Number}\\SerialNumber");
            ConfigUUT.SerialNumber = _serialNumberRegistryKey.GetValue(_serialNumberMostRecent, String.Empty).ToString();

#if !NO_HARDWARE
            if(ConfigLogger.SerialNumberDialogEnabled) _serialNumberDialog = new SerialNumberDialog(_serialNumberRegEx);
            UE24.Set(C.S.NO); // Relays should be de-energized/re-energized occasionally as preventative maintenance.  Regular exercise is good for relays, as well as people!
            UE24.Set(C.S.NC); // Besides, having 48 relays go "clack-clack" nearly simultaneously sounds awesome...
#endif
        }

        public static Boolean RegexInvalid(String RegularExpression) {
            if (String.IsNullOrWhiteSpace(RegularExpression)) return true;
            try {
                Regex.Match("", RegularExpression);
            } catch (ArgumentException) {
                return true;
            }
            return false;
        }

        public static String NotImplementedMessageEnum(Type enumType) { return $"Unimplemented Enum item; switch/case must support all items in enum '{String.Join(",", Enum.GetNames(enumType))}'."; }

        public void ErrorMessage(String Error) {
            _ = MessageBox.Show(ActiveForm, $"Unexpected error.{Environment.NewLine}{Environment.NewLine}{Error}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ErrorMessage(Exception Ex) {
            ErrorMessage($"Will attempt to E-Mail details to {EMailAdministratorTo} & CC {EMailAdministratorCC}.{Environment.NewLine}{Environment.NewLine}Please select your Outlook profile if dialog appears.");
            SendAdministratorMailMessage("Exception caught!", Ex, ConfigUUT.EMailTestEngineer);
        }

        public virtual void Initialize() {
#if !NO_HARDWARE
            SCPI99.Reset(SVIs);
            UE24.Set(C.S.NC);
            Debug.Assert(Initialized());
#endif
        }

        public virtual Boolean Initialized() {
#if !NO_HARDWARE
            return SCPI99.Are(SVIs, STATE.off) && UE24.Are(C.S.NC);
#else
            return false;
#endif
        }

        public static void SendAdministratorMailMessage(String Subject, Exception Ex, String CC="") {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Machine Name          : {Environment.MachineName}");
            sb.AppendLine($"User Principal        : {UserPrincipal.Current.DisplayName}");
            sb.AppendLine($"Exception Message     : {Ex.Message}{Environment.NewLine}");
            sb.AppendLine($"Exception Stack Trace : {Ex.StackTrace}");
            SendAdministratorMailMessage(Subject, Body: sb.ToString(), CC);
        }

        public static void SendAdministratorMailMessage(String Subject, String Body, String CC="") {
            Outlook.MailItem mailItem = GetMailItem();
            mailItem.Subject = Subject;
            mailItem.To = EMailAdministratorTo;
            Outlook.Recipient recipient = mailItem.Recipients.Add(EMailAdministratorCC);    recipient.Type = (Int32)Outlook.OlMailRecipientType.olCC;
            recipient = mailItem.Recipients.Add(CC);                                        recipient.Type = (Int32)Outlook.OlMailRecipientType.olCC;
            mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
            mailItem.Body = Body;
            mailItem.Send();
        }

        /// <summary>
        /// NOTE:  Two types of TestExecutor Cancellations possible, each having two sub-types resulting in 4 altogether:
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
        ///      4)  App.config's CancelNotPassed:
        ///          - App.config's TestMeasurement element has a Boolean "CancelNotPassed" field:
        ///          - If the current TestExecutor.MeasurementRun() has CancelNotPassed=true and it's resulting EvaluateResultMeasurement() doesn't return EventCodes.PASS,
        ///            TestExecutive.MeasurementsRun() will break/exit, stopping further testing.
        ///		    - Do not pass Go, do not collect $200, go directly to TestExecutive.MeasurementsPostRun().
        ///
        /// NOTE:  The Operator Proactive & TestExecutor/Test Developer initiated Cancellations both occur while the currently executing TestExecutor.MeasurementRun() conpletes, via 
        ///        thrown CancellationExceptions.
        /// NOTE:  The Operator Reactive & App.config's CancelNotPassed Cancellations both occur after the currently executing TestExecutor.MeasurementRun() completes, via checks
        ///        inside the TestExecutive.MeasurementsRun() loop.
        /// </para>
        /// </summary>

        #region Form
        private static Outlook.MailItem GetMailItem() {
            Outlook.Application outlook;
            try {
                if (Process.GetProcessesByName("OUTLOOK").Length > 0) {
                    outlook = Marshal.GetActiveObject("Outlook.Application") as Outlook.Application;
                } else {
                    outlook = new Outlook.Application();
                    Outlook.NameSpace nameSpace = outlook.GetNamespace("MAPI");
                    nameSpace.Logon("", "", true, true);
                    nameSpace = null;
                }
                return outlook.CreateItem(Outlook.OlItemType.olMailItem);
            } catch {
                _ = MessageBox.Show(ActiveForm, "Could not open Outlook.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void SendMailMessageWithAttachment(String subject) {
            Outlook.MailItem mailItem = GetMailItem();
            mailItem.Subject = subject;
            mailItem.To = EMailAdministratorTo;
            Outlook.Recipient recipient = mailItem.Recipients.Add(EMailAdministratorCC);    recipient.Type = (Int32)Outlook.OlMailRecipientType.olCC;
            recipient = mailItem.Recipients.Add(ConfigUUT.EMailTestEngineer);               recipient.Type = (Int32)Outlook.OlMailRecipientType.olCC;
            mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
            mailItem.Body =
                $"Please detail desired Bug Report or Improvement Request:{Environment.NewLine}" +
                $" - Please attach relevant files, and/or embed relevant screen-captures.{Environment.NewLine}" +
                $" - Be specific!  Be verbose!  Unleash your inner author!  It's your time to shine!{Environment.NewLine}";
            String rtfTempFile = $"{Path.GetTempPath()}\\{ConfigUUT.Number}.rtf";
            rtfResults.SaveFile(rtfTempFile);
            _ = mailItem.Attachments.Add(rtfTempFile, Outlook.OlAttachmentType.olByValue, 1, $"{ConfigUUT.Number}.rtf");
            mailItem.Display();
        }

        private void Form_Shown(Object sender, EventArgs e) { ButtonSelectTests_Click(sender, e); }

        private void FormModeReset() {
            TextResult.Text = String.Empty;
            TextResult.BackColor = Color.White;
            rtfResults.Text = String.Empty;
        }

        private void FormModeRun() {
            ButtonCancelReset(enabled: true);
            ButtonSelectTests.Enabled = false;
            ButtonStartReset(enabled: false);
            ButtonEmergencyStop.Enabled = true; // Always enabled.
        }

        private void FormModeWait() {
            ButtonSelectTests.Enabled = true;
            ButtonStartReset(enabled: (ConfigTest != null));
            ButtonCancelReset(enabled: false);
            ButtonEmergencyStop.Enabled = true; // Always enabled.
        }

        private void OpenApp(String CompanyID, String AppID, String Arguments="") {
            String app = (from xe in XElement.Load(GlobalConfigurationFile).Elements("Apps").Elements(CompanyID) select xe.Element(AppID).Value).ElementAt(0);
            
            if (File.Exists($"{app}")) {
                ProcessStartInfo psi = new ProcessStartInfo {
                    FileName = app,
                    WindowStyle = ProcessWindowStyle.Normal,
                    WorkingDirectory = "",
                    Arguments = Arguments
                };
                Process.Start(psi);
                // Strings with embedded spaces require enclosing double-quotes (").
                // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
            } else InvalidPathError(app);
        }

        private String GetFile(String FileID) { return (from xe in XElement.Load(GlobalConfigurationFile).Elements("Files") select xe.Element(FileID).Value).ElementAt(0); }

        private String GetFolder(String FolderID) { return (from xe in XElement.Load(GlobalConfigurationFile).Elements("Folders") select xe.Element(FolderID).Value).ElementAt(0); }

        private void OpenFolder(String FolderPath) {
            if (Directory.Exists(FolderPath)) {
                ProcessStartInfo psi = new ProcessStartInfo {
                    FileName = "explorer.exe",
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = $"\"{FolderPath}\""
                };
                Process.Start(psi);
                // Paths with embedded spaces require enclosing double-quotes (").
                // Even then, simpler 'System.Diagnostics.Process.Start("explorer.exe", path);' invocation fails - thus using ProcessStartInfo class.
                // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
            } else InvalidPathError(FolderPath);
        }

        private void InvalidPathError(String InvalidPath) { _ = MessageBox.Show(ActiveForm, $"Path {InvalidPath} invalid.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        private void PreApplicationExit() {
            if (ConfigLogger.SerialNumberDialogEnabled) _serialNumberDialog.Close();
            Initialize();
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
#if !NO_HARDWARE
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
#else
            ButtonCancel.Enabled = false;
#endif
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
                serialNumber = Interaction.InputBox(Prompt: "Please enter ABT Serial Number", Title: "Enter ABT Serial Number", DefaultResponse: ConfigUUT.SerialNumber).Trim().ToUpper();
                serialNumber = Regex.IsMatch(serialNumber, _serialNumberRegEx) ? serialNumber : String.Empty;
            }
            if (String.Equals(serialNumber, String.Empty)) return;
            _serialNumberRegistryKey.SetValue(_serialNumberMostRecent, serialNumber);
            ConfigUUT.SerialNumber = serialNumber;

            FormModeReset();
            FormModeRun();
            MeasurementsPreRun();
            await MeasurementsRun();
            MeasurementsPostRun();
            FormModeWait();
        }

        private void ButtonStartReset(Boolean enabled) {
#if !NO_HARDWARE
            if (enabled) {
                ButtonStart.UseVisualStyleBackColor = false;
                ButtonStart.BackColor = Color.Green;
            } else {
                ButtonStart.BackColor = SystemColors.Control;
                ButtonStart.UseVisualStyleBackColor = true;
            }
            ButtonStart.Enabled = enabled;
#else
            ButtonStart.Enabled = false;
#endif
        }
        #endregion Command Buttons

        #region Tool Strip Menu Items
        private void TSMI_File_Save_Click(Object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Title = "Save Test Results",
                Filter = "Rich Text Format|*.rtf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"{ConfigUUT.Number}_{ConfigTest.TestElementID}_{ConfigUUT.SerialNumber}",
                DefaultExt = "rtf",
                CreatePrompt = false,
                OverwritePrompt = true
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK) rtfResults.SaveFile(saveFileDialog.FileName);
        }
        private void TSMI_File_Exit_Click(Object sender, EventArgs e) {
            PreApplicationExit();
            System.Windows.Forms.Application.Exit();
        }

        private void TSMI_Apps_KeysightBenchVue_Click(Object sender, EventArgs e) { OpenApp("Keysight", "BenchVue"); }
        private void TSMI_Apps_KeysightCommandExpert_Click(Object sender, EventArgs e) { OpenApp("Keysight", "CommandExpert"); }
        private void TSMI_Apps_KeysightConnectionExpert_Click(Object sender, EventArgs e) { OpenApp("Keysight", "ConnectionExpert"); }
        private void TSMI_Apps_MeasurementComputingInstaCal_Click(Object sender, EventArgs e) { OpenApp("MeasurementComputing", "InstaCal"); }
        private void TSMI_Apps_MicrosoftSQL_ServerManagementStudio_Click(Object sender, EventArgs e) { OpenApp("Microsoft", "SQLServerManagementStudio"); }
        private void TSMI_Apps_MicrosoftVisualStudio_Click(Object sender, EventArgs e) { OpenApp("Microsoft", "VisualStudio"); }
        private void TSMI_Apps_MicrosoftVisualStudioCode_Click(Object sender, EventArgs e) { OpenApp("Microsoft", "VisualStudioCode"); }
        private void TSMI_Apps_MicrosoftXML_Notepad_Click(Object sender, EventArgs e) { OpenApp("Microsoft", "XMLNotepad"); }
        private void TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool_Click(Object sender, EventArgs e) { OpenApp("TexasInstruments", "SMBus_I2C_SAA_Tool", "--smbus-gui"); }

        private void TSMI_Feedback_ComplimentsPraiseεPlaudits_Click(Object sender, EventArgs e) { _ = MessageBox.Show($"You are a kind person, {UserPrincipal.Current.DisplayName}.", $"Thank you!", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void TSMI_Feedback_ComplimentsMoney_Click(Object sender, EventArgs e) { _ = MessageBox.Show($"Prefer ₿itcoin donations!", $"₿₿₿", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void TSMI_Feedback_CritiqueBugReport_Click(Object sender, EventArgs e) { SendMailMessageWithAttachment($"Bug Report from {UserPrincipal.Current.DisplayName} for {ConfigUUT.Number}, {ConfigUUT.Description}."); }
        private void TSMI_Feedback_CritiqueImprovementRequest_Click(Object sender, EventArgs e) { SendMailMessageWithAttachment($"Improvement Request from {UserPrincipal.Current.DisplayName} for {ConfigUUT.Number}, {ConfigUUT.Description}."); }

        private async void TSMI_System_BarcodeScannerDiscovery_Click(Object sender, EventArgs e) {
            DialogResult dr = MessageBox.Show($"About to clear/erase result box.{Environment.NewLine}{Environment.NewLine}" +
                $"Please Cancel & File/Save results if needed, then re-run Discovery.", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Cancel) return;
            rtfResults.Clear();
            DeviceInformationCollection dic = await DeviceInformation.FindAllAsync(BarcodeScanner.GetDeviceSelector(PosConnectionTypes.Local));
            StringBuilder sb = new StringBuilder($"Discovering Microsoft supported, corded Barcode Scanner(s):{Environment.NewLine}");
            sb.AppendLine($"  - See https://learn.microsoft.com/en-us/windows/uwp/devices-sensors/pos-device-support.");
            sb.AppendLine($"  - Note that only corded Barcode Scanners are discovered; cordless BlueTooth & Wireless scanners are ignored.");
            sb.AppendLine($"  - Modify GlobalConfigurationFile to use a discovered Barcode Scanner.");
            sb.AppendLine($"  - Scanners must be programmed into USB-HID mode to function properly:");
            sb.AppendLine(@"    - See: file:///P:/Test/Engineers/Equipment%20Manuals/TestExecutive/Honeywell%20Voyager%201200g/Honeywell%20Voyager%201200G%20User's%20Guide%20ReadMe.pdf");
            sb.AppendLine($"    - Or:  https://prod-edam.honeywell.com/content/dam/honeywell-edam/sps/ppr/en-us/public/products/barcode-scanners/general-purpose-handheld/1200g/documents/sps-ppr-vg1200-ug.pdf{Environment.NewLine}");
            foreach (DeviceInformation di in dic) {
                sb.AppendLine($"Name: '{di.Name}'.");
                sb.AppendLine($"Kind: '{di.Kind}'.");
                sb.AppendLine($"ID  : '{di.Id}'.{Environment.NewLine}");
            }
            rtfResults.Text = sb.ToString();
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Title = "Save Discovered Corded Barcode Scanner(s)",
                Filter = "Rich Text Format|*.rtf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"Discovered Corded Barcode Scanner(s)",
                DefaultExt = "rtf",
                CreatePrompt = false,
                OverwritePrompt = true
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)  rtfResults.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);
        }
        private void TSMI_System_DiagnosticsSCPI_VISA_Instruments_Click(Object sender, EventArgs e) {
            foreach (KeyValuePair<SCPI_VISA_Instrument.Alias, SCPI_VISA_Instrument> kvp in SVIs) SCPI99.SelfTest(kvp.Value);
            _ = MessageBox.Show("If you didn't receive an InvalidOperationException, SCPI VISA Instruments passed their self-tests.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void TSMI_System_DiagnosticsRelays_Click(Object sender, EventArgs e) { }
        private void TSMI_System_ManualsBarcodeScanner_Click(Object sender, EventArgs e) { OpenFolder(GetFolder("BarcodeScanner")); }
        private void TSMI_System_ManualsInstruments_Click(Object sender, EventArgs e) { OpenFolder(GetFolder("Instruments")); }
        private void TSMI_System_ManualsRelays_Click(Object sender, EventArgs e) { OpenFolder(GetFolder("Relays")); }
        private void TSMI_System_TestExecutiveConfigXML_Click(Object sender, EventArgs e) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Unlike {Assembly.GetEntryAssembly().GetName().Name}.exe.config, {GlobalConfigurationFile} is a global configuration file that applies to all TestExecutor apps on its host PC.{Environment.NewLine}");
            sb.AppendLine("Changing it thus changes behavior for all TestExecutors, so proceed with caution.");
            DialogResult dr = MessageBox.Show(sb.ToString(), $"Warning.", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.OK) OpenApp("Microsoft", "XMLNotepad", GetFile("TestExecutiveConfigXML"));
        }
        private void TSMI_System_About_Click(Object sender, EventArgs e) {
            _ = MessageBox.Show($"{Assembly.GetExecutingAssembly().GetName().Name}, {Assembly.GetExecutingAssembly().GetName().Version}.{Environment.NewLine}{Environment.NewLine}" +
                $"© 2022, Amphenol Borisch Technologies.",
            "About TestExecutive", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TSMI_UUT_AppConfig_Click(Object sender, EventArgs e) {
            StringBuilder sb = new StringBuilder();
            String UUT = Assembly.GetEntryAssembly().GetName().Name;
            sb.AppendLine($"Adapting Doug Gwyn's philosophy here: 'Unix was not designed to stop you from doing stupid things, because that would also stop you from doing clever things.'{Environment.NewLine}");
            sb.AppendLine($"Visual Studio's MS Build copies {UUT}'s 'App.config' file into the {UUT}'s executable folder as file '{UUT}.exe.config'.{Environment.NewLine}");
            sb.AppendLine($"Under normal circumstances, directly editing '{UUT}.exe.config' is highly inadvisable, but for narrow/niche circumstances may prove useful, hence is assisted.{Environment.NewLine}");
            sb.AppendLine($"- Directly editing '{UUT}.exe.config' allows temporary runtime execution changes, but they're overwritten when MS Build is subsequently executed.{Environment.NewLine}");
            sb.AppendLine($"- Changes to '{UUT}.exe.config' aren't incorporated into the source 'App.config' file, therefore permanently lost the next time MS Build is executed.{Environment.NewLine}");
            sb.AppendLine($"- For the niche case when it's useful to temporarily experiment with {UUT}.exe.config's behavior, and a C# compiler and/or");
            sb.AppendLine($"- {UUT} source code are unavailable on the {UUT} tester's PC, directly editing {UUT}.exe.config may prove useful.{Environment.NewLine}");
            sb.AppendLine($"- Be sure to backport any permanently desired {UUT}.exe.config changes to App.config.{Environment.NewLine}");
            sb.AppendLine("- Also be sure to undo any temporary undesired {UUT}.exe.config changes after experimention is completed.");
            DialogResult dr = MessageBox.Show(sb.ToString(), $"Warning.", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.OK) OpenApp("Microsoft", "XMLNotepad", $"{UUT}.exe.config");
        }
        private void TSMI_UUT_Change_Click(Object sender, EventArgs e) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.InitialDirectory = (from xe in XElement.Load(GlobalConfigurationFile).Elements("Folders") select xe.Element("TestExecutorLinks").Value).ElementAt(0);
                ofd.Filter = "Windows Shortcuts|*.lnk";
                ofd.DereferenceLinks = true;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK) {
                    PreApplicationExit();
                    ProcessStartInfo psi = new ProcessStartInfo(ofd.FileName);
                    Process.Start(psi);
                    Thread.Sleep(millisecondsTimeout: 1000);
                    System.Windows.Forms.Application.Exit();
                }
            }
        }
        private void TSMI_UUT_eDocs_Click(Object sender, EventArgs e) { OpenFolder(ConfigUUT.DocumentationFolder); }
        private void TSMI_UUT_ManualsInstruments_Click(Object sender, EventArgs e) { OpenFolder(ConfigUUT.ManualsFolder); }
        private void TSMI_UUT_TestData_P_DriveTDR_Folder_Click(Object sender, EventArgs e) { OpenFolder(ConfigLogger.FilePath); }
        private void TSMI_UUT_TestDataSQL_ReportingAndQuerying_Click(Object sender, EventArgs e) { }
        private void TSMI_UUT_About_Click(Object sender, EventArgs e) {
            _ = MessageBox.Show($"{Assembly.GetEntryAssembly().GetName().Name}, {Assembly.GetEntryAssembly().GetName().Version}.{Environment.NewLine}{Environment.NewLine}" +
                $"© 2022, Amphenol Borisch Technologies.",
            "About TestExecutor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
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
                    MeasurementIDPresent = measurementID;
                    MeasurementPresent = ConfigTest.Measurements[MeasurementIDPresent];
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
                            ErrorMessage(e);
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
            // - Likely occurred because a Measurement failed that had its App.config TestMeasurement CancelOnFail flag set to true.
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

        #region Introspective methods.
        public Boolean AreMethodNamesPriorNext(String prior, String next) { return String.Equals(GetID_MeasurementPrior(), prior) && String.Equals(GetID_MeasurementNext(), next); }

        public Boolean IsGroup(String GroupID) { return String.Equals(ConfigTest.Measurements[MeasurementIDPresent].GroupID, GroupID); }
                 
        public Boolean IsGroup(String GroupID, String Description, String MeasurementIDs, Boolean Selectable, Boolean CancelNotPassed) {
            return 
                String.Equals(ConfigTest.Measurements[MeasurementIDPresent].GroupID, GroupID) && 
                String.Equals(ConfigTest.Groups[GetID_Group()].Description, Description) && 
                String.Equals(ConfigTest.Groups[GetID_Group()].TestMeasurementIDs, MeasurementIDs) && 
                ConfigTest.Groups[GetID_Group()].Selectable == Selectable && 
                ConfigTest.Groups[GetID_Group()].CancelNotPassed == CancelNotPassed;
        }

        public Boolean IsMeasurement(String Description, String IDPrior, String IDNext, String ClassName, Boolean CancelNotPassed, String Arguments) {
            return
                IsMeasurement(Description, ClassName, CancelNotPassed, Arguments) &&
                String.Equals(GetID_MeasurementPrior(), IDPrior) &&
                String.Equals(GetID_MeasurementNext(), IDNext);
        }

        public Boolean IsMeasurement(String Description, String ClassName, Boolean CancelNotPassed, String Arguments) {
            return 
                String.Equals(MeasurementPresent.Description, Description) && 
                String.Equals(MeasurementPresent.ClassName, ClassName) && 
                MeasurementPresent.CancelNotPassed == CancelNotPassed && 
                String.Equals((String)MeasurementPresent.ClassObject.GetType().GetMethod("ArgumentsGet").Invoke(MeasurementPresent.ClassObject, null), Arguments); 
        }

        public Boolean IsOperation(String OperationID) { return String.Equals(ConfigTest.TestElementID, OperationID); }
        
        public Boolean IsOperation(String OperationID, String Description, String Revision, String GroupsIDs) {
                return
                String.Equals(ConfigTest.TestElementID, OperationID) &&
                String.Equals(ConfigTest.TestElementDescription, Description) &&
                String.Equals(ConfigTest.TestElementRevision, Revision) &&
                String.Equals(String.Join(MeasurementAbstract.SA.ToString(), ConfigTest.GroupIDsSequence.ToArray()), GroupsIDs);
        }

        private String GetID_Group() { return ConfigTest.Measurements[MeasurementIDPresent].GroupID; }
        
        private String GetID_MeasurementNext() {
            if (GetIDs_MeasurementSequence() == ConfigTest.TestMeasurementIDsSequence.Count - 1) return NONE;
            return ConfigTest.TestMeasurementIDsSequence[GetIDs_MeasurementSequence() + 1];
        }

        private String GetID_MeasurementPrior() {
            if (GetIDs_MeasurementSequence() == 0) return NONE;
            return ConfigTest.TestMeasurementIDsSequence[GetIDs_MeasurementSequence() - 1];
        }

        private Int32 GetIDs_MeasurementSequence() { return ConfigTest.TestMeasurementIDsSequence.FindIndex(x => x.Equals(MeasurementIDPresent)); }

        public String GetMeasurementNumericArguments(String measurementID) {
            MeasurementNumeric mn = (MeasurementNumeric)Measurement.Get(measurementID).ClassObject;
            return (String)mn.GetType().GetMethod("ArgumentsGet").Invoke(mn, null); 
        }
        #endregion Introspective methods.

        #region Logging methods.
        public void LogCaller([CallerFilePath] String callerFilePath = "", [CallerMemberName] String callerMemberName = "", [CallerLineNumber] Int32 callerLineNumber = 0) {
            LogMessage("Caller File", $"'{callerFilePath}'");
            LogMessage("Caller Member", $"'{callerMemberName}'");
            LogMessage("Caller Line #", $"'{callerLineNumber}'");
        }

        public void LogMessage(String Label, String Message) { MeasurementPresent.Message += $"{Label}".PadLeft(Logger.SPACES_21.Length) + $" : {Message}{Environment.NewLine}"; }

        public void LogMessages(List<(String, String)> Messages) { foreach ((String Label, String Message) in Messages) LogMessage(Label, Message); }
        #endregion Logging methods.
    }
}