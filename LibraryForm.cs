using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ABTTestLibrary.Config;
using ABTTestLibrary.Instruments;
using ABTTestLibrary.Logging;
using ABTTestLibrary.TestSupport;
using Microsoft.VisualBasic;
using Serilog;

// NOTE: Update to .Net 7.0 & C# 11.0 when possible.
// - Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas Instruments
//   TIDP.SAA Fusion Library compiled to .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & UWP.
// https://www.ti.com/tool/FUSION_USB_ADAPTER_API
// NOTE: Update to UWP instead of WinForms when possible.
// - Chose WinForms due to incompatibility of UWP with .Net Framework, and unfamiliarity with WPF.
// NOTE: With deep appreciation for https://learn.microsoft.com/en-us/docs/ & https://stackoverflow.com/!
// TODO: Emergency Stop button with image:
// https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-load-a-picture-using-the-designer-windows-forms?view=netframeworkdesktop-4.8
// https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-add-a-picture-to-a-control?view=netdesktop-6.0
namespace ABTTestLibrary {
    public abstract partial class LibraryForm : Form {
        protected ConfigLib configLib;
        protected ConfigTest configTest;
        protected Dictionary<String, Instrument> instruments;
        private String _currentTestKey;
        private String _appAssemblyVersion;
        private String _libraryAssemblyVersion;
        private Boolean _stopped;

        protected LibraryForm(Icon icon) {
            InitializeComponent();
            this._appAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            this._libraryAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Icon = icon;
            // https://stackoverflow.com/questions/40933304/how-to-create-an-icon-for-visual-studio-with-just-mspaint-and-visual-studio
        }

        public void StopDisable() {
            this.ButtonStop.Enabled = false;
            this._stopped = false;
            // Method StopDisable() permits client Test methods to disable ButtonStop during method Run().
            // Prevents test operators from Stopping Test methods mid-execution when doing so could have
            // negative consequences.
            // StopDisable() is only intended to be invoked by client Test methods during Run();
            // ButtonStop's state is controlled directly by all other methods.
        }

        public void StopEnable() {
            this.ButtonStop.Enabled = true;
            this._stopped = false;
            // Method StopEnable() permits client Test methods to enable ButtonStop during method Run().
            // Permits test operators to Stop Test methods mid-execution when doing so won't have
            // negative consequences.
            // StopEnable() is only intended to be invoked by client Test methods during Run();
            // ButtonStop's state is controlled directly by all other methods.
        }

        protected abstract String RunTest(Test test, Dictionary<String, Instrument> instruments);

        private void Form_Shown(Object sender, EventArgs e) {
            this.ButtonStop.Enabled = false;
            this._stopped = false;
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStart.Enabled = false;
            this.ButtonSaveOutput.Enabled = false;
            this.ButtonOpenTestDataFolder.Enabled = false;
            this.rtfResults.Text = String.Empty;
            this.TextUUTResult.Text = String.Empty;
            this.TextUUTResult.BackColor = Color.White;
            this.configLib = ConfigLib.Get();
            this.instruments = Instrument.Get();
            InstrumentTasks.Test(this.instruments);
        }

        private void ButtonSelectGroup_Click(Object sender, EventArgs e) {
            this.configTest = ConfigTest.Get();
            this.ButtonOpenTestDataFolder.Enabled=true;
            PreRun();
        }

        private void ButtonStart_Clicked(Object sender, EventArgs e) {
            Run();
        }

        private void ButtonStop_Clicked(Object sender, EventArgs e) {
            this._stopped = true;
        // TODO: Improve Stop function.
        // https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
        // https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation
        // https://learn.microsoft.com/en-us/dotnet/standard/threading/canceling-threads-cooperatively
        }

