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
            this.TMSI_Operation = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationConfigure = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationConfigureBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationConfigureBarcodeScannerDiscover = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationConfigureBarcodeScannerProgramDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationDiagnostics = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationDiagnosticsBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationDiagnosticsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationDiagnosticsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationSeperator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TMSI_OperationCompliments = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationComplimentsPraiseAndPlaudits = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationComplimentsMoney = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationCritiques = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationCritiquesBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_OperationCritiquesImprovementRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Help_eDocs = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_HelpTestData = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_HelpTestDataP_DriveTDR_Folder = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_HelpTestDataSQL_ReportingAndQuerying = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_HelpManuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_HelpManualsBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_HelpManualsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_HelpManualsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_HelpContents = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_HelpIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_HelpSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_HelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Administration = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrationPassword = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrationPasswordLogIn = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrationPasswordLogOut = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrationPasswordChange = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_AdministrationSeperator1 = new System.Windows.Forms.ToolStripSeparator();
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
            this.TMSI_Operation,
            this.TSMI_Administration,
            this.TSMI_Help});
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
            this.TMSI_FileSeperator1.Size = new System.Drawing.Size(140, 6);
            // 
            // TSMI_FilePrint
            // 
            this.TSMI_FilePrint.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_FilePrint.Image")));
            this.TSMI_FilePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_FilePrint.Name = "TSMI_FilePrint";
            this.TSMI_FilePrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.TSMI_FilePrint.Size = new System.Drawing.Size(143, 22);
            this.TSMI_FilePrint.Text = "&Print";
            this.TSMI_FilePrint.ToolTipText = "Print UUT results.";
            this.TSMI_FilePrint.Click += new System.EventHandler(this.TSMI_FilePrint_Click);
            // 
            // TSMI_FilePrintPreview
            // 
            this.TSMI_FilePrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_FilePrintPreview.Image")));
            this.TSMI_FilePrintPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_FilePrintPreview.Name = "TSMI_FilePrintPreview";
            this.TSMI_FilePrintPreview.Size = new System.Drawing.Size(143, 22);
            this.TSMI_FilePrintPreview.Text = "Print Pre&view";
            this.TSMI_FilePrintPreview.ToolTipText = "Preview UUT results.";
            this.TSMI_FilePrintPreview.Click += new System.EventHandler(this.TSMI_FilePrintPreview_Click);
            // 
            // TMSI_FileSeperator2
            // 
            this.TMSI_FileSeperator2.Name = "TMSI_FileSeperator2";
            this.TMSI_FileSeperator2.Size = new System.Drawing.Size(140, 6);
            // 
            // TSMI_FileExit
            // 
            this.TSMI_FileExit.Name = "TSMI_FileExit";
            this.TSMI_FileExit.Size = new System.Drawing.Size(143, 22);
            this.TSMI_FileExit.Text = "&Exit";
            this.TSMI_FileExit.ToolTipText = "Close application.";
            this.TSMI_FileExit.Click += new System.EventHandler(this.TSMI_FileExit_Click);
            // 
            // TMSI_Operation
            // 
            this.TMSI_Operation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_OperationConfigure,
            this.TMSI_OperationDiagnostics,
            this.TMSI_OperationSeperator1,
            this.TMSI_OperationCompliments,
            this.TMSI_OperationCritiques});
            this.TMSI_Operation.Name = "TMSI_Operation";
            this.TMSI_Operation.Size = new System.Drawing.Size(72, 20);
            this.TMSI_Operation.Text = "&Operation";
            // 
            // TMSI_OperationConfigure
            // 
            this.TMSI_OperationConfigure.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_OperationConfigureBarcodeScanner});
            this.TMSI_OperationConfigure.Name = "TMSI_OperationConfigure";
            this.TMSI_OperationConfigure.Size = new System.Drawing.Size(146, 22);
            this.TMSI_OperationConfigure.Text = "&Configure";
            // 
            // TMSI_OperationConfigureBarcodeScanner
            // 
            this.TMSI_OperationConfigureBarcodeScanner.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_OperationConfigureBarcodeScannerDiscover,
            this.TMSI_OperationConfigureBarcodeScannerProgramDefaults});
            this.TMSI_OperationConfigureBarcodeScanner.Name = "TMSI_OperationConfigureBarcodeScanner";
            this.TMSI_OperationConfigureBarcodeScanner.Size = new System.Drawing.Size(162, 22);
            this.TMSI_OperationConfigureBarcodeScanner.Text = "&Barcode Scanner";
            // 
            // TMSI_OperationConfigureBarcodeScannerDiscover
            // 
            this.TMSI_OperationConfigureBarcodeScannerDiscover.Name = "TMSI_OperationConfigureBarcodeScannerDiscover";
            this.TMSI_OperationConfigureBarcodeScannerDiscover.Size = new System.Drawing.Size(166, 22);
            this.TMSI_OperationConfigureBarcodeScannerDiscover.Text = "&Discover";
            this.TMSI_OperationConfigureBarcodeScannerDiscover.ToolTipText = "Corded scanners only; no Bluetooth or Wireless scanners.";
            this.TMSI_OperationConfigureBarcodeScannerDiscover.Click += new System.EventHandler(this.discoverToolStripMenuItem_Click);
            // 
            // TMSI_OperationConfigureBarcodeScannerProgramDefaults
            // 
            this.TMSI_OperationConfigureBarcodeScannerProgramDefaults.Name = "TMSI_OperationConfigureBarcodeScannerProgramDefaults";
            this.TMSI_OperationConfigureBarcodeScannerProgramDefaults.Size = new System.Drawing.Size(166, 22);
            this.TMSI_OperationConfigureBarcodeScannerProgramDefaults.Text = "&Program Defaults";
            this.TMSI_OperationConfigureBarcodeScannerProgramDefaults.ToolTipText = "Program Factory Reset & USB-HID mode.";
            this.TMSI_OperationConfigureBarcodeScannerProgramDefaults.Click += new System.EventHandler(this.programDefaultsToolStripMenuItem_Click);
            // 
            // TMSI_OperationDiagnostics
            // 
            this.TMSI_OperationDiagnostics.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_OperationDiagnosticsBarcodeScanner,
            this.TMSI_OperationDiagnosticsInstruments,
            this.TMSI_OperationDiagnosticsRelays});
            this.TMSI_OperationDiagnostics.Name = "TMSI_OperationDiagnostics";
            this.TMSI_OperationDiagnostics.Size = new System.Drawing.Size(146, 22);
            this.TMSI_OperationDiagnostics.Text = "&Diagnostics";
            // 
            // TMSI_OperationDiagnosticsBarcodeScanner
            // 
            this.TMSI_OperationDiagnosticsBarcodeScanner.Name = "TMSI_OperationDiagnosticsBarcodeScanner";
            this.TMSI_OperationDiagnosticsBarcodeScanner.Size = new System.Drawing.Size(162, 22);
            this.TMSI_OperationDiagnosticsBarcodeScanner.Text = "&Barcode Scanner";
            this.TMSI_OperationDiagnosticsBarcodeScanner.ToolTipText = "Barcode Scanner\'s power-on self-test.";
            this.TMSI_OperationDiagnosticsBarcodeScanner.Click += new System.EventHandler(this.barcodeScannerToolStripMenuItem1_Click);
            // 
            // TMSI_OperationDiagnosticsInstruments
            // 
            this.TMSI_OperationDiagnosticsInstruments.Name = "TMSI_OperationDiagnosticsInstruments";
            this.TMSI_OperationDiagnosticsInstruments.Size = new System.Drawing.Size(162, 22);
            this.TMSI_OperationDiagnosticsInstruments.Text = "&Instruments";
            this.TMSI_OperationDiagnosticsInstruments.ToolTipText = "Multi-select ListBox auto-populated from TestExecutive.config.xml.  Invoke SCPI_V" +
    "ISA_Instruments self-tests.";
            this.TMSI_OperationDiagnosticsInstruments.Click += new System.EventHandler(this.instrumentsToolStripMenuItem_Click);
            // 
            // TMSI_OperationDiagnosticsRelays
            // 
            this.TMSI_OperationDiagnosticsRelays.Enabled = false;
            this.TMSI_OperationDiagnosticsRelays.Name = "TMSI_OperationDiagnosticsRelays";
            this.TMSI_OperationDiagnosticsRelays.Size = new System.Drawing.Size(162, 22);
            this.TMSI_OperationDiagnosticsRelays.Text = "&Relays";
            this.TMSI_OperationDiagnosticsRelays.ToolTipText = "Adapt MS-Test Unit Tests of USB-ERB24 class.";
            this.TMSI_OperationDiagnosticsRelays.Click += new System.EventHandler(this.relaysToolStripMenuItem_Click);
            // 
            // TMSI_OperationSeperator1
            // 
            this.TMSI_OperationSeperator1.Name = "TMSI_OperationSeperator1";
            this.TMSI_OperationSeperator1.Size = new System.Drawing.Size(143, 6);
            // 
            // TMSI_OperationCompliments
            // 
            this.TMSI_OperationCompliments.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_OperationComplimentsPraiseAndPlaudits,
            this.TMSI_OperationComplimentsMoney});
            this.TMSI_OperationCompliments.Name = "TMSI_OperationCompliments";
            this.TMSI_OperationCompliments.Size = new System.Drawing.Size(146, 22);
            this.TMSI_OperationCompliments.Text = "Co&mpliments";
            // 
            // TMSI_OperationComplimentsPraiseAndPlaudits
            // 
            this.TMSI_OperationComplimentsPraiseAndPlaudits.Name = "TMSI_OperationComplimentsPraiseAndPlaudits";
            this.TMSI_OperationComplimentsPraiseAndPlaudits.Size = new System.Drawing.Size(163, 22);
            this.TMSI_OperationComplimentsPraiseAndPlaudits.Text = "&Praise && Plaudits";
            this.TMSI_OperationComplimentsPraiseAndPlaudits.ToolTipText = "\"I can live for two months on a good compliment.\" - Mark Twain";
            this.TMSI_OperationComplimentsPraiseAndPlaudits.Click += new System.EventHandler(this.praiseToolStripMenuItem_Click);
            // 
            // TMSI_OperationComplimentsMoney
            // 
            this.TMSI_OperationComplimentsMoney.Name = "TMSI_OperationComplimentsMoney";
            this.TMSI_OperationComplimentsMoney.Size = new System.Drawing.Size(163, 22);
            this.TMSI_OperationComplimentsMoney.Text = "&Money!";
            this.TMSI_OperationComplimentsMoney.ToolTipText = "For a good cause! ";
            this.TMSI_OperationComplimentsMoney.Click += new System.EventHandler(this.moneyToolStripMenuItem_Click);
            // 
            // TMSI_OperationCritiques
            // 
            this.TMSI_OperationCritiques.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_OperationCritiquesBugReport,
            this.TMSI_OperationCritiquesImprovementRequest});
            this.TMSI_OperationCritiques.Name = "TMSI_OperationCritiques";
            this.TMSI_OperationCritiques.Size = new System.Drawing.Size(146, 22);
            this.TMSI_OperationCritiques.Text = "Cri&tiques";
            // 
            // TMSI_OperationCritiquesBugReport
            // 
            this.TMSI_OperationCritiquesBugReport.Name = "TMSI_OperationCritiquesBugReport";
            this.TMSI_OperationCritiquesBugReport.Size = new System.Drawing.Size(191, 22);
            this.TMSI_OperationCritiquesBugReport.Text = "&Bug Report";
            this.TMSI_OperationCritiquesBugReport.ToolTipText = "Remember, \"The devil is is in the details.\" - Friedrich Nietzsche";
            this.TMSI_OperationCritiquesBugReport.Click += new System.EventHandler(this.reportBugToolStripMenuItem_Click);
            // 
            // TMSI_OperationCritiquesImprovementRequest
            // 
            this.TMSI_OperationCritiquesImprovementRequest.Name = "TMSI_OperationCritiquesImprovementRequest";
            this.TMSI_OperationCritiquesImprovementRequest.Size = new System.Drawing.Size(191, 22);
            this.TMSI_OperationCritiquesImprovementRequest.Text = "&Improvement Request";
            this.TMSI_OperationCritiquesImprovementRequest.ToolTipText = "Remember, \"God is in the details.\" - Mies van der Rohe";
            this.TMSI_OperationCritiquesImprovementRequest.Click += new System.EventHandler(this.requestImprovementToolStripMenuItem_Click);
            // 
            // TSMI_Help
            // 
            this.TSMI_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Help_eDocs,
            this.TSMI_HelpTestData,
            this.TSMI_HelpManuals,
            this.toolStripSeparator4,
            this.TSMI_HelpContents,
            this.TSMI_HelpIndex,
            this.TSMI_HelpSearch,
            this.toolStripSeparator5,
            this.TSMI_HelpAbout});
            this.TSMI_Help.Name = "TSMI_Help";
            this.TSMI_Help.Size = new System.Drawing.Size(44, 20);
            this.TSMI_Help.Text = "&Help";
            // 
            // TSMI_Help_eDocs
            // 
            this.TSMI_Help_eDocs.Name = "TSMI_Help_eDocs";
            this.TSMI_Help_eDocs.Size = new System.Drawing.Size(122, 22);
            this.TSMI_Help_eDocs.Text = "&eDocs";
            this.TSMI_Help_eDocs.ToolTipText = "UUT\'s P: drive eDocs folder.";
            this.TSMI_Help_eDocs.Click += new System.EventHandler(this.eDocsToolStripMenuItem_Click);
            // 
            // TSMI_HelpTestData
            // 
            this.TSMI_HelpTestData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_HelpTestDataP_DriveTDR_Folder,
            this.TSMI_HelpTestDataSQL_ReportingAndQuerying});
            this.TSMI_HelpTestData.Name = "TSMI_HelpTestData";
            this.TSMI_HelpTestData.Size = new System.Drawing.Size(122, 22);
            this.TSMI_HelpTestData.Text = "&Test Data";
            this.TSMI_HelpTestData.Click += new System.EventHandler(this.pDriveTDRFolderToolStripMenuItem_Click);
            // 
            // TSMI_HelpTestDataP_DriveTDR_Folder
            // 
            this.TSMI_HelpTestDataP_DriveTDR_Folder.Name = "TSMI_HelpTestDataP_DriveTDR_Folder";
            this.TSMI_HelpTestDataP_DriveTDR_Folder.Size = new System.Drawing.Size(215, 22);
            this.TSMI_HelpTestDataP_DriveTDR_Folder.Text = "&P: Drive TDR Folder";
            this.TSMI_HelpTestDataP_DriveTDR_Folder.ToolTipText = "P:\\Test\\TDR";
            this.TSMI_HelpTestDataP_DriveTDR_Folder.Click += new System.EventHandler(this.pDriveTDRFolderToolStripMenuItem_Click);
            // 
            // TSMI_HelpTestDataSQL_ReportingAndQuerying
            // 
            this.TSMI_HelpTestDataSQL_ReportingAndQuerying.Enabled = false;
            this.TSMI_HelpTestDataSQL_ReportingAndQuerying.Name = "TSMI_HelpTestDataSQL_ReportingAndQuerying";
            this.TSMI_HelpTestDataSQL_ReportingAndQuerying.Size = new System.Drawing.Size(215, 22);
            this.TSMI_HelpTestDataSQL_ReportingAndQuerying.Text = "&SQL Reporting && Querying";
            this.TSMI_HelpTestDataSQL_ReportingAndQuerying.ToolTipText = "Coming soon!";
            // 
            // TSMI_HelpManuals
            // 
            this.TSMI_HelpManuals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_HelpManualsBarcodeScanner,
            this.TSMI_HelpManualsInstruments,
            this.TSMI_HelpManualsRelays});
            this.TSMI_HelpManuals.Name = "TSMI_HelpManuals";
            this.TSMI_HelpManuals.Size = new System.Drawing.Size(122, 22);
            this.TSMI_HelpManuals.Text = "&Manuals";
            // 
            // TSMI_HelpManualsBarcodeScanner
            // 
            this.TSMI_HelpManualsBarcodeScanner.Name = "TSMI_HelpManualsBarcodeScanner";
            this.TSMI_HelpManualsBarcodeScanner.Size = new System.Drawing.Size(162, 22);
            this.TSMI_HelpManualsBarcodeScanner.Text = "&Barcode Scanner";
            this.TSMI_HelpManualsBarcodeScanner.ToolTipText = "If you\'re bored...";
            this.TSMI_HelpManualsBarcodeScanner.Click += new System.EventHandler(this.barcodeScannerToolStripMenuItem1_Click);
            // 
            // TSMI_HelpManualsInstruments
            // 
            this.TSMI_HelpManualsInstruments.Name = "TSMI_HelpManualsInstruments";
            this.TSMI_HelpManualsInstruments.Size = new System.Drawing.Size(162, 22);
            this.TSMI_HelpManualsInstruments.Text = "&Instruments";
            this.TSMI_HelpManualsInstruments.ToolTipText = "...really bored...";
            this.TSMI_HelpManualsInstruments.Click += new System.EventHandler(this.instrumentsToolStripMenuItem_Click);
            // 
            // TSMI_HelpManualsRelays
            // 
            this.TSMI_HelpManualsRelays.Name = "TSMI_HelpManualsRelays";
            this.TSMI_HelpManualsRelays.Size = new System.Drawing.Size(162, 22);
            this.TSMI_HelpManualsRelays.Text = "&Relays";
            this.TSMI_HelpManualsRelays.ToolTipText = "...zzzzzz...";
            this.TSMI_HelpManualsRelays.Click += new System.EventHandler(this.relaysToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(119, 6);
            // 
            // TSMI_HelpContents
            // 
            this.TSMI_HelpContents.Enabled = false;
            this.TSMI_HelpContents.Name = "TSMI_HelpContents";
            this.TSMI_HelpContents.Size = new System.Drawing.Size(122, 22);
            this.TSMI_HelpContents.Text = "&Contents";
            // 
            // TSMI_HelpIndex
            // 
            this.TSMI_HelpIndex.Enabled = false;
            this.TSMI_HelpIndex.Name = "TSMI_HelpIndex";
            this.TSMI_HelpIndex.Size = new System.Drawing.Size(122, 22);
            this.TSMI_HelpIndex.Text = "&Index";
            // 
            // TSMI_HelpSearch
            // 
            this.TSMI_HelpSearch.Enabled = false;
            this.TSMI_HelpSearch.Name = "TSMI_HelpSearch";
            this.TSMI_HelpSearch.Size = new System.Drawing.Size(122, 22);
            this.TSMI_HelpSearch.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // TSMI_HelpAbout
            // 
            this.TSMI_HelpAbout.Name = "TSMI_HelpAbout";
            this.TSMI_HelpAbout.Size = new System.Drawing.Size(122, 22);
            this.TSMI_HelpAbout.Text = "&About...";
            this.TSMI_HelpAbout.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // TSMI_Administration
            // 
            this.TSMI_Administration.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_AdministrationPassword,
            this.TMSI_AdministrationSeperator1,
            this.TSMI_AdministrationEdit,
            this.TMS});
            this.TSMI_Administration.Name = "TSMI_Administration";
            this.TSMI_Administration.Size = new System.Drawing.Size(98, 20);
            this.TSMI_Administration.Text = "&Administration";
            // 
            // TSMI_AdministrationPassword
            // 
            this.TSMI_AdministrationPassword.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_AdministrationPasswordLogIn,
            this.TSMI_AdministrationPasswordLogOut,
            this.TSMI_AdministrationPasswordChange});
            this.TSMI_AdministrationPassword.Name = "TSMI_AdministrationPassword";
            this.TSMI_AdministrationPassword.Size = new System.Drawing.Size(180, 22);
            this.TSMI_AdministrationPassword.Text = "&Password";
            // 
            // TSMI_AdministrationPasswordLogIn
            // 
            this.TSMI_AdministrationPasswordLogIn.Name = "TSMI_AdministrationPasswordLogIn";
            this.TSMI_AdministrationPasswordLogIn.Size = new System.Drawing.Size(180, 22);
            this.TSMI_AdministrationPasswordLogIn.Text = "Log &In";
            this.TSMI_AdministrationPasswordLogIn.ToolTipText = "Log in to enable Administration menu items.";
            this.TSMI_AdministrationPasswordLogIn.Click += new System.EventHandler(this.signInToolStripMenuItem_Click);
            // 
            // TSMI_AdministrationPasswordLogOut
            // 
            this.TSMI_AdministrationPasswordLogOut.Enabled = false;
            this.TSMI_AdministrationPasswordLogOut.Name = "TSMI_AdministrationPasswordLogOut";
            this.TSMI_AdministrationPasswordLogOut.Size = new System.Drawing.Size(180, 22);
            this.TSMI_AdministrationPasswordLogOut.Text = "Log &Out";
            this.TSMI_AdministrationPasswordLogOut.ToolTipText = "Log out to disable Administration menu items.";
            this.TSMI_AdministrationPasswordLogOut.Click += new System.EventHandler(this.signOutToolStripMenuItem_Click);
            // 
            // TSMI_AdministrationPasswordChange
            // 
            this.TSMI_AdministrationPasswordChange.Enabled = false;
            this.TSMI_AdministrationPasswordChange.Name = "TSMI_AdministrationPasswordChange";
            this.TSMI_AdministrationPasswordChange.Size = new System.Drawing.Size(180, 22);
            this.TSMI_AdministrationPasswordChange.Text = "&Change";
            this.TSMI_AdministrationPasswordChange.ToolTipText = "Change Adminstrative password.";
            this.TSMI_AdministrationPasswordChange.Click += new System.EventHandler(this.changeToolStripMenuItem_Click);
            // 
            // TMSI_AdministrationSeperator1
            // 
            this.TMSI_AdministrationSeperator1.Name = "TMSI_AdministrationSeperator1";
            this.TMSI_AdministrationSeperator1.Size = new System.Drawing.Size(177, 6);
            // 
            // TSMI_AdministrationEdit
            // 
            this.TSMI_AdministrationEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_AdministrationEditAppConfig,
            this.TSMI_AdministrationEditTestExecutiveXML});
            this.TSMI_AdministrationEdit.Enabled = false;
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
            this.TSMI_AdministrationEditAppConfig.Click += new System.EventHandler(this.appconfigToolStripMenuItem_Click);
            // 
            // TSMI_AdministrationEditTestExecutiveXML
            // 
            this.TSMI_AdministrationEditTestExecutiveXML.Name = "TSMI_AdministrationEditTestExecutiveXML";
            this.TSMI_AdministrationEditTestExecutiveXML.Size = new System.Drawing.Size(204, 22);
            this.TSMI_AdministrationEditTestExecutiveXML.Text = "&TestExecutive.config.xml";
            this.TSMI_AdministrationEditTestExecutiveXML.ToolTipText = "Test System\'s configuration.";
            this.TSMI_AdministrationEditTestExecutiveXML.Click += new System.EventHandler(this.testExecutiveconfigxmlToolStripMenuItem_Click);
            // 
            // TMS
            // 
            this.TMS.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keysightToolStripMenuItem,
            this.measurementComputingToolStripMenuItem,
            this.microsoftToolStripMenuItem});
            this.TMS.Enabled = false;
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
            this.benchVueToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.benchVueToolStripMenuItem.Text = "&BenchVue";
            this.benchVueToolStripMenuItem.ToolTipText = "Control Keysight Instruments via soft/virtual panels.";
            this.benchVueToolStripMenuItem.Click += new System.EventHandler(this.benchVueToolStripMenuItem_Click);
            // 
            // commandExpertToolStripMenuItem
            // 
            this.commandExpertToolStripMenuItem.Name = "commandExpertToolStripMenuItem";
            this.commandExpertToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.commandExpertToolStripMenuItem.Text = "Co&mmand Expert";
            this.commandExpertToolStripMenuItem.ToolTipText = "SCPI programming & debugging IDE.";
            this.commandExpertToolStripMenuItem.Click += new System.EventHandler(this.commandExpertToolStripMenuItem_Click);
            // 
            // connectionExpertToolStripMenuItem
            // 
            this.connectionExpertToolStripMenuItem.Name = "connectionExpertToolStripMenuItem";
            this.connectionExpertToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.connectionExpertToolStripMenuItem.Text = "Co&nnection Expert";
            this.connectionExpertToolStripMenuItem.ToolTipText = "Discover VISA Instruments.";
            this.connectionExpertToolStripMenuItem.Click += new System.EventHandler(this.connectionExpertToolStripMenuItem_Click);
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
            this.instaCalToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.instaCalToolStripMenuItem.Text = "&InstaCal";
            this.instaCalToolStripMenuItem.ToolTipText = "Configure & test MCC Instruments, like USB-ERB24 relays.";
            this.instaCalToolStripMenuItem.Click += new System.EventHandler(this.instaCalToolStripMenuItem_Click);
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
            this.sQLServerToolStripMenuItem.Click += new System.EventHandler(this.sQLServerToolStripMenuItem_Click);
            // 
            // visualStudioToolStripMenuItem
            // 
            this.visualStudioToolStripMenuItem.Name = "visualStudioToolStripMenuItem";
            this.visualStudioToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.visualStudioToolStripMenuItem.Text = "&Visual Studio";
            this.visualStudioToolStripMenuItem.ToolTipText = "C# forever!";
            this.visualStudioToolStripMenuItem.Click += new System.EventHandler(this.visualStudioToolStripMenuItem_Click);
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
        private ToolStripMenuItem TSMI_Help;
        private ToolStripMenuItem TSMI_HelpContents;
        private ToolStripMenuItem TSMI_HelpSearch;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem TSMI_HelpAbout;
        private ToolStripMenuItem TMSI_Operation;
        private ToolStripMenuItem TSMI_Administration;
        private ToolStripMenuItem TSMI_AdministrationPassword;
        private ToolStripMenuItem TSMI_AdministrationPasswordLogIn;
        private ToolStripMenuItem TSMI_AdministrationPasswordLogOut;
        private ToolStripMenuItem TSMI_AdministrationPasswordChange;
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
        private ToolStripMenuItem TSMI_HelpManuals;
        private ToolStripMenuItem TSMI_HelpManualsBarcodeScanner;
        private ToolStripMenuItem TSMI_HelpManualsInstruments;
        private ToolStripMenuItem TSMI_HelpManualsRelays;
        private ToolStripMenuItem TMSI_OperationConfigure;
        private ToolStripMenuItem TMSI_OperationConfigureBarcodeScanner;
        private ToolStripMenuItem TMSI_OperationConfigureBarcodeScannerDiscover;
        private ToolStripMenuItem TMSI_OperationConfigureBarcodeScannerProgramDefaults;
        private ToolStripMenuItem TMSI_OperationDiagnostics;
        private ToolStripMenuItem TMSI_OperationDiagnosticsBarcodeScanner;
        private ToolStripMenuItem TMSI_OperationDiagnosticsInstruments;
        private ToolStripMenuItem TMSI_OperationDiagnosticsRelays;
        private ToolStripSeparator TMSI_OperationSeperator1;
        private ToolStripMenuItem TMSI_OperationCompliments;
        private ToolStripMenuItem TMSI_OperationComplimentsPraiseAndPlaudits;
        private ToolStripMenuItem TMSI_OperationComplimentsMoney;
        private ToolStripMenuItem TMSI_OperationCritiques;
        private ToolStripMenuItem TMSI_OperationCritiquesBugReport;
        private ToolStripMenuItem TMSI_OperationCritiquesImprovementRequest;
        private ToolStripSeparator TMSI_AdministrationSeperator1;
        private ToolStripMenuItem TSMI_Help_eDocs;
        private ToolStripMenuItem TSMI_HelpTestData;
        private ToolStripMenuItem TSMI_HelpTestDataP_DriveTDR_Folder;
        private ToolStripMenuItem TSMI_HelpTestDataSQL_ReportingAndQuerying;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem TSMI_HelpIndex;
    }
}
