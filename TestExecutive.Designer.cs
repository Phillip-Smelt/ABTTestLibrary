using System.Windows.Forms;

namespace ABT.TestSpace.TestExec {
    public abstract partial class TestExecutive : Form {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestExecutive));
            this.ButtonStart = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.TextTest = new System.Windows.Forms.TextBox();
            this.LabelTestEvent = new System.Windows.Forms.Label();
            this.rtfResults = new System.Windows.Forms.RichTextBox();
            this.ButtonSelectTests = new System.Windows.Forms.Button();
            this.ButtonEmergencyStop = new System.Windows.Forms.Button();
            this.MS = new System.Windows.Forms.MenuStrip();
            this.TSMI_File = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_Keysight = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_KeysightBenchVue = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_KeysightCommandExpert = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_KeysightConnectionExpert = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_MeasurementComputing = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_MeasurementComputingInstaCal = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_Microchip = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_MicrohipMPLAB_IPE = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_MicrohipMPLAB_X_IDE = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_Microsoft = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_MicrosoftSQLServerManagementStudio = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_MicrosoftVisualStudio = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_MicrosoftVisualStudioCode = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_MicrosoftXML_Notepad = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_TexasInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Feedback = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Feedback_Compliments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Feedback_ComplimentsPraiseεPlaudits = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Feedback_ComplimentsMoney = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Feedback_Critiques = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Feedback_CritiquesBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Feedback_CritiquesImprovementRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_BarcodeScannerDiscovery = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_Diagnostics = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_DiagnosticsSCPI_VISA_Instruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_DiagnosticsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_Manuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_ManualsBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_ManualsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_ManualsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_TestExecutiveConfigXML = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_System_About = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_AppConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_eDocs = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_ResetStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_Manuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_ManualsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestData = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestDataP_DriveTDR_Folder = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_UUT_About = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusTestsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusCustomLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusModeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MS.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonStart.BackColor = System.Drawing.Color.Green;
            this.ButtonStart.Location = new System.Drawing.Point(152, 546);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(88, 52);
            this.ButtonStart.TabIndex = 1;
            this.ButtonStart.Text = "&Start";
            this.ButtonStart.UseVisualStyleBackColor = false;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Clicked);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonCancel.BackColor = System.Drawing.Color.Yellow;
            this.ButtonCancel.Location = new System.Drawing.Point(287, 544);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(88, 52);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = false;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Clicked);
            // 
            // TextTest
            // 
            this.TextTest.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TextTest.Location = new System.Drawing.Point(493, 567);
            this.TextTest.Name = "TextTest";
            this.TextTest.ReadOnly = true;
            this.TextTest.Size = new System.Drawing.Size(61, 20);
            this.TextTest.TabIndex = 9;
            this.TextTest.TabStop = false;
            this.TextTest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelTestEvent
            // 
            this.LabelTestEvent.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LabelTestEvent.Location = new System.Drawing.Point(493, 544);
            this.LabelTestEvent.Margin = new System.Windows.Forms.Padding(3);
            this.LabelTestEvent.Name = "LabelTestEvent";
            this.LabelTestEvent.Size = new System.Drawing.Size(60, 13);
            this.LabelTestEvent.TabIndex = 8;
            this.LabelTestEvent.Text = "Test Event";
            this.LabelTestEvent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelTestEvent.UseWaitCursor = true;
            // 
            // rtfResults
            // 
            this.rtfResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfResults.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfResults.Location = new System.Drawing.Point(23, 21);
            this.rtfResults.Name = "rtfResults";
            this.rtfResults.ReadOnly = true;
            this.rtfResults.Size = new System.Drawing.Size(1001, 500);
            this.rtfResults.TabIndex = 7;
            this.rtfResults.TabStop = false;
            this.rtfResults.Text = "";
            // 
            // ButtonSelectTests
            // 
            this.ButtonSelectTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonSelectTests.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonSelectTests.Location = new System.Drawing.Point(23, 547);
            this.ButtonSelectTests.Name = "ButtonSelectTests";
            this.ButtonSelectTests.Size = new System.Drawing.Size(88, 47);
            this.ButtonSelectTests.TabIndex = 0;
            this.ButtonSelectTests.Text = "Select &Tests";
            this.ButtonSelectTests.UseVisualStyleBackColor = true;
            this.ButtonSelectTests.Click += new System.EventHandler(this.ButtonSelectTests_Click);
            // 
            // ButtonEmergencyStop
            // 
            this.ButtonEmergencyStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonEmergencyStop.Image = global::ABT.TestSpace.Properties.Resources.EmergencyStop;
            this.ButtonEmergencyStop.Location = new System.Drawing.Point(940, 526);
            this.ButtonEmergencyStop.Name = "ButtonEmergencyStop";
            this.ButtonEmergencyStop.Size = new System.Drawing.Size(82, 83);
            this.ButtonEmergencyStop.TabIndex = 5;
            this.ButtonEmergencyStop.Text = "&Emergency Stop";
            this.ButtonEmergencyStop.UseVisualStyleBackColor = true;
            this.ButtonEmergencyStop.Click += new System.EventHandler(this.ButtonEmergencyStop_Clicked);
            // 
            // MS
            // 
            this.MS.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_File,
            this.TSMI_Apps,
            this.TSMI_Feedback,
            this.TSMI_System,
            this.TSMI_UUT});
            this.MS.Location = new System.Drawing.Point(0, 0);
            this.MS.Name = "MS";
            this.MS.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.MS.Size = new System.Drawing.Size(1045, 24);
            this.MS.TabIndex = 6;
            this.MS.TabStop = true;
            // 
            // TSMI_File
            // 
            this.TSMI_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_File_Save});
            this.TSMI_File.Name = "TSMI_File";
            this.TSMI_File.Size = new System.Drawing.Size(37, 20);
            this.TSMI_File.Text = "&File";
            // 
            // TSMI_File_Save
            // 
            this.TSMI_File_Save.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_File_Save.Image")));
            this.TSMI_File_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_File_Save.Name = "TSMI_File_Save";
            this.TSMI_File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.TSMI_File_Save.Size = new System.Drawing.Size(138, 22);
            this.TSMI_File_Save.Text = "&Save";
            this.TSMI_File_Save.ToolTipText = "Save UUT results.";
            this.TSMI_File_Save.Click += new System.EventHandler(this.TSMI_File_Save_Click);
            // 
            // TSMI_Apps
            // 
            this.TSMI_Apps.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Apps_Keysight,
            this.TSMI_Apps_MeasurementComputing,
            this.TSMI_Apps_Microchip,
            this.TSMI_Apps_Microsoft,
            this.TSMI_Apps_TexasInstruments});
            this.TSMI_Apps.Name = "TSMI_Apps";
            this.TSMI_Apps.Size = new System.Drawing.Size(49, 20);
            this.TSMI_Apps.Text = " &Apps";
            // 
            // TSMI_Apps_Keysight
            // 
            this.TSMI_Apps_Keysight.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Apps_KeysightBenchVue,
            this.TSMI_Apps_KeysightCommandExpert,
            this.TSMI_Apps_KeysightConnectionExpert});
            this.TSMI_Apps_Keysight.Name = "TSMI_Apps_Keysight";
            this.TSMI_Apps_Keysight.Size = new System.Drawing.Size(211, 22);
            this.TSMI_Apps_Keysight.Text = "&Keysight";
            // 
            // TSMI_Apps_KeysightBenchVue
            // 
            this.TSMI_Apps_KeysightBenchVue.Name = "TSMI_Apps_KeysightBenchVue";
            this.TSMI_Apps_KeysightBenchVue.Size = new System.Drawing.Size(172, 22);
            this.TSMI_Apps_KeysightBenchVue.Text = "&BenchVue";
            this.TSMI_Apps_KeysightBenchVue.ToolTipText = "Control Keysight Instruments via soft/virtual panels.";
            this.TSMI_Apps_KeysightBenchVue.Click += new System.EventHandler(this.TSMI_Apps_KeysightBenchVue_Click);
            // 
            // TSMI_Apps_KeysightCommandExpert
            // 
            this.TSMI_Apps_KeysightCommandExpert.Name = "TSMI_Apps_KeysightCommandExpert";
            this.TSMI_Apps_KeysightCommandExpert.Size = new System.Drawing.Size(172, 22);
            this.TSMI_Apps_KeysightCommandExpert.Text = "Co&mmand Expert";
            this.TSMI_Apps_KeysightCommandExpert.ToolTipText = "SCPI programming & debugging IDE.";
            this.TSMI_Apps_KeysightCommandExpert.Click += new System.EventHandler(this.TSMI_Apps_KeysightCommandExpert_Click);
            // 
            // TSMI_Apps_KeysightConnectionExpert
            // 
            this.TSMI_Apps_KeysightConnectionExpert.Name = "TSMI_Apps_KeysightConnectionExpert";
            this.TSMI_Apps_KeysightConnectionExpert.Size = new System.Drawing.Size(172, 22);
            this.TSMI_Apps_KeysightConnectionExpert.Text = "Co&nnection Expert";
            this.TSMI_Apps_KeysightConnectionExpert.ToolTipText = "Discover VISA Instruments.";
            this.TSMI_Apps_KeysightConnectionExpert.Click += new System.EventHandler(this.TSMI_Apps_KeysightConnectionExpert_Click);
            // 
            // TSMI_Apps_MeasurementComputing
            // 
            this.TSMI_Apps_MeasurementComputing.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Apps_MeasurementComputingInstaCal});
            this.TSMI_Apps_MeasurementComputing.Name = "TSMI_Apps_MeasurementComputing";
            this.TSMI_Apps_MeasurementComputing.Size = new System.Drawing.Size(211, 22);
            this.TSMI_Apps_MeasurementComputing.Text = "Measurement &Computing";
            // 
            // TSMI_Apps_MeasurementComputingInstaCal
            // 
            this.TSMI_Apps_MeasurementComputingInstaCal.Name = "TSMI_Apps_MeasurementComputingInstaCal";
            this.TSMI_Apps_MeasurementComputingInstaCal.Size = new System.Drawing.Size(116, 22);
            this.TSMI_Apps_MeasurementComputingInstaCal.Text = "&InstaCal";
            this.TSMI_Apps_MeasurementComputingInstaCal.ToolTipText = "Configure & test MCC Instruments, like USB-ERB24 relays.";
            this.TSMI_Apps_MeasurementComputingInstaCal.Click += new System.EventHandler(this.TSMI_Apps_MeasurementComputingInstaCal_Click);
            // 
            // TSMI_Apps_Microchip
            // 
            this.TSMI_Apps_Microchip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Apps_MicrohipMPLAB_IPE,
            this.TSMI_Apps_MicrohipMPLAB_X_IDE});
            this.TSMI_Apps_Microchip.Name = "TSMI_Apps_Microchip";
            this.TSMI_Apps_Microchip.Size = new System.Drawing.Size(211, 22);
            this.TSMI_Apps_Microchip.Text = "&Microchip";
            // 
            // TSMI_Apps_MicrohipMPLAB_IPE
            // 
            this.TSMI_Apps_MicrohipMPLAB_IPE.Name = "TSMI_Apps_MicrohipMPLAB_IPE";
            this.TSMI_Apps_MicrohipMPLAB_IPE.Size = new System.Drawing.Size(143, 22);
            this.TSMI_Apps_MicrohipMPLAB_IPE.Text = "MPLAB &IPE";
            this.TSMI_Apps_MicrohipMPLAB_IPE.Click += new System.EventHandler(this.TSMI_Apps_MicrochipMPLAB_IPE_Click);
            // 
            // TSMI_Apps_MicrohipMPLAB_X_IDE
            // 
            this.TSMI_Apps_MicrohipMPLAB_X_IDE.Name = "TSMI_Apps_MicrohipMPLAB_X_IDE";
            this.TSMI_Apps_MicrohipMPLAB_X_IDE.Size = new System.Drawing.Size(143, 22);
            this.TSMI_Apps_MicrohipMPLAB_X_IDE.Text = "MPLAB &X IDE";
            this.TSMI_Apps_MicrohipMPLAB_X_IDE.Click += new System.EventHandler(this.TSMI_Apps_MicrochipMPLAB_X_IDE_Click);
            // 
            // TSMI_Apps_Microsoft
            // 
            this.TSMI_Apps_Microsoft.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Apps_MicrosoftSQLServerManagementStudio,
            this.TSMI_Apps_MicrosoftVisualStudio,
            this.TSMI_Apps_MicrosoftVisualStudioCode,
            this.TSMI_Apps_MicrosoftXML_Notepad});
            this.TSMI_Apps_Microsoft.Name = "TSMI_Apps_Microsoft";
            this.TSMI_Apps_Microsoft.Size = new System.Drawing.Size(211, 22);
            this.TSMI_Apps_Microsoft.Text = "Micro&soft";
            // 
            // TSMI_Apps_MicrosoftSQLServerManagementStudio
            // 
            this.TSMI_Apps_MicrosoftSQLServerManagementStudio.Enabled = false;
            this.TSMI_Apps_MicrosoftSQLServerManagementStudio.Name = "TSMI_Apps_MicrosoftSQLServerManagementStudio";
            this.TSMI_Apps_MicrosoftSQLServerManagementStudio.Size = new System.Drawing.Size(241, 22);
            this.TSMI_Apps_MicrosoftSQLServerManagementStudio.Text = "&SQL Server Management Studio";
            this.TSMI_Apps_MicrosoftSQLServerManagementStudio.ToolTipText = "Coming soon!";
            this.TSMI_Apps_MicrosoftSQLServerManagementStudio.Click += new System.EventHandler(this.TSMI_Apps_MicrosoftSQL_ServerManagementStudio_Click);
            // 
            // TSMI_Apps_MicrosoftVisualStudio
            // 
            this.TSMI_Apps_MicrosoftVisualStudio.Name = "TSMI_Apps_MicrosoftVisualStudio";
            this.TSMI_Apps_MicrosoftVisualStudio.Size = new System.Drawing.Size(241, 22);
            this.TSMI_Apps_MicrosoftVisualStudio.Text = "&Visual Studio";
            this.TSMI_Apps_MicrosoftVisualStudio.ToolTipText = "C# forever!";
            this.TSMI_Apps_MicrosoftVisualStudio.Click += new System.EventHandler(this.TSMI_Apps_MicrosoftVisualStudio_Click);
            // 
            // TSMI_Apps_MicrosoftVisualStudioCode
            // 
            this.TSMI_Apps_MicrosoftVisualStudioCode.Name = "TSMI_Apps_MicrosoftVisualStudioCode";
            this.TSMI_Apps_MicrosoftVisualStudioCode.Size = new System.Drawing.Size(241, 22);
            this.TSMI_Apps_MicrosoftVisualStudioCode.Text = "Visual Studio &Code";
            this.TSMI_Apps_MicrosoftVisualStudioCode.Click += new System.EventHandler(this.TSMI_Apps_MicrosoftVisualStudioCode_Click);
            // 
            // TSMI_Apps_MicrosoftXML_Notepad
            // 
            this.TSMI_Apps_MicrosoftXML_Notepad.Name = "TSMI_Apps_MicrosoftXML_Notepad";
            this.TSMI_Apps_MicrosoftXML_Notepad.Size = new System.Drawing.Size(241, 22);
            this.TSMI_Apps_MicrosoftXML_Notepad.Text = "&XML Notepad";
            this.TSMI_Apps_MicrosoftXML_Notepad.Click += new System.EventHandler(this.TSMI_Apps_MicrosoftXML_Notepad_Click);
            // 
            // TSMI_Apps_TexasInstruments
            // 
            this.TSMI_Apps_TexasInstruments.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool});
            this.TSMI_Apps_TexasInstruments.Name = "TSMI_Apps_TexasInstruments";
            this.TSMI_Apps_TexasInstruments.Size = new System.Drawing.Size(211, 22);
            this.TSMI_Apps_TexasInstruments.Text = "&Texas Instruments";
            // 
            // TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool
            // 
            this.TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool.Name = "TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool";
            this.TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool.Size = new System.Drawing.Size(196, 22);
            this.TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool.Text = "&SMBus / I2C / SAA Tool";
            this.TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool.Click += new System.EventHandler(this.TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool_Click);
            // 
            // TSMI_Feedback
            // 
            this.TSMI_Feedback.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Feedback_Compliments,
            this.TSMI_Feedback_Critiques});
            this.TSMI_Feedback.Name = "TSMI_Feedback";
            this.TSMI_Feedback.Size = new System.Drawing.Size(69, 20);
            this.TSMI_Feedback.Text = "&Feedback";
            // 
            // TSMI_Feedback_Compliments
            // 
            this.TSMI_Feedback_Compliments.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Feedback_ComplimentsPraiseεPlaudits,
            this.TSMI_Feedback_ComplimentsMoney});
            this.TSMI_Feedback_Compliments.Name = "TSMI_Feedback_Compliments";
            this.TSMI_Feedback_Compliments.Size = new System.Drawing.Size(146, 22);
            this.TSMI_Feedback_Compliments.Text = "&Compliments";
            // 
            // TSMI_Feedback_ComplimentsPraiseεPlaudits
            // 
            this.TSMI_Feedback_ComplimentsPraiseεPlaudits.Name = "TSMI_Feedback_ComplimentsPraiseεPlaudits";
            this.TSMI_Feedback_ComplimentsPraiseεPlaudits.Size = new System.Drawing.Size(163, 22);
            this.TSMI_Feedback_ComplimentsPraiseεPlaudits.Text = "&Praise && Plaudits";
            this.TSMI_Feedback_ComplimentsPraiseεPlaudits.ToolTipText = "\"I can live for two months on a good compliment.\" - Mark Twain";
            this.TSMI_Feedback_ComplimentsPraiseεPlaudits.Click += new System.EventHandler(this.TSMI_Feedback_ComplimentsPraiseεPlaudits_Click);
            // 
            // TSMI_Feedback_ComplimentsMoney
            // 
            this.TSMI_Feedback_ComplimentsMoney.Name = "TSMI_Feedback_ComplimentsMoney";
            this.TSMI_Feedback_ComplimentsMoney.Size = new System.Drawing.Size(163, 22);
            this.TSMI_Feedback_ComplimentsMoney.Text = "&Money!";
            this.TSMI_Feedback_ComplimentsMoney.ToolTipText = "For a good cause! ";
            this.TSMI_Feedback_ComplimentsMoney.Click += new System.EventHandler(this.TSMI_Feedback_ComplimentsMoney_Click);
            // 
            // TSMI_Feedback_Critiques
            // 
            this.TSMI_Feedback_Critiques.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Feedback_CritiquesBugReport,
            this.TSMI_Feedback_CritiquesImprovementRequest});
            this.TSMI_Feedback_Critiques.Name = "TSMI_Feedback_Critiques";
            this.TSMI_Feedback_Critiques.Size = new System.Drawing.Size(146, 22);
            this.TSMI_Feedback_Critiques.Text = "Criti&ques";
            // 
            // TSMI_Feedback_CritiquesBugReport
            // 
            this.TSMI_Feedback_CritiquesBugReport.Name = "TSMI_Feedback_CritiquesBugReport";
            this.TSMI_Feedback_CritiquesBugReport.Size = new System.Drawing.Size(191, 22);
            this.TSMI_Feedback_CritiquesBugReport.Text = "&Bug Report";
            this.TSMI_Feedback_CritiquesBugReport.ToolTipText = "\"The devil is is in the details.\" - Friedrich Nietzsche";
            this.TSMI_Feedback_CritiquesBugReport.Click += new System.EventHandler(this.TSMI_Feedback_CritiqueBugReport_Click);
            // 
            // TSMI_Feedback_CritiquesImprovementRequest
            // 
            this.TSMI_Feedback_CritiquesImprovementRequest.Name = "TSMI_Feedback_CritiquesImprovementRequest";
            this.TSMI_Feedback_CritiquesImprovementRequest.Size = new System.Drawing.Size(191, 22);
            this.TSMI_Feedback_CritiquesImprovementRequest.Text = "&Improvement Request";
            this.TSMI_Feedback_CritiquesImprovementRequest.ToolTipText = "\"God is in the details.\" - Mies van der Rohe";
            this.TSMI_Feedback_CritiquesImprovementRequest.Click += new System.EventHandler(this.TSMI_Feedback_CritiqueImprovementRequest_Click);
            // 
            // TSMI_System
            // 
            this.TSMI_System.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_System_BarcodeScannerDiscovery,
            this.TSMI_System_Diagnostics,
            this.TSMI_System_Manuals,
            this.TSMI_System_TestExecutiveConfigXML,
            this.TSMI_System_Separator1,
            this.TSMI_System_About});
            this.TSMI_System.Name = "TSMI_System";
            this.TSMI_System.Size = new System.Drawing.Size(57, 20);
            this.TSMI_System.Text = "S&ystem";
            // 
            // TSMI_System_BarcodeScannerDiscovery
            // 
            this.TSMI_System_BarcodeScannerDiscovery.Name = "TSMI_System_BarcodeScannerDiscovery";
            this.TSMI_System_BarcodeScannerDiscovery.Size = new System.Drawing.Size(273, 22);
            this.TSMI_System_BarcodeScannerDiscovery.Text = "&Barcode Scanner Discovery";
            this.TSMI_System_BarcodeScannerDiscovery.ToolTipText = "Corded scanners only; no Bluetooth or Wireless scanners.";
            this.TSMI_System_BarcodeScannerDiscovery.Click += new System.EventHandler(this.TSMI_System_BarcodeScannerDiscovery_Click);
            // 
            // TSMI_System_Diagnostics
            // 
            this.TSMI_System_Diagnostics.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_System_DiagnosticsSCPI_VISA_Instruments,
            this.TSMI_System_DiagnosticsRelays});
            this.TSMI_System_Diagnostics.Name = "TSMI_System_Diagnostics";
            this.TSMI_System_Diagnostics.Size = new System.Drawing.Size(273, 22);
            this.TSMI_System_Diagnostics.Text = "&Diagnostics";
            // 
            // TSMI_System_DiagnosticsSCPI_VISA_Instruments
            // 
            this.TSMI_System_DiagnosticsSCPI_VISA_Instruments.Name = "TSMI_System_DiagnosticsSCPI_VISA_Instruments";
            this.TSMI_System_DiagnosticsSCPI_VISA_Instruments.Size = new System.Drawing.Size(191, 22);
            this.TSMI_System_DiagnosticsSCPI_VISA_Instruments.Text = "&SCPI VISA Instruments";
            this.TSMI_System_DiagnosticsSCPI_VISA_Instruments.ToolTipText = "Invoke SCPI_VISA_Instruments self-tests.";
            this.TSMI_System_DiagnosticsSCPI_VISA_Instruments.Click += new System.EventHandler(this.TSMI_System_DiagnosticsSCPI_VISA_Instruments_Click);
            // 
            // TSMI_System_DiagnosticsRelays
            // 
            this.TSMI_System_DiagnosticsRelays.Enabled = false;
            this.TSMI_System_DiagnosticsRelays.Name = "TSMI_System_DiagnosticsRelays";
            this.TSMI_System_DiagnosticsRelays.Size = new System.Drawing.Size(191, 22);
            this.TSMI_System_DiagnosticsRelays.Text = "&Relays";
            this.TSMI_System_DiagnosticsRelays.ToolTipText = "Adapt MS-Test Unit Tests of USB-ERB24 class.";
            this.TSMI_System_DiagnosticsRelays.Click += new System.EventHandler(this.TSMI_System_DiagnosticsRelays_Click);
            // 
            // TSMI_System_Manuals
            // 
            this.TSMI_System_Manuals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_System_ManualsBarcodeScanner,
            this.TSMI_System_ManualsInstruments,
            this.TSMI_System_ManualsRelays});
            this.TSMI_System_Manuals.Name = "TSMI_System_Manuals";
            this.TSMI_System_Manuals.Size = new System.Drawing.Size(273, 22);
            this.TSMI_System_Manuals.Text = "&Manuals";
            // 
            // TSMI_System_ManualsBarcodeScanner
            // 
            this.TSMI_System_ManualsBarcodeScanner.Name = "TSMI_System_ManualsBarcodeScanner";
            this.TSMI_System_ManualsBarcodeScanner.Size = new System.Drawing.Size(162, 22);
            this.TSMI_System_ManualsBarcodeScanner.Text = "&Barcode Scanner";
            this.TSMI_System_ManualsBarcodeScanner.ToolTipText = "If you\'re bored...";
            this.TSMI_System_ManualsBarcodeScanner.Click += new System.EventHandler(this.TSMI_System_ManualsBarcodeScanner_Click);
            // 
            // TSMI_System_ManualsInstruments
            // 
            this.TSMI_System_ManualsInstruments.Name = "TSMI_System_ManualsInstruments";
            this.TSMI_System_ManualsInstruments.Size = new System.Drawing.Size(162, 22);
            this.TSMI_System_ManualsInstruments.Text = "&Instruments";
            this.TSMI_System_ManualsInstruments.ToolTipText = "...really bored...";
            this.TSMI_System_ManualsInstruments.Click += new System.EventHandler(this.TSMI_System_ManualsInstruments_Click);
            // 
            // TSMI_System_ManualsRelays
            // 
            this.TSMI_System_ManualsRelays.Name = "TSMI_System_ManualsRelays";
            this.TSMI_System_ManualsRelays.Size = new System.Drawing.Size(162, 22);
            this.TSMI_System_ManualsRelays.Text = "&Relays";
            this.TSMI_System_ManualsRelays.ToolTipText = "...zzzzzz...";
            this.TSMI_System_ManualsRelays.Click += new System.EventHandler(this.TSMI_System_ManualsRelays_Click);
            // 
            // TSMI_System_TestExecutiveConfigXML
            // 
            this.TSMI_System_TestExecutiveConfigXML.Name = "TSMI_System_TestExecutiveConfigXML";
            this.TSMI_System_TestExecutiveConfigXML.Size = new System.Drawing.Size(273, 22);
            this.TSMI_System_TestExecutiveConfigXML.Text = "&TestExecutive.GlobalConfigurationFile";
            this.TSMI_System_TestExecutiveConfigXML.ToolTipText = "Test System\'s configuration.";
            this.TSMI_System_TestExecutiveConfigXML.Click += new System.EventHandler(this.TSMI_System_TestExecutiveConfigXML_Click);
            // 
            // TSMI_System_Separator1
            // 
            this.TSMI_System_Separator1.Name = "TSMI_System_Separator1";
            this.TSMI_System_Separator1.Size = new System.Drawing.Size(270, 6);
            // 
            // TSMI_System_About
            // 
            this.TSMI_System_About.Name = "TSMI_System_About";
            this.TSMI_System_About.Size = new System.Drawing.Size(273, 22);
            this.TSMI_System_About.Text = "&About...";
            this.TSMI_System_About.Click += new System.EventHandler(this.TSMI_System_About_Click);
            // 
            // TSMI_UUT
            // 
            this.TSMI_UUT.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_UUT_AppConfig,
            this.TSMI_UUT_eDocs,
            this.TSMI_UUT_ResetStatus,
            this.TSMI_UUT_Manuals,
            this.TSMI_UUT_TestData,
            this.TSMI_UUT_Separator1,
            this.TSMI_UUT_About});
            this.TSMI_UUT.Name = "TSMI_UUT";
            this.TSMI_UUT.Size = new System.Drawing.Size(41, 20);
            this.TSMI_UUT.Text = "&UUT";
            // 
            // TSMI_UUT_AppConfig
            // 
            this.TSMI_UUT_AppConfig.Name = "TSMI_UUT_AppConfig";
            this.TSMI_UUT_AppConfig.Size = new System.Drawing.Size(137, 22);
            this.TSMI_UUT_AppConfig.Text = "&App.config";
            this.TSMI_UUT_AppConfig.ToolTipText = "UUT\'s test configuration.";
            this.TSMI_UUT_AppConfig.Click += new System.EventHandler(this.TSMI_UUT_AppConfig_Click);
            // 
            // TSMI_UUT_eDocs
            // 
            this.TSMI_UUT_eDocs.Name = "TSMI_UUT_eDocs";
            this.TSMI_UUT_eDocs.Size = new System.Drawing.Size(137, 22);
            this.TSMI_UUT_eDocs.Text = "&eDocs";
            this.TSMI_UUT_eDocs.ToolTipText = "UUT\'s P: drive eDocs folder.";
            this.TSMI_UUT_eDocs.Click += new System.EventHandler(this.TSMI_UUT_eDocs_Click);
            // 
            // TSMI_UUT_ResetStatus
            // 
            this.TSMI_UUT_ResetStatus.Name = "TSMI_UUT_ResetStatus";
            this.TSMI_UUT_ResetStatus.Size = new System.Drawing.Size(137, 22);
            this.TSMI_UUT_ResetStatus.Text = "&Reset Status";
            this.TSMI_UUT_ResetStatus.Click += new System.EventHandler(this.TSMI_UUT_ResetStatus_Click);
            // 
            // TSMI_UUT_Manuals
            // 
            this.TSMI_UUT_Manuals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_UUT_ManualsInstruments});
            this.TSMI_UUT_Manuals.Name = "TSMI_UUT_Manuals";
            this.TSMI_UUT_Manuals.Size = new System.Drawing.Size(137, 22);
            this.TSMI_UUT_Manuals.Text = "&Manuals";
            // 
            // TSMI_UUT_ManualsInstruments
            // 
            this.TSMI_UUT_ManualsInstruments.Name = "TSMI_UUT_ManualsInstruments";
            this.TSMI_UUT_ManualsInstruments.Size = new System.Drawing.Size(137, 22);
            this.TSMI_UUT_ManualsInstruments.Text = "&Instruments";
            this.TSMI_UUT_ManualsInstruments.ToolTipText = "...really bored...";
            this.TSMI_UUT_ManualsInstruments.Click += new System.EventHandler(this.TSMI_UUT_ManualsInstruments_Click);
            // 
            // TSMI_UUT_TestData
            // 
            this.TSMI_UUT_TestData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_UUT_TestDataP_DriveTDR_Folder,
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying});
            this.TSMI_UUT_TestData.Name = "TSMI_UUT_TestData";
            this.TSMI_UUT_TestData.Size = new System.Drawing.Size(137, 22);
            this.TSMI_UUT_TestData.Text = "&Test Data";
            // 
            // TSMI_UUT_TestDataP_DriveTDR_Folder
            // 
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.Name = "TSMI_UUT_TestDataP_DriveTDR_Folder";
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.Size = new System.Drawing.Size(215, 22);
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.Text = "&P: Drive TDR Folder";
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.ToolTipText = "P:\\Test\\TDR";
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.Click += new System.EventHandler(this.TSMI_UUT_TestData_P_DriveTDR_Folder_Click);
            // 
            // TSMI_UUT_TestDataSQL_ReportingAndQuerying
            // 
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Enabled = false;
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Name = "TSMI_UUT_TestDataSQL_ReportingAndQuerying";
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Size = new System.Drawing.Size(215, 22);
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Text = "&SQL Reporting && Querying";
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.ToolTipText = "Coming soon!";
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Click += new System.EventHandler(this.TSMI_UUT_TestDataSQL_ReportingAndQuerying_Click);
            // 
            // TSMI_UUT_Separator1
            // 
            this.TSMI_UUT_Separator1.Name = "TSMI_UUT_Separator1";
            this.TSMI_UUT_Separator1.Size = new System.Drawing.Size(134, 6);
            // 
            // TSMI_UUT_About
            // 
            this.TSMI_UUT_About.Name = "TSMI_UUT_About";
            this.TSMI_UUT_About.Size = new System.Drawing.Size(137, 22);
            this.TSMI_UUT_About.Text = "&About...";
            this.TSMI_UUT_About.Click += new System.EventHandler(this.TSMI_UUT_About_Click);
            // 
            // StatusStrip
            // 
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusTimeLabel,
            this.StatusTestsLabel,
            this.StatusCustomLabel,
            this.StatusModeLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 615);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(1045, 22);
            this.StatusStrip.TabIndex = 10;
            // 
            // StatusTimeLabel
            // 
            this.StatusTimeLabel.Name = "StatusTimeLabel";
            this.StatusTimeLabel.Size = new System.Drawing.Size(33, 17);
            this.StatusTimeLabel.Text = "Time";
            this.StatusTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusTestsLabel
            // 
            this.StatusTestsLabel.Name = "StatusTestsLabel";
            this.StatusTestsLabel.Size = new System.Drawing.Size(32, 17);
            this.StatusTestsLabel.Text = "Tests";
            this.StatusTestsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusCustomLabel
            // 
            this.StatusCustomLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.StatusCustomLabel.Name = "StatusCustomLabel";
            this.StatusCustomLabel.Size = new System.Drawing.Size(49, 17);
            this.StatusCustomLabel.Text = "Custom";
            this.StatusCustomLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StatusModeLabel
            // 
            this.StatusModeLabel.Name = "StatusModeLabel";
            this.StatusModeLabel.Size = new System.Drawing.Size(885, 17);
            this.StatusModeLabel.Spring = true;
            this.StatusModeLabel.Text = "Mode";
            // 
            // TestExecutive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 637);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.ButtonEmergencyStop);
            this.Controls.Add(this.ButtonSelectTests);
            this.Controls.Add(this.rtfResults);
            this.Controls.Add(this.LabelTestEvent);
            this.Controls.Add(this.TextTest);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.MS);
            this.MainMenuStrip = this.MS;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TestExecutive";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestExecutor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.MS.ResumeLayout(false);
            this.MS.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.TextBox TextTest;
        private System.Windows.Forms.Label LabelTestEvent;
        private System.Windows.Forms.RichTextBox rtfResults;
        private System.Windows.Forms.Button ButtonSelectTests;
        private Button ButtonEmergencyStop;
        private MenuStrip MS;
        private ToolStripMenuItem TSMI_File;
        private ToolStripMenuItem TSMI_File_Save;
        private ToolStripMenuItem TSMI_System;
        private ToolStripMenuItem TSMI_Apps;
        private ToolStripMenuItem TSMI_System_Diagnostics;
        private ToolStripMenuItem TSMI_System_DiagnosticsSCPI_VISA_Instruments;
        private ToolStripMenuItem TSMI_System_DiagnosticsRelays;
        private ToolStripSeparator TSMI_System_Separator1;
        private ToolStripMenuItem TSMI_UUT;
        private ToolStripMenuItem TSMI_UUT_eDocs;
        private ToolStripMenuItem TSMI_UUT_TestData;
        private ToolStripMenuItem TSMI_UUT_TestDataP_DriveTDR_Folder;
        private ToolStripMenuItem TSMI_UUT_TestDataSQL_ReportingAndQuerying;
        private ToolStripMenuItem TSMI_UUT_Manuals;
        private ToolStripMenuItem TSMI_UUT_ManualsInstruments;
        private ToolStripMenuItem TSMI_System_Manuals;
        private ToolStripMenuItem TSMI_System_ManualsBarcodeScanner;
        private ToolStripMenuItem TSMI_System_ManualsInstruments;
        private ToolStripMenuItem TSMI_System_ManualsRelays;
        private ToolStripMenuItem TSMI_System_About;
        private ToolStripMenuItem TSMI_System_BarcodeScannerDiscovery;
        private ToolStripSeparator TSMI_UUT_Separator1;
        private ToolStripMenuItem TSMI_UUT_About;
        private ToolStripMenuItem TSMI_System_TestExecutiveConfigXML;
        private ToolStripMenuItem TSMI_UUT_AppConfig;
        private ToolStripMenuItem TSMI_Apps_Keysight;
        private ToolStripMenuItem TSMI_Apps_KeysightBenchVue;
        private ToolStripMenuItem TSMI_Apps_KeysightCommandExpert;
        private ToolStripMenuItem TSMI_Apps_KeysightConnectionExpert;
        private ToolStripMenuItem TSMI_Apps_MeasurementComputing;
        private ToolStripMenuItem TSMI_Apps_MeasurementComputingInstaCal;
        private ToolStripMenuItem TSMI_Apps_Microsoft;
        private ToolStripMenuItem TSMI_Apps_MicrosoftSQLServerManagementStudio;
        private ToolStripMenuItem TSMI_Apps_MicrosoftVisualStudio;
        private ToolStripMenuItem TSMI_Apps_MicrosoftXML_Notepad;
        private ToolStripMenuItem TSMI_Feedback;
        private ToolStripMenuItem TSMI_Feedback_Compliments;
        private ToolStripMenuItem TSMI_Feedback_ComplimentsPraiseεPlaudits;
        private ToolStripMenuItem TSMI_Feedback_ComplimentsMoney;
        private ToolStripMenuItem TSMI_Feedback_Critiques;
        private ToolStripMenuItem TSMI_Feedback_CritiquesBugReport;
        private ToolStripMenuItem TSMI_Feedback_CritiquesImprovementRequest;
        private ToolStripMenuItem TSMI_Apps_TexasInstruments;
        private ToolStripMenuItem TSMI_Apps_TexasInstrumentsSMBus_I2C_SAA_Tool;
        private ToolStripMenuItem TSMI_Apps_MicrosoftVisualStudioCode;
        private ToolStripMenuItem TSMI_Apps_Microchip;
        private ToolStripMenuItem TSMI_Apps_MicrohipMPLAB_IPE;
        private ToolStripMenuItem TSMI_Apps_MicrohipMPLAB_X_IDE;
        private StatusStrip StatusStrip;
        private ToolStripStatusLabel StatusTestsLabel;
        private ToolStripStatusLabel StatusCustomLabel;
        private ToolStripMenuItem TSMI_UUT_ResetStatus;
        private ToolStripStatusLabel StatusTimeLabel;
        private ToolStripStatusLabel StatusModeLabel;
    }
}
