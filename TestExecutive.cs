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
using Windows.ApplicationModel.VoiceCommands;

// NOTE:  Recommend using Microsoft's Visual Studio Code to develop/debug TestExecutor based closed source/proprietary projects:
//        - Visual Studio Code is a co$t free, open-source Integrated Development Environment entirely suitable for textual C# development, like TestExecutor.
//          - That is, it's excellent for non-GUI (WinForms/WPF/UWP/WinUI 3) C# development.
//          - VS Code is free for both private & commercial use:
//            - https://code.visualstudio.com/docs/supporting/FAQ
//            - https://code.visualstudio.com/license
// NOTE:  Recommend using Microsoft's Visual Studio Community Edition to develop/debug open sourced TestExecutive:
//        - https://github.com/Amphenol-Borisch-Technologies/TestExecutive/blob/master/LICENSE.txt
//        - "An unlimited number of users within an organization can use Visual Studio Community for the following scenarios:
//           in a classroom learning environment, for academic research, or for contributing to open source projects."
//        - TestExecutor based projects are very likely closed source/proprietary, which are disqualified from using VS Studio Community Edition.
//          - https://visualstudio.microsoft.com/vs/community/
//          - https://visualstudio.microsoft.com/license-terms/vs2022-ga-community/
// NOTE:  - VS Studio Community Edition is more preferable for GUI C# development than VS Code.
//          - If not developing GUI code (WinForms/WPF/UWP/WinUI 3), then VS Code is entirely sufficient & potentially preferable.
// TODO:  Eventually; refactor TestExecutive to Microsoft's C# Coding Conventions, https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions.
// NOTE:  For public methods, will deviate by using PascalCasing for parameters.  Will use recommended camelCasing for internal & private method parameters.
//        - Prefer named arguments for public methods be Capitalized/PascalCased, not uncapitalized/camelCased.
//        - Invoking public methods with named arguments is a superb, self-documenting coding technique, improved by PascalCasing.
// TODO:  Eventually; add documentation per https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments.
// TODO:  Eventually; update to .Net 8.0 & C# 12.0 instead of .Net FrameWork 4.8 & C# 7.3 when possible.
// NOTE:  Used .Net FrameWork 4.8 instead of .Net 8.0 because required Texas instruments' TIDP.SAA Fusion API targets
//        .Net FrameWork 4.5, incompatible with .Net 8.0, C# 12.0 & WinUI 3.
//        - https://www.ti.com/tool/FUSION_USB_ADAPTER_API
// TODO:  Eventually; update to WinUI 3 or WPF instead of WinForms when possible.
// NOTE:  Chose WinForms due to incompatibility of WinUI 3 with .Net Framework, and unfamiliarity with WPF.
// NOTE:  With deep appreciation for https://learn.microsoft.com/en-us/docs/ & https://stackoverflow.com/!
// NOTE:  ABT's Zero Trust, Cloudflare Warp enterprise security solution inhibits GitHub's security, causing below error when sychronizing with
//        TestExecutive's GitHub repository at https://github.com/Amphenol-Borisch-Technologies/TestExecutive:
//             Opening repositories:
//             P:\Test\Engineers\repos\TestExecutor
//             Opening repositories:
//             P:\Test\Engineers\repos\TestExecutor
//             C:\Users\phils\source\repos\TestExecutive
//             Git failed with a fatal error.
//             Git failed with a fatal error.
//             unable to access 'https://github.com/Amphenol-Borisch-Technologies/TestLibrary/': schannel: CertGetCertificateChain trust error CERT_TRUST_IS_PARTIAL_CHAIN
//        - Temporarily disabling Zero Trust by "pausing" it resolves above error.
//        - https://stackoverflow.com/questions/27087483/how-to-resolve-git-pull-fatal-unable-to-access-https-github-com-empty
//        - FYI, synchronizing with TestExecutor's repository doesn't error out, as it doesn't utilize a Git server.

/// <para>
///  References:
///  - https://github.com/Amphenol-Borisch-Technologies/TestExecutive
///  - https://github.com/Amphenol-Borisch-Technologies/TestExecutor
///  </para>
/// <summary>
/// NOTE:  Test Developer is responsible for ensuring Measurements can be both safely & correctly called in sequence defined in App.config:
/// <para>
///        - That is, if Measurements execute sequentially as (M1, M2, M3, M4, M5), Test Developer is responsible for ensuring all equipment is
///          configured safely & correctly between each Measurement step.
///          - If:
///            - M1 is unpowered Shorts & Opens measurements.
///            - M2 is powered voltage measurements.
///            - M3 begins with unpowered operator cable connections/disconnections for In-System Programming.
///          - Then Test Developer must ensure necessary equipment state transitions are implemented so test operator isn't
///            plugging/unplugging a powered UUT in T03.
/// </para>
/// </summary>
/// 
/// <summary>
/// NOTE:  Two types of TestExecutor Cancellations possible, each having two sub-types resulting in 4 altogether:
/// <para>
/// A) Spontaneous Operator Initiated Cancellations:
///      1)  Operator Proactive:
///          - Microsoft's recommended CancellationTokenSource technique, permitting Operator to proactively
///            cancel currently executing Measurement.
///          - Requires TestExecutor implementation by the Test Developer, but is initiated by Operator, so categorized as such.
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

