using System.Windows.Forms;

namespace TestLibrary {
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
            this.TextUUTResult = new System.Windows.Forms.TextBox();
            this.LabelUUTResult = new System.Windows.Forms.Label();
            this.rtfResults = new System.Windows.Forms.RichTextBox();
            this.ButtonSaveOutput = new System.Windows.Forms.Button();
            this.ButtonSelectGroup = new System.Windows.Forms.Button();
            this.ButtonOpenTestDataFolder = new System.Windows.Forms.Button();
            this.ButtonEmergencyStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonStart.BackColor = System.Drawing.Color.Green;
            this.ButtonStart.Location = new System.Drawing.Point(152, 560);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(88, 52);
            this.ButtonStart.TabIndex = 1;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = false;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Clicked);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonCancel.BackColor = System.Drawing.Color.Yellow;
            this.ButtonCancel.Location = new System.Drawing.Point(281, 560);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(88, 52);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = false;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Clicked);
            // 
            // TextUUTResult
            // 
            this.TextUUTResult.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TextUUTResult.Location = new System.Drawing.Point(493, 580);
            this.TextUUTResult.Name = "TextUUTResult";
            this.TextUUTResult.ReadOnly = true;
            this.TextUUTResult.Size = new System.Drawing.Size(60, 20);
            this.TextUUTResult.TabIndex = 3;
            this.TextUUTResult.TabStop = false;
            this.TextUUTResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelUUTResult
            // 
            this.LabelUUTResult.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LabelUUTResult.AutoSize = true;
            this.LabelUUTResult.Location = new System.Drawing.Point(490, 564);
            this.LabelUUTResult.Name = "LabelUUTResult";
            this.LabelUUTResult.Size = new System.Drawing.Size(63, 13);
            this.LabelUUTResult.TabIndex = 7;
            this.LabelUUTResult.Text = "UUT Result";
            this.LabelUUTResult.UseWaitCursor = true;
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
            this.rtfResults.TabIndex = 8;
            this.rtfResults.TabStop = false;
            this.rtfResults.Text = "";
            // 
            // ButtonSaveOutput
            // 
            this.ButtonSaveOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSaveOutput.Location = new System.Drawing.Point(683, 564);
            this.ButtonSaveOutput.Name = "ButtonSaveOutput";
            this.ButtonSaveOutput.Size = new System.Drawing.Size(88, 47);
            this.ButtonSaveOutput.TabIndex = 4;
            this.ButtonSaveOutput.Text = "Save Output";
            this.ButtonSaveOutput.UseVisualStyleBackColor = true;
            this.ButtonSaveOutput.Click += new System.EventHandler(this.ButtonSaveOutput_Click);
            // 
            // ButtonSelectGroup
            // 
            this.ButtonSelectGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonSelectGroup.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonSelectGroup.Location = new System.Drawing.Point(23, 561);
            this.ButtonSelectGroup.Name = "ButtonSelectGroup";
            this.ButtonSelectGroup.Size = new System.Drawing.Size(88, 47);
            this.ButtonSelectGroup.TabIndex = 0;
            this.ButtonSelectGroup.Text = "Select Group";
            this.ButtonSelectGroup.UseVisualStyleBackColor = true;
            this.ButtonSelectGroup.Click += new System.EventHandler(this.ButtonSelectGroup_Click);
            // 
            // ButtonOpenTestDataFolder
            // 
            this.ButtonOpenTestDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOpenTestDataFolder.Location = new System.Drawing.Point(813, 563);
            this.ButtonOpenTestDataFolder.Name = "ButtonOpenTestDataFolder";
            this.ButtonOpenTestDataFolder.Size = new System.Drawing.Size(88, 47);
            this.ButtonOpenTestDataFolder.TabIndex = 5;
            this.ButtonOpenTestDataFolder.Text = "Open Test Data Folder";
            this.ButtonOpenTestDataFolder.UseVisualStyleBackColor = true;
            this.ButtonOpenTestDataFolder.Click += new System.EventHandler(this.ButtonOpenTestDataFolder_Click);
            // 
            // ButtonEmergencyStop
            // 
            this.ButtonEmergencyStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonEmergencyStop.Image = ((System.Drawing.Image)(resources.GetObject("ButtonEmergencyStop.Image")));
            this.ButtonEmergencyStop.Location = new System.Drawing.Point(942, 546);
            this.ButtonEmergencyStop.Name = "ButtonEmergencyStop";
            this.ButtonEmergencyStop.Size = new System.Drawing.Size(82, 82);
            this.ButtonEmergencyStop.TabIndex = 6;
            this.ButtonEmergencyStop.UseVisualStyleBackColor = true;
            this.ButtonEmergencyStop.Click += new System.EventHandler(this.ButtonEmergencyStop_Clicked);
            // 
            // TestExecutive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 637);
            this.Controls.Add(this.ButtonEmergencyStop);
            this.Controls.Add(this.ButtonOpenTestDataFolder);
            this.Controls.Add(this.ButtonSelectGroup);
            this.Controls.Add(this.ButtonSaveOutput);
            this.Controls.Add(this.rtfResults);
            this.Controls.Add(this.LabelUUTResult);
            this.Controls.Add(this.TextUUTResult);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonStart);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TestExecutive";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Program";
            this.Load += new System.EventHandler(this.Form_Load);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.TextBox TextUUTResult;
        private System.Windows.Forms.Label LabelUUTResult;
        private System.Windows.Forms.RichTextBox rtfResults;
        private System.Windows.Forms.Button ButtonSaveOutput;
        private System.Windows.Forms.Button ButtonSelectGroup;
        private System.Windows.Forms.Button ButtonOpenTestDataFolder;
        private Button ButtonEmergencyStop;
    }
}
