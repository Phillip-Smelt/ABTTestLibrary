using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using TestLibrary.Config;
using TestLibrary.Instruments;
using TestLibrary.Logging;
using TestLibrary.TestSupport;
using Microsoft.VisualBasic;
using Serilog;

// TODO: Replace RichTextBox in this TestLibraryForm with a DataGridView, change Logging output from current discrete records to DataGrid rows.
// TODO: Update to .Net 7.0 & C# 11.0 when possible.
// - Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas Instruments TIDP.SAA Fusion Library
//   is compiled to .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & UWP.
// https://www.ti.com/tool/FUSION_USB_ADAPTER_API
// TODO: Update to UWP instead of WinForms when possible.
// - Chose WinForms due to incompatibility of UWP with .Net Framework, and unfamiliarity with WPF.
// NOTE: With deep appreciation for https://learn.microsoft.com/en-us/docs/ & https://stackoverflow.com/!
//
//  References:
//  - https://github.com/Amphenol-Borisch-Technologies/TestLibrary
//  - https://github.com/Amphenol-Borisch-Technologies/TestProgram
//  - https://github.com/Amphenol-Borisch-Technologies/TestLibraryTests
namespace TestLibrary {
    public abstract partial class TestLibraryForm : Form {
        protected ConfigLib configLib;
        protected ConfigTest configTest;
        protected Dictionary<String, Instrument> instruments;
        // NOTE: Above object declarations protected so they can be inhereited & extended if needed.
        private String _appAssemblyVersion;
        private String _libraryAssemblyVersion;
        private Boolean _cancelled;
        private CancellationTokenSource _cancellationTokenSource;

        protected abstract String RunTest(Test test, Dictionary<String, Instrument> instruments, CancellationToken cancellationToken);

        protected TestLibraryForm(Icon icon) {
            this.InitializeComponent();
            this._appAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            this._libraryAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Icon = icon;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio
        }

        private void Form_Load(Object sender, EventArgs e) {
            this.configLib = ConfigLib.Get();
            this.instruments = Instrument.Get();
            InstrumentTasks.SCPI99Test(this.instruments);
            InstrumentTasks.InstrumentResetClear(this.instruments);
            this._cancellationTokenSource = new CancellationTokenSource();
        }

