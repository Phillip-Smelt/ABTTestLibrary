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
            this.TSMI_FileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_FileSeperator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_FilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_FilePrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_FileSeperator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_FileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemDiagnostics = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemDiagnosticsBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemDiagnosticsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemDiagnosticsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemSeperator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TMSI_SystemCompliments = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemComplimentsPraiseAndPlaudits = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemComplimentsMoney = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemCritiques = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemCritiquesBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemCritiquesImprovementRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrationEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrationEditAppConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrationEditTestExecutiveXML = new System.Windows.Forms.ToolStripMenuItem();
            this.TMS = new System.Windows.Forms.ToolStripMenuItem();
            this.keysightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.benchVueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commandExpertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionExpertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.measurementComputingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instaCalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.microsoftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualStudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_eDocs = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestData = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestDataP_DriveTDR_Folder = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_Manuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_ManualsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SystemManuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SystemManualsBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SystemManualsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SystemManualsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemBarcodeScannerDiscover = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemBarcodeScannerProgramDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_SystemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_SystemSeperator2 = new System.Windows.Forms.ToolStripSeparator();
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
            this.TSMI_FileSave,
            this.TMSI_FileSeperator1,
            this.TSMI_FilePrint,
            this.TSMI_FilePrintPreview,
            this.TMSI_FileSeperator2,
            this.TSMI_FileExit});
            this.TSMI_File.Name = "TSMI_File";
            this.TSMI_File.Size = new System.Drawing.Size(37, 20);
            this.TSMI_File.Text = "&File";
            // 
            // TSMI_FileSave
            // 
            this.TSMI_FileSave.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_FileSave.Image")));
            this.TSMI_FileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_FileSave.Name = "TSMI_FileSave";
            this.TSMI_FileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.TSMI_FileSave.Size = new System.Drawing.Size(188, 30);
            this.TSMI_FileSave.Text = "&Save";
            this.TSMI_FileSave.ToolTipText = "Save UUT results.";
            this.TSMI_FileSave.Click += new System.EventHandler(this.TSMI_FileSave_Click);
            // 
            // TMSI_FileSeperator1
            // 
            this.TMSI_FileSeperator1.Name = "TMSI_FileSeperator1";
            this.TMSI_FileSeperator1.Size = new System.Drawing.Size(185, 6);
            // 
            // TSMI_FilePrint
            // 
            this.TSMI_FilePrint.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_FilePrint.Image")));
            this.TSMI_FilePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_FilePrint.Name = "TSMI_FilePrint";
            this.TSMI_FilePrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.TSMI_FilePrint.Size = new System.Drawing.Size(188, 30);
            this.TSMI_FilePrint.Text = "&Print";
            this.TSMI_FilePrint.ToolTipText = "Print UUT results.";
            this.TSMI_FilePrint.Click += new System.EventHandler(this.TSMI_FilePrint_Click);
            // 
            // TSMI_FilePrintPreview
            // 
            this.TSMI_FilePrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_FilePrintPreview.Image")));
            this.TSMI_FilePrintPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_FilePrintPreview.Name = "TSMI_FilePrintPreview";
            this.TSMI_FilePrintPreview.Size = new System.Drawing.Size(188, 30);
            this.TSMI_FilePrintPreview.Text = "Print Pre&view";
            this.TSMI_FilePrintPreview.ToolTipText = "Preview UUT results.";
            this.TSMI_FilePrintPreview.Click += new System.EventHandler(this.TSMI_FilePrintPreview_Click);
            // 
            // TMSI_FileSeperator2
            // 
            this.TMSI_FileSeperator2.Name = "TMSI_FileSeperator2";
            this.TMSI_FileSeperator2.Size = new System.Drawing.Size(185, 6);
            // 
            // TSMI_FileExit
            // 
            this.TSMI_FileExit.Name = "TSMI_FileExit";
            this.TSMI_FileExit.Size = new System.Drawing.Size(188, 30);
            this.TSMI_FileExit.Text = "&Exit";
            this.TSMI_FileExit.ToolTipText = "Close application.";
            this.TSMI_FileExit.Click += new System.EventHandler(this.TSMI_FileExit_Click);
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
            // TSMI_Administration
            // 
            this.TSMI_Administration.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_AdministrationEdit,
            this.TMS});
            this.TSMI_Administration.Enabled = false;
            this.TSMI_Administration.Name = "TSMI_Administration";
            this.TSMI_Administration.Size = new System.Drawing.Size(98, 20);
            this.TSMI_Administration.Text = "&Administration";
            // 
            // TSMI_AdministrationEdit
            // 
            this.TSMI_AdministrationEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_AdministrationEditAppConfig,
            this.TSMI_AdministrationEditTestExecutiveXML});
            this.TSMI_AdministrationEdit.Name = "TSMI_AdministrationEdit";
            this.TSMI_AdministrationEdit.Size = new System.Drawing.Size(180, 22);
            this.TSMI_AdministrationEdit.Text = "&Edit";
            // 
            // TSMI_AdministrationEditAppConfig
            // 
            this.TSMI_AdministrationEditAppConfig.Name = "TSMI_AdministrationEditAppConfig";
            this.TSMI_AdministrationEditAppConfig.Size = new System.Drawing.Size(204, 22);
            this.TSMI_AdministrationEditAppConfig.Text = "&App.config";
            this.TSMI_AdministrationEditAppConfig.ToolTipText = "UUT\'s test configuration.";
            this.TSMI_AdministrationEditAppConfig.Click += new System.EventHandler(this.TSMI_AdministrationEditAppConfig_Click);
            // 
            // TSMI_AdministrationEditTestExecutiveXML
            // 
            this.TSMI_AdministrationEditTestExecutiveXML.Name = "TSMI_AdministrationEditTestExecutiveXML";
            this.TSMI_AdministrationEditTestExecutiveXML.Size = new System.Drawing.Size(204, 22);
            this.TSMI_AdministrationEditTestExecutiveXML.Text = "&TestExecutive.config.xml";
            this.TSMI_AdministrationEditTestExecutiveXML.ToolTipText = "Test System\'s configuration.";
            this.TSMI_AdministrationEditTestExecutiveXML.Click += new System.EventHandler(this.TSMI_AdministrationEditTestExecutiveConfigXML_Click);
            // 
            // TMS
            // 
            this.TMS.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keysightToolStripMenuItem,
            this.measurementComputingToolStripMenuItem,
            this.microsoftToolStripMenuItem});
            this.TMS.Name = "TMS";
            this.TMS.Size = new System.Drawing.Size(180, 22);
            this.TMS.Text = "&Launch";
            // 
            // keysightToolStripMenuItem
            // 
            this.keysightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.benchVueToolStripMenuItem,
            this.commandExpertToolStripMenuItem,
            this.connectionExpertToolStripMenuItem});
            this.keysightToolStripMenuItem.Name = "keysightToolStripMenuItem";
            this.keysightToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.keysightToolStripMenuItem.Text = "&Keysight";
            // 
            // benchVueToolStripMenuItem
            // 
            this.benchVueToolStripMenuItem.Name = "benchVueToolStripMenuItem";
            this.benchVueToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.benchVueToolStripMenuItem.Text = "&BenchVue";
            this.benchVueToolStripMenuItem.ToolTipText = "Control Keysight Instruments via soft/virtual panels.";
            this.benchVueToolStripMenuItem.Click += new System.EventHandler(this.TSMI_AdministrationLaunchKeysightBenchVue_Click);
            // 
            // commandExpertToolStripMenuItem
            // 
            this.commandExpertToolStripMenuItem.Name = "commandExpertToolStripMenuItem";
            this.commandExpertToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.commandExpertToolStripMenuItem.Text = "Co&mmand Expert";
            this.commandExpertToolStripMenuItem.ToolTipText = "SCPI programming & debugging IDE.";
            this.commandExpertToolStripMenuItem.Click += new System.EventHandler(this.TSMI_AdministrationLaunchKeysightCommandExpert_Click);
            // 
            // connectionExpertToolStripMenuItem
            // 
            this.connectionExpertToolStripMenuItem.Name = "connectionExpertToolStripMenuItem";
            this.connectionExpertToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.connectionExpertToolStripMenuItem.Text = "Co&nnection Expert";
            this.connectionExpertToolStripMenuItem.ToolTipText = "Discover VISA Instruments.";
            this.connectionExpertToolStripMenuItem.Click += new System.EventHandler(this.TSMI_AdministrationLaunchKeysightConnectionExpert_Click);
            // 
            // measurementComputingToolStripMenuItem
            // 
            this.measurementComputingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.instaCalToolStripMenuItem});
            this.measurementComputingToolStripMenuItem.Name = "measurementComputingToolStripMenuItem";
            this.measurementComputingToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.measurementComputingToolStripMenuItem.Text = "Measurement &Computing";
            // 
            // instaCalToolStripMenuItem
            // 
            this.instaCalToolStripMenuItem.Name = "instaCalToolStripMenuItem";
            this.instaCalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.instaCalToolStripMenuItem.Text = "&InstaCal";
            this.instaCalToolStripMenuItem.ToolTipText = "Configure & test MCC Instruments, like USB-ERB24 relays.";
            this.instaCalToolStripMenuItem.Click += new System.EventHandler(this.TSMI_AdministrationLaunchKeysightMeasurementComputingInstaCal_Click);
            // 
            // microsoftToolStripMenuItem
            // 
            this.microsoftToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sQLServerToolStripMenuItem,
            this.visualStudioToolStripMenuItem});
            this.microsoftToolStripMenuItem.Name = "microsoftToolStripMenuItem";
            this.microsoftToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.microsoftToolStripMenuItem.Text = "&Microsoft";
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
            // TSMI_SystemAbout
            // 
            this.TSMI_SystemAbout.Name = "TSMI_SystemAbout";
            this.TSMI_SystemAbout.Size = new System.Drawing.Size(180, 22);
            this.TSMI_SystemAbout.Text = "&About...";
            this.TSMI_SystemAbout.Click += new System.EventHandler(this.TSMI_SystemAbout_Click);
            // 
            // TMSI_SystemSeperator2
            // 
            this.TMSI_SystemSeperator2.Name = "TMSI_SystemSeperator2";
            this.TMSI_SystemSeperator2.Size = new System.Drawing.Size(177, 6);
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
        private ToolStripMenuItem TSMI_FileSave;
        private ToolStripSeparator TMSI_FileSeperator1;
        private ToolStripMenuItem TSMI_FilePrint;
        private ToolStripMenuItem TSMI_FilePrintPreview;
        private ToolStripSeparator TMSI_FileSeperator2;
        private ToolStripMenuItem TSMI_FileExit;
        private ToolStripMenuItem TMSI_System;
        private ToolStripMenuItem TSMI_Administration;
        private ToolStripMenuItem TSMI_AdministrationEdit;
        private ToolStripMenuItem TSMI_AdministrationEditAppConfig;
        private ToolStripMenuItem TSMI_AdministrationEditTestExecutiveXML;
        private ToolStripMenuItem TMS;
        private ToolStripMenuItem keysightToolStripMenuItem;
        private ToolStripMenuItem benchVueToolStripMenuItem;
        private ToolStripMenuItem commandExpertToolStripMenuItem;
        private ToolStripMenuItem connectionExpertToolStripMenuItem;
        private ToolStripMenuItem measurementComputingToolStripMenuItem;
        private ToolStripMenuItem instaCalToolStripMenuItem;
        private ToolStripMenuItem microsoftToolStripMenuItem;
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
