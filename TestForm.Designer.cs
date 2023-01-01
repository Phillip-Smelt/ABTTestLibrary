namespace ABTTestLibrary {
    public partial class TestForm {
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
            this.ButtonStart = new System.Windows.Forms.Button();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.TextUUTResult = new System.Windows.Forms.TextBox();
            this.LabelUUTResult = new System.Windows.Forms.Label();
            this.rtfResults = new System.Windows.Forms.RichTextBox();
            this.ButtonSaveOutput = new System.Windows.Forms.Button();
            this.ButtonSelectGroup = new System.Windows.Forms.Button();
            this.ButtonOpenTestDataFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.BackColor = System.Drawing.Color.Green;
            this.ButtonStart.Location = new System.Drawing.Point(240, 561);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(88, 52);
            this.ButtonStart.TabIndex = 1;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = false;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Clicked);
            // 
            // ButtonStop
            // 
            this.ButtonStop.BackColor = System.Drawing.Color.Red;
            this.ButtonStop.Location = new System.Drawing.Point(366, 561);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(88, 52);
            this.ButtonStop.TabIndex = 2;
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.UseVisualStyleBackColor = false;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Clicked);
            // 
            // TextUUTResult
            // 
            this.TextUUTResult.Location = new System.Drawing.Point(928, 579);
            this.TextUUTResult.Name = "TextUUTResult";
            this.TextUUTResult.ReadOnly = true;
            this.TextUUTResult.Size = new System.Drawing.Size(60, 20);
            this.TextUUTResult.TabIndex = 6;
            this.TextUUTResult.TabStop = false;
            this.TextUUTResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelUUTResult
            // 
            this.LabelUUTResult.AutoSize = true;
            this.LabelUUTResult.Location = new System.Drawing.Point(925, 563);
            this.LabelUUTResult.Name = "LabelUUTResult";
            this.LabelUUTResult.Size = new System.Drawing.Size(63, 13);
            this.LabelUUTResult.TabIndex = 5;
            this.LabelUUTResult.Text = "UUT Result";
            this.LabelUUTResult.UseWaitCursor = true;
            // 
            // rtfResults
            // 
            this.rtfResults.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfResults.Location = new System.Drawing.Point(23, 21);
            this.rtfResults.Name = "rtfResults";
            this.rtfResults.ReadOnly = true;
            this.rtfResults.Size = new System.Drawing.Size(1001, 521);
            this.rtfResults.TabIndex = 7;
            this.rtfResults.TabStop = false;
            this.rtfResults.Text = "";
            // 
            // ButtonSaveOutput
            // 
            this.ButtonSaveOutput.Location = new System.Drawing.Point(735, 563);
            this.ButtonSaveOutput.Name = "ButtonSaveOutput";
            this.ButtonSaveOutput.Size = new System.Drawing.Size(88, 47);
            this.ButtonSaveOutput.TabIndex = 4;
            this.ButtonSaveOutput.Text = "Save Output";
            this.ButtonSaveOutput.UseVisualStyleBackColor = true;
            this.ButtonSaveOutput.Click += new System.EventHandler(this.ButtonSaveOutput_Click);
            // 
            // ButtonSelectGroup
            // 
            this.ButtonSelectGroup.Location = new System.Drawing.Point(59, 561);
            this.ButtonSelectGroup.Name = "ButtonSelectGroup";
            this.ButtonSelectGroup.Size = new System.Drawing.Size(88, 47);
            this.ButtonSelectGroup.TabIndex = 0;
            this.ButtonSelectGroup.Text = "Select Group";
            this.ButtonSelectGroup.UseVisualStyleBackColor = true;
            this.ButtonSelectGroup.Click += new System.EventHandler(this.ButtonSelectGroup_Click);
            // 
            // ButtonOpenTestDataFolder
            // 
            this.ButtonOpenTestDataFolder.Location = new System.Drawing.Point(606, 563);
            this.ButtonOpenTestDataFolder.Name = "ButtonOpenTestDataFolder";
            this.ButtonOpenTestDataFolder.Size = new System.Drawing.Size(88, 47);
            this.ButtonOpenTestDataFolder.TabIndex = 3;
            this.ButtonOpenTestDataFolder.Text = "Open Test Data Folder";
            this.ButtonOpenTestDataFolder.UseVisualStyleBackColor = true;
            this.ButtonOpenTestDataFolder.Click += new System.EventHandler(this.ButtonOpenTestDataFolder_Click);
            // 
            // ABTTestLibraryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 637);
            this.Controls.Add(this.ButtonOpenTestDataFolder);
            this.Controls.Add(this.ButtonSelectGroup);
            this.Controls.Add(this.ButtonSaveOutput);
            this.Controls.Add(this.rtfResults);
            this.Controls.Add(this.LabelUUTResult);
            this.Controls.Add(this.TextUUTResult);
            this.Controls.Add(this.ButtonStop);
            this.Controls.Add(this.ButtonStart);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ABTTestLibraryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABT Test Program";
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.TextBox TextUUTResult;
        private System.Windows.Forms.Label LabelUUTResult;
        private System.Windows.Forms.RichTextBox rtfResults;
        private System.Windows.Forms.Button ButtonSaveOutput;
        private System.Windows.Forms.Button ButtonSelectGroup;
        private System.Windows.Forms.Button ButtonOpenTestDataFolder;
    }
}
