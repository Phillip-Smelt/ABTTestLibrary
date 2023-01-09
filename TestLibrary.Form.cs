using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using TestLibrary.Config;
using TestLibrary.Instruments;
using TestLibrary.Logging;
using TestLibrary.TestSupport;
using Microsoft.VisualBasic;
using Serilog;

// NOTE: Update to .Net 7.0 & C# 11.0 when possible.
// - Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas Instruments
//   TIDP.SAA Fusion Library compiled to .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & UWP.
// https://www.ti.com/tool/FUSION_USB_ADAPTER_API
// NOTE: Update to UWP instead of WinForms when possible.
// - Chose WinForms due to incompatibility of UWP with .Net Framework, and unfamiliarity with WPF.
// NOTE: With deep appreciation for https://learn.microsoft.com/en-us/docs/ & https://stackoverflow.com/!
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
            InitializeComponent();
            this._appAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            this._libraryAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Icon = icon;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio
        }

        private void Form_Load(Object sender, EventArgs e) {
            this.configLib = ConfigLib.Get();
            this.instruments = Instrument.Get();
            InstrumentTasks.Test(this.instruments);
            this._cancellationTokenSource = new CancellationTokenSource();
        }

        private void Form_Shown(Object sender, EventArgs e) {
            FormReset();
            this.Text = $"{this.configLib.UUT.Number}, {this.configLib.UUT.Description}";
            this.ButtonSelectGroup.Enabled = true;
        }

        private void ButtonSelectGroup_Click(Object sender, EventArgs e) {
            this.configTest = ConfigTest.Get();
            this.Text = $"{this.configLib.UUT.Number}, {this.configLib.UUT.Description}, {this.configTest.Group.ID}";
            FormReset();
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStartReset(Enabled: true);
        }

        private void ButtonStart_Clicked(Object sender, EventArgs e) {
            this.configLib.UUT.SerialNumber = Interaction.InputBox(Prompt: "Please enter UUT Serial Number", Title: "Enter Serial Number", DefaultResponse: this.configLib.UUT.SerialNumber);
            if (String.Equals(this.configLib.UUT.SerialNumber, String.Empty)) return;
            FormReset();
            this.ButtonCancelReset(Enabled: true);
            Run();
        }

        private void ButtonCancel_Clicked(Object sender, EventArgs e) {
            this.ButtonCancel.Text = "Cancelling..."; // Here's to British English spelling!
            this.ButtonCancel.Enabled = false;  this.ButtonCancel.UseVisualStyleBackColor = false; this.ButtonCancel.BackColor = Color.Red;
            this._cancellationTokenSource.Cancel();
            this._cancelled = true;
            // NOTE: Two types of cancellation possible:
            //  1)  Microsoft's recommended CancellationTokenSource technique, which can cancel the
            //      currently executing Test, *if* implemented.
            //      - Implementation is the Test Developer's responsibility.
            //      - Implementation necessary if the *currently* executing Test must be cancellable.
            //  2)  TestLibrary's basic "Cancel before next Test" technique, which simply sets the Boolean
            //      this._cancelled flag to true, checked at the end of RunTest()'s foreach loop.
            //      - If this._cancelled is true, RunTest()'s foreach loop is broken, causing cancellation
            //        prior to the next Test's execution.
            //      - Note this doesn't cancel the *currently* executing Test, which runs to completion.
            //  Summary:
            //      - If it's necessary to deterministically cancel a specific Test's execution, Microsoft's CancellationTokenSource
            //        technique must be implemented by the Test Developer.
            //      - If it's only necessary to deterministically cancel the Program's execution, TestLibrary's basic
            //        "Cancel before next Test" technique is always operational without any Test Development implemenation needed.
            // https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
            // https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation
            // https://learn.microsoft.com/en-us/dotnet/standard/threading/canceling-threads-cooperatively
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
            this.ButtonSaveOutput.Enabled = false;
            this.rtfResults.Text = String.Empty;
        }

        private void ButtonEmergencyStop_Clicked(Object sender, EventArgs e) {
            InstrumentTasks.Reset(this.instruments);
            if (this.ButtonCancel.Enabled) ButtonCancel_Clicked(this, null);
       }

        private void ButtonSaveOutput_Click(Object sender, EventArgs e) {
            // NOTE: Using RichTextBox instead of TextBox control in TestLibraryForm for below reasons:
            // - RichTextBox doesn't have a character limit, whereas TextBox control limited to 64KByte of characters.
            //   Doubt > 64KBytes necessary, but why risk it?
            // - RichTextBox can display rich text, specifically the color coded text of EventCode.CANCEL, EventCode.ERROR, 
            //   EventCode.FAIL, EventCode.PASS & EventCode.UNSET.
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
            if (dr == DialogResult.OK && !String.Equals(sfd.FileName, String.Empty)) this.rtfResults.SaveFile(sfd.FileName);
        }

        private void ButtonOpenTestDataFolder_Click(Object sender, EventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", this.configLib.Logger.FilePath);
        }

        private void Run() {
            foreach (KeyValuePair<String, Test> t in this.configTest.Tests) {
                t.Value.Measurement = String.Empty;
                t.Value.Result = EventCodes.UNSET;
            }
            this.configLib.UUT.EventCode = EventCodes.UNSET;
            InstrumentTasks.Reset(this.instruments);
            LogTasks.Start(this.configLib, this._appAssemblyVersion, this._libraryAssemblyVersion, this.configTest.Group, ref this.rtfResults);
            foreach (KeyValuePair<String, Test> t in this.configTest.Tests) {
                try {
                    t.Value.Measurement = RunTest(t.Value, this.instruments, this._cancellationTokenSource.Token);
                    t.Value.Result = TestTasks.EvaluateTestResult(t.Value);
                } catch (Exception e) {
                    if ((e.GetType() == typeof(TestCancelException)) || (e.InnerException.GetType() == typeof(TestCancelException))) {
                        t.Value.Result = EventCodes.CANCEL;
                    } else {
                        InstrumentTasks.Reset(this.instruments);
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
            PostRun();
        }

        private void PostRun() {
            InstrumentTasks.Reset(this.instruments);
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStartReset(Enabled: true);
            this.ButtonCancelReset(Enabled: false);
            this.configLib.UUT.EventCode = TestTasks.EvaluateUUTResult(this.configTest);
            this.TextUUTResult.Text = this.configLib.UUT.EventCode;
            this.TextUUTResult.BackColor = EventCodes.GetColor(this.configLib.UUT.EventCode);
            if (this.configTest.Group.Required && String.Equals(this.configLib.UUT.EventCode, EventCodes.PASS)) this.ButtonSaveOutput.Enabled = false;
            // Disallow saving output if this was a Required Group & UUT passed, because, why bother?  UUT passed & saved test data attesting such; take the win & $hip it.
            else this.ButtonSaveOutput.Enabled = true;
            LogTasks.Stop(this.configLib, this.configTest.Group);
        }
    }
}