namespace ABT.TestSpace.TestExec {
    public abstract partial class TestExecutive : Form {
        public const String GlobalConfigurationFile = @"C:\Program Files\TestExecutive\TestExecutive.config.xml"; // NOTE:  Update this path if installed into another folder.
        public const String NONE = "NONE";
        public readonly AppConfigLogger ConfigLogger = AppConfigLogger.Get();
        public readonly Dictionary<SCPI_VISA_Instrument.Alias, SCPI_VISA_Instrument> SVIs = null;
        public readonly AppConfigUUT ConfigUUT = AppConfigUUT.Get();
        public AppConfigTest ConfigTest { get; private set; } = null; // Requires form; instantiated by ButtonSelectTests_Click method.
        public CancellationTokenSource CancelTokenSource { get; private set; } = new CancellationTokenSource();
        public String MeasurementIDPresent { get; private set; } = String.Empty;
        public Measurement MeasurementPresent { get; private set; } = null;
        public readonly Boolean Simulate;
        private static readonly String _administratorEMailTo = XElement.Load(GlobalConfigurationFile).Element("Administrators").Element("EMailTo").Value;
        private static readonly String _aministratorEMailCC = XElement.Load(GlobalConfigurationFile).Element("Administrators").Element("EMailCC").Value;
        private readonly String _serialNumberRegEx = null;
        private readonly SerialNumberDialog _serialNumberDialog = null;
        private readonly RegistryKey _serialNumberRegistryKey = null;
        private const String _serialNumberMostRecent = "MostRecent";
        private Boolean _cancelled = false;

        protected TestExecutive(Icon icon) {
            InitializeComponent();
            Icon = icon;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio

            if (String.Equals(ConfigUUT.SerialNumberRegExCustom, "NotApplicable")) _serialNumberRegEx = XElement.Load(GlobalConfigurationFile).Element("SerialNumberRegExDefault").Value;
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

            Simulate = Boolean.Parse(XElement.Load(GlobalConfigurationFile).Element("Simulate").Value) || ConfigUUT.Simulate;
            if (!Simulate) {
                SVIs = SCPI_VISA_Instrument.Get();
                if (ConfigLogger.SerialNumberDialogEnabled) _serialNumberDialog = new SerialNumberDialog(_serialNumberRegEx);
                UE24.Set(C.S.NO); // Relays should be de-energized/re-energized occasionally as preventative maintenance.  Regular exercise is good for relays, as well as people!
                UE24.Set(C.S.NC); // Besides, having 48 relays go "clack-clack" nearly simultaneously sounds awesome...
            }
        }