        private void ButtonSaveOutput_Click(Object sender, EventArgs e) {
            // NOTE: Using RichTextBox instead of TextBox control in ABTTestLibraryForm for below reasons:
            // - RichTextBox doesn't have a character limit, whereas TextBox control limited to 64KByte of characters.
            //   Doubt > 64KBytes necessary, but why risk it?
            // - RichTextBox can display rich text, specifically the color coded text of EventCode.ABORT, EventCode.ERROR, 
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

        private void PreRun() {
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStop.Enabled = false;
            this._stopped= false;
            this.ButtonStart.Enabled = true;
            this.ButtonSaveOutput.Enabled = false;
            this.rtfResults.Text = String.Empty;
            this.TextUUTResult.Text = String.Empty;
            this.TextUUTResult.BackColor = Color.White;
            this._currentTestKey = String.Empty;
            this.Text = $"{this.configLib.UUT.Number}, {this.configLib.UUT.Description}, {this.configTest.Group.ID}";
        }

        private void Run() {
            this.configLib.UUT.SerialNumber = Interaction.InputBox(Prompt: "Please enter UUT Serial Number", Title: "Enter Serial Number", DefaultResponse: this.configLib.UUT.SerialNumber);
            if (String.Equals(this.configLib.UUT.SerialNumber, String.Empty)) return;
            this.ButtonSelectGroup.Enabled = false;
            this.ButtonStart.Enabled = false;
            this.ButtonStop.Enabled = true;
            this._stopped = false;
            this.ButtonSaveOutput.Enabled = false;
            this.rtfResults.Text = String.Empty;
            this.TextUUTResult.Text = String.Empty;
            this.TextUUTResult.BackColor = Color.White;
            foreach (KeyValuePair<String, Test> t in this.configTest.Tests) {
                t.Value.Measurement = String.Empty;
                t.Value.Result = EventCodes.UNSET;
            }
            this.configLib.UUT.EventCode = EventCodes.UNSET;
            InstrumentTasks.Reset(this.instruments);
            LogTasks.Start(this.configLib, this._appAssemblyVersion, this._libraryAssemblyVersion, this.configTest.Group, ref this.rtfResults);
            foreach (KeyValuePair<String, Test> t in this.configTest.Tests) {
                this._currentTestKey = t.Key;
                try {
                    t.Value.Measurement = RunTest(t.Value, this.instruments);
                    t.Value.Result = TestTasks.EvaluateTestResult(t.Value);
                } catch (Exception e) {
                    InstrumentTasks.Reset(this.instruments);
                    if (e.GetType() == typeof(TestAbortException)) t.Value.Result = EventCodes.ABORT;
                    else {
                        t.Value.Result = EventCodes.ERROR;
                        Log.Error(e.ToString());
                        MessageBox.Show($"Unexpected error.  Details logged for analysis & resolution.{Environment.NewLine}{Environment.NewLine}" +
                            $"If reoccurs, please contact Test Engineering.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                } finally {
                    LogTasks.LogTest(t.Value);
                }
                if (this._stopped) {
                    InstrumentTasks.Reset(this.instruments);
                    t.Value.Result = EventCodes.ABORT;
                    break;
                }
            }
            PostRun();
        }

        private void PostRun() {
            InstrumentTasks.Reset(this.instruments);
            this.configLib.UUT.EventCode = TestTasks.EvaluateUUTResult(this.configTest);
            this.TextUUTResult.Text = this.configLib.UUT.EventCode;
            this.TextUUTResult.BackColor = EventCodes.GetColor(this.configLib.UUT.EventCode);
            this._currentTestKey = String.Empty;
            LogTasks.Stop(this.configLib, this.configTest.Group);
            if (this.configLib.Logger.TestEventsEnabled) LogTasks.TestEvents(this.configLib.UUT);
            this.ButtonSelectGroup.Enabled = true;
            this.ButtonStop.Enabled = false;
            this._stopped = false;
            this.ButtonStart.Enabled = true;
            if (this.configTest.Group.Required && String.Equals(this.configLib.UUT.EventCode, EventCodes.PASS)) this.ButtonSaveOutput.Enabled = false;
            // Disallow saving output if this was a Required Group & UUT passed, because, why bother?
            // UUT passed & saved test data attesting such; take the win & $hip it.
            else this.ButtonSaveOutput.Enabled = true;
        }
    }
}