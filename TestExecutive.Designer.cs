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
            ButtonStart = new System.Windows.Forms.Button();
            ButtonCancel = new System.Windows.Forms.Button();
            TextResult = new System.Windows.Forms.TextBox();
            LabelResult = new System.Windows.Forms.Label();
            rtfResults = new System.Windows.Forms.RichTextBox();
            ButtonSaveOutput = new System.Windows.Forms.Button();
            ButtonSelectTests = new System.Windows.Forms.Button();
            ButtonOpenTestDataFolder = new System.Windows.Forms.Button();
            ButtonEmergencyStop = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // ButtonStart
            // 
            ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            ButtonStart.BackColor = System.Drawing.Color.Green;
            ButtonStart.Location = new System.Drawing.Point(203, 689);
            ButtonStart.Margin = new System.Windows.Forms.Padding(4);
            ButtonStart.Name = "ButtonStart";
            ButtonStart.Size = new System.Drawing.Size(117, 64);
            ButtonStart.TabIndex = 1;
            ButtonStart.Text = "Start";
            ButtonStart.UseVisualStyleBackColor = false;
            ButtonStart.Click += new System.EventHandler(ButtonStart_Clicked);
            // 
            // ButtonCancel
            // 
            ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            ButtonCancel.BackColor = System.Drawing.Color.Yellow;
            ButtonCancel.Location = new System.Drawing.Point(375, 689);
            ButtonCancel.Margin = new System.Windows.Forms.Padding(4);
            ButtonCancel.Name = "ButtonCancel";
            ButtonCancel.Size = new System.Drawing.Size(117, 64);
            ButtonCancel.TabIndex = 2;
            ButtonCancel.Text = "Cancel";
            ButtonCancel.UseVisualStyleBackColor = false;
            ButtonCancel.Click += new System.EventHandler(ButtonCancel_Clicked);
            // 
            // TextResult
            // 
            TextResult.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            TextResult.Location = new System.Drawing.Point(657, 714);
            TextResult.Margin = new System.Windows.Forms.Padding(4);
            TextResult.Name = "TextResult";
            TextResult.ReadOnly = true;
            TextResult.Size = new System.Drawing.Size(79, 22);
            TextResult.TabIndex = 3;
            TextResult.TabStop = false;
            TextResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelResult
            // 
            LabelResult.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            LabelResult.AutoSize = true;
            LabelResult.Location = new System.Drawing.Point(672, 689);
            LabelResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            LabelResult.Name = "LabelResult";
            LabelResult.Size = new System.Drawing.Size(45, 16);
            LabelResult.TabIndex = 7;
            LabelResult.Text = "Result";
            LabelResult.UseWaitCursor = true;
            // 
            // rtfResults
            // 
            rtfResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            rtfResults.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            rtfResults.Location = new System.Drawing.Point(31, 26);
            rtfResults.Margin = new System.Windows.Forms.Padding(4);
            rtfResults.Name = "rtfResults";
            rtfResults.ReadOnly = true;
            rtfResults.Size = new System.Drawing.Size(1333, 640);
            rtfResults.TabIndex = 8;
            rtfResults.TabStop = false;
            rtfResults.Text = "";
            // 
            // ButtonSaveOutput
            // 
            ButtonSaveOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            ButtonSaveOutput.Location = new System.Drawing.Point(911, 694);
            ButtonSaveOutput.Margin = new System.Windows.Forms.Padding(4);
            ButtonSaveOutput.Name = "ButtonSaveOutput";
            ButtonSaveOutput.Size = new System.Drawing.Size(117, 58);
            ButtonSaveOutput.TabIndex = 4;
            ButtonSaveOutput.Text = "Save Output";
            ButtonSaveOutput.UseVisualStyleBackColor = true;
            ButtonSaveOutput.Click += new System.EventHandler(ButtonSaveOutput_Click);
            // 
            // ButtonSelectTests
            // 
            ButtonSelectTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            ButtonSelectTests.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            ButtonSelectTests.Location = new System.Drawing.Point(31, 690);
            ButtonSelectTests.Margin = new System.Windows.Forms.Padding(4);
            ButtonSelectTests.Name = "ButtonSelectTests";
            ButtonSelectTests.Size = new System.Drawing.Size(117, 58);
            ButtonSelectTests.TabIndex = 0;
            ButtonSelectTests.Text = "Select Tests";
            ButtonSelectTests.UseVisualStyleBackColor = true;
            ButtonSelectTests.Click += new System.EventHandler(ButtonSelectTests_Click);
            // 
            // ButtonOpenTestDataFolder
            // 
            ButtonOpenTestDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            ButtonOpenTestDataFolder.Location = new System.Drawing.Point(1084, 693);
            ButtonOpenTestDataFolder.Margin = new System.Windows.Forms.Padding(4);
            ButtonOpenTestDataFolder.Name = "ButtonOpenTestDataFolder";
            ButtonOpenTestDataFolder.Size = new System.Drawing.Size(117, 58);
            ButtonOpenTestDataFolder.TabIndex = 5;
            ButtonOpenTestDataFolder.Text = "Open Test Data Folder";
            ButtonOpenTestDataFolder.UseVisualStyleBackColor = true;
            ButtonOpenTestDataFolder.Click += new System.EventHandler(ButtonOpenTestDataFolder_Click);
            // 
            // ButtonEmergencyStop
            // 
            ButtonEmergencyStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            ButtonEmergencyStop.Image = ((System.Drawing.Image)(resources.GetObject("ButtonEmergencyStop.Image")));
            ButtonEmergencyStop.Location = new System.Drawing.Point(1256, 672);
            ButtonEmergencyStop.Margin = new System.Windows.Forms.Padding(4);
            ButtonEmergencyStop.Name = "ButtonEmergencyStop";
            ButtonEmergencyStop.Size = new System.Drawing.Size(109, 101);
            ButtonEmergencyStop.TabIndex = 6;
            ButtonEmergencyStop.UseVisualStyleBackColor = true;
            ButtonEmergencyStop.Click += new System.EventHandler(ButtonEmergencyStop_Clicked);
            // 
            // TestExecutive
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1400, 784);
            Controls.Add(ButtonEmergencyStop);
            Controls.Add(ButtonOpenTestDataFolder);
            Controls.Add(ButtonSelectTests);
            Controls.Add(ButtonSaveOutput);
            Controls.Add(rtfResults);
            Controls.Add(LabelResult);
            Controls.Add(TextResult);
            Controls.Add(ButtonCancel);
            Controls.Add(ButtonStart);
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "TestExecutive";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Test Program";
            Shown += new System.EventHandler(Form_Shown);
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.TextBox TextResult;
        private System.Windows.Forms.Label LabelResult;
        private System.Windows.Forms.RichTextBox rtfResults;
        private System.Windows.Forms.Button ButtonSaveOutput;
        private System.Windows.Forms.Button ButtonSelectTests;
        private System.Windows.Forms.Button ButtonOpenTestDataFolder;
        private Button ButtonEmergencyStop;
    }
}
