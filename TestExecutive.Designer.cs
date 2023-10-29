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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eDocsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pDriveTDRFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLReportingQueryingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barcodeScannerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discoverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.programDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diagnosticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barcodeScannerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.instrumentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.relaysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.submissionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.complimentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.praiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moneyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.criticismToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportBugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.requestImprovementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.passwordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.signInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.signOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appconfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testExecutiveconfigxmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keysightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.benchVueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commandExpertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionExpertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.measurementComputingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instaCalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.microsoftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualStudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
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
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.operationToolStripMenuItem,
            this.administrationToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1045, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.TabStop = true;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(185, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.ToolTipText = "Save UUT results.";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.printToolStripMenuItem.Text = "&Print";
            this.printToolStripMenuItem.ToolTipText = "Print UUT results.";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
            this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
            this.printPreviewToolStripMenuItem.ToolTipText = "Preview UUT results.";
            this.printPreviewToolStripMenuItem.Click += new System.EventHandler(this.printPreviewToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.ToolTipText = "Close application.";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // operationToolStripMenuItem
            // 
            this.operationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eDocsToolStripMenuItem,
            this.testDataToolStripMenuItem,
            this.systemToolStripMenuItem,
            this.submissionsToolStripMenuItem});
            this.operationToolStripMenuItem.Name = "operationToolStripMenuItem";
            this.operationToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.operationToolStripMenuItem.Text = "&Operation";
            // 
            // eDocsToolStripMenuItem
            // 
            this.eDocsToolStripMenuItem.Name = "eDocsToolStripMenuItem";
            this.eDocsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.eDocsToolStripMenuItem.Text = "&eDocs";
            this.eDocsToolStripMenuItem.ToolTipText = "UUT\'s P: drive eDocs folder.";
            this.eDocsToolStripMenuItem.Click += new System.EventHandler(this.eDocsToolStripMenuItem_Click);
            // 
            // testDataToolStripMenuItem
            // 
            this.testDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pDriveTDRFolderToolStripMenuItem,
            this.sQLReportingQueryingToolStripMenuItem});
            this.testDataToolStripMenuItem.Name = "testDataToolStripMenuItem";
            this.testDataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.testDataToolStripMenuItem.Text = "&Test Data";
            // 
            // pDriveTDRFolderToolStripMenuItem
            // 
            this.pDriveTDRFolderToolStripMenuItem.Name = "pDriveTDRFolderToolStripMenuItem";
            this.pDriveTDRFolderToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.pDriveTDRFolderToolStripMenuItem.Text = "&P: Drive TDR Folder";
            this.pDriveTDRFolderToolStripMenuItem.ToolTipText = "P:\\Test\\TDR";
            this.pDriveTDRFolderToolStripMenuItem.Click += new System.EventHandler(this.pDriveTDRFolderToolStripMenuItem_Click);
            // 
            // sQLReportingQueryingToolStripMenuItem
            // 
            this.sQLReportingQueryingToolStripMenuItem.Enabled = false;
            this.sQLReportingQueryingToolStripMenuItem.Name = "sQLReportingQueryingToolStripMenuItem";
            this.sQLReportingQueryingToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.sQLReportingQueryingToolStripMenuItem.Text = "&SQL Reporting && Querying";
            this.sQLReportingQueryingToolStripMenuItem.ToolTipText = "Coming soon!";
            this.sQLReportingQueryingToolStripMenuItem.Click += new System.EventHandler(this.sQLReportingQueryingToolStripMenuItem_Click);
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem,
            this.diagnosticsToolStripMenuItem});
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.systemToolStripMenuItem.Text = "&System";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barcodeScannerToolStripMenuItem});
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.configureToolStripMenuItem.Text = "&Configure";
            // 
            // barcodeScannerToolStripMenuItem
            // 
            this.barcodeScannerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.discoverToolStripMenuItem,
            this.programDefaultsToolStripMenuItem});
            this.barcodeScannerToolStripMenuItem.Name = "barcodeScannerToolStripMenuItem";
            this.barcodeScannerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.barcodeScannerToolStripMenuItem.Text = "&Barcode Scanner";
            // 
            // discoverToolStripMenuItem
            // 
            this.discoverToolStripMenuItem.Name = "discoverToolStripMenuItem";
            this.discoverToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.discoverToolStripMenuItem.Text = "&Discover";
            this.discoverToolStripMenuItem.ToolTipText = "Corded scanners only; no Bluetooth or Wireless scanners.";
            this.discoverToolStripMenuItem.Click += new System.EventHandler(this.discoverToolStripMenuItem_Click);
            // 
            // programDefaultsToolStripMenuItem
            // 
            this.programDefaultsToolStripMenuItem.Name = "programDefaultsToolStripMenuItem";
            this.programDefaultsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.programDefaultsToolStripMenuItem.Text = "&Program Defaults";
            this.programDefaultsToolStripMenuItem.ToolTipText = "Program Factory Reset & USB-HID mode.";
            this.programDefaultsToolStripMenuItem.Click += new System.EventHandler(this.programDefaultsToolStripMenuItem_Click);
            // 
            // diagnosticsToolStripMenuItem
            // 
            this.diagnosticsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barcodeScannerToolStripMenuItem1,
            this.instrumentsToolStripMenuItem,
            this.relaysToolStripMenuItem});
            this.diagnosticsToolStripMenuItem.Name = "diagnosticsToolStripMenuItem";
            this.diagnosticsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.diagnosticsToolStripMenuItem.Text = "&Diagnostics";
            // 
            // barcodeScannerToolStripMenuItem1
            // 
            this.barcodeScannerToolStripMenuItem1.Name = "barcodeScannerToolStripMenuItem1";
            this.barcodeScannerToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.barcodeScannerToolStripMenuItem1.Text = "&Barcode Scanner";
            this.barcodeScannerToolStripMenuItem1.ToolTipText = "Barcode Scanner\'s power-on self-test.";
            this.barcodeScannerToolStripMenuItem1.Click += new System.EventHandler(this.barcodeScannerToolStripMenuItem1_Click);
            // 
            // instrumentsToolStripMenuItem
            // 
            this.instrumentsToolStripMenuItem.Name = "instrumentsToolStripMenuItem";
            this.instrumentsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.instrumentsToolStripMenuItem.Text = "&Instruments";
            this.instrumentsToolStripMenuItem.ToolTipText = "Multi-select ListBox auto-populated from TestExecutive.config.xml.  Invoke SCPI_V" +
    "ISA_Instruments self-tests.";
            this.instrumentsToolStripMenuItem.Click += new System.EventHandler(this.instrumentsToolStripMenuItem_Click);
            // 
            // relaysToolStripMenuItem
            // 
            this.relaysToolStripMenuItem.Enabled = false;
            this.relaysToolStripMenuItem.Name = "relaysToolStripMenuItem";
            this.relaysToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.relaysToolStripMenuItem.Text = "&Relays";
            this.relaysToolStripMenuItem.ToolTipText = "Adapt MS-Test Unit Tests of USB-ERB24 class.";
            this.relaysToolStripMenuItem.Click += new System.EventHandler(this.relaysToolStripMenuItem_Click);
            // 
            // submissionsToolStripMenuItem
            // 
            this.submissionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.complimentsToolStripMenuItem,
            this.criticismToolStripMenuItem});
            this.submissionsToolStripMenuItem.Name = "submissionsToolStripMenuItem";
            this.submissionsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.submissionsToolStripMenuItem.Text = "Su&bmissions";
            // 
            // complimentsToolStripMenuItem
            // 
            this.complimentsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.praiseToolStripMenuItem,
            this.moneyToolStripMenuItem});
            this.complimentsToolStripMenuItem.Name = "complimentsToolStripMenuItem";
            this.complimentsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.complimentsToolStripMenuItem.Text = "Co&mpliments";
            // 
            // praiseToolStripMenuItem
            // 
            this.praiseToolStripMenuItem.Name = "praiseToolStripMenuItem";
            this.praiseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.praiseToolStripMenuItem.Text = "&Praise && Plaudits";
            this.praiseToolStripMenuItem.ToolTipText = "\"I can live for two months on a good compliment.\" - Mark Twain";
            this.praiseToolStripMenuItem.Click += new System.EventHandler(this.praiseToolStripMenuItem_Click);
            // 
            // moneyToolStripMenuItem
            // 
            this.moneyToolStripMenuItem.Name = "moneyToolStripMenuItem";
            this.moneyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.moneyToolStripMenuItem.Text = "&Money!";
            this.moneyToolStripMenuItem.ToolTipText = "For a good cause! ";
            this.moneyToolStripMenuItem.Click += new System.EventHandler(this.moneyToolStripMenuItem_Click);
            // 
            // criticismToolStripMenuItem
            // 
            this.criticismToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportBugToolStripMenuItem,
            this.requestImprovementToolStripMenuItem});
            this.criticismToolStripMenuItem.Name = "criticismToolStripMenuItem";
            this.criticismToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.criticismToolStripMenuItem.Text = "Cri&ticism";
            // 
            // reportBugToolStripMenuItem
            // 
            this.reportBugToolStripMenuItem.Name = "reportBugToolStripMenuItem";
            this.reportBugToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.reportBugToolStripMenuItem.Text = "&Bug Report";
            this.reportBugToolStripMenuItem.ToolTipText = "Remember, \"The devil is is in the details.\" - Friedrich Nietzsche";
            this.reportBugToolStripMenuItem.Click += new System.EventHandler(this.reportBugToolStripMenuItem_Click);
            // 
            // requestImprovementToolStripMenuItem
            // 
            this.requestImprovementToolStripMenuItem.Name = "requestImprovementToolStripMenuItem";
            this.requestImprovementToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.requestImprovementToolStripMenuItem.Text = "&Improvement Request";
            this.requestImprovementToolStripMenuItem.ToolTipText = "Remember, \"God is in the details.\" - Mies van der Rohe";
            this.requestImprovementToolStripMenuItem.Click += new System.EventHandler(this.requestImprovementToolStripMenuItem_Click);
            // 
            // administrationToolStripMenuItem
            // 
            this.administrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.passwordToolStripMenuItem,
            this.editToolStripMenuItem,
            this.launchToolStripMenuItem});
            this.administrationToolStripMenuItem.Name = "administrationToolStripMenuItem";
            this.administrationToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.administrationToolStripMenuItem.Text = "&Administration";
            // 
            // passwordToolStripMenuItem
            // 
            this.passwordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.signInToolStripMenuItem,
            this.signOutToolStripMenuItem,
            this.changeToolStripMenuItem});
            this.passwordToolStripMenuItem.Name = "passwordToolStripMenuItem";
            this.passwordToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.passwordToolStripMenuItem.Text = "&Password";
            // 
            // signInToolStripMenuItem
            // 
            this.signInToolStripMenuItem.Name = "signInToolStripMenuItem";
            this.signInToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.signInToolStripMenuItem.Text = "Sign &In";
            this.signInToolStripMenuItem.ToolTipText = "Sign in to enable Administration menu items.";
            this.signInToolStripMenuItem.Click += new System.EventHandler(this.signInToolStripMenuItem_Click);
            // 
            // signOutToolStripMenuItem
            // 
            this.signOutToolStripMenuItem.Enabled = false;
            this.signOutToolStripMenuItem.Name = "signOutToolStripMenuItem";
            this.signOutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.signOutToolStripMenuItem.Text = "Sign &Out";
            this.signOutToolStripMenuItem.ToolTipText = "Sign out to disable Administration menu items.";
            this.signOutToolStripMenuItem.Click += new System.EventHandler(this.signOutToolStripMenuItem_Click);
            // 
            // changeToolStripMenuItem
            // 
            this.changeToolStripMenuItem.Enabled = false;
            this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
            this.changeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.changeToolStripMenuItem.Text = "&Change";
            this.changeToolStripMenuItem.ToolTipText = "Change Adminstrative password.";
            this.changeToolStripMenuItem.Click += new System.EventHandler(this.changeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appconfigToolStripMenuItem,
            this.testExecutiveconfigxmlToolStripMenuItem});
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // appconfigToolStripMenuItem
            // 
            this.appconfigToolStripMenuItem.Name = "appconfigToolStripMenuItem";
            this.appconfigToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.appconfigToolStripMenuItem.Text = "&App.config";
            this.appconfigToolStripMenuItem.ToolTipText = "UUT\'s test configuration.";
            this.appconfigToolStripMenuItem.Click += new System.EventHandler(this.appconfigToolStripMenuItem_Click);
            // 
            // testExecutiveconfigxmlToolStripMenuItem
            // 
            this.testExecutiveconfigxmlToolStripMenuItem.Name = "testExecutiveconfigxmlToolStripMenuItem";
            this.testExecutiveconfigxmlToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.testExecutiveconfigxmlToolStripMenuItem.Text = "&TestExecutive.config.xml";
            this.testExecutiveconfigxmlToolStripMenuItem.ToolTipText = "Test System\'s configuration.";
            this.testExecutiveconfigxmlToolStripMenuItem.Click += new System.EventHandler(this.testExecutiveconfigxmlToolStripMenuItem_Click);
            // 
            // launchToolStripMenuItem
            // 
            this.launchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keysightToolStripMenuItem,
            this.measurementComputingToolStripMenuItem,
            this.microsoftToolStripMenuItem});
            this.launchToolStripMenuItem.Enabled = false;
            this.launchToolStripMenuItem.Name = "launchToolStripMenuItem";
            this.launchToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.launchToolStripMenuItem.Text = "&Launch";
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
            this.benchVueToolStripMenuItem.Click += new System.EventHandler(this.benchVueToolStripMenuItem_Click);
            // 
            // commandExpertToolStripMenuItem
            // 
            this.commandExpertToolStripMenuItem.Name = "commandExpertToolStripMenuItem";
            this.commandExpertToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.commandExpertToolStripMenuItem.Text = "Co&mmand Expert";
            this.commandExpertToolStripMenuItem.ToolTipText = "SCPI programming & debugging IDE.";
            this.commandExpertToolStripMenuItem.Click += new System.EventHandler(this.commandExpertToolStripMenuItem_Click);
            // 
            // connectionExpertToolStripMenuItem
            // 
            this.connectionExpertToolStripMenuItem.Name = "connectionExpertToolStripMenuItem";
            this.connectionExpertToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            this.instaCalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "&Manuals";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem2.Text = "&Barcode Scanner";
            this.toolStripMenuItem2.ToolTipText = "If you\'re bored...";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem3.Text = "&Instruments";
            this.toolStripMenuItem3.ToolTipText = "...really bored...";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem4.Text = "&Relays";
            this.toolStripMenuItem4.ToolTipText = "...zzzzzz...";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(177, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
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
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TestExecutive";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Program";
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem printPreviewToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem contentsToolStripMenuItem;
        private ToolStripMenuItem indexToolStripMenuItem;
        private ToolStripMenuItem searchToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem operationToolStripMenuItem;
        private ToolStripMenuItem eDocsToolStripMenuItem;
        private ToolStripMenuItem testDataToolStripMenuItem;
        private ToolStripMenuItem pDriveTDRFolderToolStripMenuItem;
        private ToolStripMenuItem sQLReportingQueryingToolStripMenuItem;
        private ToolStripMenuItem systemToolStripMenuItem;
        private ToolStripMenuItem submissionsToolStripMenuItem;
        private ToolStripMenuItem administrationToolStripMenuItem;
        private ToolStripMenuItem configureToolStripMenuItem;
        private ToolStripMenuItem barcodeScannerToolStripMenuItem;
        private ToolStripMenuItem discoverToolStripMenuItem;
        private ToolStripMenuItem programDefaultsToolStripMenuItem;
        private ToolStripMenuItem diagnosticsToolStripMenuItem;
        private ToolStripMenuItem barcodeScannerToolStripMenuItem1;
        private ToolStripMenuItem instrumentsToolStripMenuItem;
        private ToolStripMenuItem relaysToolStripMenuItem;
        private ToolStripMenuItem complimentsToolStripMenuItem;
        private ToolStripMenuItem criticismToolStripMenuItem;
        private ToolStripMenuItem praiseToolStripMenuItem;
        private ToolStripMenuItem moneyToolStripMenuItem;
        private ToolStripMenuItem reportBugToolStripMenuItem;
        private ToolStripMenuItem requestImprovementToolStripMenuItem;
        private ToolStripMenuItem passwordToolStripMenuItem;
        private ToolStripMenuItem signInToolStripMenuItem;
        private ToolStripMenuItem signOutToolStripMenuItem;
        private ToolStripMenuItem changeToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem appconfigToolStripMenuItem;
        private ToolStripMenuItem testExecutiveconfigxmlToolStripMenuItem;
        private ToolStripMenuItem launchToolStripMenuItem;
        private ToolStripMenuItem keysightToolStripMenuItem;
        private ToolStripMenuItem benchVueToolStripMenuItem;
        private ToolStripMenuItem commandExpertToolStripMenuItem;
        private ToolStripMenuItem connectionExpertToolStripMenuItem;
        private ToolStripMenuItem measurementComputingToolStripMenuItem;
        private ToolStripMenuItem instaCalToolStripMenuItem;
        private ToolStripMenuItem microsoftToolStripMenuItem;
        private ToolStripMenuItem sQLServerToolStripMenuItem;
        private ToolStripMenuItem visualStudioToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
    }
}
