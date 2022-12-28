namespace ABTTestLibrary {
    public partial class ABTTestLibraryForm {
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.textUUTResult = new System.Windows.Forms.TextBox();
            this.LabelUUTResult = new System.Windows.Forms.Label();
            this.rtfResults = new System.Windows.Forms.RichTextBox();
            this.buttonSaveOutput = new System.Windows.Forms.Button();
            this.buttonSelectGroup = new System.Windows.Forms.Button();
            this.buttonOpenTestDataFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.Green;
            this.buttonStart.Location = new System.Drawing.Point(320, 690);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(117, 64);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Clicked);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.Red;
            this.buttonStop.Location = new System.Drawing.Point(488, 690);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(117, 64);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Clicked);
            // 
            // textUUTResult
            // 
            this.textUUTResult.Location = new System.Drawing.Point(1237, 713);
            this.textUUTResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textUUTResult.Name = "textUUTResult";
            this.textUUTResult.ReadOnly = true;
            this.textUUTResult.Size = new System.Drawing.Size(79, 22);
            this.textUUTResult.TabIndex = 6;
            this.textUUTResult.TabStop = false;
            this.textUUTResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelUUTResult
            // 
            this.LabelUUTResult.AutoSize = true;
            this.LabelUUTResult.Location = new System.Drawing.Point(1233, 693);
            this.LabelUUTResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelUUTResult.Name = "LabelUUTResult";
            this.LabelUUTResult.Size = new System.Drawing.Size(77, 16);
            this.LabelUUTResult.TabIndex = 5;
            this.LabelUUTResult.Text = "UUT Result";
            this.LabelUUTResult.UseWaitCursor = true;
            // 
            // rtfResults
            // 
            this.rtfResults.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfResults.Location = new System.Drawing.Point(31, 26);
            this.rtfResults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rtfResults.Name = "rtfResults";
            this.rtfResults.ReadOnly = true;
            this.rtfResults.Size = new System.Drawing.Size(1333, 640);
            this.rtfResults.TabIndex = 7;
            this.rtfResults.TabStop = false;
            this.rtfResults.Text = "";
            // 
            // buttonSaveOutput
            // 
            this.buttonSaveOutput.Location = new System.Drawing.Point(980, 693);
            this.buttonSaveOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSaveOutput.Name = "buttonSaveOutput";
            this.buttonSaveOutput.Size = new System.Drawing.Size(117, 58);
            this.buttonSaveOutput.TabIndex = 4;
            this.buttonSaveOutput.Text = "Save Output";
            this.buttonSaveOutput.UseVisualStyleBackColor = true;
            this.buttonSaveOutput.Click += new System.EventHandler(this.buttonSaveOutput_Click);
            // 
            // buttonSelectGroup
            // 
            this.buttonSelectGroup.Location = new System.Drawing.Point(79, 690);
            this.buttonSelectGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSelectGroup.Name = "buttonSelectGroup";
            this.buttonSelectGroup.Size = new System.Drawing.Size(117, 58);
            this.buttonSelectGroup.TabIndex = 0;
            this.buttonSelectGroup.Text = "Select Group";
            this.buttonSelectGroup.UseVisualStyleBackColor = true;
            this.buttonSelectGroup.Click += new System.EventHandler(this.buttonSelectGroup_Click);
            // 
            // buttonOpenTestDataFolder
            // 
            this.buttonOpenTestDataFolder.Location = new System.Drawing.Point(808, 693);
            this.buttonOpenTestDataFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOpenTestDataFolder.Name = "buttonOpenTestDataFolder";
            this.buttonOpenTestDataFolder.Size = new System.Drawing.Size(117, 58);
            this.buttonOpenTestDataFolder.TabIndex = 3;
            this.buttonOpenTestDataFolder.Text = "Open Test Data Folder";
            this.buttonOpenTestDataFolder.UseVisualStyleBackColor = true;
            this.buttonOpenTestDataFolder.Click += new System.EventHandler(this.buttonOpenTestDataFolder_Click);
            // 
            // ABTTestLibraryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 784);
            this.Controls.Add(this.buttonOpenTestDataFolder);
            this.Controls.Add(this.buttonSelectGroup);
            this.Controls.Add(this.buttonSaveOutput);
            this.Controls.Add(this.rtfResults);
            this.Controls.Add(this.LabelUUTResult);
            this.Controls.Add(this.textUUTResult);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ABTTestLibraryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABT Test Program";
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textUUTResult;
        private System.Windows.Forms.Label LabelUUTResult;
        private System.Windows.Forms.RichTextBox rtfResults;
        private System.Windows.Forms.Button buttonSaveOutput;
        private System.Windows.Forms.Button buttonSelectGroup;
        private System.Windows.Forms.Button buttonOpenTestDataFolder;
    }
}
