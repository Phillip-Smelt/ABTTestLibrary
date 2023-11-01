﻿using System.Windows.Forms;

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
            this.TMSI_FileSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_File_Print = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_File_PrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_FileSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_File_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_BarcodeScannerDiscovery = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_Diagnostics = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_DiagnosticsBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_DiagnosticsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_DiagnosticsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_Manuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_ManualsBarcodeScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_ManualsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_ManualsRelays = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_System_TestExecutiveConfigXML = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TMSI_System_Compliments = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_ComplimentsPraiseAndPlaudits = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_ComplimentsMoney = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_Critiques = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_CritiquesBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_CritiquesImprovementRequest = new System.Windows.Forms.ToolStripMenuItem();
            this.TMSI_System_Separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_System_About = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_AppConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_eDocs = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_Manuals = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_ManualsInstruments = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestData = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestDataP_DriveTDR_Folder = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_UUT_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMI_UUT_About = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_LaunchMeasurementComputing = new System.Windows.Forms.ToolStripMenuItem();
            this.instaCalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_LaunchKeysight = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_LaunchKeysightBenchVue = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_LaunchKeysightCommandExpert = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_LaunchKeysightConnectionExpert = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Apps_LaunchMicrosoft = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualStudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MS.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonStart.BackColor = System.Drawing.Color.Green;
            this.ButtonStart.Location = new System.Drawing.Point(203, 694);
            this.ButtonStart.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(117, 64);
            this.ButtonStart.TabIndex = 1;
            this.ButtonStart.Text = "&Start";
            this.ButtonStart.UseVisualStyleBackColor = false;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Clicked);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonCancel.BackColor = System.Drawing.Color.Yellow;
            this.ButtonCancel.Location = new System.Drawing.Point(375, 694);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(117, 64);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = false;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Clicked);
            // 
            // TextResult
            // 
            this.TextResult.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TextResult.Location = new System.Drawing.Point(659, 720);
            this.TextResult.Margin = new System.Windows.Forms.Padding(4);
            this.TextResult.Name = "TextResult";
            this.TextResult.ReadOnly = true;
            this.TextResult.Size = new System.Drawing.Size(79, 22);
            this.TextResult.TabIndex = 9;
            this.TextResult.TabStop = false;
            this.TextResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelResult
            // 
            this.LabelResult.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LabelResult.AutoSize = true;
            this.LabelResult.Location = new System.Drawing.Point(673, 695);
            this.LabelResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelResult.Name = "LabelResult";
            this.LabelResult.Size = new System.Drawing.Size(45, 16);
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
            this.rtfResults.Location = new System.Drawing.Point(31, 26);
            this.rtfResults.Margin = new System.Windows.Forms.Padding(4);
            this.rtfResults.Name = "rtfResults";
            this.rtfResults.ReadOnly = true;
            this.rtfResults.Size = new System.Drawing.Size(1333, 640);
            this.rtfResults.TabIndex = 7;
            this.rtfResults.TabStop = false;
            this.rtfResults.Text = "";
            // 
            // ButtonSelectTests
            // 
            this.ButtonSelectTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonSelectTests.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonSelectTests.Location = new System.Drawing.Point(31, 695);
            this.ButtonSelectTests.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonSelectTests.Name = "ButtonSelectTests";
            this.ButtonSelectTests.Size = new System.Drawing.Size(117, 58);
            this.ButtonSelectTests.TabIndex = 0;
            this.ButtonSelectTests.Text = "Select &Tests";
            this.ButtonSelectTests.UseVisualStyleBackColor = true;
            this.ButtonSelectTests.Click += new System.EventHandler(this.ButtonSelectTests_Click);
            // 
            // ButtonEmergencyStop
            // 
            this.ButtonEmergencyStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonEmergencyStop.Image = global::ABT.TestSpace.Properties.Resources.EmergencyStop;
            this.ButtonEmergencyStop.Location = new System.Drawing.Point(1259, 671);
            this.ButtonEmergencyStop.Margin = new System.Windows.Forms.Padding(4);
            this.ButtonEmergencyStop.Name = "ButtonEmergencyStop";
            this.ButtonEmergencyStop.Size = new System.Drawing.Size(107, 98);
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
            this.TMSI_System,
            this.TSMI_UUT});
            this.MS.Location = new System.Drawing.Point(0, 0);
            this.MS.Name = "MS";
            this.MS.Size = new System.Drawing.Size(1393, 28);
            this.MS.TabIndex = 6;
            this.MS.TabStop = true;
            this.MS.Text = "menuStrip1";
            // 
            // TSMI_File
            // 
            this.TSMI_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_File_Save,
            this.TMSI_FileSeparator1,
            this.TSMI_File_Print,
            this.TSMI_File_PrintPreview,
            this.TMSI_FileSeparator2,
            this.TSMI_File_Exit});
            this.TSMI_File.Name = "TSMI_File";
            this.TSMI_File.Size = new System.Drawing.Size(46, 24);
            this.TSMI_File.Text = "&File";
            // 
            // TSMI_File_Save
            // 
            this.TSMI_File_Save.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_File_Save.Image")));
            this.TSMI_File_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_File_Save.Name = "TSMI_File_Save";
            this.TSMI_File_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.TSMI_File_Save.Size = new System.Drawing.Size(228, 30);
            this.TSMI_File_Save.Text = "&Save";
            this.TSMI_File_Save.ToolTipText = "Save UUT results.";
            this.TSMI_File_Save.Click += new System.EventHandler(this.TSMI_File_Save_Click);
            // 
            // TMSI_FileSeparator1
            // 
            this.TMSI_FileSeparator1.Name = "TMSI_FileSeparator1";
            this.TMSI_FileSeparator1.Size = new System.Drawing.Size(225, 6);
            // 
            // TSMI_File_Print
            // 
            this.TSMI_File_Print.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_File_Print.Image")));
            this.TSMI_File_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_File_Print.Name = "TSMI_File_Print";
            this.TSMI_File_Print.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.TSMI_File_Print.Size = new System.Drawing.Size(228, 30);
            this.TSMI_File_Print.Text = "&Print";
            this.TSMI_File_Print.ToolTipText = "Print UUT results.";
            this.TSMI_File_Print.Click += new System.EventHandler(this.TSMI_File_Print_Click);
            // 
            // TSMI_File_PrintPreview
            // 
            this.TSMI_File_PrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("TSMI_File_PrintPreview.Image")));
            this.TSMI_File_PrintPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSMI_File_PrintPreview.Name = "TSMI_File_PrintPreview";
            this.TSMI_File_PrintPreview.Size = new System.Drawing.Size(228, 30);
            this.TSMI_File_PrintPreview.Text = "Print Pre&view";
            this.TSMI_File_PrintPreview.ToolTipText = "Preview UUT results.";
            this.TSMI_File_PrintPreview.Click += new System.EventHandler(this.TSMI_File_PrintPreview_Click);
            // 
            // TMSI_FileSeparator2
            // 
            this.TMSI_FileSeparator2.Name = "TMSI_FileSeparator2";
            this.TMSI_FileSeparator2.Size = new System.Drawing.Size(225, 6);
            // 
            // TSMI_File_Exit
            // 
            this.TSMI_File_Exit.Name = "TSMI_File_Exit";
            this.TSMI_File_Exit.Size = new System.Drawing.Size(228, 30);
            this.TSMI_File_Exit.Text = "&Exit";
            this.TSMI_File_Exit.ToolTipText = "Close application.";
            this.TSMI_File_Exit.Click += new System.EventHandler(this.TSMI_File_Exit_Click);
            // 
            // TSMI_Apps
            // 
            this.TSMI_Apps.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Apps_LaunchKeysight,
            this.TSMI_Apps_LaunchMeasurementComputing,
            this.TSMI_Apps_LaunchMicrosoft});
            this.TSMI_Apps.Name = "TSMI_Apps";
            this.TSMI_Apps.Size = new System.Drawing.Size(61, 24);
            this.TSMI_Apps.Text = " &Apps";
            // 
            // TMSI_System
            // 
            this.TMSI_System.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_System_BarcodeScannerDiscovery,
            this.TMSI_System_Diagnostics,
            this.TSMI_System_Manuals,
            this.TSMI_System_TestExecutiveConfigXML,
            this.TMSI_System_Separator1,
            this.TMSI_System_Compliments,
            this.TMSI_System_Critiques,
            this.TMSI_System_Separator2,
            this.TSMI_System_About});
            this.TMSI_System.Name = "TMSI_System";
            this.TMSI_System.Size = new System.Drawing.Size(70, 24);
            this.TMSI_System.Text = "S&ystem";
            // 
            // TMSI_System_BarcodeScannerDiscovery
            // 
            this.TMSI_System_BarcodeScannerDiscovery.Name = "TMSI_System_BarcodeScannerDiscovery";
            this.TMSI_System_BarcodeScannerDiscovery.Size = new System.Drawing.Size(271, 26);
            this.TMSI_System_BarcodeScannerDiscovery.Text = "&Barcode Scanner Discovery";
            this.TMSI_System_BarcodeScannerDiscovery.ToolTipText = "Corded scanners only; no Bluetooth or Wireless scanners.";
            this.TMSI_System_BarcodeScannerDiscovery.Click += new System.EventHandler(this.TSMI_System_BarcodeScannerDiscovery_Click);
            // 
            // TMSI_System_Diagnostics
            // 
            this.TMSI_System_Diagnostics.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_System_DiagnosticsBarcodeScanner,
            this.TMSI_System_DiagnosticsInstruments,
            this.TMSI_System_DiagnosticsRelays});
            this.TMSI_System_Diagnostics.Name = "TMSI_System_Diagnostics";
            this.TMSI_System_Diagnostics.Size = new System.Drawing.Size(271, 26);
            this.TMSI_System_Diagnostics.Text = "&Diagnostics";
            // 
            // TMSI_System_DiagnosticsBarcodeScanner
            // 
            this.TMSI_System_DiagnosticsBarcodeScanner.Name = "TMSI_System_DiagnosticsBarcodeScanner";
            this.TMSI_System_DiagnosticsBarcodeScanner.Size = new System.Drawing.Size(224, 26);
            this.TMSI_System_DiagnosticsBarcodeScanner.Text = "&Barcode Scanner";
            this.TMSI_System_DiagnosticsBarcodeScanner.ToolTipText = "Barcode Scanner\'s power-on self-test.";
            this.TMSI_System_DiagnosticsBarcodeScanner.Click += new System.EventHandler(this.TSMI_System_DiagnosticsBarcodeScanner_Click);
            // 
            // TMSI_System_DiagnosticsInstruments
            // 
            this.TMSI_System_DiagnosticsInstruments.Name = "TMSI_System_DiagnosticsInstruments";
            this.TMSI_System_DiagnosticsInstruments.Size = new System.Drawing.Size(224, 26);
            this.TMSI_System_DiagnosticsInstruments.Text = "&Instruments";
            this.TMSI_System_DiagnosticsInstruments.ToolTipText = "Multi-select ListBox auto-populated from TestExecutive.config.xml.  Invoke SCPI_V" +
    "ISA_Instruments self-tests.";
            this.TMSI_System_DiagnosticsInstruments.Click += new System.EventHandler(this.TSMI_System_DiagnosticsInstruments_Click);
            // 
            // TMSI_System_DiagnosticsRelays
            // 
            this.TMSI_System_DiagnosticsRelays.Enabled = false;
            this.TMSI_System_DiagnosticsRelays.Name = "TMSI_System_DiagnosticsRelays";
            this.TMSI_System_DiagnosticsRelays.Size = new System.Drawing.Size(224, 26);
            this.TMSI_System_DiagnosticsRelays.Text = "&Relays";
            this.TMSI_System_DiagnosticsRelays.ToolTipText = "Adapt MS-Test Unit Tests of USB-ERB24 class.";
            this.TMSI_System_DiagnosticsRelays.Click += new System.EventHandler(this.TSMI_System_DiagnosticsRelays_Click);
            // 
            // TSMI_System_Manuals
            // 
            this.TSMI_System_Manuals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_System_ManualsBarcodeScanner,
            this.TSMI_System_ManualsInstruments,
            this.TSMI_System_ManualsRelays});
            this.TSMI_System_Manuals.Name = "TSMI_System_Manuals";
            this.TSMI_System_Manuals.Size = new System.Drawing.Size(271, 26);
            this.TSMI_System_Manuals.Text = "&Manuals";
            // 
            // TSMI_System_ManualsBarcodeScanner
            // 
            this.TSMI_System_ManualsBarcodeScanner.Name = "TSMI_System_ManualsBarcodeScanner";
            this.TSMI_System_ManualsBarcodeScanner.Size = new System.Drawing.Size(224, 26);
            this.TSMI_System_ManualsBarcodeScanner.Text = "&Barcode Scanner";
            this.TSMI_System_ManualsBarcodeScanner.ToolTipText = "If you\'re bored...";
            this.TSMI_System_ManualsBarcodeScanner.Click += new System.EventHandler(this.TSMI_System_ManualsBarcodeScanner_Click);
            // 
            // TSMI_System_ManualsInstruments
            // 
            this.TSMI_System_ManualsInstruments.Name = "TSMI_System_ManualsInstruments";
            this.TSMI_System_ManualsInstruments.Size = new System.Drawing.Size(224, 26);
            this.TSMI_System_ManualsInstruments.Text = "&Instruments";
            this.TSMI_System_ManualsInstruments.ToolTipText = "...really bored...";
            this.TSMI_System_ManualsInstruments.Click += new System.EventHandler(this.TSMI_System_ManualsInstruments_Click);
            // 
            // TSMI_System_ManualsRelays
            // 
            this.TSMI_System_ManualsRelays.Name = "TSMI_System_ManualsRelays";
            this.TSMI_System_ManualsRelays.Size = new System.Drawing.Size(224, 26);
            this.TSMI_System_ManualsRelays.Text = "&Relays";
            this.TSMI_System_ManualsRelays.ToolTipText = "...zzzzzz...";
            this.TSMI_System_ManualsRelays.Click += new System.EventHandler(this.TSMI_System_ManualsRelays_Click);
            // 
            // TSMI_System_TestExecutiveConfigXML
            // 
            this.TSMI_System_TestExecutiveConfigXML.Name = "TSMI_System_TestExecutiveConfigXML";
            this.TSMI_System_TestExecutiveConfigXML.Size = new System.Drawing.Size(271, 26);
            this.TSMI_System_TestExecutiveConfigXML.Text = "&TestExecutive.config.xml";
            this.TSMI_System_TestExecutiveConfigXML.ToolTipText = "Test System\'s configuration.";
            // 
            // TMSI_System_Separator1
            // 
            this.TMSI_System_Separator1.Name = "TMSI_System_Separator1";
            this.TMSI_System_Separator1.Size = new System.Drawing.Size(268, 6);
            // 
            // TMSI_System_Compliments
            // 
            this.TMSI_System_Compliments.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_System_ComplimentsPraiseAndPlaudits,
            this.TMSI_System_ComplimentsMoney});
            this.TMSI_System_Compliments.Name = "TMSI_System_Compliments";
            this.TMSI_System_Compliments.Size = new System.Drawing.Size(271, 26);
            this.TMSI_System_Compliments.Text = "Co&mpliments";
            // 
            // TMSI_System_ComplimentsPraiseAndPlaudits
            // 
            this.TMSI_System_ComplimentsPraiseAndPlaudits.Name = "TMSI_System_ComplimentsPraiseAndPlaudits";
            this.TMSI_System_ComplimentsPraiseAndPlaudits.Size = new System.Drawing.Size(224, 26);
            this.TMSI_System_ComplimentsPraiseAndPlaudits.Text = "&Praise && Plaudits";
            this.TMSI_System_ComplimentsPraiseAndPlaudits.ToolTipText = "\"I can live for two months on a good compliment.\" - Mark Twain";
            this.TMSI_System_ComplimentsPraiseAndPlaudits.Click += new System.EventHandler(this.TSMI_System_ComplimentsPraiseAndPlaudits_Click);
            // 
            // TMSI_System_ComplimentsMoney
            // 
            this.TMSI_System_ComplimentsMoney.Name = "TMSI_System_ComplimentsMoney";
            this.TMSI_System_ComplimentsMoney.Size = new System.Drawing.Size(224, 26);
            this.TMSI_System_ComplimentsMoney.Text = "&Money!";
            this.TMSI_System_ComplimentsMoney.ToolTipText = "For a good cause! ";
            this.TMSI_System_ComplimentsMoney.Click += new System.EventHandler(this.TSMI_System_ComplimentsMoney_Click);
            // 
            // TMSI_System_Critiques
            // 
            this.TMSI_System_Critiques.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TMSI_System_CritiquesBugReport,
            this.TMSI_System_CritiquesImprovementRequest});
            this.TMSI_System_Critiques.Name = "TMSI_System_Critiques";
            this.TMSI_System_Critiques.Size = new System.Drawing.Size(271, 26);
            this.TMSI_System_Critiques.Text = "Cri&tiques";
            // 
            // TMSI_System_CritiquesBugReport
            // 
            this.TMSI_System_CritiquesBugReport.Name = "TMSI_System_CritiquesBugReport";
            this.TMSI_System_CritiquesBugReport.Size = new System.Drawing.Size(238, 26);
            this.TMSI_System_CritiquesBugReport.Text = "&Bug Report";
            this.TMSI_System_CritiquesBugReport.ToolTipText = "Remember, \"The devil is is in the details.\" - Friedrich Nietzsche";
            this.TMSI_System_CritiquesBugReport.Click += new System.EventHandler(this.TSMI_System_CritiqueBugReport_Click);
            // 
            // TMSI_System_CritiquesImprovementRequest
            // 
            this.TMSI_System_CritiquesImprovementRequest.Name = "TMSI_System_CritiquesImprovementRequest";
            this.TMSI_System_CritiquesImprovementRequest.Size = new System.Drawing.Size(238, 26);
            this.TMSI_System_CritiquesImprovementRequest.Text = "&Improvement Request";
            this.TMSI_System_CritiquesImprovementRequest.ToolTipText = "Remember, \"God is in the details.\" - Mies van der Rohe";
            this.TMSI_System_CritiquesImprovementRequest.Click += new System.EventHandler(this.TSMI_System_CritiqueImprovementRequest_Click);
            // 
            // TMSI_System_Separator2
            // 
            this.TMSI_System_Separator2.Name = "TMSI_System_Separator2";
            this.TMSI_System_Separator2.Size = new System.Drawing.Size(268, 6);
            // 
            // TSMI_System_About
            // 
            this.TSMI_System_About.Name = "TSMI_System_About";
            this.TSMI_System_About.Size = new System.Drawing.Size(271, 26);
            this.TSMI_System_About.Text = "&About...";
            this.TSMI_System_About.Click += new System.EventHandler(this.TSMI_System_About_Click);
            // 
            // TSMI_UUT
            // 
            this.TSMI_UUT.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_UUT_AppConfig,
            this.TSMI_UUT_eDocs,
            this.TSMI_UUT_Manuals,
            this.TSMI_UUT_TestData,
            this.TSMI_UUT_Separator1,
            this.TSMI_UUT_About});
            this.TSMI_UUT.Name = "TSMI_UUT";
            this.TSMI_UUT.Size = new System.Drawing.Size(51, 24);
            this.TSMI_UUT.Text = "&UUT";
            // 
            // TSMI_UUT_AppConfig
            // 
            this.TSMI_UUT_AppConfig.Name = "TSMI_UUT_AppConfig";
            this.TSMI_UUT_AppConfig.Size = new System.Drawing.Size(224, 26);
            this.TSMI_UUT_AppConfig.Text = "&App.config";
            this.TSMI_UUT_AppConfig.ToolTipText = "UUT\'s test configuration.";
            // 
            // TSMI_UUT_eDocs
            // 
            this.TSMI_UUT_eDocs.Name = "TSMI_UUT_eDocs";
            this.TSMI_UUT_eDocs.Size = new System.Drawing.Size(224, 26);
            this.TSMI_UUT_eDocs.Text = "&eDocs";
            this.TSMI_UUT_eDocs.ToolTipText = "UUT\'s P: drive eDocs folder.";
            this.TSMI_UUT_eDocs.Click += new System.EventHandler(this.TSMI_UUT_eDocs_Click);
            // 
            // TSMI_UUT_Manuals
            // 
            this.TSMI_UUT_Manuals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_UUT_ManualsInstruments});
            this.TSMI_UUT_Manuals.Name = "TSMI_UUT_Manuals";
            this.TSMI_UUT_Manuals.Size = new System.Drawing.Size(224, 26);
            this.TSMI_UUT_Manuals.Text = "&Manuals";
            // 
            // TSMI_UUT_ManualsInstruments
            // 
            this.TSMI_UUT_ManualsInstruments.Name = "TSMI_UUT_ManualsInstruments";
            this.TSMI_UUT_ManualsInstruments.Size = new System.Drawing.Size(224, 26);
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
            this.TSMI_UUT_TestData.Size = new System.Drawing.Size(224, 26);
            this.TSMI_UUT_TestData.Text = "&Test Data";
            // 
            // TSMI_UUT_TestDataP_DriveTDR_Folder
            // 
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.Name = "TSMI_UUT_TestDataP_DriveTDR_Folder";
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.Size = new System.Drawing.Size(268, 26);
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.Text = "&P: Drive TDR Folder";
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.ToolTipText = "P:\\Test\\TDR";
            this.TSMI_UUT_TestDataP_DriveTDR_Folder.Click += new System.EventHandler(this.TSMI_UUT_TestData_P_DriveTDR_Folder_Click);
            // 
            // TSMI_UUT_TestDataSQL_ReportingAndQuerying
            // 
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Enabled = false;
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Name = "TSMI_UUT_TestDataSQL_ReportingAndQuerying";
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Size = new System.Drawing.Size(268, 26);
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Text = "&SQL Reporting && Querying";
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.ToolTipText = "Coming soon!";
            this.TSMI_UUT_TestDataSQL_ReportingAndQuerying.Click += new System.EventHandler(this.TSMI_UUT_TestDataSQL_ReportingAndQuerying_Click);
            // 
            // TSMI_UUT_Separator1
            // 
            this.TSMI_UUT_Separator1.Name = "TSMI_UUT_Separator1";
            this.TSMI_UUT_Separator1.Size = new System.Drawing.Size(221, 6);
            // 
            // TSMI_UUT_About
            // 
            this.TSMI_UUT_About.Name = "TSMI_UUT_About";
            this.TSMI_UUT_About.Size = new System.Drawing.Size(224, 26);
            this.TSMI_UUT_About.Text = "&About...";
            // 
            // TSMI_Apps_LaunchMeasurementComputing
            // 
            this.TSMI_Apps_LaunchMeasurementComputing.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.instaCalToolStripMenuItem});
            this.TSMI_Apps_LaunchMeasurementComputing.Name = "TSMI_Apps_LaunchMeasurementComputing";
            this.TSMI_Apps_LaunchMeasurementComputing.Size = new System.Drawing.Size(260, 26);
            this.TSMI_Apps_LaunchMeasurementComputing.Text = "Measurement &Computing";
            // 
            // instaCalToolStripMenuItem
            // 
            this.instaCalToolStripMenuItem.Name = "instaCalToolStripMenuItem";
            this.instaCalToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.instaCalToolStripMenuItem.Text = "&InstaCal";
            this.instaCalToolStripMenuItem.ToolTipText = "Configure & test MCC Instruments, like USB-ERB24 relays.";
            // 
            // TSMI_Apps_LaunchKeysight
            // 
            this.TSMI_Apps_LaunchKeysight.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Apps_LaunchKeysightBenchVue,
            this.TSMI_Apps_LaunchKeysightCommandExpert,
            this.TSMI_Apps_LaunchKeysightConnectionExpert});
            this.TSMI_Apps_LaunchKeysight.Name = "TSMI_Apps_LaunchKeysight";
            this.TSMI_Apps_LaunchKeysight.Size = new System.Drawing.Size(260, 26);
            this.TSMI_Apps_LaunchKeysight.Text = "&Keysight";
            // 
            // TSMI_Apps_LaunchKeysightBenchVue
            // 
            this.TSMI_Apps_LaunchKeysightBenchVue.Name = "TSMI_Apps_LaunchKeysightBenchVue";
            this.TSMI_Apps_LaunchKeysightBenchVue.Size = new System.Drawing.Size(224, 26);
            this.TSMI_Apps_LaunchKeysightBenchVue.Text = "&BenchVue";
            this.TSMI_Apps_LaunchKeysightBenchVue.ToolTipText = "Control Keysight Instruments via soft/virtual panels.";
            // 
            // TSMI_Apps_LaunchKeysightCommandExpert
            // 
            this.TSMI_Apps_LaunchKeysightCommandExpert.Name = "TSMI_Apps_LaunchKeysightCommandExpert";
            this.TSMI_Apps_LaunchKeysightCommandExpert.Size = new System.Drawing.Size(224, 26);
            this.TSMI_Apps_LaunchKeysightCommandExpert.Text = "Co&mmand Expert";
            this.TSMI_Apps_LaunchKeysightCommandExpert.ToolTipText = "SCPI programming & debugging IDE.";
            // 
            // TSMI_Apps_LaunchKeysightConnectionExpert
            // 
            this.TSMI_Apps_LaunchKeysightConnectionExpert.Name = "TSMI_Apps_LaunchKeysightConnectionExpert";
            this.TSMI_Apps_LaunchKeysightConnectionExpert.Size = new System.Drawing.Size(224, 26);
            this.TSMI_Apps_LaunchKeysightConnectionExpert.Text = "Co&nnection Expert";
            this.TSMI_Apps_LaunchKeysightConnectionExpert.ToolTipText = "Discover VISA Instruments.";
            // 
            // TSMI_Apps_LaunchMicrosoft
            // 
            this.TSMI_Apps_LaunchMicrosoft.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sQLServerToolStripMenuItem,
            this.visualStudioToolStripMenuItem});
            this.TSMI_Apps_LaunchMicrosoft.Name = "TSMI_Apps_LaunchMicrosoft";
            this.TSMI_Apps_LaunchMicrosoft.Size = new System.Drawing.Size(260, 26);
            this.TSMI_Apps_LaunchMicrosoft.Text = "&Microsoft";
            // 
            // sQLServerToolStripMenuItem
            // 
            this.sQLServerToolStripMenuItem.Enabled = false;
            this.sQLServerToolStripMenuItem.Name = "sQLServerToolStripMenuItem";
            this.sQLServerToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.sQLServerToolStripMenuItem.Text = "&SQL Server Management Studio";
            this.sQLServerToolStripMenuItem.ToolTipText = "Coming soon!";
            // 
            // visualStudioToolStripMenuItem
            // 
            this.visualStudioToolStripMenuItem.Name = "visualStudioToolStripMenuItem";
            this.visualStudioToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            this.visualStudioToolStripMenuItem.Text = "&Visual Studio";
            this.visualStudioToolStripMenuItem.ToolTipText = "C# forever!";
            // 
            // TestExecutive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1393, 784);
            this.Controls.Add(this.ButtonEmergencyStop);
            this.Controls.Add(this.ButtonSelectTests);
            this.Controls.Add(this.rtfResults);
            this.Controls.Add(this.LabelResult);
            this.Controls.Add(this.TextResult);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.MS);
            this.MainMenuStrip = this.MS;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
        private ToolStripSeparator TMSI_FileSeparator1;
        private ToolStripMenuItem TSMI_File_Print;
        private ToolStripMenuItem TSMI_File_PrintPreview;
        private ToolStripSeparator TMSI_FileSeparator2;
        private ToolStripMenuItem TSMI_File_Exit;
        private ToolStripMenuItem TMSI_System;
        private ToolStripMenuItem TSMI_Apps;
        private ToolStripMenuItem TMSI_System_Diagnostics;
        private ToolStripMenuItem TMSI_System_DiagnosticsBarcodeScanner;
        private ToolStripMenuItem TMSI_System_DiagnosticsInstruments;
        private ToolStripMenuItem TMSI_System_DiagnosticsRelays;
        private ToolStripSeparator TMSI_System_Separator1;
        private ToolStripMenuItem TMSI_System_Compliments;
        private ToolStripMenuItem TMSI_System_ComplimentsPraiseAndPlaudits;
        private ToolStripMenuItem TMSI_System_ComplimentsMoney;
        private ToolStripMenuItem TMSI_System_Critiques;
        private ToolStripMenuItem TMSI_System_CritiquesBugReport;
        private ToolStripMenuItem TMSI_System_CritiquesImprovementRequest;
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
        private ToolStripSeparator TMSI_System_Separator2;
        private ToolStripMenuItem TSMI_System_About;
        private ToolStripMenuItem TMSI_System_BarcodeScannerDiscovery;
        private ToolStripSeparator TSMI_UUT_Separator1;
        private ToolStripMenuItem TSMI_UUT_About;
        private ToolStripMenuItem TSMI_System_TestExecutiveConfigXML;
        private ToolStripMenuItem TSMI_UUT_AppConfig;
        private ToolStripMenuItem TSMI_Apps_LaunchKeysight;
        private ToolStripMenuItem TSMI_Apps_LaunchKeysightBenchVue;
        private ToolStripMenuItem TSMI_Apps_LaunchKeysightCommandExpert;
        private ToolStripMenuItem TSMI_Apps_LaunchKeysightConnectionExpert;
        private ToolStripMenuItem TSMI_Apps_LaunchMeasurementComputing;
        private ToolStripMenuItem instaCalToolStripMenuItem;
        private ToolStripMenuItem TSMI_Apps_LaunchMicrosoft;
        private ToolStripMenuItem sQLServerToolStripMenuItem;
        private ToolStripMenuItem visualStudioToolStripMenuItem;
    }
}
