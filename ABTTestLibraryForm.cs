using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ABTTestLibrary.AppConfig;
using ABTTestLibrary.Instruments;
using ABTTestLibrary.Logging;
using ABTTestLibrary.TestSupport;
using Microsoft.VisualBasic;
using Serilog;

// NOTE: ABTTestLibrary - Update to .Net 7.0 & C# 11.0 when possible.
// - Used .Net FrameWork 4.8 instead of .Net 7.0 because required Texas Instruments
//   TIDP.SAA Fusion Library compiled to .Net FrameWork 2.0, incompatible with .Net 7.0, C# 11.0 & UWP.
// https://www.ti.com/tool/FUSION_USB_ADAPTER_API
// NOTE: ABTTestLibrary - Update to UWP instead of WinForms when possible.
// - Chose WinForms due to incompatibility of UWP with .Net Framework, and unfamiliarity with WPF.
// NOTE: With deep gratitude to https://learn.microsoft.com/en-us/docs/ & https://stackoverflow.com/!
namespace ABTTestLibrary {
    public abstract partial class ABTTestLibraryForm : Form {
        public Config Config;
        public Dictionary<String, Instrument> Instruments;
        public String CurrentTestKey;

        public ABTTestLibraryForm() { InitializeComponent(); }

        public String RunTest(Test test) {
            // https://stackoverflow.com/questions/540066/calling-a-function-from-a-string-in-c-sharp
            Type type = this.GetType();
            MethodInfo methodInfo = type.GetMethod(test.ID, BindingFlags.Instance | BindingFlags.NonPublic);
            return (String)methodInfo.Invoke(this, new object[] { test });
        }

        private void Form_Shown(Object sender, EventArgs e) {
            Instruments = Instrument.Get();
            InstrumentTasks.Test(Instruments);
            this.buttonSelectGroup.Enabled = true;
            this.buttonStart.Enabled = false;
            this.buttonStop.Enabled = false;
            this.buttonSaveOutput.Enabled = false;
            this.buttonOpenTestDataFolder.Enabled = false;
            this.rtfResults.Text = String.Empty;
            this.textUUTResult.Text = String.Empty;
            this.textUUTResult.BackColor = Color.White;
        }

        private void buttonSelectGroup_Click(Object sender, EventArgs e) {
            Config = Config.Get();
            this.buttonOpenTestDataFolder.Enabled=true;
            PreRun();
        }

        private void buttonStart_Clicked(Object sender, EventArgs e) {
            Run();
        }

        private void buttonStop_Clicked(Object sender, EventArgs e) {
            Config.Tests[this.CurrentTestKey].Result = EventCodes.ABORT;
            PostRun();
        }

        private void buttonSaveOutput_Click(Object sender, EventArgs e) {
            // NOTE: ABTTestLibrary - Using RichTextBox instead of TextBox control in ABTTestLibraryForm for below reasons:
            // - RichTextBox doesn't have a character limit, whereas TextBox control limited to 64KByte of characters.
            //   Doubt > 64KBytes is needed, but why risk it?
            // - RichTextBox can display rich text, particularly the color coded text of EventCode.ABORT, EventCode.ERROR, 
            //   EventCode.FAIL, EventCode.PASS & EventCode.UNSET.
            SaveFileDialog sfd = new SaveFileDialog {
                Title = "Save Test Results",
                Filter = "Rich Text Format|*.rtf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = $"{Config.UUT.Number}_{Config.UUT.SerialNumber}",
                DefaultExt = "rtf",
                CreatePrompt = false,
                OverwritePrompt = true
            };
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK && sfd.FileName != String.Empty) this.rtfResults.SaveFile(sfd.FileName);
        }

        private void buttonOpenTestDataFolder_Click(Object sender, EventArgs e) {
            System.Diagnostics.Process.Start("explorer.exe", this.Config.Logger.FilePath);
        }

        public void PreRun() {
            this.buttonSelectGroup.Enabled = true;
            this.buttonStart.Enabled = true;
            this.buttonStop.Enabled = false;
            this.buttonSaveOutput.Enabled = false;
            this.rtfResults.Text = String.Empty;
            this.textUUTResult.Text = String.Empty;
            this.textUUTResult.BackColor = Color.White;
            this.CurrentTestKey = String.Empty;
            this.Text = $"{Config.UUT.Number}, {Config.UUT.Description}, {Config.Group.ID}";
        }

        public void Run() {
            Config.UUT.SerialNumber = Interaction.InputBox(Prompt: "Please enter UUT Serial Number", Title: "Enter Serial Number", DefaultResponse: Config.UUT.SerialNumber);
            if (Config.UUT.SerialNumber == String.Empty) return;
            InstrumentTasks.Reset(Instruments);
            this.buttonSelectGroup.Enabled = false;
            this.buttonStart.Enabled = false;
            this.buttonStop.Enabled = true;
            this.buttonSaveOutput.Enabled = false;
            this.rtfResults.Text = String.Empty;
            this.textUUTResult.Text = String.Empty;
            this.textUUTResult.BackColor = Color.White;
            foreach (KeyValuePair<String, Test> t in Config.Tests) {
                t.Value.Measurement = String.Empty;
                t.Value.Result = EventCodes.UNSET;
            }
            Config.UUT.EventCode = EventCodes.UNSET;
            LogTasks.Start(Config, ref this.rtfResults);

            foreach (KeyValuePair<String, Test> t in Config.Tests) {
                try {
                    this.CurrentTestKey = t.Key;
                    t.Value.Measurement = RunTest(t.Value);
                } catch (Exception e) {
                    InstrumentTasks.Reset(Instruments);
                    if (e.GetType() == typeof(ABTTestLibraryException)) {
                        t.Value.Result = EventCodes.ABORT;
                        Log.Warning(e.ToString());
                    } else {
                        t.Value.Result = EventCodes.ERROR;
                        Log.Error(e.ToString());
                    }
                    break;
                }
                TestTasks.EvaluateTestResult(t.Value, out String eventCode);
                t.Value.Result = eventCode;
                LogTasks.LogTest(t.Value);
            }
            PostRun();
        }

        public void PostRun() {
            InstrumentTasks.Reset(Instruments);
            Config.UUT.EventCode = TestTasks.EvaluateUUTResult(Config);
            this.textUUTResult.Text = Config.UUT.EventCode;
            this.textUUTResult.BackColor = EventCodes.GetColor(Config.UUT.EventCode);
            this.CurrentTestKey = String.Empty;
            LogTasks.Stop(Config);
            if (Config.App.TestEventsEnabled) LogTasks.TestEvents(Config);
            this.buttonSelectGroup.Enabled = true;
            this.buttonStart.Enabled = true;
            this.buttonStop.Enabled = false;
            if (Config.Group.Required && Config.UUT.EventCode == EventCodes.PASS) this.buttonSaveOutput.Enabled = false;
            // Disallow saving output if this was a Required Group & UUT passed, because, why bother?  UUT passed & saved test data attesting such; take the win & $hip it.  
            else this.buttonSaveOutput.Enabled = true;
        }
    }
}