using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using TestLibrary.AppConfig;
using TestLibrary.SCPI_VISA;
using TestLibrary.Logging;
using TestLibrary.Utility;
using TestLibrary.Switching;

// TODO: Replace RichTextBox in this TestExecutive with a DataGridView, change Logging output from current discrete records to DataGrid rows.
// TODO: Update to .Net 7.0 & C# 11.0 instead of .Net FrameWork 4.8 & C# 7.0 when possible.
// NOTE: Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas instruments TIDP.SAA Fusion Library is compiled to .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & UWP.
// https://www.ti.com/tool/FUSION_USB_ADAPTER_API
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
        public ConfigLib ConfigLib;
        public ConfigTest ConfigTest;
        public Dictionary<Instrument.IDs, Instrument> Instruments;
        public CancellationTokenSource CancelTokenSource;
        private readonly String _appAssemblyVersion;
        private readonly String _libraryAssemblyVersion;
        private Boolean _cancelled;

        protected abstract Task<String> RunTestAsync(String testID);

        protected TestExecutive(Icon icon) {
            this.InitializeComponent();
            this._appAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            this._libraryAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Icon = icon;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio
        }

        private void Form_Load(Object sender, EventArgs e) {
            this.ConfigLib = ConfigLib.Get();
            this.Instruments = Instrument.Get();
            USB_ERB24.Reset(USB_ERB24.ERB24s);
            this.CancelTokenSource = new CancellationTokenSource();
        }

        private void Form_Shown(Object sender, EventArgs e) {
            this.FormReset();
            this.Text = $"{this.ConfigLib.UUT.Number}, {this.ConfigLib.UUT.Description}";
            if (!String.Equals(String.Empty, this.ConfigLib.UUT.DocumentationFolder)) {
                if (Directory.Exists(this.ConfigLib.UUT.DocumentationFolder)) {
                    ProcessStartInfo psi = new ProcessStartInfo {
                        FileName = "explorer.exe",
                        WindowStyle= ProcessWindowStyle.Minimized,
                        Arguments = $"\"{this.ConfigLib.UUT.DocumentationFolder}\""
                    };
                    Process.Start(psi);
                    // Paths with embedded spaces require enclosing double-quotes (").
                    // Even then, simpler 'System.Diagnostics.Process.Start("explorer.exe", path);' invocation fails - must use ProcessStartInfo class.
                    // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
                } else MessageBox.Show(Form.ActiveForm, $"Path {this.ConfigLib.UUT.DocumentationFolder} invalid.", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.ButtonSelectGroup.Enabled = true;
        }

        private void ButtonSelectGroup_Click(Object sender, EventArgs e) {
            this.ConfigTest = ConfigTest.Get();
            this.Text = $"{this.ConfigLib.UUT.Number}, {this.ConfigLib.UUT.Description}, {this.ConfigTest.Group.ID}";
            this.FormReset();
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStartReset(enabled: true);
        }

        private async void ButtonStart_Clicked(Object sender, EventArgs e) {
            this.ConfigLib.UUT.SerialNumber = Interaction.InputBox(Prompt: "Please enter UUT Serial Number", Title: "Enter Serial Number", DefaultResponse: this.ConfigLib.UUT.SerialNumber);
            if (String.Equals(this.ConfigLib.UUT.SerialNumber, String.Empty)) return;
            await this.RunAsync();
        }

        private void ButtonCancel_Clicked(Object sender, EventArgs e) {
            this.CancelTokenSource.Cancel();
            this._cancelled = true;
            this.ButtonCancel.Text = "Cancelling..."; // Here's to British English spelling!
            this.ButtonCancel.Enabled = false;  this.ButtonCancel.UseVisualStyleBackColor = false; this.ButtonCancel.BackColor = Color.Red;
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
                this.ButtonOpenTestDataFolder.Enabled = (this.ConfigTest.Group.Required && this.ConfigLib.Logger.FileEnabled);
            } else {
                this.ButtonSaveOutput.Enabled = false;
                this.ButtonOpenTestDataFolder.Enabled = false;
            }
            this.ButtonEmergencyStop.Enabled = true;
            this.rtfResults.Text = String.Empty;
        }

        private void ButtonEmergencyStop_Clicked(Object sender, EventArgs e) {
            Instrument.SCPI99_Reset(this.Instruments);
            USB_ERB24.Reset(USB_ERB24.ERB24s);
            if (this.ButtonCancel.Enabled) ButtonCancel_Clicked(this, null);
       }

        private void ButtonSaveOutput_Click(Object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Title = "Save Test Results",
                Filter = "Rich Text Format|*.rtf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"{this.ConfigLib.UUT.Number}_{this.ConfigTest.Group.ID}_{this.ConfigLib.UUT.SerialNumber}",
                DefaultExt = "rtf",
                CreatePrompt = false,
                OverwritePrompt = true
            };
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if ((dialogResult == DialogResult.OK) && !String.Equals(saveFileDialog.FileName, String.Empty)) this.rtfResults.SaveFile(saveFileDialog.FileName);
        }

        private void ButtonOpenTestDataFolder_Click(Object sender, EventArgs e) {
            ProcessStartInfo psi = new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"\"{this.ConfigLib.Logger.FilePath}\"" };
            Process.Start(psi);
            // Will fail if this.ConfigLib.Logger.FilePath is invalid.  Don't catch resulting Exception though; this has to be fixed in App.config.
        }

        private void PreRun() {
            this.FormReset();
            foreach (KeyValuePair<String, Test> test in this.ConfigTest.Tests) {
                if (String.Equals(test.Value.ClassName, TestNumerical.ClassName)) test.Value.Measurement = Double.NaN.ToString();
                else test.Value.Measurement = String.Empty;
                test.Value.Result = EventCodes.UNSET;
            }
            this.ConfigLib.UUT.EventCode = EventCodes.UNSET;
            Instrument.SCPI99_Reset(this.Instruments);
            USB_ERB24.Reset(USB_ERB24.ERB24s);
            LogTasks.Start(this.ConfigLib, this.ConfigTest, this._appAssemblyVersion, this._libraryAssemblyVersion, this.ConfigTest.Group, ref this.rtfResults);
            this.ButtonCancelReset(enabled: true);
        }

        private async Task RunAsync() {
            PreRun();
            foreach (KeyValuePair<String, Test> test in this.ConfigTest.Tests) {
                try {
                    test.Value.Measurement = await Task.Run(() => RunTestAsync(test.Value.ID));
                    test.Value.Result = TestTasks.EvaluateTestResult(test.Value);
                } catch (Exception e) {
                    if (e.ToString().Contains(TestCancellationException.ClassName)) {
                        test.Value.Result = EventCodes.CANCEL;
                        while (!(e is TestCancellationException) && (e.InnerException != null)) e = e.InnerException;
                        if ((e is TestCancellationException) && !String.IsNullOrEmpty(e.Message)) test.Value.Measurement = e.Message;
                    } else {
                        StopRun(test, e.ToString());
                    }
                    break;
                } finally {
                    LogTasks.LogTest(test.Value);
                }
                if (this._cancelled) {
                    test.Value.Result = EventCodes.CANCEL;
                    break;
                }
            }
            PostRun();
        }

        private void StopRun(KeyValuePair<String, Test> test, String exceptionString) {
            Instrument.SCPI99_Reset(this.Instruments);
            USB_ERB24.Reset(USB_ERB24.ERB24s);
            test.Value.Result = EventCodes.ERROR;
            TestTasks.UnexpectedErrorHandler(exceptionString.ToString());
        }

        private void PostRun() {
            Instrument.SCPI99_Reset(this.Instruments);
            USB_ERB24.Reset(USB_ERB24.ERB24s);
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStartReset(enabled: true);
            this.ButtonCancelReset(enabled: false);
            this.ConfigLib.UUT.EventCode = TestTasks.EvaluateUUTResult(this.ConfigTest);
            this.TextUUTResult.Text = this.ConfigLib.UUT.EventCode;
            this.TextUUTResult.BackColor = EventCodes.GetColor(this.ConfigLib.UUT.EventCode);
            LogTasks.Stop(this.ConfigLib, this.ConfigTest.Group, ref this.rtfResults);
        }
    }
}