        private void Form_Shown(Object sender, EventArgs e) {
            this.FormReset();
            this.Text = $"{this.configLib.UUT.Number}, {this.configLib.UUT.Description}";
            if (!String.Equals(String.Empty, this.configLib.UUT.DocumentationFolder)) {
                if (Directory.Exists(this.configLib.UUT.DocumentationFolder)) {
                    ProcessStartInfo psi = new ProcessStartInfo {
                        FileName = "explorer.exe",
                        WindowStyle= ProcessWindowStyle.Minimized,
                        Arguments = $"\"{this.configLib.UUT.DocumentationFolder}\""
                    };
                    Process.Start(psi);
                    // Paths with embedded spaces require enclosing double-quotes (").
                    // Even then, simpler 'System.Diagnostics.Process.Start("explorer.exe", path);' invocation fails - must use ProcessStartInfo class.
                    // https://stackoverflow.com/questions/334630/opening-a-folder-in-explorer-and-selecting-a-file
                } else MessageBox.Show($"Path {this.configLib.UUT.DocumentationFolder} invalid.", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.ButtonSelectGroup.Enabled = true;
        }

        private void ButtonSelectGroup_Click(Object sender, EventArgs e) {
            this.configTest = ConfigTest.Get();
            this.Text = $"{this.configLib.UUT.Number}, {this.configLib.UUT.Description}, {this.configTest.Group.ID}";
            this.FormReset();
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStartReset(Enabled: true);
        }

        private void ButtonStart_Clicked(Object sender, EventArgs e) {
            this.configLib.UUT.SerialNumber = Interaction.InputBox(Prompt: "Please enter UUT Serial Number", Title: "Enter Serial Number", DefaultResponse: this.configLib.UUT.SerialNumber);
            if (String.Equals(this.configLib.UUT.SerialNumber, String.Empty)) return;
            this.PreRun();
            this.Run();
            this.PostRun();
        }

        private void ButtonCancel_Clicked(Object sender, EventArgs e) {
            this._cancellationTokenSource.Cancel();
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
            //      - TestLibrary's already implemented/always available & default reactive "Cancel before next Test" technique,
            //        which simply sets this._cancelled Boolean to true, checked at the end of RunTest()'s foreach loop.
            //      - If this._cancelled is true, RunTest()'s foreach loop is broken, causing reactive cancellation
            //        prior to the next Test's execution.
            //      - Note: This doesn't proactively cancel the *currently* executing Test, which runs to completion.
            //  Summary:
            //      - If it's necessary to deterministically cancel a specific Test's execution, Microsoft's
            //        CancellationTokenSource technique *must* be implemented by the Test Developer.
            //      - If it's only necessary to deterministically cancel overall Test Program execution,
            //        TestLibrary's basic "Cancel before next Test" technique is already available without
            //        any Test Developer implemenation needed.
            //      - Note: Some Test's may not be safely cancellable mid-execution.
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
            //        - So, simply directly set test.Measurement's value, then throw a
            //          TestCancellationException if an applied power bus fails.
            //        - This is simulated in T01 in https://github.com/Amphenol-Borisch-Technologies/TestProgram/blob/master/TestProgram.Shared.cs
            //        - Test Developer must set test.Measurement's value for it to be Logged,
            //          else default String.Empty or Double.NaN values are Logged.
        }

        private void ButtonStartReset(Boolean Enabled) {
            if (Enabled) {
                this.ButtonStart.UseVisualStyleBackColor = false;
                this.ButtonStart.BackColor = Color.Green;
            } else {
                this.ButtonStart.BackColor = SystemColors.Control;
                this.ButtonStart.UseVisualStyleBackColor = true;
            }
            this.ButtonStart.Enabled = Enabled;
        }

        private void ButtonCancelReset(Boolean Enabled) {
            if (Enabled) {
                this.ButtonCancel.UseVisualStyleBackColor = false;
                this.ButtonCancel.BackColor = Color.Yellow;
            } else {
                this.ButtonCancel.BackColor = SystemColors.Control;
                this.ButtonCancel.UseVisualStyleBackColor = true;
            }
            this.ButtonCancel.Text = "Cancel";
            if (this._cancellationTokenSource.IsCancellationRequested) {
                this._cancellationTokenSource.Dispose();
                this._cancellationTokenSource = new CancellationTokenSource();
            }
            this._cancelled = false;
            this.ButtonCancel.Enabled = Enabled;
        }

        private void FormReset() {
            this.ButtonSelectGroup.Enabled = false;
            this.ButtonStartReset(Enabled: false);
            this.ButtonCancelReset(Enabled: false);
            this.TextUUTResult.Text = String.Empty;
            this.TextUUTResult.BackColor = Color.White;
            if (this.configTest != null) {
                this.ButtonSaveOutput.Enabled = !this.configTest.Group.Required;
                this.ButtonOpenTestDataFolder.Enabled = (this.configTest.Group.Required && this.configLib.Logger.FileEnabled);
            } else {
                this.ButtonSaveOutput.Enabled = false;
                this.ButtonOpenTestDataFolder.Enabled = false;
            }
            this.ButtonEmergencyStop.Enabled = true;
            this.rtfResults.Text = String.Empty;
        }

        private void ButtonEmergencyStop_Clicked(Object sender, EventArgs e) {
            InstrumentTasks.InstrumentResetClear(this.instruments);
            if (this.ButtonCancel.Enabled) ButtonCancel_Clicked(this, null);
       }

        private void ButtonSaveOutput_Click(Object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog {
                Title = "Save Test Results",
                Filter = "Rich Text Format|*.rtf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"{this.configLib.UUT.Number}_{this.configLib.UUT.SerialNumber}",
                DefaultExt = "rtf",
                CreatePrompt = false,
                OverwritePrompt = true
            };
            DialogResult dr = sfd.ShowDialog();
            if ((dr == DialogResult.OK) && !String.Equals(sfd.FileName, String.Empty)) this.rtfResults.SaveFile(sfd.FileName);
        }

        private void ButtonOpenTestDataFolder_Click(Object sender, EventArgs e) {
            ProcessStartInfo psi = new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"\"{this.configLib.Logger.FilePath}\"" };
            Process.Start(psi);
            // Will fail if this.configLib.Logger.FilePath is invalid.  Don't catch resulting Exception though; this has to be fixed in App.config.
        }

        private void PreRun() {
            this.FormReset();
            foreach (KeyValuePair<String, Test> t in this.configTest.Tests) {
                if (String.Equals(t.Value.ClassName, TestNumerical.ClassName)) t.Value.Measurement = Double.NaN.ToString();
                else t.Value.Measurement = String.Empty;
                t.Value.Result = EventCodes.UNSET;
            }
            this.configLib.UUT.EventCode = EventCodes.UNSET;
            InstrumentTasks.SCPI99Reset(this.instruments);
            LogTasks.Start(this.configLib, this._appAssemblyVersion, this._libraryAssemblyVersion, this.configTest.Group, ref this.rtfResults);
            this.ButtonCancelReset(Enabled: true);
        }

        private void Run() {
            foreach (KeyValuePair<String, Test> t in this.configTest.Tests) {
                try {
                    Application.DoEvents(); // Necessary for ButtonEmergencyStop_Clicked() & ButtonCancel_Clicked().
                    t.Value.Measurement = RunTest(t.Value, this.instruments, this._cancellationTokenSource.Token);
                    t.Value.Result = TestTasks.EvaluateTestResult(t.Value);
                    Application.DoEvents();
                } catch (Exception e) {
                    if (e.GetType() == typeof(TestCancellationException)) {
                        t.Value.Result = EventCodes.CANCEL;
                    } else {
                        InstrumentTasks.InstrumentResetClear(this.instruments);
                        t.Value.Result = EventCodes.ERROR;
                        Log.Error(e.ToString());
                        MessageBox.Show($"Unexpected error.  Details logged for analysis & resolution.{Environment.NewLine}{Environment.NewLine}" +
                            $"If reoccurs, please contact Test Engineering.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                } finally {
                    LogTasks.LogTest(t.Value);
                }
                if (this._cancelled) {
                    t.Value.Result = EventCodes.CANCEL;
                    break;
                }
            }
        }

        private void PostRun() {
            InstrumentTasks.SCPI99Reset(this.instruments);
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStartReset(Enabled: true);
            this.ButtonCancelReset(Enabled: false);
            this.configLib.UUT.EventCode = TestTasks.EvaluateUUTResult(this.configTest);
            this.TextUUTResult.Text = this.configLib.UUT.EventCode;
            this.TextUUTResult.BackColor = EventCodes.GetColor(this.configLib.UUT.EventCode);
            LogTasks.Stop(this.configLib, this.configTest.Group, ref this.rtfResults);
        }
    }
}