        #region Form Miscellaneous
        public static void ErrorMessage(String Error) {
            _ = MessageBox.Show(ActiveForm, $"Unexpected error:{Environment.NewLine}{Error}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ErrorMessage(Exception Ex) {
            ErrorMessage($"'{Ex.Message}'{Environment.NewLine}{Environment.NewLine}Will attempt to E-Mail details To {_administratorEMailTo} & CC {_aministratorEMailCC}.{Environment.NewLine}{Environment.NewLine}Please select your Outlook profile if dialog appears.");
            SendAdministratorMailMessage("Exception caught!", Ex);
        }

        private void Form_Shown(Object sender, EventArgs e) { ButtonSelectTests_Click(sender, e); }

        private void FormModeReset() {
            TextResult.Text = String.Empty;
            TextResult.BackColor = Color.White;
            rtfResults.Text = String.Empty;
            StatusClear();
        }

        private void FormModeRun() {
            ButtonCancelReset(enabled: true);
            ButtonSelectTests.Enabled = false;
            ButtonStartReset(enabled: false);
            ButtonEmergencyStop.Enabled = true; // Always enabled.
        }

        private void FormModeWait() {
            ButtonCancelReset(enabled: false);
            ButtonSelectTests.Enabled = true;
            ButtonStartReset(enabled: ConfigTest != null);
            ButtonEmergencyStop.Enabled = true; // Always enabled.
        }

        private String GetFolder(String FolderID) { return XElement.Load(GlobalConfigurationFile).Element("Folders").Element(FolderID).Value; }

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

        public virtual void Initialize() {
            if (!Simulate) {
                SCPI99.Reset(SVIs);
                UE24.Set(C.S.NC);
            }
        }

        public virtual Boolean Initialized() {
            if (!Simulate) { return SCPI99.Are(SVIs, STATE.off) && UE24.Are(C.S.NC); }
            return false;
        }

        private void InvalidPathError(String InvalidPath) { _ = MessageBox.Show(ActiveForm, $"Path {InvalidPath} invalid.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        public static String NotImplementedMessageEnum(Type enumType) { return $"Unimplemented Enum item; switch/case must support all items in enum '{String.Join(",", Enum.GetNames(enumType))}'."; }

        private void OpenApp(String CompanyID, String AppID, String Arguments="") {
            String app = XElement.Load(GlobalConfigurationFile).Element("Apps").Element(CompanyID).Element(AppID).Value;

            if (File.Exists(app)) {
                ProcessStartInfo psi = new ProcessStartInfo {
                    FileName = $"\"{app}\"",
                    WindowStyle = ProcessWindowStyle.Normal,
                    WorkingDirectory = "",
                    Arguments = $"\"{Arguments}\""
                    // Paths with embedded spaces require enclosing double-quotes (").
                    // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
                };
                Process.Start(psi);
            } else InvalidPathError(app);
        }

        private void OpenFolder(String FolderPath) {
            if (Directory.Exists(FolderPath)) {
                ProcessStartInfo psi = new ProcessStartInfo {
                    FileName = "explorer.exe",
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = $"\"{FolderPath}\""
                    // Paths with embedded spaces require enclosing double-quotes (").
                    // Even then, simpler 'System.Diagnostics.Process.Start("explorer.exe", path);' invocation fails - thus using ProcessStartInfo class.
                    // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
                };
                Process.Start(psi);
            } else InvalidPathError(FolderPath);
        }

        private void PreApplicationExit() {
            if (ConfigLogger.SerialNumberDialogEnabled) _serialNumberDialog.Close();
            Initialize();
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

        public static void SendAdministratorMailMessage(String Subject, Exception Ex, String CC="") {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"MachineName           : {Environment.MachineName}");
            sb.AppendLine($"UserPrincipal         : {UserPrincipal.Current.DisplayName}");
            sb.AppendLine($"Exception.ToString()  : {Ex}"); 
            SendAdministratorMailMessage(Subject, Body: sb.ToString(), CC);
        }

        public static void SendAdministratorMailMessage(String Subject, String Body, String CC="") {
            Outlook.MailItem mailItem = GetMailItem();
            mailItem.Subject = Subject;
            mailItem.To = _administratorEMailTo;
            Outlook.Recipient recipient = mailItem.Recipients.Add(_aministratorEMailCC);    recipient.Type = (Int32)Outlook.OlMailRecipientType.olCC;
            if (!String.Equals(CC, String.Empty)) { recipient = mailItem.Recipients.Add(CC); recipient.Type = (Int32)Outlook.OlMailRecipientType.olCC; }
            mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
            mailItem.BodyFormat = Outlook.OlBodyFormat.olFormatPlain;
            mailItem.Body = Body;
            mailItem.Send();
        }
 
        private void SendMailMessageWithAttachment(String subject) {
            Outlook.MailItem mailItem = GetMailItem();
            mailItem.Subject = subject;
            mailItem.To = _administratorEMailTo;
            Outlook.Recipient recipient = mailItem.Recipients.Add(_aministratorEMailCC);    recipient.Type = (Int32)Outlook.OlMailRecipientType.olCC;
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
        #endregion Form Miscellaneous
        
        #region Form Command Buttons
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
            if (enabled) {
                ButtonStart.UseVisualStyleBackColor = false;
                ButtonStart.BackColor = Color.Green;
            } else {
                ButtonStart.BackColor = SystemColors.Control;
                ButtonStart.UseVisualStyleBackColor = true;
            }
            ButtonStart.Enabled = enabled;
        }
        #endregion Form Command Buttons

        #region Form Tool Strip Menu Items
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

        private void TSMI_Apps_MicrochipMPLAB_IPE_Click(Object sender, EventArgs e) { OpenApp("Microchip", "MPLAB_IPE"); }
        private void TSMI_Apps_MicrochipMPLAB_X_IDE_Click(Object sender, EventArgs e) { OpenApp("Microchip", "MPLAB_X_IDE"); }
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
            if (dr == DialogResult.OK) OpenApp("Microsoft", "XMLNotepad", GlobalConfigurationFile);
        }
        private void TSMI_System_About_Click(Object sender, EventArgs e) {
            _ = MessageBox.Show($"{Assembly.GetExecutingAssembly().GetName().Name}, {Assembly.GetExecutingAssembly().GetName().Version}.{Environment.NewLine}{Environment.NewLine}" +
                $"© 2022, Amphenol Borisch Technologies.",
                "About TestExecutive", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TSMI_UUT_AppConfig_Click(Object sender, EventArgs e) {
            StringBuilder sb = new StringBuilder();
            String EA = Assembly.GetEntryAssembly().GetName().Name;
            sb.AppendLine($"Adapting Doug Gwyn's philosophy here: 'Unix was not designed to stop you from doing stupid things, because that would also stop you from doing clever things.'{Environment.NewLine}");
            sb.AppendLine($"Visual Studio's MS Build copies {EA}'s 'App.config' file into the {EA}'s executable folder as file '{EA}.exe.config'.{Environment.NewLine}");
            sb.AppendLine($"Under normal circumstances, directly editing '{EA}.exe.config' is highly inadvisable, but for narrow/niche circumstances may prove useful, hence is assisted.{Environment.NewLine}");
            sb.AppendLine($"- Directly editing '{EA}.exe.config' allows temporary runtime execution changes, but they're overwritten when MS Build is subsequently executed.{Environment.NewLine}");
            sb.AppendLine($"- Changes to '{EA}.exe.config' aren't incorporated into the source 'App.config' file, therefore permanently lost the next time MS Build is executed.{Environment.NewLine}");
            sb.AppendLine($"- For the niche case when it's useful to temporarily experiment with {EA}.exe.config's behavior, and a C# compiler and/or");
            sb.AppendLine($" {EA} source code are unavailable on the {EA} tester's PC, directly editing '{EA}.exe.config' may prove useful.{Environment.NewLine}");
            sb.AppendLine($"- Be sure to backport any permanently desired '{EA}.exe.config' changes to 'App.config'.{Environment.NewLine}");
            sb.AppendLine($"- Also be sure to undo any temporary undesired '{EA}.exe.config' changes after experimention is completed.");
            DialogResult dr = MessageBox.Show(sb.ToString(), $"Warning.", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.OK) OpenApp("Microsoft", "XMLNotepad", $"{EA}.exe.config");
        }
        private void TSMI_UUT_Change_Click(Object sender, EventArgs e) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.InitialDirectory = XElement.Load(GlobalConfigurationFile).Element("Folders").Element("TestExecutorLinks").Value;
                ofd.Filter = "Windows Shortcuts|*.lnk";
                ofd.DereferenceLinks = true;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK) {
                    PreApplicationExit();
                    ProcessStartInfo psi = new ProcessStartInfo(ofd.FileName);
                    Process.Start(psi);
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
        #endregion Form Tool Strip Menu Items

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
                        StatusWrite(ConfigTest.Events.Status());
                        ConfigTest.Measurements[measurementID].Value = await Task.Run(() => MeasurementRun(measurementID));
                        ConfigTest.Measurements[measurementID].Result = MeasurementEvaluate(ConfigTest.Measurements[measurementID]);
                    } catch (Exception e) {
                        Initialize();
                        if (e.ToString().Contains(CancellationException.ClassName)) {
                            ConfigTest.Measurements[measurementID].Result = EventCodes.CANCEL;
                            while (!(e is CancellationException) && (e.InnerException != null)) e = e.InnerException; // No fluff, just stuff.
                            ConfigTest.Measurements[measurementID].Message += $"{Environment.NewLine}{CancellationException.ClassName}:{Environment.NewLine}{e.Message}";
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
            ConfigTest.Events.Update(ConfigUUT.EventCode);
            StatusWrite(ConfigTest.Events.Status());
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
            Logger.LogError($"{Environment.NewLine}Invalid Measurement ID(s) to Result(s):{Environment.NewLine}{invalidTests}");
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
            MessageAppend("Caller File", $"'{callerFilePath}'");
            MessageAppend("Caller Member", $"'{callerMemberName}'");
            MessageAppend("Caller Line #", $"'{callerLineNumber}'");
        }

        public void MessageAppend(String Message, Boolean AppendNewLine = true) { MeasurementPresent.Message += Message + (AppendNewLine ? Environment.NewLine : String.Empty); }
        
        public void MessageAppend(String Label, String Message) { MeasurementPresent.Message += $"{Label}".PadLeft(Logger.SPACES_21.Length) + $" : {Message}{Environment.NewLine}"; }

        public void MessagesAppend(List<(String, String)> Messages) { foreach ((String Label, String Message) in Messages) MessageAppend(Label, Message); }
        #endregion Logging methods.

        #region Status Strip methods.
        public void StatusClear() { StatusWrite(String.Empty); }

        public void StatusWrite(String Message) { Invoke((Action)(() => toolStripStatusLabel.Text = Message)); }
        #endregion Status Strip methods.
    }
}