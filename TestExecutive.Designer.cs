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
            this.TextResult = new System.Windows.Forms.TextBox();
            this.LabelResult = new System.Windows.Forms.Label();
            this.rtfResults = new System.Windows.Forms.RichTextBox();
            this.ButtonSelectTests = new System.Windows.Forms.Button();
            this.ButtonEmergencyStop = new System.Windows.Forms.Button();
            this.MS = new System.Windows.Forms.MenuStrip();
            this.TSMI_File = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_FileSeperator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_File_Print = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_File_PrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_FileSeperator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_File_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_EditAppConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_EditTestExecutiveXML = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_Launch = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_LaunchKeysight = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_LaunchKeysightBenchVue = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_LaunchKeysightCommandExpert = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_LaunchKeysightConnectionExpert = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_LaunchMeasurementComputing = new System.Windows.Forms.ToolStripMenuItem();
            this.instaCalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration_LaunchMicrosoft = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualStudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemBarcodeScannerDiscover = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemBarcodeScannerProgramDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemDiagnostics = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemDiagnosticsBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemDiagnosticsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemDiagnosticsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SystemManuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SystemManualsBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SystemManualsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SystemManualsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemSeperator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TMSI_SystemCompliments = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemComplimentsPraiseAndPlaudits = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemComplimentsMoney = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemCritiques = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemCritiquesBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemCritiquesImprovementRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemSeperator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_SystemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_eDocs = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_Manuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_ManualsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestData = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestDataP_DriveTDR_Folder = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying = new System.Windows.Forms.ToolStripMenuItem();
            this.MS.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonStart.BackColor = System.Drawing.Color.Green;
            this.ButtonStart.Location = new System.Drawing.Point(152, 564);
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
            this.ButtonCancel.Location = new System.Drawing.Point(281, 564);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(88, 52);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = false;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Clicked);
            // 
            // TextResult
            // 
            this.TextResult.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TextResult.Location = new System.Drawing.Point(494, 585);
            this.TextResult.Name = "TextResult";
            this.TextResult.ReadOnly = true;
            this.TextResult.Size = new System.Drawing.Size(60, 20);
            this.TextResult.TabIndex = 9;
            this.TextResult.TabStop = false;
            this.TextResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelResult
            // 
            this.LabelResult.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LabelResult.AutoSize = true;
            this.LabelResult.Location = new System.Drawing.Point(505, 565);
            this.LabelResult.Name = "LabelResult";
            this.LabelResult.Size = new System.Drawing.Size(37, 13);
            this.LabelResult.TabIndex = 8;
            this.LabelResult.Text = "Result";
            this.LabelResult.UseWaitCursor = true;
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
            this.rtfResults.Size = new System.Drawing.Size(1001, 521);
            this.rtfResults.TabIndex = 7;
            this.rtfResults.TabStop = false;
            this.rtfResults.Text = "";
            // 
            // ButtonSelectTests
            // 
            this.ButtonSelectTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonSelectTests.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonSelectTests.Location = new System.Drawing.Point(23, 565);
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
            this.ButtonEmergencyStop.Location = new System.Drawing.Point(944, 545);
            this.ButtonEmergencyStop.Name = "ButtonEmergencyStop";
            this.ButtonEmergencyStop.Size = new System.Drawing.Size(80, 80);
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
            this.TSMI_Administration,
            this.TMSI_System,
            this.TSMI_UUT});
            this.MS.Location = new System.Drawing.Point(0, 0);
            this.MS.Name = "MS";
            this.MS.Size = new System.Drawing.Size(1045, 24);
            this.MS.TabIndex = 6;
            this.MS.TabStop = true;
            this.MS.Text = "menuStrip1";
            // 
            // TSMI_File
            // 
            this.TSMI_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_File_Save,
            this.TMSI_FileSeperator1,
            this.TSMI_File_Print,
            this.TSMI_File_PrintPreview,
            this.TMSI_FileSeperator2,
            this.TSMI_File_Exit});
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
            this.TSMI_File_Save.Size = new System.Drawing.Size(188, 30);
            this.TSMI_File_Save.Text = "&Save";
            this.TSMI_File_Save.ToolTipText = "Save UUT results.";
            this.TSMI_File_Save.Click += new System.EventHandler(this.TSMI_File_Save_Click);
            // 
            // TMSI_FileSeperator1
            // 
            this.TMSI_FileSeperator1.Name = "TMSI_FileSeperator1";
            this.TMSI_FileSeperator1.Size = new System.Drawing.Size(185, 6);
            // 
            // TSMI_File_Print
            // 
            this.TSMI_File_Print.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_File_Print.Image")));
            this.TSMI_File_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_File_Print.Name = "TSMI_File_Print";
            this.TSMI_File_Print.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.TSMI_File_Print.Size = new System.Drawing.Size(188, 30);
            this.TSMI_File_Print.Text = "&Print";
            this.TSMI_File_Print.ToolTipText = "Print UUT results.";
            this.TSMI_File_Print.Click += new System.EventHandler(this.TSMI_File_Print_Click);
            // 
            // TSMI_File_PrintPreview
            // 
            this.TSMI_File_PrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_File_PrintPreview.Image")));
            this.TSMI_File_PrintPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_File_PrintPreview.Name = "TSMI_File_PrintPreview";
            this.TSMI_File_PrintPreview.Size = new System.Drawing.Size(188, 30);
            this.TSMI_File_PrintPreview.Text = "Print Pre&view";
            this.TSMI_File_PrintPreview.ToolTipText = "Preview UUT results.";
            this.TSMI_File_PrintPreview.Click += new System.EventHandler(this.TSMI_File_PrintPreview_Click);
            // 
            // TMSI_FileSeperator2
            // 
            this.TMSI_FileSeperator2.Name = "TMSI_FileSeperator2";
            this.TMSI_FileSeperator2.Size = new System.Drawing.Size(185, 6);
            // 
            // TSMI_File_Exit
            // 
            this.TSMI_File_Exit.Name = "TSMI_File_Exit";
            this.TSMI_File_Exit.Size = new System.Drawing.Size(188, 30);
            this.TSMI_File_Exit.Text = "&Exit";
            this.TSMI_File_Exit.ToolTipText = "Close application.";
            this.TSMI_File_Exit.Click += new System.EventHandler(this.TSMI_File_Exit_Click);
            // 
            // TSMI_Administration
            // 
            this.TSMI_Administration.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Administration_Edit,
            this.TSMI_Administration_Launch});
            this.TSMI_Administration.Enabled = false;
            this.TSMI_Administration.Name = "TSMI_Administration";
            this.TSMI_Administration.Size = new System.Drawing.Size(98, 20);
            this.TSMI_Administration.Text = "&Administration";
            // 
            // TSMI_Administration_Edit
            // 
            this.TSMI_Administration_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Administration_EditAppConfig,
            this.TSMI_Administration_EditTestExecutiveXML});
            this.TSMI_Administration_Edit.Name = "TSMI_Administration_Edit";
            this.TSMI_Administration_Edit.Size = new System.Drawing.Size(180, 22);
            this.TSMI_Administration_Edit.Text = "&Edit";
            // 
            // TSMI_Administration_EditAppConfig
            // 
            this.TSMI_Administration_EditAppConfig.Name = "TSMI_Administration_EditAppConfig";
            this.TSMI_Administration_EditAppConfig.Size = new System.Drawing.Size(204, 22);
            this.TSMI_Administration_EditAppConfig.Text = "&App.config";
            this.TSMI_Administration_EditAppConfig.ToolTipText = "UUT\'s test configuration.";
            this.TSMI_Administration_EditAppConfig.Click += new System.EventHandler(this.TSMI_Administration_EditAppConfig_Click);
            // 
            // TSMI_Administration_EditTestExecutiveXML
            // 
            this.TSMI_Administration_EditTestExecutiveXML.Name = "TSMI_Administration_EditTestExecutiveXML";
            this.TSMI_Administration_EditTestExecutiveXML.Size = new System.Drawing.Size(204, 22);
            this.TSMI_Administration_EditTestExecutiveXML.Text = "&TestExecutive.config.xml";
            this.TSMI_Administration_EditTestExecutiveXML.ToolTipText = "Test System\'s configuration.";
            this.TSMI_Administration_EditTestExecutiveXML.Click += new System.EventHandler(this.TSMI_Administration_EditTestExecutiveConfigXML_Click);
            // 
            // TSMI_Administration_Launch
            // 
            this.TSMI_Administration_Launch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Administration_LaunchKeysight,
            this.TSMI_Administration_LaunchMeasurementComputing,
            this.TSMI_Administration_LaunchMicrosoft});
            this.TSMI_Administration_Launch.Name = "TSMI_Administration_Launch";
            this.TSMI_Administration_Launch.Size = new System.Drawing.Size(180, 22);
            this.TSMI_Administration_Launch.Text = "&Launch";
            // 
            // TSMI_Administration_LaunchKeysight
            // 
            this.TSMI_Administration_LaunchKeysight.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Administration_LaunchKeysightBenchVue,
            this.TSMI_Administration_LaunchKeysightCommandExpert,
            this.TSMI_Administration_LaunchKeysightConnectionExpert});
            this.TSMI_Administration_LaunchKeysight.Name = "TSMI_Administration_LaunchKeysight";
            this.TSMI_Administration_LaunchKeysight.Size = new System.Drawing.Size(211, 22);
            this.TSMI_Administration_LaunchKeysight.Text = "&Keysight";
            // 
            // TSMI_Administration_LaunchKeysightBenchVue
            // 
            this.TSMI_Administration_LaunchKeysightBenchVue.Name = "TSMI_Administration_LaunchKeysightBenchVue";
            this.TSMI_Administration_LaunchKeysightBenchVue.Size = new System.Drawing.Size(180, 22);
            this.TSMI_Administration_LaunchKeysightBenchVue.Text = "&BenchVue";
            this.TSMI_Administration_LaunchKeysightBenchVue.ToolTipText = "Control Keysight Instruments via soft/virtual panels.";
            this.TSMI_Administration_LaunchKeysightBenchVue.Click += new System.EventHandler(this.TSMI_AdministrationLaunchKeysightBenchVue_Click);
            // 
            // TSMI_Administration_LaunchKeysightCommandExpert
            // 
            this.TSMI_Administration_LaunchKeysightCommandExpert.Name = "TSMI_Administration_LaunchKeysightCommandExpert";
            this.TSMI_Administration_LaunchKeysightCommandExpert.Size = new System.Drawing.Size(180, 22);
            this.TSMI_Administration_LaunchKeysightCommandExpert.Text = "Co&mmand Expert";
            this.TSMI_Administration_LaunchKeysightCommandExpert.ToolTipText = "SCPI programming & debugging IDE.";
            this.TSMI_Administration_LaunchKeysightCommandExpert.Click += new System.EventHandler(this.TSMI_AdministrationLaunchKeysightCommandExpert_Click);
            // 
            // TSMI_Administration_LaunchKeysightConnectionExpert
            // 
            this.TSMI_Administration_LaunchKeysightConnectionExpert.Name = "TSMI_Administration_LaunchKeysightConnectionExpert";
            this.TSMI_Administration_LaunchKeysightConnectionExpert.Size = new System.Drawing.Size(180, 22);
            this.TSMI_Administration_LaunchKeysightConnectionExpert.Text = "Co&nnection Expert";
            this.TSMI_Administration_LaunchKeysightConnectionExpert.ToolTipText = "Discover VISA Instruments.";
            this.TSMI_Administration_LaunchKeysightConnectionExpert.Click += new System.EventHandler(this.TSMI_AdministrationLaunchKeysightConnectionExpert_Click);
            // 
            // TSMI_Administration_LaunchMeasurementComputing
            // 
            this.TSMI_Administration_LaunchMeasurementComputing.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.instaCalToolStripMenuItem});
            this.TSMI_Administration_LaunchMeasurementComputing.Name = "TSMI_Administration_LaunchMeasurementComputing";
            this.TSMI_Administration_LaunchMeasurementComputing.Size = new System.Drawing.Size(211, 22);
            this.TSMI_Administration_LaunchMeasurementComputing.Text = "Measurement &Computing";
            // 
            // instaCalToolStripMenuItem
            // 
            this.instaCalToolStripMenuItem.Name = "instaCalToolStripMenuItem";
            this.instaCalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.instaCalToolStripMenuItem.Text = "&InstaCal";
            this.instaCalToolStripMenuItem.ToolTipText = "Configure & test MCC Instruments, like USB-ERB24 relays.";
            this.instaCalToolStripMenuItem.Click += new System.EventHandler(this.TSMI_AdministrationLaunchKeysightMeasurementComputingInstaCal_Click);
            // 
            // TSMI_Administration_LaunchMicrosoft
            // 
            this.TSMI_Administration_LaunchMicrosoft.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sQLServerToolStripMenuItem,
            this.visualStudioToolStripMenuItem});
            this.TSMI_Administration_LaunchMicrosoft.Name = "TSMI_Administration_LaunchMicrosoft";
            this.TSMI_Administration_LaunchMicrosoft.Size = new System.Drawing.Size(211, 22);
            this.TSMI_Administration_LaunchMicrosoft.Text = "&Microsoft";
            // 
            // sQLServerToolStripMenuItem
            // 
            this.sQLServerToolStripMenuItem.Enabled = false;
            this.sQLServerToolStripMenuItem.Name = "sQLServerToolStripMenuItem";
            this.sQLServerToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.sQLServerToolStripMenuItem.Text = "&SQL Server Management Studio";
            this.sQLServerToolStripMenuItem.ToolTipText = "Coming soon!";
            this.sQLServerToolStripMenuItem.Click += new System.EventHandler(this.TSMI_AdministrationLaunchMicrosoftSQL_ServerManagementStudio_Click);
            // 
            // visualStudioToolStripMenuItem
            // 
            this.visualStudioToolStripMenuItem.Name = "visualStudioToolStripMenuItem";
            this.visualStudioToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.visualStudioToolStripMenuItem.Text = "&Visual Studio";
            this.visualStudioToolStripMenuItem.ToolTipText = "C# forever!";
            this.visualStudioToolStripMenuItem.Click += new System.EventHandler(this.TSMI_AdministrationLaunchMicrosoftVisualStudio_Click);
            // 
            // TMSI_System
            // 
            this.TMSI_System.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_SystemBarcodeScanner,
            this.TMSI_SystemDiagnostics,
            this.TSMI_SystemManuals,
            this.TMSI_SystemSeperator1,
            this.TMSI_SystemCompliments,
            this.TMSI_SystemCritiques,
            this.TMSI_SystemSeperator2,
            this.TSMI_SystemAbout});
            this.TMSI_System.Name = "TMSI_System";
            this.TMSI_System.Size = new System.Drawing.Size(57, 20);
            this.TMSI_System.Text = "S&ystem";
            // 
            // TMSI_SystemBarcodeScanner
            // 
            this.TMSI_SystemBarcodeScanner.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_SystemBarcodeScannerDiscover,
            this.TMSI_SystemBarcodeScannerProgramDefaults});
            this.TMSI_SystemBarcodeScanner.Name = "TMSI_SystemBarcodeScanner";
            this.TMSI_SystemBarcodeScanner.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemBarcodeScanner.Text = "&Barcode Scanner";
            // 
            // TMSI_SystemBarcodeScannerDiscover
            // 
            this.TMSI_SystemBarcodeScannerDiscover.Name = "TMSI_SystemBarcodeScannerDiscover";
            this.TMSI_SystemBarcodeScannerDiscover.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemBarcodeScannerDiscover.Text = "&Discover";
            this.TMSI_SystemBarcodeScannerDiscover.ToolTipText = "Corded scanners only; no Bluetooth or Wireless scanners.";
            this.TMSI_SystemBarcodeScannerDiscover.Click += new System.EventHandler(this.TSMI_SystemBarcodeScannerDiscover_Click);
            // 
            // TMSI_SystemBarcodeScannerProgramDefaults
            // 
            this.TMSI_SystemBarcodeScannerProgramDefaults.Name = "TMSI_SystemBarcodeScannerProgramDefaults";
            this.TMSI_SystemBarcodeScannerProgramDefaults.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemBarcodeScannerProgramDefaults.Text = "&Program Defaults";
            this.TMSI_SystemBarcodeScannerProgramDefaults.ToolTipText = "Program Factory Reset & USB-HID mode.";
            this.TMSI_SystemBarcodeScannerProgramDefaults.Click += new System.EventHandler(this.TSMI_SystemBarcodeScannerProgramDefaults_Click);
            // 
            // TMSI_SystemDiagnostics
            // 
            this.TMSI_SystemDiagnostics.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_SystemDiagnosticsBarcodeScanner,
            this.TMSI_SystemDiagnosticsInstruments,
            this.TMSI_SystemDiagnosticsRelays});
            this.TMSI_SystemDiagnostics.Name = "TMSI_SystemDiagnostics";
            this.TMSI_SystemDiagnostics.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemDiagnostics.Text = "&Diagnostics";
            // 
            // TMSI_SystemDiagnosticsBarcodeScanner
            // 
            this.TMSI_SystemDiagnosticsBarcodeScanner.Name = "TMSI_SystemDiagnosticsBarcodeScanner";
            this.TMSI_SystemDiagnosticsBarcodeScanner.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemDiagnosticsBarcodeScanner.Text = "&Barcode Scanner";
            this.TMSI_SystemDiagnosticsBarcodeScanner.ToolTipText = "Barcode Scanner\'s power-on self-test.";
            this.TMSI_SystemDiagnosticsBarcodeScanner.Click += new System.EventHandler(this.TSMI_SystemDiagnosticsBarcodeScanner_Click);
            // 
            // TMSI_SystemDiagnosticsInstruments
            // 
            this.TMSI_SystemDiagnosticsInstruments.Name = "TMSI_SystemDiagnosticsInstruments";
            this.TMSI_SystemDiagnosticsInstruments.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemDiagnosticsInstruments.Text = "&Instruments";
            this.TMSI_SystemDiagnosticsInstruments.ToolTipText = "Multi-select ListBox auto-populated from TestExecutive.config.xml.  Invoke SCPI_V" +
    "ISA_Instruments self-tests.";
            this.TMSI_SystemDiagnosticsInstruments.Click += new System.EventHandler(this.TSMI_SystemDiagnosticsInstruments_Click);
            // 
            // TMSI_SystemDiagnosticsRelays
            // 
            this.TMSI_SystemDiagnosticsRelays.Enabled = false;
            this.TMSI_SystemDiagnosticsRelays.Name = "TMSI_SystemDiagnosticsRelays";
            this.TMSI_SystemDiagnosticsRelays.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemDiagnosticsRelays.Text = "&Relays";
            this.TMSI_SystemDiagnosticsRelays.ToolTipText = "Adapt MS-Test Unit Tests of USB-ERB24 class.";
            this.TMSI_SystemDiagnosticsRelays.Click += new System.EventHandler(this.TSMI_SystemDiagnosticsRelays_Click);
            // 
            // TSMI_SystemManuals
            // 
            this.TSMI_SystemManuals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_SystemManualsBarcodeScanner,
            this.TSMI_SystemManualsInstruments,
            this.TSMI_SystemManualsRelays});
            this.TSMI_SystemManuals.Name = "TSMI_SystemManuals";
            this.TSMI_SystemManuals.Size = new System.Drawing.Size(180, 22);
            this.TSMI_SystemManuals.Text = "&Manuals";
            // 
            // TSMI_SystemManualsBarcodeScanner
            // 
            this.TSMI_SystemManualsBarcodeScanner.Name = "TSMI_SystemManualsBarcodeScanner";
            this.TSMI_SystemManualsBarcodeScanner.Size = new System.Drawing.Size(180, 22);
            this.TSMI_SystemManualsBarcodeScanner.Text = "&Barcode Scanner";
            this.TSMI_SystemManualsBarcodeScanner.ToolTipText = "If you\'re bored...";
            this.TSMI_SystemManualsBarcodeScanner.Click += new System.EventHandler(this.TSMI_SystemManualsBarcodeScanner_Click);
            // 
            // TSMI_SystemManualsInstruments
            // 
            this.TSMI_SystemManualsInstruments.Name = "TSMI_SystemManualsInstruments";
            this.TSMI_SystemManualsInstruments.Size = new System.Drawing.Size(180, 22);
            this.TSMI_SystemManualsInstruments.Text = "&Instruments";
            this.TSMI_SystemManualsInstruments.ToolTipText = "...really bored...";
            this.TSMI_SystemManualsInstruments.Click += new System.EventHandler(this.TSMI_SystemManualsInstruments_Click);
            // 
            // TSMI_SystemManualsRelays
            // 
            this.TSMI_SystemManualsRelays.Name = "TSMI_SystemManualsRelays";
            this.TSMI_SystemManualsRelays.Size = new System.Drawing.Size(180, 22);
            this.TSMI_SystemManualsRelays.Text = "&Relays";
            this.TSMI_SystemManualsRelays.ToolTipText = "...zzzzzz...";
            this.TSMI_SystemManualsRelays.Click += new System.EventHandler(this.TSMI_SystemManualsRelays_Click);
            // 
            // TMSI_SystemSeperator1
            // 
            this.TMSI_SystemSeperator1.Name = "TMSI_SystemSeperator1";
            this.TMSI_SystemSeperator1.Size = new System.Drawing.Size(177, 6);
            // 
            // TMSI_SystemCompliments
            // 
            this.TMSI_SystemCompliments.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_SystemComplimentsPraiseAndPlaudits,
            this.TMSI_SystemComplimentsMoney});
            this.TMSI_SystemCompliments.Name = "TMSI_SystemCompliments";
            this.TMSI_SystemCompliments.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemCompliments.Text = "Co&mpliments";
            // 
            // TMSI_SystemComplimentsPraiseAndPlaudits
            // 
            this.TMSI_SystemComplimentsPraiseAndPlaudits.Name = "TMSI_SystemComplimentsPraiseAndPlaudits";
            this.TMSI_SystemComplimentsPraiseAndPlaudits.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemComplimentsPraiseAndPlaudits.Text = "&Praise && Plaudits";
            this.TMSI_SystemComplimentsPraiseAndPlaudits.ToolTipText = "\"I can live for two months on a good compliment.\" - Mark Twain";
            this.TMSI_SystemComplimentsPraiseAndPlaudits.Click += new System.EventHandler(this.TSMI_SystemComplimentsPraiseAndPlaudits_Click);
            // 
            // TMSI_SystemComplimentsMoney
            // 
            this.TMSI_SystemComplimentsMoney.Name = "TMSI_SystemComplimentsMoney";
            this.TMSI_SystemComplimentsMoney.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemComplimentsMoney.Text = "&Money!";
            this.TMSI_SystemComplimentsMoney.ToolTipText = "For a good cause! ";
            this.TMSI_SystemComplimentsMoney.Click += new System.EventHandler(this.TSMI_SystemComplimentsMoney_Click);
            // 
            // TMSI_SystemCritiques
            // 
            this.TMSI_SystemCritiques.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_SystemCritiquesBugReport,
            this.TMSI_SystemCritiquesImprovementRequest});
            this.TMSI_SystemCritiques.Name = "TMSI_SystemCritiques";
            this.TMSI_SystemCritiques.Size = new System.Drawing.Size(180, 22);
            this.TMSI_SystemCritiques.Text = "Cri&tiques";
            // 
            // TMSI_SystemCritiquesBugReport
            // 
            this.TMSI_SystemCritiquesBugReport.Name = "TMSI_SystemCritiquesBugReport";
            this.TMSI_SystemCritiquesBugReport.Size = new System.Drawing.Size(191, 22);
            this.TMSI_SystemCritiquesBugReport.Text = "&Bug Report";
            this.TMSI_SystemCritiquesBugReport.ToolTipText = "Remember, \"The devil is is in the details.\" - Friedrich Nietzsche";
            this.TMSI_SystemCritiquesBugReport.Click += new System.EventHandler(this.TSMI_SystemCritiqueBugReport_Click);
            // 
            // TMSI_SystemCritiquesImprovementRequest
            // 
            this.TMSI_SystemCritiquesImprovementRequest.Name = "TMSI_SystemCritiquesImprovementRequest";
            this.TMSI_SystemCritiquesImprovementRequest.Size = new System.Drawing.Size(191, 22);
            this.TMSI_SystemCritiquesImprovementRequest.Text = "&Improvement Request";
            this.TMSI_SystemCritiquesImprovementRequest.ToolTipText = "Remember, \"God is in the details.\" - Mies van der Rohe";
            this.TMSI_SystemCritiquesImprovementRequest.Click += new System.EventHandler(this.TSMI_SystemCritiqueImprovementRequest_Click);
            // 
            // TMSI_SystemSeperator2
            // 
            this.TMSI_SystemSeperator2.Name = "TMSI_SystemSeperator2";
            this.TMSI_SystemSeperator2.Size = new System.Drawing.Size(177, 6);
            // 
            // TSMI_SystemAbout
            // 
            this.TSMI_SystemAbout.Name = "TSMI_SystemAbout";
            this.TSMI_SystemAbout.Size = new System.Drawing.Size(180, 22);
            this.TSMI_SystemAbout.Text = "&About...";
            this.TSMI_SystemAbout.Click += new System.EventHandler(this.TSMI_SystemAbout_Click);
            // 
            // TSMI_UUT
            // 
            this.TSMI_UUT.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_UUT_eDocs,
            this.TSMI_UUT_Manuals,
            this.TSMI_UUT_TestData});
            this.TSMI_UUT.Name = "TSMI_UUT";
            this.TSMI_UUT.Size = new System.Drawing.Size(41, 20);
            this.TSMI_UUT.Text = "&UUT";
            // 
            // TSMI_UUT_eDocs
            // 
            this.TSMI_UUT_eDocs.Name = "TSMI_UUT_eDocs";
            this.TSMI_UUT_eDocs.Size = new System.Drawing.Size(180, 22);
            this.TSMI_UUT_eDocs.Text = "&eDocs";
            this.TSMI_UUT_eDocs.ToolTipText = "UUT\'s P: drive eDocs folder.";
            this.TSMI_UUT_eDocs.Click += new System.EventHandler(this.TSMI_UUT_eDocs_Click);
            // 
            // TSMI_UUT_Manuals
            // 
            this.TSMI_UUT_Manuals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_UUT_ManualsInstruments});
            this.TSMI_UUT_Manuals.Name = "TSMI_UUT_Manuals";
            this.TSMI_UUT_Manuals.Size = new System.Drawing.Size(180, 22);
            this.TSMI_UUT_Manuals.Text = "&Manuals";
            // 
            // TSMI_UUT_ManualsInstruments
            // 
            this.TSMI_UUT_ManualsInstruments.Name = "TSMI_UUT_ManualsInstruments";
            this.TSMI_UUT_ManualsInstruments.Size = new System.Drawing.Size(180, 22);
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
            this.TSMI_UUT_TestData.Size = new System.Drawing.Size(180, 22);
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
            // TestExecutive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 637);
            this.Controls.Add(this.ButtonEmergencyStop);
            this.Controls.Add(this.ButtonSelectTests);
            this.Controls.Add(this.rtfResults);
            this.Controls.Add(this.LabelResult);
            this.Controls.Add(this.TextResult);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.MS);
            this.MainMenuStrip = this.MS;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TestExecutive";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Program";
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.MS.ResumeLayout(false);
            this.MS.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.TextBox TextResult;
        private System.Windows.Forms.Label LabelResult;
        private System.Windows.Forms.RichTextBox rtfResults;
        private System.Windows.Forms.Button ButtonSelectTests;
        private Button ButtonEmergencyStop;
        private MenuStrip MS;
        private ToolStripMenuItem TSMI_File;
        private ToolStripMenuItem TSMI_File_Save;
        private ToolStripSeparator TMSI_FileSeperator1;
        private ToolStripMenuItem TSMI_File_Print;
        private ToolStripMenuItem TSMI_File_PrintPreview;
        private ToolStripSeparator TMSI_FileSeperator2;
        private ToolStripMenuItem TSMI_File_Exit;
        private ToolStripMenuItem TMSI_System;
        private ToolStripMenuItem TSMI_Administration;
        private ToolStripMenuItem TSMI_Administration_Edit;
        private ToolStripMenuItem TSMI_Administration_EditAppConfig;
        private ToolStripMenuItem TSMI_Administration_EditTestExecutiveXML;
        private ToolStripMenuItem TSMI_Administration_Launch;
        private ToolStripMenuItem TSMI_Administration_LaunchKeysight;
        private ToolStripMenuItem TSMI_Administration_LaunchKeysightBenchVue;
        private ToolStripMenuItem TSMI_Administration_LaunchKeysightCommandExpert;
        private ToolStripMenuItem TSMI_Administration_LaunchKeysightConnectionExpert;
        private ToolStripMenuItem TSMI_Administration_LaunchMeasurementComputing;
        private ToolStripMenuItem instaCalToolStripMenuItem;
        private ToolStripMenuItem TSMI_Administration_LaunchMicrosoft;
        private ToolStripMenuItem sQLServerToolStripMenuItem;
        private ToolStripMenuItem visualStudioToolStripMenuItem;
        private ToolStripMenuItem TMSI_SystemDiagnostics;
        private ToolStripMenuItem TMSI_SystemDiagnosticsBarcodeScanner;
        private ToolStripMenuItem TMSI_SystemDiagnosticsInstruments;
        private ToolStripMenuItem TMSI_SystemDiagnosticsRelays;
        private ToolStripSeparator TMSI_SystemSeperator1;
        private ToolStripMenuItem TMSI_SystemCompliments;
        private ToolStripMenuItem TMSI_SystemComplimentsPraiseAndPlaudits;
        private ToolStripMenuItem TMSI_SystemComplimentsMoney;
        private ToolStripMenuItem TMSI_SystemCritiques;
        private ToolStripMenuItem TMSI_SystemCritiquesBugReport;
        private ToolStripMenuItem TMSI_SystemCritiquesImprovementRequest;
        private ToolStripMenuItem TSMI_UUT;
        private ToolStripMenuItem TSMI_UUT_eDocs;
        private ToolStripMenuItem TSMI_UUT_TestData;
        private ToolStripMenuItem TSMI_UUT_TestDataP_DriveTDR_Folder;
        private ToolStripMenuItem TSMI_UUT_TestDataSQL_ReportingAndQuerying;
        private ToolStripMenuItem TSMI_UUT_Manuals;
        private ToolStripMenuItem TSMI_UUT_ManualsInstruments;
        private ToolStripMenuItem TSMI_SystemManuals;
        private ToolStripMenuItem TSMI_SystemManualsBarcodeScanner;
        private ToolStripMenuItem TSMI_SystemManualsInstruments;
        private ToolStripMenuItem TSMI_SystemManualsRelays;
        private ToolStripMenuItem TMSI_SystemBarcodeScanner;
        private ToolStripMenuItem TMSI_SystemBarcodeScannerDiscover;
        private ToolStripMenuItem TMSI_SystemBarcodeScannerProgramDefaults;
        private ToolStripSeparator TMSI_SystemSeperator2;
        private ToolStripMenuItem TSMI_SystemAbout;
    }
}
